using BasvuruSistemi.Server.Application.FormTemplates;
using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.ApplicationEvaluations;
public sealed record GetEvaluationFormForApplicationEvaluationQuery(
    Guid ApplicationEvaluationId
    ) : IRequest<Result<GetEvaluationFormForApplicationEvaluationQueryResponse>>;

public sealed class GetEvaluationFormForApplicationEvaluationQueryResponse
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<FormFieldResponse> Fields { get; set; } = new();
}

internal sealed class GetEvaluationFormForApplicationEvaluationQueryHandler(
    ICurrentUserService currentUserService,
    IApplicationEvaluationRepository applicationEvaluationRepository
    ) : IRequestHandler<GetEvaluationFormForApplicationEvaluationQuery, Result<GetEvaluationFormForApplicationEvaluationQueryResponse>>
{
    public async Task<Result<GetEvaluationFormForApplicationEvaluationQueryResponse>> Handle(GetEvaluationFormForApplicationEvaluationQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<GetEvaluationFormForApplicationEvaluationQueryResponse>.Failure(401,"User not found");

        var applicationEvaluation = await applicationEvaluationRepository
            .Where(p => p.Id == request.ApplicationEvaluationId)
            .Include(p => p.JobPostingEvaluationPipelineStage)
                .ThenInclude(p => p.EvaluationStage)
            .Include(p => p.JobPostingEvaluationPipelineStage)
                .ThenInclude(p => p.EvaluationForm)
                .ThenInclude(p => p.Fields)
            .FirstOrDefaultAsync();

        if (applicationEvaluation is null)
            return Result<GetEvaluationFormForApplicationEvaluationQueryResponse>.Failure(404, "Application not found");

        var evaluationPipelineStage = applicationEvaluation.JobPostingEvaluationPipelineStage;

        if (evaluationPipelineStage is null)
            return Result<GetEvaluationFormForApplicationEvaluationQueryResponse>.Failure(404, "PipelineStage not found");

        var response = new GetEvaluationFormForApplicationEvaluationQueryResponse()
        {
            Name = evaluationPipelineStage.EvaluationForm.Name,
            Description = evaluationPipelineStage.EvaluationForm.Description,
            Fields = evaluationPipelineStage.EvaluationForm.Fields.Select(field => new FormFieldResponse
            {
                Id = field.Id,
                Label = field.Label,
                Type = (int)field.Type,
                IsRequired = field.IsRequired,
                Order = field.Order,
                Description = field.Description,
                Placeholder = field.Placeholder,
                OptionsJson = field.OptionsJson,
                IsReadOnly = field.IsReadOnly,
                DefaultValue = field.DefaultValue,
                VerificationSource = 0,
                VerificationParametersJson = null,
                AllowedFileTypes = field.AllowedFileTypes,
                MaxFileSizeMB = field.MaxFileSizeMB
            }).OrderBy(p => p.Order).ToList(),
        };

        return Result<GetEvaluationFormForApplicationEvaluationQueryResponse>.Succeed(response);
    }
}
