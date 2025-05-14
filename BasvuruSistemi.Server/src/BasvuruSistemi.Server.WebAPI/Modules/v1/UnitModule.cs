using BasvuruSistemi.Server.Application.Units;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class UnitModule
{
    public static void RegisterUnitRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/api/units").WithTags("Units");

        group.MapPost("",
            async (ISender sender, UnitCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>();

        group.MapPut("update/{id}",
            async (Guid id, UnitUpdateCommand request, ISender sender, CancellationToken cancellationToken) =>
            {
                if (id != request.id)
                    return Results.BadRequest("ID in route does not match ID in body.");

                var command = new UnitUpdateCommand(request.id, request.parentId, request.name, request.code);
                var response = await sender.Send(command, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.BadRequest(response);
            })
            .RequireAuthorization().Produces<Result<string>>();


        group.MapDelete("delete/{id}",
           async (ISender sender, Guid id, CancellationToken cancellationToken) =>
           {
               var request = new UnitDeleteCommand(id);
               var response = await sender.Send(request, cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>();
    }
}
