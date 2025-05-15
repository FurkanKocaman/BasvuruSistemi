using BasvuruSistemi.Server.Application.Addresses;
using BasvuruSistemi.Server.Application.Users;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class UserModule
{
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/users").WithTags("Users");

        group.MapPost("address",
           async (ISender sender, AddressCreateCommand request, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();

        group.MapPut("address-update/{id}",
           async (ISender sender,Guid id, UserAddressUpdateCommand request, CancellationToken cancellationToken) =>
           {
               if(id != request.UserId)
                   return Results.BadRequest("Id mismatch");
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();

    }
}
