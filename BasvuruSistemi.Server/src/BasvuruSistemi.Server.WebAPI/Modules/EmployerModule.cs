using BasvuruSistemi.Server.Application.Candidates;
using BasvuruSistemi.Server.Application.Employers;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules;

public static class EmployerModule
{
    public static void RegisterEmployerRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/employers").WithTags("Employers");

        group.MapPost("/",
            async (ISender sender, EmployerCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
