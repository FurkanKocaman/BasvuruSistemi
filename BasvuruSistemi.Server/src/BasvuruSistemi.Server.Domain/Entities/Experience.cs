using BasvuruSistemi.Server.Domain.Candidates;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Experience
{
    public Guid Id { get; set; }

    public Guid CandidateId { get; private set; }
    public Candidate Candidate { get; private set; } = default!;

    public string CompanyName { get; private set; } = default!;
    public string Position { get; private set; } = default!;
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? Responsibilities { get; private set; }

    private Experience() { }
    public Experience(Guid candidateId, string company, string position, DateOnly start, DateOnly? end = null, string? resp = null)
    {
        if (end.HasValue && start > end.Value)
            throw new ArgumentException("StartDate must be before EndDate.");
        Id = Guid.CreateVersion7();
        CandidateId = candidateId;
        CompanyName = company;
        Position = position;
        StartDate = start;
        EndDate = end;
        Responsibilities = resp;
    }
}
