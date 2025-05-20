namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record CertificationDto(
    string Title,
    string Issuer,
    DateOnly DateReceived,
    DateOnly? ExpiryDate = null
    );
