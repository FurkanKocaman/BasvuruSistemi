using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.ApplicationFieldValues;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Applications;
public sealed class Application : Entity
{
    public Guid JobPostingId { get; private set; }
    public JobPosting JobPosting { get; private set; } = default!;

    public Guid UserId { get; private set; }
    public AppUser User { get; private set; } = default!;

    public DateTimeOffset AppliedDate { get; private set; }
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Pending;
    public DateTimeOffset? ReviewDate { get; private set; }
    public string? ReviewDescription { get; set; }

    public ICollection<ApplicationFieldValue> FieldValues { get; private set; } = new List<ApplicationFieldValue>();

    private Application() { }

    public Application(Guid jobPostingId, Guid userId, DateTimeOffset appliedDate, ApplicationStatus status)
    {
        JobPostingId = jobPostingId;
        UserId = userId;
        AppliedDate = appliedDate;
        Status = status;
        FieldValues = new List<ApplicationFieldValue>();
    }

    //public void Reject() => Status = ApplicationStatus.Rejected;
    //public void Accept() => Status = ApplicationStatus.Approved;
    //public void Withdrawn() => Status = ApplicationStatus.Withdrawn;

    public void Review(ApplicationStatus status,string? reviewDescription)
    {
        Status = status;
        ReviewDescription = reviewDescription;
        ReviewDate = DateTimeOffset.Now;
    }

    public void AddFieldValue(ApplicationFieldValue value)
    {
        FieldValues.Add(value);
    }
}
