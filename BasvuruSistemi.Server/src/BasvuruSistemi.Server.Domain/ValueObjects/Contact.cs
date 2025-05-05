namespace BasvuruSistemi.Server.Domain.ValueObjects;
public sealed class Contact
{
    public string? Email { get; set; }
    public string? Phone { get; set; }

    private Contact() { }
    public Contact(string email, string phone)
    {
        if (!email.Contains("@")) throw new ArgumentException("Email is invalid.");
        Email = email;
        Phone = phone;
    }
}
