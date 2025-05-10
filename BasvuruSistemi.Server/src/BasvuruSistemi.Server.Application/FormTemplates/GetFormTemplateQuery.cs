using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record GetFormTemplateQuery(
    Guid id
    ) : IRequest<GetFormTemplateQueryResponse>;

public sealed class GetFormTemplateQueryResponse : EntityDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<FormFieldResponse> Fields { get; set; } = new();
}
public sealed class FormFieldResponse
{
    public Guid Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public int Type { get; set; }
    public bool IsRequired { get; set; }
    public int Order { get; set; }
    public string? Description { get; set; }
    public string? Placeholder { get; set; }
    public string? OptionsJson { get; set; }

    public bool IsReadOnly { get; set; }
    public string? DefaultValue { get; set; }

    public int VerificationSource { get; set; }
    public string? VerificationParametersJson { get; set; }

    public string? AllowedFileTypes { get;  set; }
    public int? MaxFileSizeMB { get;  set; }
}


internal sealed class GetFormTemplateQueryHandler(
    IFormTemplateRepository formTemplateRepository
    ) : IRequestHandler<GetFormTemplateQuery, GetFormTemplateQueryResponse>
{
    public Task<GetFormTemplateQueryResponse> Handle(GetFormTemplateQuery request, CancellationToken cancellationToken)
    {
        var formTemplate = formTemplateRepository.Where(p => p.Id == request.id && !p.IsDeleted).Include(p => p.FieldDefinitions).FirstOrDefault();

        if(formTemplate is null)
            return Task.FromResult(new GetFormTemplateQueryResponse());

        var response = new GetFormTemplateQueryResponse
        {
            Id = formTemplate.Id,
            Name = formTemplate.Name,
            Description = formTemplate.Description,

            Fields = formTemplate.FieldDefinitions.Select(field => new FormFieldResponse
            {
                Id = field.Id,
                Label = field.Label,
                Type = (int)field.Type,
                IsRequired = field.IsRequired,
                Order = field.Order,
                Description = field.Description,
                Placeholder = field.Placeholder,
                OptionsJson = field.OptionsJson,

                IsReadOnly = field.IsReadOnly,
                DefaultValue = field.DefaultValue,

                VerificationSource = (int)field.VerificationSource,
                VerificationParametersJson = field.VerificationParametersJson,

                AllowedFileTypes = field.AllowedFileTypes,
                MaxFileSizeMB = field.MaxFileSizeMB,
            }).OrderBy(p => p.Order).ToList(),
            IsActive = formTemplate.IsActive,
            CreatedAt = formTemplate.CreatedAt,
            CreateUserId = formTemplate.CreateUserId,
            CreateUserName = "",
            UpdateAt = formTemplate.UpdateAt,
            UpdateUserId = formTemplate.UpdateUserId,
            UpdateUserName = null,
            IsDeleted = formTemplate.IsDeleted,
            DeleteAt = formTemplate.DeleteAt,
        };

        return Task.FromResult(response);
    }
}
