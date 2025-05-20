namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record EducationDto(
    string Institution,
    string Department,
    string? Degree,
    string? Description,
    DateOnly StartDate,
    DateOnly GraduationDate,
    double? GPA = null
    );
