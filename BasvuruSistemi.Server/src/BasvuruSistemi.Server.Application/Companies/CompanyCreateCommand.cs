using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Companies;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Companies;
public sealed record CompanyCreateCommand(
    string name,
    string? description,
    string? code
    ) : IRequest<Result<string>>;

internal sealed class CompanyCreateCommandHandler(
    ICurrentUserService currentUserService,
    ICompanyRepository companyRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<CompanyCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CompanyCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        Company company = new(tenantId.Value, request.name, request.description, request.code);

        companyRepository.Add(company);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Company created");
    }
}
