namespace BasvuruSistemi.Server.Domain.ValueObjects;
public sealed class Address
{
    public string? Country { get; }
    public string? City { get; }
    public string? District { get; }
    public string? Street { get; }
    public string? FullAddress { get; }
    public string? PostalCode { get; }

    public Address() { }
    public Address(string street, string district, string city, string postalCode, string fullAdress)
    {
        if (new[] { street, district, city, postalCode, fullAdress }.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("All address fields must be provided.");
        Street = street;
        District = district;
        City = city;
        PostalCode = postalCode;
        FullAddress = fullAdress;
    }

    public override string ToString() =>
        $"{Street}, {District}, {City},{FullAddress} / {PostalCode}";
}
