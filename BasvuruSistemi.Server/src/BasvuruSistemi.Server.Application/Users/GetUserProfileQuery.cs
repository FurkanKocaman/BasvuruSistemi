using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record GetUserProfileQuery(
    Guid userId
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
    public AddressDto? Address { get; set; }

    public List<CertificationDto> Certifications { get; set; } = new List<CertificationDto>();
    public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
    public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
    public List<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();
}

internal sealed class GetUserProfileQueryHandler(
    UserManager<AppUser> userManager,
    IAddressRepository addressRepository
    ) : IRequestHandler<GetUserProfileQuery, GetUserProfileQueryResponse>
{
    public Task<GetUserProfileQueryResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = userManager.Users.Where(p => p.Id == request.userId)
            .Include(p => p.Certifications)
            .Include(p => p.SkillSet)
            .Include(p => p.EducationHistory)
            .Include(p => p.WorkExperience)
            .FirstOrDefault();
        if (user is null)
            return Task.FromResult(new GetUserProfileQueryResponse());

        var address = addressRepository.Where(p => p.UserId == user.Id).FirstOrDefault();

        var response = new GetUserProfileQueryResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthOfDate = user.BirthOfDate,
            Nationality = user.Nationality,
            TCKN = user.TCKN,
            ProfileStatus = user.ProfileStatus,
            AvatarUrl = user.AvatarUrl,
            Email = user.Email,
            Phone = user.Contact.Phone,
            Address = address != null ? new(
                address.Country,
                address.City,
                address.District,
                address.Street,
                address.FullAddress,
                address.PostalCode
            ) : null,

            Certifications = user.Certifications.Select(certification => new CertificationDto(certification.Title, certification.Issuer, certification.DateReceived, certification.ExpiryDate)).ToList(),
            Skills = user.SkillSet.Select(skill => new SkillDto(skill.Name,skill.Description,skill.Level)).ToList(),
            Educations = user.EducationHistory.Select(education => new EducationDto(education.Institution,education.Department,education.Degree,education.Description,education.StartDate,education.GraduationDate,education.GPA)).ToList(),
            Experiences = user.WorkExperience.Select(experience => new ExperienceDto(experience.CompanyName,experience.Position,experience.Location,experience.StartDate,experience.EndDate,experience.Description)).ToList()
        };

        return Task.FromResult(response);
    }
}
