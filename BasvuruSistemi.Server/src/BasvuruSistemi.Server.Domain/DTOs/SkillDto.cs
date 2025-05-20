using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record SkillDto(
    string Name,
    string? Description,
    SkillLevel level
    );
