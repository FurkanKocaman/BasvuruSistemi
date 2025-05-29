using BasvuruSistemi.Server.Application.Evaluations;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class EvaluationStageModule
{
    public static void RegisterEvaluationStageRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/evaluation-stages").WithTags("EvaluationStages");

        group.MapPost("",
            async (ISender sender, CreateEvaluationStageCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationStageCreate");

        group.MapPut("{id:guid}",
            async (ISender sender, Guid id, UpdateEvaluationStageCommand request, CancellationToken cancellationToken) =>
            {
                if (id != request.Id)
                    return Results.BadRequest("ID in route does not match ID in body.");

                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationStageUpdate");

        group.MapDelete("{id:guid}",
           async (ISender sender, Guid id, CancellationToken cancellationToken) =>
           {
               var response = await sender.Send(new DeleteEvaluationStageCommand(id), cancellationToken);
               return response.IsSuccessful ? Results.Ok(response) : response.StatusCode == 404 ? Results.NotFound(response) : Results.InternalServerError(response);
           })
           .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationStageDelete");
    }
}
