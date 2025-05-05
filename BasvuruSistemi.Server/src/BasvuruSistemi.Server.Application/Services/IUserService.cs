namespace BasvuruSistemi.Server.Application.Services;
public interface IUserService
{
    public Task<Guid> CreateUserAsync(string? firstName, string? lastName, bool isEmployer, string email,string password);
}
