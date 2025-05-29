namespace BasvuruSistemi.Server.Application.Services;
public interface IInReviewApplications
{
    Task Execute(Guid jobPostingId);
}
