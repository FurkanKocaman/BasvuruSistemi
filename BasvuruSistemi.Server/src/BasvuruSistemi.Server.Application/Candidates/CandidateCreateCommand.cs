using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Candidates;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Candidates;
public sealed record CandidateCreateCommand(
    string firstName,
    string lastName,
    DateOnly dateOfBirth,
    string? nationality,
    string? tckn,
    Address address,
    Contact contact,
    string password
    ) : IRequest<Result<string>>;

internal sealed class CandidateCreateCommandHandler(
    ICandidateRepository candidateRepository,
    IUnitOfWork unitOfWork,
    IUserService userService
    ) : IRequestHandler<CandidateCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CandidateCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                var isCandidateExist = await candidateRepository.Where(p => p.Contact.Email == request.contact.Email).AnyAsync();

                if (isCandidateExist)
                    return Result<string>.Failure("This email is in use");

                var userId = await userService.CreateUserAsync(request.firstName, request.lastName, false, request.contact.Email!,request.password);

                Candidate candidate = new(userId, request.firstName, request.lastName, request.dateOfBirth, request.nationality, request.tckn, request.address, request.contact);

                candidateRepository.Add(candidate);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await unitOfWork.CommitTransactionAsync(transaction);
                return Result<string>.Succeed("Candidate created successfully");
            }
            catch( Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync( transaction );
                return Result<string>.Failure("Exception : " + ex);
            }
        }
    }
}
