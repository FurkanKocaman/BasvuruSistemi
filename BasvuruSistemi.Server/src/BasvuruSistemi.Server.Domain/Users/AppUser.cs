using System.ComponentModel.DataAnnotations.Schema;
using BasvuruSistemi.Server.Domain.Addresses;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace BasvuruSistemi.Server.Domain.Users;

public sealed class AppUser : IdentityUser<Guid>
{
   
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string FullName => FirstName + " " + LastName;

    public DateOnly? BirthOfDate { get; private set; }
    public string? Nationality { get; private set; }
    public string? TCKN { get; private set; }
    public ProfileStatus ProfileStatus { get; private set; } = ProfileStatus.Draft;
    public string? AvatarUrl { get; private set; }
    public Contact Contact { get; private set; } = default!;

    public ICollection<Address>? Addresses { get; private set; } = new List<Address>();
    public ICollection<Education> EducationHistory { get; private set; } = new List<Education>();
    public ICollection<Experience> WorkExperience { get; private set; } = new List<Experience>();
    public ICollection<Certification> Certifications { get; private set; } = new List<Certification>();
    public ICollection<Skill> SkillSet { get; private set; } = new List<Skill>();

    public ICollection<Application> Applications { get; private set; } = new List<Application>();


    #region Audit Log
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion

    private AppUser() { }
    public AppUser(string firstName, string lastName, DateOnly? bod, string? nationality, string? tCKN, Contact contact)
    {
        Id = Guid.CreateVersion7();
        FirstName = firstName;
        LastName = lastName;
        Email = contact.Email;
        BirthOfDate = bod;
        Nationality = nationality;
        TCKN = tCKN;
        ProfileStatus = ProfileStatus.Draft;
        Contact = contact;
        CreatedAt = DateTimeOffset.Now;
    }
    public void Update(string firstName, string lastName, DateOnly? bod, string? nationality, string? tCKN, Contact contact){
        FirstName = firstName;
        LastName = lastName;
        BirthOfDate = bod;
        Nationality = nationality;
        TCKN = tCKN;
        ProfileStatus = ProfileStatus.Draft;
        Contact = contact;
    }

    public void setAvatar(string url)
    {
        AvatarUrl = url;
    }


}
