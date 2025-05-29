using BasvuruSistemi.Server.Domain.Comissions;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Comissions;
public sealed record DeleteCommissionCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class DeleteCommissionCommandHandler(
    IApprovalCommissionRepository approvalCommissionRepository,
    IJobPostingEvaluationPipelineStageRepository jobPostingEvaluationPipelineStageRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DeleteCommissionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteCommissionCommand request, CancellationToken cancellationToken)
    {
        var isPipelineExist = await jobPostingEvaluationPipelineStageRepository.AnyAsync(p => p.ResponsibleCommissionId == request.Id && !p.IsDeleted);

        var commission = await approvalCommissionRepository.FirstOrDefaultAsync(p => p.Id == request.Id);

        if (commission is null)
            return Result<string>.Failure(404, "Komisyon bulunamamdı");

        if (isPipelineExist)
            return Result<string>.Failure(400,"Bu komisyonun mevcut bir ilana ataması olduğu için silemezsiniz önce ilanı silmelisiniz");
                
        commission.IsDeleted = true;
        approvalCommissionRepository.Update( commission );
        await unitOfWork.SaveChangesAsync( cancellationToken );

        return Result<string>.Succeed("Komisyon başarıyla silindi");
    }
}
