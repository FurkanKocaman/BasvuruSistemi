namespace BasvuruSistemi.Server.Domain.Enums;
public enum SkillLevel { Beginner, Intermediate, Advanced, Expert }

public enum ProfileStatus
{
    Draft,
    Completed,
    Verified
}
public enum ApplicationStatus
{
    Pending,
    Approved,
    Rejected
}

public enum VerificationStatus
{
    Pending,
    Approved,
    Rejected
}
public enum FieldTypeEnum
{
    Text,
    Number,
    Date,
    File,
    Dropdown,
    Checkbox
}