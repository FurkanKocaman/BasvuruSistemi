namespace BasvuruSistemi.Server.Application.Services;
public interface IPublishJob
{
    Task Publish(Guid jobPostingId);
    Task Close(Guid jobPostingId);
}
