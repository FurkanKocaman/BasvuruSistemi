using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed class ChangeJobPostingStatusDto
{
    public JobPostingStatus NewStatus { get; set; }
    public DateTimeOffset? PublishStartDate { get; set; }
}
