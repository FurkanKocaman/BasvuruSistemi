using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record ChangeJobPostingStatusCommand(
    Guid jobPostingId,
    int status,
    DateTimeOffset? publishStartDate
    ) : IRequest<Result<string>>;

internal sealed class ChangeJobPostingStatusCommandHandler(
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork,
    IJobScheduler jobScheduler
    ) : IRequestHandler<ChangeJobPostingStatusCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ChangeJobPostingStatusCommand request, CancellationToken cancellationToken)
    {
        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == request.jobPostingId,cancellationToken);

        if (jobPosting is null)
            return Result<string>.Failure("Job posting not found");

        if (!System.Enum.IsDefined(typeof(JobPostingStatus), request.status))
        {
            return Result<string>.Failure($"Invalid JobPostingStatus: {request.status}.");
        }
        JobPostingStatus status = (JobPostingStatus)request.status;

        switch (status)
        {
            case JobPostingStatus.Draft:
                jobPosting.Draft();
                break;
            case JobPostingStatus.Published:
                jobPosting.Publish();
                if(jobPosting.HangfirePublishJobId is not null)
                {
                    await jobScheduler.CancelScheduleAsync(jobPosting.HangfirePublishJobId);
                }
                break;
            case JobPostingStatus.OnHold:
                jobPosting.PutOnHold();
                break;
            case JobPostingStatus.Archived:
                jobPosting.Archive();
                break;
            default:
                throw new ArgumentException("Invalid status transition.");
        }

        jobPostingRepository.Update(jobPosting);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Job post "+ request.status.ToString());
    }
}
