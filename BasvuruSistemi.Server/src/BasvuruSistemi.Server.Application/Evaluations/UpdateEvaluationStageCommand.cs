using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Evaluations;
public sealed record UpdateEvaluationStageCommand(
    Guid Id,
    string Name, 
    int Order,
    string? Description = null,
    Guid? DefaultEvaluationFormId = null
    ) : IRequest<Result<string>>;

internal sealed class UpdateEvaluationStageCommandHandler(
    IEvaluationStageRepository evaluationStageRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<UpdateEvaluationStageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateEvaluationStageCommand request, CancellationToken cancellationToken)
    {
        var evaluationStage = await evaluationStageRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if(evaluationStage is null)
            return Result<string>.Failure(404,"Değerlendirme aşaması bulunamadı.");

        evaluationStage.Update(request.Name, request.Order, request.Description, request.DefaultEvaluationFormId);

        evaluationStageRepository.Update(evaluationStage);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Değerlendirme aşaması başarıyla güncellendi.");
    }
}
