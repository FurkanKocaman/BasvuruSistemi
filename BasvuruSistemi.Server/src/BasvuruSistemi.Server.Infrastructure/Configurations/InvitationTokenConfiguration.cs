using BasvuruSistemi.Server.Domain.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class InvitationTokenConfiguration : IEntityTypeConfiguration<InvitationToken>
{
    public void Configure(EntityTypeBuilder<InvitationToken> builder)
    {
        builder
            .HasOne(p => p.Inviter)
            .WithMany()
            .HasForeignKey(p => p.InviterId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
           .HasOne(p => p.Invitee)
           .WithMany()
           .HasForeignKey(p => p.InviteeId)
           .OnDelete(DeleteBehavior.NoAction);

        builder
           .HasOne(p => p.Role)
           .WithMany()
           .HasForeignKey(p => p.RoleId)
           .OnDelete(DeleteBehavior.NoAction);

        builder
           .HasOne(p => p.Unit)
           .WithMany()
           .HasForeignKey(p => p.UnitId)
           .OnDelete(DeleteBehavior.NoAction);
    }
}
