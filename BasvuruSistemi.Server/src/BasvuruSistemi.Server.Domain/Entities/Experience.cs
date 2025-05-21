using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Experience
{
    public Guid Id { get; set; }

    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public string CompanyName { get; private set; } = default!;
    public string Position { get; private set; } = default!;
    public string? Location { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? Description { get; private set; }

    private Experience() { }
    public Experience(Guid userId, string company, string position, DateOnly start, DateOnly? end = null, string? description = null, string? location = null)
    {
        if (end.HasValue && start > end.Value)
            throw new ArgumentException("StartDate must be before EndDate.");
        Id = Guid.CreateVersion7();
        UserId = userId;
        CompanyName = company;
        Position = position;
        StartDate = start;
        EndDate = end;
        Description = description;
        Location = location;
    }
    public void Update(string company, string position, DateOnly start, DateOnly? end = null, string? description = null, string? location = null)
    {
        if (end.HasValue && start > end.Value)
            throw new ArgumentException("StartDate must be before EndDate.");
        CompanyName = company;
        Position = position;
        StartDate = start;
        EndDate = end;
        Description = description;
        Location = location;
    }
}
