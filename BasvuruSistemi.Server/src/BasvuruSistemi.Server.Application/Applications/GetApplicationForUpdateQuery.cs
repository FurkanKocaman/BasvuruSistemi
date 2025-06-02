using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record GetApplicationForUpdateQuery(
    Guid Id
    ) : IRequest<Result<GetApplicationForUpdateQueryResponse>>;

public sealed class GetApplicationForUpdateQueryResponse
{
    public Guid JobPostingId { get; set; }
    public int Type { get; set; }
    public List<FieldValueResponse> FieldValues { get; set; } = new List<FieldValueResponse>();
}
   
public sealed class FieldValueResponse
{
        public Guid Id { get; set; }
        public FieldTypeEnum Type { get; set; }
        public string? Value { get; set; }
    }

internal sealed class GetApplicationForUpdateQueryHandler(
        IApplicationRepository applicationRepository,
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor
        ) : IRequestHandler<GetApplicationForUpdateQuery, Result<GetApplicationForUpdateQueryResponse>>
{
    public async Task<Result<GetApplicationForUpdateQueryResponse>> Handle(GetApplicationForUpdateQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;

        if (!userId.HasValue)
            return Result<GetApplicationForUpdateQueryResponse>.Failure(403, "Unauthorized");

        var application = await applicationRepository
            .Where(p => p.Id == request.Id && p.UserId == userId.Value)
            .Include(p => p.FieldValues)
                .ThenInclude(p => p.FieldDefinition)
            .FirstOrDefaultAsync(cancellationToken);

        if (application is null)
            return Result<GetApplicationForUpdateQueryResponse>.Failure(404, "Application not found");

        var context = httpContextAccessor.HttpContext?.Request;
        string baseUrl = context is null
            ? string.Empty
            : $"{context.Scheme}://{context.Host}";

        var response = new GetApplicationForUpdateQueryResponse
        {
            JobPostingId = application.JobPostingId,
            Type = 0,
            FieldValues = application.FieldValues.Select(fieldValue=> new FieldValueResponse
            {
                Id = fieldValue.FieldDefinitionId,
                Type = fieldValue.FieldDefinition.Type,
                Value = fieldValue.FieldDefinition.Type == FieldTypeEnum.File || fieldValue.FieldDefinition.Type == FieldTypeEnum.Image || fieldValue.FieldDefinition.Type == FieldTypeEnum.EDevletVerifiedFile ? $"{baseUrl}{fieldValue.Value}" : fieldValue.Value,
            }).ToList(),
        };

        return Result<GetApplicationForUpdateQueryResponse>.Succeed(response);
    }
}

