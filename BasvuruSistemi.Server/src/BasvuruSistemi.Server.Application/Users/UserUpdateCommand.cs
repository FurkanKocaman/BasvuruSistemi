using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Users;
public sealed record UserUpdateCommand(
    ) : IRequest<Result<string>>;

internal sealed class UserUpdateCommandHandler(
    ) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    public Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
