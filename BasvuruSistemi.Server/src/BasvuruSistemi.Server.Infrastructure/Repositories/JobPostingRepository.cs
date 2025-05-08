using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class JobPostingRepository : Repository<JobPosting, ApplicationDbContext>, IJobPostingRepository
{
    public JobPostingRepository(ApplicationDbContext context) : base(context)
    {
    }
}
