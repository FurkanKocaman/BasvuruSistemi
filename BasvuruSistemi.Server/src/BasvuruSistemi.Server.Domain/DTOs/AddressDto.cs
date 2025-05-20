namespace BasvuruSistemi.Server.Domain.DTOs;
public sealed record AddressDto(
     Guid? Id,
     string Name,
     string? Country ,
     string? City,
     string? District ,
     string? Street ,
     string? FullAddress ,
     string? PostalCode 
    );
