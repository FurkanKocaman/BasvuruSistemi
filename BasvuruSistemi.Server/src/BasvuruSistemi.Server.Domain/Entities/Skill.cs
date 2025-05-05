using BasvuruSistemi.Server.Domain.Candidates;
using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Skill
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; private set; }
    public Candidate Candidate { get; private set; } = default!;

    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public SkillLevel Level { get; private set; }

    private Skill() { }
    public Skill(Guid candidateId, string name, string? description, SkillLevel level)
    {
        Id = Guid.CreateVersion7();
        CandidateId = candidateId;
        Name = name;
        Description = description;
        Level = level;
    }
}
