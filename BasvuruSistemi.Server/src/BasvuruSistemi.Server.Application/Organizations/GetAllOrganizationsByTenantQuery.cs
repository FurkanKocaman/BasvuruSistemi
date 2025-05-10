using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Companies;
using BasvuruSistemi.Server.Domain.Departments;
using MediatR;

namespace BasvuruSistemi.Server.Application.Organizations;
public sealed record GetAllOrganizationsByTenantQuery(
    ) : IRequest<List<GetAllOrganizationsByTenantQueryResponse>>;

public sealed class GetAllOrganizationsByTenantQueryResponse
{
    public Guid CompanyId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string Name { get; set; } = default!;
    
}

internal sealed class GetAllOrganizationsByTenantQueryHandler(
    ICurrentUserService currentUserService,
    ICompanyRepository companyRepository,
    IDepartmentRepository departmentRepository
    ) : IRequestHandler<GetAllOrganizationsByTenantQuery, List<GetAllOrganizationsByTenantQueryResponse>>
{
    public Task<List<GetAllOrganizationsByTenantQueryResponse>> Handle(GetAllOrganizationsByTenantQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
        if(!tenantId.HasValue)
            return Task.FromResult(new List<GetAllOrganizationsByTenantQueryResponse>());

        var companies = companyRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted).ToList();
        var departments = departmentRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted).ToList();

        var response = companies.Select(company => new GetAllOrganizationsByTenantQueryResponse
        {
            CompanyId = company.Id,
            DepartmentId = null,
            Name = company.Name,
        }).Concat(departments.Select(department => new GetAllOrganizationsByTenantQueryResponse
        {
            CompanyId = department.CompanyId,
            DepartmentId = department.Id,
            Name = department.Name,
        })).ToList();

        return Task.FromResult(response);

    }
}
