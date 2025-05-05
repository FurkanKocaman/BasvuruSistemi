using BasvuruSistemi.Server.Domain.Candidates;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Education
{
    public Guid Id { get; set; }

    public Guid CandidateId { get; private set; }
    public Candidate Candidate { get; private set; } = default!;
    public string Institution { get; private set; } = default!;
    public string Department { get; private set; } = default!;
    public DateOnly StartDate { get; private set; }
    public DateOnly GraduationDate { get; private set; }
    public double? GPA { get; private set; }

    private Education() { }
    public Education(Guid candidateId, string institution, string department, DateOnly start, DateOnly grad, double? gpa = null)
    {
        if (start > grad) throw new ArgumentException("StartDate must be before GraduationDate.");
        Id = Guid.CreateVersion7();
        CandidateId = candidateId;
        Institution = institution;
        Department = department;
        StartDate = start;
        GraduationDate = grad;
        GPA = gpa;
    }
}
