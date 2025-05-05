using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Entities;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.Users;
using BasvuruSistemi.Server.Domain.ValueObjects;

namespace BasvuruSistemi.Server.Domain.Candidates;
public sealed class Candidate : Entity
{
    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!; 
    public string FullName => FirstName + " " + LastName;
    public DateOnly DateOfBirth { get; private set; }
    public string? Nationality {  get; private set; }
    public string? TCKN { get; private set; }
    public ProfileStatus ProfileStatus { get; private set; } = ProfileStatus.Draft;

    public Address Address { get; private set; } = default!;
    public Contact Contact { get; private set; } = default!;

    public ICollection<Education> EducationHistory { get; private set; } = new List<Education>();
    public ICollection<Experience> WorkExperience { get; private set; } = new List<Experience>();
    public ICollection<Certification> Certifications { get; private set; } = new List<Certification>();
    public ICollection<Skill> SkillSet { get; private set; } = new List<Skill>();

    public ICollection<Application> Applications { get; private set; } = new List<Application>();

    private Candidate() { }
    public Candidate(Guid userId, string firstName, string lastName, DateOnly dob, string? nationality, string? tckn, Address address, Contact contact)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        Nationality = nationality;
        TCKN = tckn;

        Address = address;
        Contact = contact;

        EducationHistory = new List<Education>();
        WorkExperience = new List<Experience>();
        Certifications = new List<Certification>();
        SkillSet = new List<Skill>();
    }
}
