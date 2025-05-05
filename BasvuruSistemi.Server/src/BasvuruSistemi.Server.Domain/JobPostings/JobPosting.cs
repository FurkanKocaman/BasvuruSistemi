using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Employers;

namespace BasvuruSistemi.Server.Domain.JobPostings;
public sealed class JobPosting : Entity
{
    public Guid EmployerId { get; private set; }
    public Employer Employer { get; private set; } = default!;

    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public DateTimeOffset PostedDate { get; private set; }
    public DateTimeOffset ClosingDate { get; private set; }

    public ICollection<Application> Applications { get; private set; } = new List<Application>();

    private JobPosting() { }

    public JobPosting(Guid employerId, string title, string description, DateTimeOffset postedDate, DateTimeOffset closingDate)
    {
        EmployerId = employerId;
        Title = title;
        Description = description;
        PostedDate = postedDate;
        ClosingDate = closingDate;
    }
}
