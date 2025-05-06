using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Companies;

namespace BasvuruSistemi.Server.Domain.Tenants;
public sealed class Tenant : Entity
{
    public string Name { get; private set; } = default!;
    public string? Code { get; private set; }

    public ICollection<Company> Companies { get; set; } = new List<Company>();

    private Tenant() { }

    public Tenant(string name, string? code)
    {
        Name = name;
        Code = code;
    }
}
