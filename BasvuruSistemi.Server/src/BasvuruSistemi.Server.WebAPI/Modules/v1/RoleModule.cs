using BasvuruSistemi.Server.Application.AppRoles;
using BasvuruSistemi.Server.Application.RoleAssignments;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class RoleModule
{
    public static void RegisterRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/roles").WithTags("Roles");

        group.MapPost("",
            async (ISender sender, RoleCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).RequireAuthorization().Produces<Result<string>>();

        group.MapPut("{id:guid}",
            async (ISender sender, Guid id, RoleUpdateCommand request, CancellationToken cancellationToken) =>
            {
                if(id != request.Id)
                    return Results.BadRequest("Id in the route and body do not match");
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            }).RequireAuthorization().Produces<Result<string>>();

        group.MapDelete("{id:guid}",
           async (ISender sender, Guid id, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(new RoleDeleteCommand(id), cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           }).RequireAuthorization().Produces<Result<string>>();

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

        group.MapPost("generate-default",
           async (ISender sender, GenerateDefaultRolesCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           }).RequireAuthorization().Produces<Result<string>>();
    }
}
