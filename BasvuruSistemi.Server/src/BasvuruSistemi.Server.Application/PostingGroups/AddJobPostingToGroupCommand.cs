using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.PostingGroups;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.PostingGroups;
public sealed record AddJobPostingToGroupCommand(
    Guid jobPostingId,
    Guid postingGroupId
    ) : IRequest<Result<string>>;

internal sealed class AddJobPostingToGroupCommandHandler(
    IJobPostingRepository jobPostingRepository,
    IPostingGroupRepository postingGroupRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<AddJobPostingToGroupCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AddJobPostingToGroupCommand request, CancellationToken cancellationToken)
    {
        var jobPosting = await jobPostingRepository.Where(p => p.Id == request.jobPostingId && !p.IsDeleted).FirstOrDefaultAsync();

        if (jobPosting is null)
        {
            return Result<string>.Failure(404,"Job posting not found or already deleted");
        }
            

        var postingGroup = await postingGroupRepository.Where(p => p.Id == request.postingGroupId && !p.IsDeleted).FirstOrDefaultAsync();
        if (postingGroup is null)
            return Result<string>.Failure(404,"Posting group not found or already deleted");

        jobPosting.JoinGroup(postingGroup.Id);

        jobPostingRepository.Update(jobPosting);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Job posting added to group successfully");
    }
}
