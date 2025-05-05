using BasvuruSistemi.Server.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasvuruSistemi.Server.Infrastructure.Configurations;
internal sealed class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder
            .HasOne(p => p.Candidate)
            .WithMany(p => p.SkillSet)
            .HasForeignKey(p => p.CandidateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
