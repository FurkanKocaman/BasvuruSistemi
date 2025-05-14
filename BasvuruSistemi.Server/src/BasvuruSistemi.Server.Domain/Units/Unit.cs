using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.Tenants;

namespace BasvuruSistemi.Server.Domain.Units;
public sealed class Unit : Entity
{
    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; set; } = default!;

    public string Name { get; private set; } = default!;
    public string? Code { get; private set; }

    public Guid? ParentId { get; private set; } 
    public Unit? Parent { get; private set; }
    public ICollection<Unit> Children { get; private set; } = new List<Unit>();

    public ICollection<JobPosting> JobPostings { get; private set; } = new List<JobPosting>();

    private Unit() { }

    public Unit(Guid tenantId, string name, string? code, Guid? parentId)
    {
        TenantId = tenantId;
        Name = name;
        Code = code;
        ParentId = parentId;
    }
}
