using BasvuruSistemi.Server.Application.FormTemplates;
using BasvuruSistemi.Server.Domain.Constants;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.WebAPI.Modules.v1;

public static class JobPostingEvaluationPipelineStageModule
{
    public static void RegisterJobPostingEvaluationPipelineStageModule(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("v1/jobposting-evaluation-pipeline").WithTags("JobpostingEvaluationPipeline");

        group.MapPost("",
            async (ISender sender, FormTemplateCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .RequireAuthorization(CustomClaimTypes.ManageTemplates).Produces<Result<string>>().WithName("FormTemplateCreate");
    }
}
