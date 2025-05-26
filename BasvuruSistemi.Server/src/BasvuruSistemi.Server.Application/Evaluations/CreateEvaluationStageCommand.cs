using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Evaluations;
public sealed record CreateEvaluationStageCommand(
    string Name,
    int Order,
    string? Description = null
    ) : IRequest<Result<string>>;

internal sealed class CreateEvaluationStageCommandHandler(
    IEvaluationStageRepository evaluationStageRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService
    ) : IRequestHandler<CreateEvaluationStageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateEvaluationStageCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
        {
            return Result<string>.Failure(401, "Tenant ID is not set.");
        }

        var evaluationStage = new EvaluationStage(
            request.Name,
            request.Order,
            tenantId.Value,
            request.Description
        );

        evaluationStageRepository.Add(evaluationStage);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<string>.Succeed(evaluationStage.Id.ToString());

    }
}
