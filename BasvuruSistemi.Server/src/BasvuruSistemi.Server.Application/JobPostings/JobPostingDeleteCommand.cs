using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record JobPostingDeleteCommand(
    Guid Id
    ) : IRequest<Result<string>>;

internal sealed class JobPostingDeleteCommandhandler(
    IJobPostingRepository jobPostingRepository,
    IApplicationRepository applicationRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IAuthorizationService authorizationService,
    IJobScheduler jobScheduler
    ) : IRequestHandler<JobPostingDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(JobPostingDeleteCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if (!userId.HasValue)
            return Result<string>.Failure(404, "User not found");

        Guid? tenantId = currentUserService.TenantId;
        if (!tenantId.HasValue)
            return Result<string>.Failure(404, "Tenant not found");

        var jobPosting = await jobPostingRepository.Where(p => p.Id == request.Id && !p.IsDeleted).Include(p => p.Applications).Include(p => p.Unit).FirstOrDefaultAsync();
        if(jobPosting is null)
            return Result<string>.Failure(404, "Job posting not found");

        if((jobPosting.UnitId != null && await authorizationService.IsUnitManagerAsync(userId.Value, tenantId.Value, jobPosting.UnitId.Value, cancellationToken)) || (await authorizationService.IsTenantManagerAsync(userId.Value, tenantId.Value, cancellationToken)))
        {
            jobPosting.IsDeleted = true;
            jobPostingRepository.Update(jobPosting);

            foreach(var application in jobPosting.Applications)
            {
                application.SetStatus(Domain.Enums.ApplicationStatus.JobPostingClosed);
                applicationRepository.Update(application);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Job posting deleted");
        }
        if(jobPosting.HangfirePublishJobId != null)
            await jobScheduler.CancelScheduleAsync(jobPosting.HangfirePublishJobId);
        if (jobPosting.HangfireCloseJobId != null)
            await jobScheduler.CancelScheduleAsync(jobPosting.HangfireCloseJobId);

        await jobScheduler.CancelScheduleInReviewApplications(jobPosting.Id);

        return Result<string>.Failure("You dont have permission to delete this jobPosting");
    }
}
