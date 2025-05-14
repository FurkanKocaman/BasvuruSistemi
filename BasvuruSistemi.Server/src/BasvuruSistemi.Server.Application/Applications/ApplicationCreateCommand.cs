using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationFieldValues;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostings;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.Applications;
public sealed record ApplicationCreateCommand(
    Guid jobPostingId,
    List<FieldValueDto> fieldValues
    ) : IRequest<Result<string>>;

internal sealed class ApplicationCreateCommandHandler(
    ICurrentUserService currentUserService,
    IJobPostingRepository jobPostingRepository,
    IApplicationRepository applicationRepository,
    IApplicationFieldValueRepository applicationFieldValueRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<ApplicationCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ApplicationCreateCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;
                if (!userId.HasValue)
                    return Result<string>.Failure("User nor found");

                var jobPosting = await jobPostingRepository.Where(p => p.Id == request.jobPostingId)
                    .Include(p => p.Applications)
                    .FirstOrDefaultAsync(cancellationToken);

                if(jobPosting is null)
                    return Result<string>.Failure("Job posting not found.");

                if (jobPosting.Applications.Any(p => p.UserId == userId))
                    return Result<string>.Failure("You have already applied for this job posting.");

                if(!jobPosting.IsOpenForApplication())
                    return Result<string>.Failure("Application deadline has passed or not published.");

                var application = new Domain.Applications.Application(request.jobPostingId,userId.Value, DateTimeOffset.Now,ApplicationStatus.Pending);

                applicationRepository.Add(application);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                foreach(var field in request.fieldValues)
                {
                    var fieldValue = new ApplicationFieldValue(application.Id, field.FieldDefinitionId, field.Value);
                    applicationFieldValueRepository.Add(fieldValue);
                }
                await unitOfWork.SaveChangesAsync(cancellationToken);

                await unitOfWork.CommitTransactionAsync(transaction);

                return Result<string>.Succeed("Application created successfully.");
            }
            catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync(transaction);
                return Result<string>.Failure("Exception : " + ex);
            }
        }
    }
}
