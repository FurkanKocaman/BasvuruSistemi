using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record RepublishJobpostingCommand(
    Guid JobPostingId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate
    ) : IRequest<Result<string>>;

internal sealed class RepublishJobpostingCommandHandler(
    IJobPostingRepository jobPostingRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<RepublishJobpostingCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RepublishJobpostingCommand request, CancellationToken cancellationToken)
    {
        var jobPosting = await jobPostingRepository.FirstOrDefaultAsync(p => p.Id == request.JobPostingId);

        if (jobPosting is null)
            return Result<string>.Failure(404,"Not found");

        jobPosting.Republish(request.StartDate,request.EndDate);

        jobPostingRepository.Update(jobPosting);

        await unitOfWork.SaveChangesAsync(cancellationToken);



        return Result<string>.Succeed("Republished successfully");
    }
}
