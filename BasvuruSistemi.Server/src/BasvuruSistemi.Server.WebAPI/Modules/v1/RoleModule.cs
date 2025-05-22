using BasvuruSistemi.Server.Application.RoleAssignments;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class RoleModule
{
    public static void RegisterRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/roles").WithTags("Roles");

        group.MapPost("invitation",
            async (ISender sender, CreateRoleAssignmentCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapPatch("verify-invitation/{token}",
           async (ISender sender, string token, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(new VerifyInvitationCommand(token), cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();
    }
}
