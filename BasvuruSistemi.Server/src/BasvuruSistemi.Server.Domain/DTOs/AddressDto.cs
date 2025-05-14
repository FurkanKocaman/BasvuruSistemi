namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record AddressDto(
     string? Country ,
     string? City,
     string? District ,
     string? Street ,
     string? FullAddress ,
     string? PostalCode 
    );
