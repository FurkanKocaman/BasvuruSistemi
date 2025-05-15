using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record GetApplicationsByUserQuery(
    int page,
    int pageSize
    ) : IRequest<PagedResult<GetApplicationsByUserQueryResponse>>;

public sealed class GetApplicationsByUserQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string? Unit { get; set; } = default!;
    public DateTimeOffset AppliedDate { get; set; }
    public ApplicationStatus Status { get; set; } = default!;
    public DateTimeOffset? ReviewDate { get; set; }
    public string? ReviewDescription { get; set; }
    public string? Reviewer { get;set; } = default!;


}

internal sealed class GetApplicationsByUserQueryHandler(
    ICurrentUserService currentUserService,
     IApplicationRepository applicationRepository
    ) : IRequestHandler<GetApplicationsByUserQuery, PagedResult<GetApplicationsByUserQueryResponse>>
{
    public Task<PagedResult<GetApplicationsByUserQueryResponse>> Handle(GetApplicationsByUserQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if(!userId.HasValue)
            return Task.FromResult(new PagedResult<GetApplicationsByUserQueryResponse>(new List<GetApplicationsByUserQueryResponse>(), 1, 10, 0));

        var applications = applicationRepository.Where(p =>  userId.Value == p.UserId && !p.IsDeleted).Include(p => p.JobPosting).ThenInclude(p => p.Unit).Include(p => p.Reviewer).ToList();

        var totalCount = applications.Count();

        var pagedApplications = applications
            .Skip((request.page - 1) * request.pageSize)
            .Take(request.pageSize);

        var response = pagedApplications.Select(application => new GetApplicationsByUserQueryResponse
        {
            Id = application.Id,
            Title = application.JobPosting.Title,
            Unit = application.JobPosting.Unit != null ? application.JobPosting.Unit.Name : null,
            AppliedDate = application.AppliedDate,
            Status = application.Status,
            ReviewDate = application.ReviewDate,
            ReviewDescription = application.ReviewDescription,
            Reviewer = application.Reviewer != null ? application.Reviewer.FullName : null,
        }).ToList();

        return Task.FromResult(new PagedResult<GetApplicationsByUserQueryResponse>(response, request.page, request.pageSize, totalCount));
    }
}
