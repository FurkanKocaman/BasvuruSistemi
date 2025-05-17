using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.Units;

namespace BasvuruSistemi.Server.Domain.PostingGroups;
public sealed class PostingGroup : Entity
{

    public string Name { get; private set; } = default!; // Örn: "Sosyal Bilimler Enstitüsü Yüksek Lisans Başvuruları 2025 Bahar"
    public string? Description { get; private set; } // Grup hakkında genel açıklama
    public DateTimeOffset? AnnouncementDate { get; private set; } // Grubun duyurulma tarihi
    public DateTimeOffset? OverallApplicationStartDate { get; private set; } // Grup için genel başvuru başlangıç tarihi
    public DateTimeOffset? OverallApplicationEndDate { get; private set; } // Grup için genel başvuru bitiş tarihi

    public JobPostingStatus Status { get; private set; } = JobPostingStatus.Draft; // İlanın durumu
    public bool IsPublic { get; set; }

    public Guid? ParentPostingGroupId { get; private set; }
    public PostingGroup? ParentPostingGroup { get; private set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public Guid? UnitId { get; set; }
    public Unit? Unit { get; set; }

    public ICollection<PostingGroup> ChildPostingGroups { get; private set; } = new List<PostingGroup>();

    public ICollection<JobPosting> JobPostings { get; private set; } = new List<JobPosting>();

    private PostingGroup() { }

    public PostingGroup(
        Guid tenantId,
        Guid? unitId,
        string name,
        string? description,
        bool isActive = true,
        bool isPublic = true,
        JobPostingStatus status = JobPostingStatus.Draft,
        DateTimeOffset? announcementDate = null,
        DateTimeOffset? overallApplicationStartDate = null,
        DateTimeOffset? overallApplicationEndDate = null,
        Guid? parentPostingGroupId = null)
    {
        TenantId = tenantId;
        UnitId = unitId;
        Name = name;
        Description = description;
        IsActive = isActive;
        IsPublic = isPublic;
        Status = status;
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
