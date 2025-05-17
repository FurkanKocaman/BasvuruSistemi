using BasvuruSistemi.Server.Domain.PostingGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class PostingGroupConfiguration : IEntityTypeConfiguration<PostingGroup>
{
    public void Configure(EntityTypeBuilder<PostingGroup> builder)
    {
        builder
            .HasOne(p => p.Tenant)
            .WithMany()
            .HasForeignKey(p => p.TenantId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.Unit)
            .WithMany()
            .HasForeignKey(p => p.UnitId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.ParentPostingGroup)
            .WithMany(p => p.ChildPostingGroups)
            .HasForeignKey(p => p.ParentPostingGroupId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
