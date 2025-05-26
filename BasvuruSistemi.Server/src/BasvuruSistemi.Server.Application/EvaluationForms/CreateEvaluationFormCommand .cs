using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.EvaluationForms;
public sealed record CreateEvaluationFormCommand(
    string Name,
    Guid EvaluationStageId,
    string? Description = null
    ) : IRequest<Result<string>>;

internal sealed class CreateEvaluationFormCommandHandler(
    IEvaluationFormRepository evaluationFormRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    ) : IRequestHandler<CreateEvaluationFormCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateEvaluationFormCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
        {
            return Result<string>.Failure(401, "Tenant ID is not set.");
        }

        var evaluationForm = new EvaluationForm(request.Name,request.EvaluationStageId, tenantId.Value,request.Description);
        evaluationFormRepository.Add(evaluationForm);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(evaluationForm.Id.ToString());
    }
}