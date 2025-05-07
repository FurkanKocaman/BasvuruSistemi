using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.Tenants;

namespace BasvuruSistemi.Server.Domain.PostingGroups;
public sealed class PostingGroup : Entity
{
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public string Name { get; private set; } = default!; // Örn: "Sosyal Bilimler Enstitüsü Yüksek Lisans Başvuruları 2025 Bahar"
    public string? Description { get; private set; } // Grup hakkında genel açıklama
    public DateTimeOffset? AnnouncementDate { get; private set; } // Grubun duyurulma tarihi
    public DateTimeOffset? OverallApplicationStartDate { get; private set; } // Grup için genel başvuru başlangıç tarihi
    public DateTimeOffset? OverallApplicationEndDate { get; private set; } // Grup için genel başvuru bitiş tarihi

    // Hiyerarşik gruplar için 
    public Guid? ParentPostingGroupId { get; private set; }
    public PostingGroup? ParentPostingGroup { get; private set; }

    public ICollection<PostingGroup> ChildPostingGroups { get; private set; } = new List<PostingGroup>();

    public ICollection<JobPosting> JobPostings { get; private set; } = new List<JobPosting>();

    private PostingGroup() { }

    public PostingGroup(
        Guid tenantId,
        string name,
        string? description,
        bool isActive = true,
        DateTimeOffset? announcementDate = null,
        DateTimeOffset? overallApplicationStartDate = null,
        DateTimeOffset? overallApplicationEndDate = null,
        Guid? parentPostingGroupId = null)
    {
        TenantId = tenantId;
        Name = name;
        Description = description;
        IsActive = isActive;
        AnnouncementDate = announcementDate ?? DateTimeOffset.Now;
        OverallApplicationStartDate = overallApplicationStartDate;
        OverallApplicationEndDate = overallApplicationEndDate;
        ParentPostingGroupId = parentPostingGroupId;
    }

    public void UpdateDetails(string name, string? description, bool isActive, DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        OverallApplicationStartDate = startDate;
        OverallApplicationEndDate = endDate;
    }

}
