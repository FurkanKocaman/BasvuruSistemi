namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record CertificationDto(
    Guid Id,
    string Title,
    string Issuer,
    DateOnly DateReceived,
    DateOnly? ExpiryDate = null
    );
