using BasvuruSistemi.Server.Application.Companies;
using BasvuruSistemi.Server.Application.Departments;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class DepartmentModule
{
    public static void RegisterDepartmentRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/departments").WithTags("Departments");

        group.MapPost("",
            async (ISender sender, DepartmentCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();
    }
}
