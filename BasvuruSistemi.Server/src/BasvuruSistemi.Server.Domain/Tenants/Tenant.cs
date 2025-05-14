using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Units;

namespace BasvuruSistemi.Server.Domain.Tenants;
public sealed class Tenant : Entity
{
    public string Name { get; private set; } = default!;
    public string? Code { get; private set; }

    public ICollection<Unit> Units { get; set; } = new List<Unit>();

    private Tenant() { }

    public Tenant(string name, string? code)
    {
        Name = name;
        Code = code;
    }
}
