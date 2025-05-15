using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.FormFieldDefinitions;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record FormTemplateCreateTempCommand(
    List<FormFieldDefinitionDto> fields
    ) : IRequest<Result<string>>;

internal sealed class FormTemplateCreateTempCommandHandler(
    IFormTemplateRepository formTemplateRepository,
    ICurrentUserService currentUserService,
    IFormFieldDefinitionRepository formFieldDefinitionRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<FormTemplateCreateTempCommand, Result<string>>
{
    public async Task<Result<string>> Handle(FormTemplateCreateTempCommand request, CancellationToken cancellationToken)
    {
        using (var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? tenantId = currentUserService.TenantId;

                if (!tenantId.HasValue)
                    return Result<string>.Failure("Tenant not found");

                ApplicationFormTemplate applicationFormTemplate = new(tenantId.Value, Guid.CreateVersion7().ToString()+"TempFormTemplate",null, false);

                formTemplateRepository.Add(applicationFormTemplate);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                int startingOrder = applicationFormTemplate.FieldDefinitions.Count + 1;

                foreach (var (field, index) in request.fields.Select((x, i) => (x, i)))
                {
                    if (!Enum.IsDefined(typeof(FieldTypeEnum), field.type))
                        return Result<string>.Failure($"Invalid FieldType: {field.type}");

                    var formFieldDefinition = new FormFieldDefinition(
                        applicationFormTemplate.Id,
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

                return Result<string>.Succeed("Template created successfully");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("An error occurred while creating the template :" + ex);
            }
        }
    }
}
