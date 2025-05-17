using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.JobPostings;
public sealed record GetActiveJobPostingQuery(
    Guid? id
    ) : IRequest<GetActiveJobPostingQueryResponse?>;

public sealed class GetActiveJobPostingQueryResponse
{
    public Guid Id { get; set; }

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string? Responsibilities { get;  set; }
    public string? Qualifications { get;  set; }
    public string? Benefits { get;  set; }

    public DateTimeOffset? ValidFrom { get;  set; }
    public DateTimeOffset? ValidTo { get;  set; }

    public bool IsRemote { get;  set; }
    public string? LocationText { get;  set; }

    public int? VacancyCount { get; set; }                // Açık pozisyon sayısı (Kontenjan)
    public string? EmploymentType { get; set; }
    public string? ExperienceLevelRequired { get; set; }
    public string? SalaryRange { get; set; }
    public string? SkillsRequired { get; set; }
    
    public string? ContactInfo { get; set; }
    public bool IsPublic { get; set; }

    public string Tenant { get; set; } = default!;
    public string? Unit { get; set; }

    public Guid FormTemplateId { get; set; }

}

internal sealed class GetActiveJobPostingQueryHandler(
    IJobPostingRepository jobPostingRepository
    ) : IRequestHandler<GetActiveJobPostingQuery, GetActiveJobPostingQueryResponse?>
{
    public Task<GetActiveJobPostingQueryResponse?> Handle(GetActiveJobPostingQuery request, CancellationToken cancellationToken)
    {
        var jobPosting = jobPostingRepository
            .Where(p => p.Status == JobPostingStatus.Published && p.IsPublic && !p.IsDeleted && request.id != null ? request.id == p.Id : true).Include(p => p.Unit);


        var response = jobPosting.Select(p => new GetActiveJobPostingQueryResponse
        {
            Id = p.Id,

            Title = p.Title,
            Description = p.Description,
            Responsibilities = p.Responsibilities,
            Qualifications = p.Qualifications,
            Benefits = p.Benefits,

            ValidFrom = p.ValidFrom,
            ValidTo = p.ValidTo,

            IsRemote = p.IsRemote,
            LocationText = p.LocationText,

            VacancyCount = p.VacancyCount,
            EmploymentType = p.EmploymentType.ToString(),
            ExperienceLevelRequired = p.ExperienceLevelRequired.ToString(),
            SalaryRange = p.MinSalary.ToString()+p.Currency + p.MaxSalary.ToString()+p.Currency,
            SkillsRequired = p.SkillsRequired,

            ContactInfo = p.ContactInfo,
            IsPublic = p.IsPublic,

            Tenant = p.Tenant.Name,
            Unit = p.Unit != null ? p.Unit.Name : null,

            FormTemplateId = p.FormTemplateId,
        }).FirstOrDefault();

        return Task.FromResult(response ?? null);
    }
}
