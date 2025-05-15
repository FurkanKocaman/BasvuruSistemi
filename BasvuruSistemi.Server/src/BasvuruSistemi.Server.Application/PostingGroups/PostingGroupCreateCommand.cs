using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.PostingGroups;
public sealed record PostingGroupCreateCommand(
    ) : IRequest<Result<string>>;
