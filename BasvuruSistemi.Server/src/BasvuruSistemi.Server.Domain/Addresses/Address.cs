using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.Addresses;
public sealed class Address : Entity
{
    public string? Country { get; private set; }
    public string? City { get; private set; }
    public string? District { get; private set; }
    public string? Street { get; private set; }
    public string? FullAddress { get; private set; }
    public string? PostalCode { get; private set; }

    public Guid UserId { get; set; }
    public AppUser User { get; set; } = default!;

    public Address() { }
    public Address(string? street, string? district, string? city, string? country, string? postalCode, string? fullAdress, Guid userId)
    {
       
        Street = street;
        District = district;
        City = city;
        Country = country;
        PostalCode = postalCode;
        FullAddress = fullAdress;
        UserId = userId;
    }
    public void Update(string? street, string? district, string? city, string? country, string? postalCode, string? fullAdress)
    {
        Street = street;
        District = district;
        City = city;
        Country = country;
        PostalCode = postalCode;
        FullAddress = fullAdress;
    }

    public override string ToString() =>
        $"{Street}, {District}, {City},{FullAddress} / {PostalCode}";
}