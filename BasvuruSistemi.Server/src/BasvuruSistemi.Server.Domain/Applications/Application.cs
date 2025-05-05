using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.ApplicationFieldValues;
using BasvuruSistemi.Server.Domain.Candidates;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;

namespace BasvuruSistemi.Server.Domain.Applications;
public sealed class Application : Entity
{
    public Guid JobPostingId { get; private set; }
    public Guid CandidateId { get; private set; }
    public DateTimeOffset AppliedDate { get; private set; }
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Pending;

    public JobPosting JobPosting { get; private set; } = default!;
    public Candidate Candidate { get; private set; } = default!;
    public ICollection<ApplicationFieldValue> FieldValues { get; private set; } = new List<ApplicationFieldValue>();


    private Application() { }

    public Application(Guid jobPostingId, Guid candidateId, DateTimeOffset appliedDate, ApplicationStatus status)
    {
        JobPostingId = jobPostingId;
        CandidateId = candidateId;
        AppliedDate = appliedDate;
        Status = status;
        FieldValues = new List<ApplicationFieldValue>();
    }

    public void Reject() => Status = ApplicationStatus.Rejected;
    public void Accept() => Status = ApplicationStatus.Approved;

    public void AddFieldValue(ApplicationFieldValue value)
    {
        FieldValues.Add(value);
    }
}
