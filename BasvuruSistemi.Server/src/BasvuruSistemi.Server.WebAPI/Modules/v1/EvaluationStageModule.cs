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
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization().Produces<Result<string>>().WithName("EvaluationStageCreate");
    }
}
