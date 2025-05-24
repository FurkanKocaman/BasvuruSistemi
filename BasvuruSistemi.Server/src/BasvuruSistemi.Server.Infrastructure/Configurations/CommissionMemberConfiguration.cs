using BasvuruSistemi.Server.Domain.Comissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class CommissionMemberConfiguration : IEntityTypeConfiguration<CommissionMember>
{
    public void Configure(EntityTypeBuilder<CommissionMember> builder)
    {
       builder
            .HasOne(cm => cm.Commission)
            .WithMany(c => c.CommissionMembers)
            .HasForeignKey(cm => cm.CommissionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
