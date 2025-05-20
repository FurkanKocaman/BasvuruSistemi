using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Skill
{
    public Guid Id { get; set; }

    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public SkillLevel Level { get; private set; }

    private Skill() { }
    public Skill(Guid userId, string name, string? description, SkillLevel level)
    {
        Id = Guid.CreateVersion7();
        UserId = userId;
        Name = name;
        Description = description;
        Level = level;
    }
    public void Update(string name, string? description, SkillLevel level)
    {
        Name = name;
        Description = description;
        Level = level;
    }
}
