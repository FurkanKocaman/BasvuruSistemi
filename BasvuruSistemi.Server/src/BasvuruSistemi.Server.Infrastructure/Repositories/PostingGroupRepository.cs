using BasvuruSistemi.Server.Domain.PostingGroups;
using BasvuruSistemi.Server.Infrastructure.Context;
using GenericRepository;

namespace BasvuruSistemi.Server.Infrastructure.Repositories;
internal sealed class PostingGroupRepository : Repository<PostingGroup, ApplicationDbContext>, IPostingGroupRepository
{
    public PostingGroupRepository(ApplicationDbContext context) : base(context)
    {
    }
}
