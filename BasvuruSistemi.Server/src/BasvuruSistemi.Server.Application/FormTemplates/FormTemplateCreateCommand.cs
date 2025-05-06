using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record FormTemplateCreateCommand(
    string name,
    string? description
    ) : IRequest<Result<string>>;

internal sealed class FormTemplateCreateCommandHandler(
    IFormTemplateRepository formTemplateRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<FormTemplateCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(FormTemplateCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        ApplicationFormTemplate applicationFormTemplate = new(tenantId.Value,request.name, request.description);

        formTemplateRepository.Add(applicationFormTemplate);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Template created successfully");
    }
}
