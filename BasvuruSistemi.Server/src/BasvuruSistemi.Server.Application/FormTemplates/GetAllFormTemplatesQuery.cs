using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.FormTemplates;
public sealed record GetAllFormTemplatesQuery(
    int page,
    int pageSize
    ) : IRequest<Result<PagedResult<GetFormTemplateQueryResponse>>>;

internal sealed class GetAllFormTemplatesQueryHandler(
    ICurrentUserService currentUserService,
    IFormTemplateRepository formTemplateRepository,
    UserManager<AppUser> userManager
    ) : IRequestHandler<GetAllFormTemplatesQuery, Result<PagedResult<GetFormTemplateQueryResponse>>>
{
    public async Task<Result<PagedResult<GetFormTemplateQueryResponse>>> Handle(GetAllFormTemplatesQuery request, CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;

        if (!tenantId.HasValue)
            return Result<PagedResult<GetFormTemplateQueryResponse>>.Failure(401, "Tenant not found");

        var query = formTemplateRepository
             .Where(p => p.TenantId == tenantId.Value && !p.IsDeleted && p.IsSaved)
             .Include(p => p.FieldDefinitions);

        var totalCount = await query.CountAsync(cancellationToken);

        var pagedTemplates = await query
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToListAsync(cancellationToken);

        var userIds = pagedTemplates
           .SelectMany(p => new[] { p.CreateUserId, p.UpdateUserId })
           .Where(id => id.HasValue)
           .Select(id => id!.Value)
           .Distinct()
           .ToList();

        var userNames = await userManager.Users
           .Where(u => userIds.Contains(u.Id))
           .ToDictionaryAsync(u => u.Id, u => u.FullName, cancellationToken);

        var response = pagedTemplates.Select(p =>
        {
            userNames.TryGetValue(p.CreateUserId, out var createUserName);
            userNames.TryGetValue(p.UpdateUserId ?? Guid.Empty, out var updateUserName);

            return new GetFormTemplateQueryResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Fields = p.FieldDefinitions
                    .Where(f => !f.IsDeleted)
                    .OrderBy(f => f.Order)
                    .Select(field => new FormFieldResponse
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
                    })
                    .ToList(),
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                CreateUserId = p.CreateUserId,
                CreateUserName = createUserName ?? "Unknown",
                UpdateAt = p.UpdateAt,
                UpdateUserId = p.UpdateUserId,
                UpdateUserName = updateUserName,
                IsDeleted = p.IsDeleted,
                DeleteAt = p.DeleteAt
            };
        }).ToList();

        return Result<PagedResult<GetFormTemplateQueryResponse>>.Succeed(new PagedResult<GetFormTemplateQueryResponse>(response, request.page, request.pageSize, totalCount));
    

}
}
