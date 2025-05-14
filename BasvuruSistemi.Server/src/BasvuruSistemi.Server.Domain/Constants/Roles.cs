namespace BasvuruSistemi.Server.Domain.Constants;
public static class Roles
{
    public static readonly RoleDefinition Admin = new("Admin", "Sistemi yöneten global yönetici");
    public static readonly RoleDefinition TenantManager = new("TenantManager", "Tenant genel yöneticisi");
    public static readonly RoleDefinition UnitManager = new("UnitManager", "Birim yöneticisi");
    public static readonly RoleDefinition HumanResources = new("HumanResources", "İK sorumlusu");
    public static readonly RoleDefinition Recruiter = new("Recruiter", "İlan oluşturucusu");
    public static readonly RoleDefinition Approver = new("Approver", "Belge/onay sorumlusu");
    public static readonly RoleDefinition Candidate = new("Candidate", "Başvuru yapan kullanıcı");

    public static readonly RoleDefinition[] All =
    {
        Admin,
        TenantManager,
        UnitManager,
        HumanResources,
        Recruiter,
        Approver,
        Candidate
    };

    public sealed record RoleDefinition(string Name, string Description);
}
