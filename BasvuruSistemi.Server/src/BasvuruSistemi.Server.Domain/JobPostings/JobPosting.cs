using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.ApplicationFormTemplates;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.PostingGroups;
using BasvuruSistemi.Server.Domain.Tenants;
using BasvuruSistemi.Server.Domain.Units;

namespace BasvuruSistemi.Server.Domain.JobPostings;
public sealed class JobPosting : Entity
{
    public string Title { get; private set; } = default!;          // İlan Başlığı
    public string Description { get; private set; } = default!;    // Genel Açıklama (HTML/Markdown destekleyebilir)
    public string? Responsibilities { get; private set; }         // Sorumluluklar (HTML/Markdown)
    public string? Qualifications { get; private set; }           // Nitelikler (HTML/Markdown)
    public string? Benefits { get; private set; }

    public DateTimeOffset DatePosted { get; private set; }        // İlanın yayınlandığı veya oluşturulduğu tarih
    public DateTimeOffset ApplicationDeadline { get; private set; } // Son Başvuru Tarihi
    public DateTimeOffset? ValidFrom { get; private set; }        // İlanın aktif olacağı başlangıç tarihi (opsiyonel, ileri tarihli yayınlama için)
    public DateTimeOffset? ValidTo { get; private set; }

    public JobPostingStatus Status { get; private set; } = JobPostingStatus.Draft; // İlanın durumu
    public bool IsRemote { get; private set; }                    // Uzaktan çalışma imkanı var mı?
    public string? LocationText { get; private set; }             // Lokasyon (örn: "İstanbul, Türkiye", "Uzaktan/Hibrit")

    public int? VacancyCount { get; private set; }                // Açık pozisyon sayısı (Kontenjan)
    public EmploymentType? EmploymentType { get; private set; }   // Çalışma Şekli
    public ExperienceLevel? ExperienceLevelRequired { get; private set; } // İstenen Deneyim Seviyesi
    public string? SkillsRequired { get; private set; }           // İstenen Yetenekler (virgülle ayrılmış veya JSON)

    public List<string> AllowedNationalIds { get; private set; } = new List<string>(); // İlanın geçerli olduğu ulusal kimlik numaraları (örn: Türkiye için TCKN)
    public string? ContactInfo { get; private set; }              // İletişim Bilgisi (örn: e-posta veya sorumlu kişi)
    public bool IsPublic { get; private set; } // Sadece seçili kişiler mi yoksa herkes mi görebilir
    public bool IsAnonymous { get; private set; } // Başvuru yapmak için oturum açmaya gerek var mı

    public decimal? MinSalary { get; private set; }
    public decimal? MaxSalary { get; private set; }
    public string? Currency { get; private set; }

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public Guid? UnitId { get; set; }
    public Unit? Unit { get; set; } 

    public Guid FormTemplateId { get; private set; }
    public ApplicationFormTemplate FormTemplate { get; private set; } = default!;

    public ICollection<Application> Applications { get; private set; } = new List<Application>();

    public Guid? PostingGroupId { get; private set; } // Ait olduğu İlan Grubu (opsiyonel)
    public PostingGroup? PostingGroup { get; private set; } // Navigation property

    private JobPosting() { }

    public JobPosting(
        string title,
        string description,
        DateTimeOffset datePosted,
        DateTimeOffset applicationDeadline,
        Guid tenantId,
        Guid? unitId,
        Guid formTemplateId,
        Guid? postingGroupId = null,
        JobPostingStatus status = JobPostingStatus.Draft,
        bool isPublic = true,
        bool isAnonymous = false,

        DateTimeOffset? validFrom = null,
        DateTimeOffset? validTo = null,

        string? contactInfo = null,

        string? responsibilities = null,
        string? qualifications = null,
        string? benefits = null,
        string? locationText = null,
        bool isRemote = false,
        EmploymentType? employmentType = null,
        ExperienceLevel? experienceLevelRequired = null,
        int? vacancyCount = null,
        string? skillsRequired = null,

        decimal? minSalary = null,
        decimal? maxSalary = null,
        string? currency = null
        )
    {
        Title = title;
        Description = description;
        DatePosted = datePosted;
        ApplicationDeadline = applicationDeadline;
        TenantId = tenantId;
        UnitId = unitId;
        FormTemplateId = formTemplateId;
        PostingGroupId = postingGroupId;
        Status = status;
        IsPublic = isPublic;
        IsAnonymous = isAnonymous;

        ContactInfo = contactInfo;

        Responsibilities = responsibilities;
        Qualifications = qualifications;
        Benefits = benefits;
        LocationText = locationText;
        IsRemote = isRemote;
        EmploymentType = employmentType;
        ExperienceLevelRequired = experienceLevelRequired;
        VacancyCount = vacancyCount;
        SkillsRequired = skillsRequired;

        MinSalary = minSalary;
        MaxSalary = maxSalary;
        Currency = currency;

        // ValidFrom ve ValidTo gibi diğer alanlar için de parametreler eklenebilir veya
        // publish/schedule gibi metotlarla ayarlanabilir.
        ValidFrom = validFrom ?? datePosted; // Varsayılan olarak yayınlandığı an aktif
        ValidTo = validTo ?? applicationDeadline;
    }
    public bool IsOpenForApplication()
    {
        var now = DateTimeOffset.Now;
        return Status == JobPostingStatus.Published &&
               now >= (ValidFrom ?? DatePosted) &&
               now <= (ValidTo ?? ApplicationDeadline);
    }

