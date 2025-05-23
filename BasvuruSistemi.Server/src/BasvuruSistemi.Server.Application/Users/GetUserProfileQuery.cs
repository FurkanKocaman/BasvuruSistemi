using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record GetUserProfileQuery(
    Guid? userId
    ) : IRequest<Result<GetUserProfileQueryResponse>>;

public sealed class GetUserProfileQueryResponse
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly? BirthOfDate { get;  set; }
    public string? Nationality { get;  set; }
    public string? TCKN { get;  set; }
    public ProfileStatus ProfileStatus { get;  set; } = ProfileStatus.Draft;
    public string? AvatarUrl { get;  set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public List<AddressDto>? Addresses { get; set; } = new List<AddressDto>();

    public List<CertificationDto> Certificates { get; set; } = new List<CertificationDto>();
    public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
    public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
    public List<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();
}
internal sealed class GetUserProfileQueryHandler(
    ICurrentUserService currentUserService,
    UserManager<AppUser> userManager,
    IAddressRepository addressRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileQueryResponse>>
{
    public async Task<Result<GetUserProfileQueryResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        Guid? currentUserId = currentUserService.UserId;
        if (!currentUserId.HasValue)
            return Result<GetUserProfileQueryResponse>.Failure(401,"Unauthorized");

        var queryUserId = request.userId ?? currentUserId.Value;

        var user = await userManager.Users
            .Where(p => p.Id == queryUserId)
            .Include(p => p.Certifications)
            .Include(p => p.SkillSet)
            .Include(p => p.EducationHistory)
            .Include(p => p.WorkExperience)
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result<GetUserProfileQueryResponse>.Failure(404,"User not found");

        var addresses = await addressRepository
            .Where(p => p.UserId == user.Id && !p.IsDeleted)
            .ToListAsync(cancellationToken);

        var context = httpContextAccessor.HttpContext?.Request;
        var avatarUrl = user.AvatarUrl != null ? $"{context?.Scheme}://{context?.Host}/{user.AvatarUrl}" : null;

        var response = new GetUserProfileQueryResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthOfDate = user.BirthOfDate,
            Nationality = user.Nationality,
            TCKN = user.TCKN,
            ProfileStatus = user.ProfileStatus,
            AvatarUrl = avatarUrl,
            Email = user.Email,
            Phone = user.Contact?.Phone,
            Addresses = addresses.Select(a =>
                new AddressDto(a.Id, a.Name, a.Country, a.City, a.District, a.Street, a.FullAddress, a.PostalCode)
            ).ToList(),
            Certificates = user.Certifications.Select(c =>
                new CertificationDto(c.Id, c.Title, c.Issuer, c.DateReceived, c.ExpiryDate)
            ).ToList(),
            Skills = user.SkillSet.Select(s =>
                new SkillDto(s.Id, s.Name, s.Description, s.Level)
            ).ToList(),
            Educations = user.EducationHistory.Select(e =>
                new EducationDto(e.Id, e.Institution, e.Department, e.Degree, e.Description, e.StartDate, e.GraduationDate, e.GPA)
            ).ToList(),
            Experiences = user.WorkExperience.Select(e =>
                new ExperienceDto(e.Id, e.CompanyName, e.Position, e.Location, e.StartDate, e.EndDate, e.Description)
            ).ToList()
        };

        return Result<GetUserProfileQueryResponse>.Succeed(response);
    }
}

