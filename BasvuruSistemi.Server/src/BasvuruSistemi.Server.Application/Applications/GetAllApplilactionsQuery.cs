using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record GetAllApplilactionsQuery(
    Guid jobPostingId,
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetAllApplilactionsQueryResponse>>;

public sealed class GetAllApplilactionsQueryResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string UserFullName { get; set; } = default!;
    public string? TCKN { get; set; }

    public DateTimeOffset AppliedDate { get; set; }
    public string Status { get; set; } = default!;
    public DateTimeOffset? ReviewDate { get; set; }
    public string? ReviewDescription { get; set; }

}

internal sealed class GetAllApplilactionsQueryHandler(
    IApplicationRepository applicationRepository
    ) : IRequestHandler<GetAllApplilactionsQuery, PagedResult<GetAllApplilactionsQueryResponse>>
{
    public Task<PagedResult<GetAllApplilactionsQueryResponse>> Handle(GetAllApplilactionsQuery request, CancellationToken cancellationToken)
    {
        var applications = applicationRepository.Where(p => p.JobPostingId == request.jobPostingId).Include(p => p.User);

        var totalCount = applications.Count();

        var pagedApplications = applications
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize);

        var response = pagedApplications.Select(application => new GetAllApplilactionsQueryResponse
        {
            Id = application.Id,
            UserId = application.UserId,
            UserFullName = application.User.FullName,
            TCKN = application.User.TCKN,

            AppliedDate = application.AppliedDate,
            Status = application.Status.ToString(),
            ReviewDate = application.ReviewDate,
            ReviewDescription = application.ReviewDescription,
        }).ToList();

        return Task.FromResult(new PagedResult<GetAllApplilactionsQueryResponse>(response, totalCount, request.page, request.pageSize));

    }
}
