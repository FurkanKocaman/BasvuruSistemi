using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFieldValues;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record ApplicationUpdateCommand(
    Guid applicationId,
    List<FieldValueDto> fields
    ) : IRequest<Result<string>>;

internal sealed class ApplicationUpdateCommandHandler(
    IApplicationRepository applicationRepository,
    IApplicationFieldValueRepository applicationFieldValueRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ApplicationUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ApplicationUpdateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;
                if(!userId.HasValue)
                    return Result<string>.Failure(404,"User not found");

                var application = await applicationRepository.Where(p => p.Id == request.applicationId && !p.IsDeleted && p.IsActive).Include(p => p.FieldValues).FirstOrDefaultAsync();

                if(application is null)
                    return Result<string>.Failure(404, "Application not found");

                var existingValues = application.FieldValues;

                var dtoIds = request.fields.Select(p => p.Id).ToHashSet();

                foreach(var ev in existingValues)
                {
                    if (!dtoIds.Contains(ev.Id))
                    {
                        ev.IsDeleted = true;
                        applicationFieldValueRepository.Update(ev);
                    }       
                }

                foreach(var dto in request.fields)
                {
                    if (dto.Id.HasValue)
                    {
                        var toUpdate = existingValues.FirstOrDefault(p => p.Id == dto.Id);
                        if (toUpdate is not null)
                        {
                            toUpdate.Update(dto.Value);
                            applicationFieldValueRepository.Update(toUpdate);
                        }
                    }
                    else
                    {
                        var newValue = new ApplicationFieldValue(application.Id, dto.FieldDefinitionId, dto.Value);
                        await applicationFieldValueRepository.AddAsync(newValue);
                    }
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Application updated successfully.");
            }
            catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("An error occurred while updating the application. "+ex.Message);
            }
        }
    }
}
