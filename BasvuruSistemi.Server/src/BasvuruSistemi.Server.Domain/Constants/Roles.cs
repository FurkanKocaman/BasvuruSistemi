namespace BasvuruSistemi.Server.Domain.Constants;
public static class CustomClaimTypes
{
    // Job Postings
    public const string CreateJobPosting = "jobposting.create";
    public const string EditJobPosting = "jobposting.edit";
    public const string DeleteJobPosting = "jobposting.delete";
    public const string ViewJobPosting = "jobposting.view";
    public const string PublishJobPosting = "jobposting.publish";

    // Applications
    public const string CreateApplication = "application.create";
    public const string ViewApplications = "application.view";
    public const string ReviewApplications = "application.review";
    public const string ApproveApplications = "application.approve";
    public const string ScoreApplications = "application.score";

    // User, Role, Tenant, Unit
    public const string ManageUsers = "user.manage";
    public const string ManageRoles = "role.manage";
    public const string ManageTenants = "tenant.manage";
    public const string ManageUnits = "unit.manage";

    // Commission
    public const string CommissionMember = "commission.member";
    public const string CommissionManager = "commission.manager";

    // Templates, Forms
    public const string ManageTemplates = "template.manage";
    public const string ManageFormFields = "formfield.manage";

    // Documents
    public const string DownloadDocuments = "document.download";

    // New
    public const string AssignCommission = "commission.assign";
    public const string ConfigurePipeline = "pipeline.configure";
}

public sealed record RoleDefinition(
      string Name,
      string Description,
      string[] AllowedClaims
  );


public static class Roles
{
    /// <summary>Global sistem yöneticisi: her şeye erişir.</summary>
    public static readonly RoleDefinition Admin = new(
        Name: "Admin",
        Description: "Sistemi yöneten global yönetici",
        AllowedClaims: new[]
        {
                CustomClaimTypes.ManageTenants,
                CustomClaimTypes.ManageUnits,
                CustomClaimTypes.ManageUsers,
                CustomClaimTypes.ManageRoles,
                CustomClaimTypes.ManageTemplates,
                CustomClaimTypes.ManageFormFields,
                CustomClaimTypes.CreateJobPosting,
                CustomClaimTypes.EditJobPosting,
                CustomClaimTypes.DeleteJobPosting,
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.ReviewApplications,
                CustomClaimTypes.ApproveApplications,
        }
    );

    /// <summary>Tenant bazında yönetici: o tenant altındaki tüm birimleri, kullanıcıları yönetebilir.</summary>
    public static readonly RoleDefinition TenantManager = new(
        Name: "TenantManager",
        Description: "Tenant genel yöneticisi",
        AllowedClaims: new[]
        {
                CustomClaimTypes.ManageUnits,
                CustomClaimTypes.ManageUsers,
                CustomClaimTypes.ManageRoles,
                CustomClaimTypes.ManageTemplates,
                CustomClaimTypes.ManageFormFields,
                CustomClaimTypes.CreateJobPosting,
                CustomClaimTypes.EditJobPosting,
                CustomClaimTypes.DeleteJobPosting,
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.ReviewApplications,
                CustomClaimTypes.ApproveApplications,
        }
    );

    /// <summary>Birim yöneticisi: sadece kendi birimindeki ilan ve başvuruları yönetir.</summary>
    public static readonly RoleDefinition UnitManager = new(
        Name: "UnitManager",
        Description: "Belirli bir birimin yöneticisi",
        AllowedClaims: new[]
        {
                CustomClaimTypes.CreateJobPosting,
                CustomClaimTypes.EditJobPosting,
                CustomClaimTypes.DeleteJobPosting,
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.ReviewApplications,
                CustomClaimTypes.ApproveApplications,
        }
    );

    /// <summary>İK sorumlusu: başvuruları görüntüler, review eder, kullanıcı ekleyebilir.</summary>
    public static readonly RoleDefinition HumanResources = new(
        Name: "HumanResources",
        Description: "İnsan kaynakları sorumlusu",
        AllowedClaims: new[]
        {
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.ReviewApplications,
                CustomClaimTypes.ApproveApplications,
                CustomClaimTypes.CreateApplication,
                CustomClaimTypes.ManageUsers,
        }
    );

