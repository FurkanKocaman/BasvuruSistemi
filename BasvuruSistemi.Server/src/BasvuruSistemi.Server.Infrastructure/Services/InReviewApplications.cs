using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Infrastructure.Services;
internal sealed class InReviewApplications(
    IJobPostingRepository jobPostingRepository,
    IApplicationRepository applicationRepository,
    IUnitOfWork unitOfWork
    ) : IInReviewApplications
{
    public async Task Execute(Guid jobPostingId)
    {
        var jobPosting = await jobPostingRepository.WhereWithTracking(p => p.Id == jobPostingId).Include(p => p.Applications).FirstOrDefaultAsync();
        if (jobPosting is null)
            return;

        foreach(var application  in jobPosting.Applications)
        {
            application.SetStatus(Domain.Enums.ApplicationStatus.InReview);
            applicationRepository.Update(application);
        }

        await unitOfWork.SaveChangesAsync();
    }
}
