namespace BasvuruSistemi.Server.Application.Services;
public interface IPublishJob
{
    Task Execute(Guid jobPostingId);
}
