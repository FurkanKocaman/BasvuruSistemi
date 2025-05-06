using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Companies;
using BasvuruSistemi.Server.Domain.Departments;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Departments;
public sealed record DepartmentCreateCommand(
    string name,
    string? description,
    string? code,
    Guid companyId
    ) : IRequest<Result<string>>;

internal sealed class DepartmentCreateCommandHandler(
    ICurrentUserService currentUserService,
    IDepartmentRepository departmentRepository,
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<DepartmentCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DepartmentCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        var isCompanyExist = await companyRepository.AnyAsync(p => p.Id == request.companyId && !p.IsDeleted);

        if (!isCompanyExist)
            return Result<string>.Failure("Company nor found");

        Department department = new(request.name, request.description, request.code, tenantId.Value, request.companyId);

        departmentRepository.Add(department);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Department created successfully");
    }
}
