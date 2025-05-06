using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Departments;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.Tenants;

namespace BasvuruSistemi.Server.Domain.Companies;
public sealed class Company : Entity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; } = default!;
    public string? Code { get; private set; }

    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; } = default!;

    public ICollection<Department> Departments { get; private set; } = new List<Department>();

    public ICollection<JobPosting> JobPostings { get; private set; } = new List<JobPosting>();

    private Company() { }

    public Company(Guid tenantId, string name, string? description, string? code)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        Code = code;
    }
}
