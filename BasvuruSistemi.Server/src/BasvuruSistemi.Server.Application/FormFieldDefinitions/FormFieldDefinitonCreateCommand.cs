using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormFieldDefinitions;
public sealed record FormFieldDefinitonCreateCommand(
    Guid templateId,
    string label,
    int type,
    bool isRequired,
    string? description = null,
    string? placeholder = null,
    string? optionsJson = null,
    bool isReadOnly = false,
    string? defaultValue = null,
    int verificationSource = 0,
    string? verificationParametersJson = null,
    string? allowedFileTypes = null,
    int? maxFileSizeMB = null
    ) : IRequest<Result<string>>;

internal sealed class FormFieldDefinitonCreateCommandHandler(
    IFormTemplateRepository formTemplateRepository,
    IFormFieldDefinitionRepository formFieldDefinitionRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<FormFieldDefinitonCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(FormFieldDefinitonCreateCommand request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<string>.Failure("Tenant not found");

        var template = await formTemplateRepository.Where(p => p.Id == request.templateId).Include(p => p.FieldDefinitions).FirstOrDefaultAsync();

        if (template is null)
            return Result<string>.Failure("Template not found");

        if (!System.Enum.IsDefined(typeof(FieldTypeEnum), request.type))
        {
            return Result<string>.Failure($"Invalid FielType: {request.type}.");
        }
        FieldTypeEnum fieldType = (FieldTypeEnum)request.type;

        if (!System.Enum.IsDefined(typeof(VerificationSourceEnum), request.verificationSource))
        {
            return Result<string>.Failure($"Invalid VerificationSource: {request.type}.");
        }
        VerificationSourceEnum verificationSource= (VerificationSourceEnum)request.verificationSource;

        int order = template.FieldDefinitions.Count + 1;

        FormFieldDefinition formFieldDefinition = new(request.templateId,request.label,fieldType,request.isRequired,order,request.description,request.placeholder,request.optionsJson,request.isReadOnly,request.defaultValue,verificationSource,request.verificationParametersJson,request.allowedFileTypes,request.maxFileSizeMB);

        formFieldDefinitionRepository.Add(formFieldDefinition);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Form field created");

    }
}
