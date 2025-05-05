using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;
using BasvuruSistemi.Server.Domain.ValueObjects;

namespace BasvuruSistemi.Server.Domain.Employers;
public sealed class Employer : Entity
{
    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public string CompanyName { get; private set; } = default!;
    public Address HeadOffice { get; private set; } = default!;
    public Contact Contact { get; private set; } = default!;
    public VerificationStatus Verification { get; private set; } = VerificationStatus.Pending;

    private Employer() { }
    public Employer(Guid userId, string companyName,Address headOffice, Contact contact)
    {
        UserId = userId;
        CompanyName = companyName;
        HeadOffice = headOffice;
        Contact = contact;
    }
}
