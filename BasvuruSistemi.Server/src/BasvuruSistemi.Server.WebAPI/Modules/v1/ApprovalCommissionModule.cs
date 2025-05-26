
using BasvuruSistemi.Server.Application.Auth;
using BasvuruSistemi.Server.Application.Comissions;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class ApprovalCommissionModule
{
    public static void RegisterApprovalCommissionRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/commissions").WithTags("Commissions");

        group.MapPost("",
            async (ISender sender, CreateApprovalCommissionCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

    }
}
