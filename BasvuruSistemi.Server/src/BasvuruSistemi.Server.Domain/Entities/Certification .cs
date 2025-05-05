using BasvuruSistemi.Server.Domain.Candidates;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Certification
{
    public Guid Id { get; set; }

    public Guid CandidateId { get; private set; }
    public Candidate Candidate { get; private set; } = default!;

    public string Title { get; private set; } = default!;
    public string Issuer { get; private set; }= default!;
    public DateOnly DateReceived { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }

    private Certification() { }
    public Certification(Guid candidateId, string title, string issuer, DateOnly received, DateOnly? expiry = null)
    {
        Id = Guid.CreateVersion7();
        CandidateId = candidateId;
        Title = title;
        Issuer = issuer;
        DateReceived = received;
        ExpiryDate = expiry;
    }
}
