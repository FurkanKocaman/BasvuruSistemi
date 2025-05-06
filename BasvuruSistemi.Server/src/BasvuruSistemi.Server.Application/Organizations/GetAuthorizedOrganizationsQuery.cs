using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Companies;
using BasvuruSistemi.Server.Domain.Constants;
using BasvuruSistemi.Server.Domain.Departments;
using BasvuruSistemi.Server.Domain.UserRoles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.Organizations;
public sealed record GetAuthorizedOrganizationsQuery(
    ) : IRequest<List<GetAuthorizedOrganizationsQueryResponse>>;

public sealed class GetAuthorizedOrganizationsQueryResponse
{
    public string Type { get; set; } = default!;
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid? CompanyId { get; set; }
}

internal sealed class GetAuthorizedOrganizationsQueryHandler(
    ICurrentUserService currentUserService,
    IUserTenantRoleRepository userTenantRoleRepository,
    ICompanyRepository companyRepository,
    IDepartmentRepository departmanRepository
    ) : IRequestHandler<GetAuthorizedOrganizationsQuery, List<GetAuthorizedOrganizationsQueryResponse>>
{
    public Task<List<GetAuthorizedOrganizationsQueryResponse>> Handle(GetAuthorizedOrganizationsQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        Guid? tenantId = currentUserService.TenantId;

        if(!userId.HasValue || !tenantId.HasValue)
            return Task.FromResult(new List<GetAuthorizedOrganizationsQueryResponse>());

        var userTenantRoles = userTenantRoleRepository.Where(p => p.TenantId == tenantId).Include(p => p.Company).Include(p => p.Department).Include(p => p.Role).ToList();

        if (userTenantRoles is null || !userTenantRoles.Any())
            return Task.FromResult(new List<GetAuthorizedOrganizationsQueryResponse>());

        var allCompanies = companyRepository.Where(p => !p.IsDeleted && p.TenantId == tenantId);
        var allDepartmans = departmanRepository.Where(p => !p.IsDeleted && p.TenantId == tenantId);

        List<GetAuthorizedOrganizationsQueryResponse> response = new List<GetAuthorizedOrganizationsQueryResponse>();

        if(userTenantRoles.Any(p => p.Role.Name == Roles.Admin.Name || p.Role.Name == Roles.TenantManager.Name))
        {
            response = allCompanies.Select(c => new GetAuthorizedOrganizationsQueryResponse
            {
                Type = "Company",
                Id = c.Id,
                Name = c.Name,
                CompanyId = null,
            }).Concat(allDepartmans.Select(d => new GetAuthorizedOrganizationsQueryResponse
            {
                Type = "Department",
                Id = d.Id,
                Name = d.Name,
                CompanyId = d.CompanyId,
            })).ToList();
        }else if(userTenantRoles.Any(p => p.Role.Name == Roles.CompanyManager.Name))
        {
            var companies = allCompanies.Where(p => userTenantRoles.Any(u => u.CompanyId == p.Id && u.Role.Name == Roles.CompanyManager.Name)).ToList();
            var departments = allDepartmans.Where(p => companies.Any(c => c.Id == p.CompanyId));

            response = companies.Select(c => new GetAuthorizedOrganizationsQueryResponse
            {
                Type = "Company",
                Id = c.Id,
                Name = c.Name,
            }).Concat(departments.Select(d => new GetAuthorizedOrganizationsQueryResponse
            {
                Type = "Department",
                Id = d.Id,
                Name = d.Name,
                CompanyId = d.CompanyId,
            })).ToList();

        }else if(userTenantRoles.Any(p => p.Role.Name == Roles.DepartmentManager.Name))
        {
            var departments = allDepartmans.Where(p => userTenantRoles.Any(u => u.DepartmentId == p.Id && u.Role.Name == Roles.DepartmentManager.Name));

            response = departments.Select(d => new GetAuthorizedOrganizationsQueryResponse
            {
                Type = "Department",
                Id = d.Id,
                Name = d.Name,
                CompanyId = d.CompanyId,
            }).ToList();
        }


        return Task.FromResult(response);

    }
}

