using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record GetApplicationQuery(
    Guid applicationId
    ) : IRequest<Result<GetApplicationQueryResponse>>;

public sealed class GetApplicationQueryResponse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public string? TCKN { get; set; }

    public Guid JobPostingId { get; set; }
    public string JobPostingTitle { get; set; } = default!;
    public DateTimeOffset JobPostingCreateDate { get; set; } = default!;
    public int? JobPostingVacancyCount { get; set; }
    public int? TotalApplicationCount { get; set; }
    public string Unit { get; set; } = default!;

    public DateTimeOffset AppliedDate { get; set; }
    public ApplicationStatus Status { get; set; } = default!;
    public DateTimeOffset? ReviewDate { get; set; }
    public string? ReviewDescription { get; set; }

    //FieldValues
    public List<FieldValueResponseDto> FieldValues { get; set; } = new List<FieldValueResponseDto>();
}

internal sealed class GetApplicationQueryHandler(
    IApplicationRepository applicationRepository,
    IHttpContextAccessor httpContextAccessor
    ) : IRequestHandler<GetApplicationQuery, Result<GetApplicationQueryResponse>>
{
    public Task<Result<GetApplicationQueryResponse>> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        var application = applicationRepository.Where(p => p.Id == request.applicationId)
            .Include(p => p.User)
            .ThenInclude(p => p.Addresses)
            .Include(p => p.FieldValues)
            .ThenInclude(p => p.FieldDefinition)
            .Include(p => p.JobPosting)
                .ThenInclude(p => p.Tenant)
            .Include(p => p.JobPosting)
                .ThenInclude(p => p.Unit)
            .Include(p => p.JobPosting)
                .ThenInclude(p => p.Tenant)
                .FirstOrDefault();

        if (application is null)
            return Task.FromResult(Result<GetApplicationQueryResponse>.Failure("Application not found."));

        var totalApplicationCount = applicationRepository.Where(p => p.JobPostingId == application.JobPostingId && !p.IsDeleted).Count();

        var context = httpContextAccessor.HttpContext?.Request;

        var response = new GetApplicationQueryResponse
        {
            Id = application.Id,

            UserId = application.UserId,
            FirstName = application.User.FirstName,
            LastName = application.User.LastName,
            Email = application.User.Email,
            Phone = application.User.Contact.Phone,
            TCKN = application.User.TCKN,

            //Country = application.User.Address?.Country,
            //City = application.User.Address?.City,
            //District = application.User.Address?.District,
            //Street = application.User.Address?.Street,
            //FullAddress = application.User.Address?.FullAddress,
            //PostalCode = application.User.Address?.PostalCode,

            JobPostingId = application.JobPostingId,
            JobPostingTitle = application.JobPosting.Title,
            JobPostingCreateDate = application.JobPosting.CreatedAt,
            JobPostingVacancyCount = application.JobPosting.VacancyCount,
            TotalApplicationCount = totalApplicationCount,
            Unit = application.JobPosting.Unit != null ? application.JobPosting.Unit.Name : application.JobPosting.Tenant.Name ,

            AppliedDate = application.AppliedDate,
            Status = application.Status,
            ReviewDate = application.ReviewDate,
            ReviewDescription = application.ReviewDescription,

            FieldValues = application.FieldValues.OrderBy(p => p.FieldDefinition.Order).Select(p => new FieldValueResponseDto(
                p.FieldDefinitionId,
                p.FieldDefinition.Label,
                p.FieldDefinition.Type,
                (p.FieldDefinition.Type == FieldTypeEnum.File || p.FieldDefinition.Type == FieldTypeEnum.Image || p.FieldDefinition.Type == FieldTypeEnum.EDevletVerifiedFile || p.FieldDefinition.Type == FieldTypeEnum.YoksisAlesDocument) ? $"{context?.Scheme}://{context?.Host}{p.Value}" : p.Value
            )).ToList(),
        };

        return Task.FromResult(Result<GetApplicationQueryResponse>.Succeed(response));

    }
}
