using BasvuruSistemi.Server.Domain.Enums;

namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record FieldValueResponseDto(
     Guid Id,
     string Title,
     string Type,
     string? Value
    );
