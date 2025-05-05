using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.Employers;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using BasvuruSistemi.Server.Domain.ValueObjects;
using MediatR;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Employers;
public sealed record EmployerCreateCommand(
    string companyName,
    Address headOffice,
    Contact contact,
    string password
    ) : IRequest<Result<string>>;

internal sealed class EmployerCreateCommandHandler(
    IEmployerRepository employerRepository,
    IUnitOfWork unitOfWork,
    IUserService userService
    ) : IRequestHandler<EmployerCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployerCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                var userId = await userService.CreateUserAsync(null, null, true, request.contact.Email!, request.password);

                if (userId == Guid.Empty)
                    return Result<string>.Failure("User wasn't created");

                Employer employer = new(userId, request.companyName, request.headOffice, request.contact);

                employerRepository.Add(employer);

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Employer created successfully");
            }catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Exception : " + ex);
            }
        }
       
    }
}
