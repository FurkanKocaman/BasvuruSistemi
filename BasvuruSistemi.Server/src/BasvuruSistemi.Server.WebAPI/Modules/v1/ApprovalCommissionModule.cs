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

        group.MapPut("{id:guid}",
            async (ISender sender, Guid id, UpdateApprovalCommissionCommand request, CancellationToken cancellationToken) =>
            {
                if( request.Id != id)
                {
                    return Results.BadRequest("Commission ID in the request body does not match the ID in the URL.");
                }

                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
