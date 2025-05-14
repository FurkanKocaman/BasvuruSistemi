using BasvuruSistemi.Server.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;

internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(i => i.UserName).IsUnique();
        builder.Property(P => P.UserName).HasColumnType("varchar(20)");
        builder.Property(P => P.Email).HasColumnType("varchar(MAX)");

        builder
            .HasOne(p => p.Address)
            .WithOne()
            .HasForeignKey<AppUser>(p => p.AddressId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.OwnsOne(p => p.Contact);
    }
}
