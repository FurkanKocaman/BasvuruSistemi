using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record SkillDto(
    Guid Id,
    string Name,
    string? Description,
    SkillLevel level
    );
  