using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record GetUserProfileQuery(
    Guid? userId
    ) : IRequest<GetUserProfileQueryResponse>;

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
    ) : IRequestHandler<GetUserProfileQuery, GetUserProfileQueryResponse>
{
    public Task<GetUserProfileQueryResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        Guid? userId = currentUserService.UserId;
        if(!userId.HasValue)
            return Task.FromResult(new GetUserProfileQueryResponse());

        var user = userManager.Users.Where(p => request.userId != null ? p.Id == request.userId : p.Id == userId.Value)
            .Include(p => p.Certifications)
            .Include(p => p.SkillSet)
            .Include(p => p.EducationHistory)
            .Include(p => p.WorkExperience)
            .FirstOrDefault();
        if (user is null)
            return Task.FromResult(new GetUserProfileQueryResponse());

        var addresses = addressRepository.Where(p => p.UserId == user.Id && !p.IsDeleted).ToList();

        var context = httpContextAccessor.HttpContext?.Request;

        var response = new GetUserProfileQueryResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthOfDate = user.BirthOfDate,
            Nationality = user.Nationality,
            TCKN = user.TCKN,
            ProfileStatus = user.ProfileStatus,
            AvatarUrl = user.AvatarUrl != null ? $"{context?.Scheme}://{context?.Host}/{user.AvatarUrl}" : null,
            Email = user.Email,
            Phone = user.Contact.Phone,
            Addresses = addresses.Select(address => new AddressDto(address.Id, address.Name, address.Country, address.City, address.District, address.Street, address.FullAddress, address.PostalCode)).ToList(),
            Certificates = user.Certifications.Select(certification => new CertificationDto(certification.Id, certification.Title, certification.Issuer, certification.DateReceived, certification.ExpiryDate)).ToList(),
            Skills = user.SkillSet.Select(skill => new SkillDto(skill.Id, skill.Name, skill.Description, skill.Level)).ToList(),
            Educations = user.EducationHistory.Select(education => new EducationDto(education.Id, education.Institution, education.Department, education.Degree, education.Description, education.StartDate, education.GraduationDate, education.GPA)).ToList(),
            Experiences = user.WorkExperience.Select(experience => new ExperienceDto(experience.Id, experience.CompanyName, experience.Position, experience.Location, experience.StartDate, experience.EndDate, experience.Description)).ToList()
        };

        return Task.FromResult(response);
    }
}