    public void CheckAndExpire()
    {
        var now = DateTimeOffset.Now;
        if (Status == JobPostingStatus.Published && now > (ValidTo ?? ApplicationDeadline))
        {
            Status = JobPostingStatus.Expired;
        }
    }
    public void Publish(DateTimeOffset? publishStartDate = null)
    {
        if (Status == JobPostingStatus.Draft || Status == JobPostingStatus.OnHold)
        {
            Status = JobPostingStatus.Published;
            ValidFrom = publishStartDate ?? DateTimeOffset.UtcNow; // Eğer başlangıç tarihi verilmezse hemen
            DatePosted = DateTimeOffset.UtcNow; // Yayınlama işleminin yapıldığı an olarak güncellenebilir
        }
        // else: Hata fırlat veya logla (zaten yayınlanmış veya kapatılmış bir ilanı tekrar yayınlayamazsın)
    }

    public void Close()
    {
        if (Status == JobPostingStatus.Published || Status == JobPostingStatus.OnHold)
        {
            Status = JobPostingStatus.Closed;
        }
    }

    public void PutOnHold()
    {
        if (Status == JobPostingStatus.Published)
        {
            Status = JobPostingStatus.OnHold;
        }
    }

    public void Archive()
    {
        // Sadece kapalı veya süresi dolmuş ilanlar arşivlenebilir gibi bir kural eklenebilir
        if (Status == JobPostingStatus.Closed || Status == JobPostingStatus.Expired)
        {
            Status = JobPostingStatus.Archived;
        }
    }
    public void UpdateDetails(
        string title, 
        string description,
        string? responsibilities,
        string? qualifications,
        DateTimeOffset applicationDeadline,
        string? locationText,
        bool isRemote,
        EmploymentType? employmentType,
        ExperienceLevel? experienceLevelRequired,
        int? vacancyCount
        )
    {
        // Sadece belirli durumlarda güncellemeye izin verilebilir (örn: taslak iken)
        if (Status == JobPostingStatus.Draft || Status == JobPostingStatus.OnHold) // Veya admin yetkisine bağlı olarak her zaman
        {
            Title = title;
            Description = description;
            Responsibilities = responsibilities;
            Qualifications = qualifications;
            ApplicationDeadline = applicationDeadline;
            LocationText = locationText;
            IsRemote = isRemote;
            EmploymentType = employmentType;
            ExperienceLevelRequired = experienceLevelRequired;
            VacancyCount = vacancyCount;
            // ... diğer alanların güncellenmesi
        }
    }
    public void UpdateAdditionalFields(
    DateTimeOffset datePosted,
    DateTimeOffset? validFrom,
    DateTimeOffset? validTo,

    decimal? minSalary,
    decimal? maxSalary,
    string? currency,

    string? skillsRequired,
    string? contactInfo,
    bool isPublic,
    Guid? postingGroupId)
    {
        DatePosted = datePosted;
        ValidFrom = validFrom;
        ValidTo = validTo;

        MinSalary = minSalary;
        MaxSalary = maxSalary;
        Currency = currency;

        SkillsRequired = skillsRequired;
        ContactInfo = contactInfo;
        IsPublic = isPublic;
        PostingGroupId = postingGroupId;
    }

    public void SetAllowedNationalIds(IEnumerable<string> nationalIds)
    {
        AllowedNationalIds.Clear();
        AllowedNationalIds.AddRange(nationalIds
            .Select(id => id.Trim())
            .Where(id => !string.IsNullOrWhiteSpace(id) && id.Length == 11)); // T.C. No 11 hanelidir
    }

    public void JoinGroup(Guid postingGroupId)
    {
        PostingGroupId = postingGroupId;
    }
}
