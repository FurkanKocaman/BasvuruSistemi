using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record JobPostingSummaryDto(
    Guid Id,
    string Title,
    DateTimeOffset? ValidFrom,
    DateTimeOffset? ValidTo,
    int TotalApplicationsCount,
    JobPostingStatus Status,
    string? Unit
);
