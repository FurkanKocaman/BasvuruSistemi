using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record ChangeJobPostingStatusCommand(
    Guid jobPostingId,
    JobPostingStatus status,
    DateTimeOffset? publishStartDate
    ) : IRequest<Result<string>>;

internal sealed class ChangeJobPostingStatusCommandHandler(
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ChangeJobPostingStatusCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ChangeJobPostingStatusCommand request, CancellationToken cancellationToken)
    {
        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == request.jobPostingId,cancellationToken);

        if (jobPosting is null)
            return Result<string>.Failure("Job posting not found");

        switch (request.status)
        {
            case JobPostingStatus.Draft:
                jobPosting.Draft();
                break;
            case JobPostingStatus.Published:
                jobPosting.Publish();
                break;
            case JobPostingStatus.Closed:
                jobPosting.Close();
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
