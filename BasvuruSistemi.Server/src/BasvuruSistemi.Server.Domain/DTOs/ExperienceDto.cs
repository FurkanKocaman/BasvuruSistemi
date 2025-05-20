namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record ExperienceDto(
    Guid Id,
    string CompanyName,
    string Position,
    string? Location,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description
    ); 
