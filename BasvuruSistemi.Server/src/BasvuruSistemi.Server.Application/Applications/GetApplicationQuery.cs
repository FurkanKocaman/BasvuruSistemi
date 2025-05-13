using BasvuruSistemi.Server.Domain.DTOs;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record GetApplicationQuery(
    ) : IRequest<Result<GetApplicationQueryResponse>>;

public sealed class GetApplicationQueryResponse
{
    public Guid Id { get; set; }
    public Guid JobPostingId { get; set; }
    public string JobPostingTitle { get; set; } = default!;
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public DateTimeOffset AppliedDate { get; set; }
    public string Status { get; set; } = default!;
    public DateTimeOffset? ReviewDate { get; set; }
    public string? ReviewDescription { get; set; }

    public List<FieldValueDto> FieldValues { get; set; } = new List<FieldValueDto>();
}

internal sealed class GetApplicationQueryHandler(
    ) : IRequestHandler<GetApplicationQuery, Result<GetApplicationQueryResponse>>
{
    public Task<Result<GetApplicationQueryResponse>> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