    /// <summary>İlan oluşturucu: kendi birimi için ilan açar ve düzenler.</summary>
    public static readonly RoleDefinition Recruiter = new(
        Name: "Recruiter",
        Description: "İlan oluşturucusu",
        AllowedClaims: new[]
        {
                CustomClaimTypes.CreateJobPosting,
                CustomClaimTypes.EditJobPosting,
                CustomClaimTypes.DeleteJobPosting,
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.ViewApplications,
        }
    );

    /// <summary>Onaycı: başvuruları onay/ret eder.</summary>
    public static readonly RoleDefinition Approver = new(
        Name: "Approver",
        Description: "Belge/onay sorumlusu",
        AllowedClaims: new[]
        {
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.ReviewApplications,
                CustomClaimTypes.ApproveApplications,
                CustomClaimTypes.DownloadDocuments,
        }
    );

    public static readonly RoleDefinition CommissionManager = new(
        Name: "CommissionManager",
        Description: "Komisyon yöneticisi",
        AllowedClaims: new[]
        {
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.CommissionManager,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.ReviewApplications,
                CustomClaimTypes.ApproveApplications,
                CustomClaimTypes.DownloadDocuments,
        }
    );

    public static readonly RoleDefinition CommissionMember = new(
      Name: "CommissionMember",
      Description: "Komisyon Üyesi",
      AllowedClaims: new[]
      {
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.CommissionManager,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.ReviewApplications,
                CustomClaimTypes.ApproveApplications,
                CustomClaimTypes.DownloadDocuments,
      }
  );

    /// <summary>Aday: ilana başvurur, kendi başvurularını görür.</summary>
    public static readonly RoleDefinition Candidate = new(
        Name: "Candidate",
        Description: "Başvuru yapan kullanıcı",
        AllowedClaims: new[]
        {
                CustomClaimTypes.ViewJobPosting,
                CustomClaimTypes.CreateApplication,
                CustomClaimTypes.ViewApplications,
                CustomClaimTypes.DownloadDocuments,
        }
    );
    public static readonly RoleDefinition PipelineManager = new(
        Name: "PipelineManager",
        Description: "Değerlendirme aşamalarını yöneten kişi",
        AllowedClaims: new[]
        {
            CustomClaimTypes.ConfigurePipeline,
            CustomClaimTypes.ViewJobPosting,
            CustomClaimTypes.ViewApplications,
        }
    );

    public static readonly RoleDefinition CommissionAssigner = new(
        Name: "CommissionAssigner",
        Description: "Komisyon üyelerini atayabilir",
        AllowedClaims: new[]
        {
            CustomClaimTypes.AssignCommission,
            CustomClaimTypes.ViewJobPosting,
            CustomClaimTypes.ViewApplications,
        }
    );

    public static readonly RoleDefinition ApplicationScorer = new(
        Name: "ApplicationScorer",
        Description: "Başvurulara puan verebilir",
        AllowedClaims: new[]
        {
            CustomClaimTypes.ScoreApplications,
            CustomClaimTypes.ViewApplications,
        }
    );

    public static readonly RoleDefinition[] All =
        {
        Admin,
        TenantManager,
        UnitManager,
        HumanResources,
        Recruiter,
        Approver,
        CommissionManager,
        CommissionMember,
        PipelineManager,
        CommissionAssigner,
        ApplicationScorer,
        Candidate
    };

    public static readonly string[] AllCustomClaimTypes =
      {
        CustomClaimTypes.CreateJobPosting,
        CustomClaimTypes.EditJobPosting,
        CustomClaimTypes.DeleteJobPosting,
        CustomClaimTypes.ViewJobPosting,
        CustomClaimTypes.PublishJobPosting,
        CustomClaimTypes.CreateApplication,
        CustomClaimTypes.ViewApplications,
        CustomClaimTypes.ReviewApplications,
        CustomClaimTypes.ApproveApplications,
        CustomClaimTypes.ScoreApplications,
        CustomClaimTypes.ManageUsers,
        CustomClaimTypes.ManageRoles,
        CustomClaimTypes.ManageTenants,
        CustomClaimTypes.ManageUnits,
        CustomClaimTypes.ManageTemplates,
        CustomClaimTypes.ManageFormFields,
        CustomClaimTypes.DownloadDocuments,
        CustomClaimTypes.CommissionMember,
        CustomClaimTypes.CommissionManager,
        CustomClaimTypes.AssignCommission,
        CustomClaimTypes.ConfigurePipeline
    };
}
