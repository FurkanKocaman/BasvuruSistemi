using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormFieldDefinitions;
public sealed record FormFieldDefinitonBulkCreateCommand(
    Guid templateId,
    List<FormFieldDefinitionDto> fields
) : IRequest<Result<string>>;

internal sealed class FormFieldDefinitonBulkCreateCommandHandler(
    IFormTemplateRepository formTemplateRepository,
    IFormFieldDefinitionRepository formFieldDefinitionRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<FormFieldDefinitonBulkCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(FormFieldDefinitonBulkCreateCommand request, CancellationToken cancellationToken)
    {
        var tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        var template = await formTemplateRepository
            .Where(p => p.Id == request.templateId)
            .Include(p => p.FieldDefinitions)
            .FirstOrDefaultAsync(cancellationToken);

        if (template is null)
            return Result<string>.Failure("Template not found");

        int startingOrder = template.FieldDefinitions.Count + 1;

        foreach (var (field, index) in request.fields.Select((x, i) => (x, i)))
        {
            if (!Enum.IsDefined(typeof(FieldTypeEnum), field.type))
                return Result<string>.Failure($"Invalid FieldType: {field.type}");

            if (!Enum.IsDefined(typeof(VerificationSourceEnum), field.verificationSource))
                return Result<string>.Failure($"Invalid VerificationSource: {field.verificationSource}");

            var formFieldDefinition = new FormFieldDefinition(
                request.templateId,
                field.label,
                (FieldTypeEnum)field.type,
                field.isRequired,
                startingOrder + index,
                field.description,
                field.placeholder,
                field.optionsJson,
                field.isReadOnly,
                field.defaultValue,
                (VerificationSourceEnum)field.verificationSource,
                field.verificationParametersJson,
                field.allowedFileTypes,
                field.maxFileSizeMB
            );

            formFieldDefinitionRepository.Add(formFieldDefinition);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Form fields created successfully");
    }
}