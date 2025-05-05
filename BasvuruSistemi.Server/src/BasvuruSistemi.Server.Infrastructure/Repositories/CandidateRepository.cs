using BasvuruSistemi.Server.Domain.Candidates;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class CandidateRepository : Repository<Candidate, ApplicationDbContext>, ICandidateRepository
{
    public CandidateRepository(ApplicationDbContext context) : base(context)
    {
    }
}
