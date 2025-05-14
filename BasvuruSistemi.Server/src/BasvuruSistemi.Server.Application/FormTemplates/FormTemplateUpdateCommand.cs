using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record FormTemplateUpdateCommand(
    Guid Id,
    string Name,
    string? Description,
    List<FormFieldDefinitionDto> Fields
    ) : IRequest<Result<string>>;

internal sealed class FormTemplateUpdateCommandHandler(
    IFormTemplateRepository formTemplateRepository,
    IFormFieldDefinitionRepository formFieldDefinitionRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<FormTemplateUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(FormTemplateUpdateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                var tenantId = currentUserService.TenantId;
                if (!tenantId.HasValue)
                    return Result<string>.Failure("Tenant not found");

                var formTemplate = await formTemplateRepository.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
                if (formTemplate is null)
                    return Result<string>.Failure("Form template not found");

                formTemplate.Update(request.Name, request.Description);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                var existingFields = await formFieldDefinitionRepository.WhereWithTracking(p => p.TemplateId == request.Id).ToListAsync(cancellationToken);

                var updatedFieldSignatures = request.Fields
                    .Select(f => (f.label.Trim(), (FieldTypeEnum)f.type))
                    .ToHashSet();

                foreach (var existing in existingFields)
                {
                    var signature = (existing.Label.Trim(), existing.Type);
                    if (!updatedFieldSignatures.Contains(signature))
                    {
                        existing.MarkAsDeleted();
                        formFieldDefinitionRepository.Update(existing);
                    }
                }

                var existingSignatures = existingFields
                    .Where(f => !f.IsDeleted)
                    .Select(f => (f.Label.Trim(), f.Type))
                    .ToHashSet();

                int startingOrder = 1;
                foreach (var (field, index) in request.Fields.Select((f, i) => (f, i)))
                {
                    var signature = (field.label.Trim(), (FieldTypeEnum)field.type);
                    if (!Enum.IsDefined(typeof(FieldTypeEnum), field.type))
                        return Result<string>.Failure($"Invalid FieldType: {field.type}");

                    if (existingSignatures.Contains(signature))
                        continue;

                    var formFieldDefinition = new FormFieldDefinition(
                        formTemplate.Id,
                        field.label,
                        (FieldTypeEnum)field.type,
                        field.isRequired,
                        startingOrder + index,
                        field.description,
                        field.placeholder,
                        field.optionsJson,
                        field.isReadOnly,
                        field.defaultValue,
                        field.allowedFileTypes,
                        field.maxFileSizeMB
                    );

                    formFieldDefinitionRepository.Add(formFieldDefinition);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Template updated successfully");
            }
            catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure(ex.Message);
            }
        }
    }
}
