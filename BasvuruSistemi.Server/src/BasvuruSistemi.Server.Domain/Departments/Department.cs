using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Companies;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.Tenants;

namespace BasvuruSistemi.Server.Domain.Departments;
public sealed class Department : Entity
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public string? Code { get; private set; }

    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; } = default!;

    public Guid CompanyId { get; private set; } = default!;
    public Company Company { get; private set; } = default!;

    public ICollection<JobPosting> JobPostings { get; private set; } = new List<JobPosting>();

    private Department() { }

    public Department(string name, string? description, string? code, Guid tenantId, Guid companyId)
    {
        Name = name;
        Description = description;
        Code = code;
        TenantId = tenantId;
        CompanyId = companyId;
    }
}
