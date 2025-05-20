namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record ExperienceDto(
    string CompanyName,
    string Position,
    string? Location,
    DateOnly StartDate,
    DateOnly? EndDate,
    string? Description
    ); 
