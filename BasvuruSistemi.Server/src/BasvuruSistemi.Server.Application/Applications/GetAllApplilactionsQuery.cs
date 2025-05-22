using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record GetAllApplilactionsQuery(
    Guid? jobPostingId,
    int page,
    int pageSize
    ) : IRequest<Result<PagedResult<GetAllApplilactionsQueryResponse>>>;

public sealed class GetAllApplilactionsQueryResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = default!;
    public string? TCKN { get; set; }

    public string JobPosting { get; set; } = default!;
    public DateTimeOffset AppliedDate { get; set; }
    public ApplicationStatus Status { get; set; } = default!;
    public DateTimeOffset? ReviewDate { get; set; }
    public string? ReviewDescription { get; set; }

}
internal sealed class GetAllApplilactionsQueryHandler(
    IApplicationRepository applicationRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<GetAllApplilactionsQuery, Result<PagedResult<GetAllApplilactionsQueryResponse>>>
{
    public async Task<Result<PagedResult<GetAllApplilactionsQueryResponse>>> Handle(
        GetAllApplilactionsQuery request,
        CancellationToken cancellationToken)
    {
        Guid? tenantId = currentUserService.TenantId;
         if(!tenantId.HasValue)
            return new Result<PagedResult<GetAllApplilactionsQueryResponse>>(404, "Tenant not found");


        var query = applicationRepository
            .Where(p =>
                !p.IsDeleted &&
                p.JobPosting.TenantId == tenantId &&
                (!request.jobPostingId.HasValue || p.JobPostingId == request.jobPostingId.Value))
            .Include(p => p.JobPosting)
            .Include(p => p.User);


        var totalCount = await query.CountAsync(cancellationToken);


        var applications = await query
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize)
            .ToListAsync(cancellationToken);

        var response = applications.Select(application => new GetAllApplilactionsQueryResponse
        {
            Id = application.Id,
            UserId = application.UserId,
            UserFullName = application.User.FullName,
            TCKN = application.User.TCKN,
            JobPosting = application.JobPosting.Title,
            AppliedDate = application.AppliedDate,
            Status = application.Status,
            ReviewDate = application.ReviewDate,
            ReviewDescription = application.ReviewDescription,
        }).ToList();

        return new PagedResult<GetAllApplilactionsQueryResponse>(
            response,
            request.page,
            request.pageSize,
            totalCount
        );
    }
}

