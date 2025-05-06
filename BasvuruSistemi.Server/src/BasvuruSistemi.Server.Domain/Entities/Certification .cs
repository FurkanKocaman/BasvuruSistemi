using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Entities;
public sealed class Certification
{
    public Guid Id { get; set; }

    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public string Title { get; private set; } = default!;
    public string Issuer { get; private set; }= default!;
    public DateOnly DateReceived { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }

    private Certification() { }
    public Certification(Guid userId, string title, string issuer, DateOnly received, DateOnly? expiry = null)
    {
        Id = Guid.CreateVersion7();
        UserId = userId;
        Title = title;
        Issuer = issuer;
        DateReceived = received;
        ExpiryDate = expiry;
    }
}
