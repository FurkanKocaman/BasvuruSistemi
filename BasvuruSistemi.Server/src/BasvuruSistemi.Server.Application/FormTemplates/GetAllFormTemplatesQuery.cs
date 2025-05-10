using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record GetAllFormTemplatesQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetFormTemplateQueryResponse>>;

internal sealed class GetAllFormTemplatesQueryHandler(
    ICurrentUserService currentUserService,
    IFormTemplateRepository formTemplateRepository,
    UserManager<AppUser> userManager
    ) : IRequestHandler<GetAllFormTemplatesQuery, PagedResult<GetFormTemplateQueryResponse>>
{
    public Task<PagedResult<GetFormTemplateQueryResponse>> Handle(GetAllFormTemplatesQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Task.FromResult(new PagedResult<GetFormTemplateQueryResponse>(new List<GetFormTemplateQueryResponse>(),0,0,0));

        var templates = formTemplateRepository.Where(p => p.TenantId == tenantId && !p.IsDeleted).Include(p => p.FieldDefinitions);

        var totalCount = templates.Count();

        var pagedTemplates = templates
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToList();

        var response = pagedTemplates.Select(p => new GetFormTemplateQueryResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Fields = p.FieldDefinitions.Select(field => new FormFieldResponse
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
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt,
            CreateUserId = p.CreateUserId,
            CreateUserName = userManager.Users.Where(u => u.Id == p.CreateUserId).FirstOrDefault() != null ? userManager.Users.Where(u => u.Id == p.CreateUserId).FirstOrDefault()!.FullName : "null",
            UpdateAt = p.UpdateAt,
            UpdateUserId = p.UpdateUserId,
            UpdateUserName = userManager.Users.Where(u => u.Id == p.UpdateUserId).FirstOrDefault()?.FullName,
            IsDeleted = p.IsDeleted,
            DeleteAt = p.DeleteAt,
        });

        return Task.FromResult(new PagedResult<GetFormTemplateQueryResponse>(response, request.page, request.pageSize, totalCount));
    

}
}
