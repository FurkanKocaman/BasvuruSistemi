using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Education
{
    public Guid Id { get; set; }

    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public string Institution { get; private set; } = default!;
    public string Department { get; private set; } = default!;
    public string? Degree { get; private set; }
    public string? Description { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? GraduationDate { get; private set; }
    public double? GPA { get; private set; }

    private Education() { }
    public Education(Guid userId, string institution, string department,  DateOnly start, DateOnly? grad, double? gpa = null, string? degree = null, string? description = null)
    {
        if (start > grad) throw new ArgumentException("StartDate must be before GraduationDate.");
        Id = Guid.CreateVersion7();
        UserId = userId;
        Institution = institution;
        Department = department;
        StartDate = start;
        GraduationDate = grad;
        GPA = gpa;
        Degree = degree;
        Description = description;
    }

    public void Update(string institution, string department, DateOnly start, DateOnly? grad, double? gpa = null, string? degree = null, string? description = null)
    {
        if (start > grad) throw new ArgumentException("StartDate must be before GraduationDate.");
        Institution = institution;
        Department = department;
        StartDate = start;
        GraduationDate = grad;
        GPA = gpa;
        Degree = degree;
        Description = description;
    }
}
