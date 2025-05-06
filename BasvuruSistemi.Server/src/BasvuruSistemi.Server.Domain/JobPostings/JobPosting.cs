using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Companies;
using BasvuruSistemi.Server.Domain.Departments;

namespace BasvuruSistemi.Server.Domain.JobPostings;
public sealed class JobPosting : Entity
{
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public DateTimeOffset PostedDate { get; private set; }
    public DateTimeOffset ClosingDate { get; private set; }
    public bool IsPublic { get; private set; }
    public bool IsPublished { get; private set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = default!;

    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public Guid FormTemplateId { get; private set; }
    public ApplicationFormTemplate FormTemplate { get; private set; } = default!;

    public ICollection<Application> Applications { get; private set; } = new List<Application>();

    private JobPosting() { }

    public JobPosting(string title, string description, DateTimeOffset postedDate, DateTimeOffset closingDate, bool isPublic, bool ispublished, Guid companyId, Guid? departmentId, Guid formTemplateId)
    {
        Title = title;
        Description = description;
        PostedDate = postedDate;
        ClosingDate = closingDate;
        IsPublic = isPublic;
        IsPublished = ispublished;
        CompanyId = companyId;
        DepartmentId = departmentId;
        FormTemplateId = formTemplateId;
    }
}
