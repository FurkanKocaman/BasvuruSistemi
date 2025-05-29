using BasvuruSistemi.Server.Application.Comissions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class CommissionMemberModule
{
    public static void RegisterCommissionMemberRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/commission-members").WithTags("CommissionMembers");

        group.MapPost("",
            async (ISender sender, AddMemberToCommissionCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        group.MapPut("{id:guid}",
            async (ISender sender, Guid Id, UpdateCommissionMemberCommand request, CancellationToken cancellationToken) =>
            {
                if (Id != request.Id)
                    return Results.BadRequest("Id in the route and body do not match.");

                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();

        group.MapDelete("",
           async(ISender sender, [FromQuery] Guid userId, [FromQuery] Guid commissionId, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(new RemoveMemberFromCommissionCommand(userId,commissionId), cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .Produces<Result<string>>();

    }
}
