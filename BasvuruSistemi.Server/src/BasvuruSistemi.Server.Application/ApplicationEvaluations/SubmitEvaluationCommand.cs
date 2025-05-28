using BasvuruSistemi.Server.Application.Services;
using BasvuruSistemi.Server.Domain.ApplicationEvaluations;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.DTOs;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace BasvuruSistemi.Server.Application.ApplicationEvaluations;
public sealed record SubmitEvaluationCommand(
    Guid ApplicationId,
    Guid EvaluationPipelineStageId,
    int status,
    string? overallComment,
    List<FieldValueDto> Values
    ) : IRequest<Result<string>>;

//Değerlendiren kişinin değerlendirme formunu doldurup göndermesi için kullanılan command (Örn : Ön değerlendirme) 

internal sealed class SubmitEvaluationCommandHandler(
    ICurrentUserService currentUserService,
    IApplicationRepository applicationRepository,
    IApplicationEvaluationRepository applicationEvaluationRepository,
    IApplicationEvaluationValueRepository applicationEvaluationValueRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<SubmitEvaluationCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SubmitEvaluationCommand request, CancellationToken cancellationToken)
    {
        using(var transaction = unitOfWork.BeginTransaction())
        {
            try
            {
                Guid? userId = currentUserService.UserId;
                if (!userId.HasValue)
                    return Result<string>.Failure(401, "Unauthorized");

                if (!Enum.IsDefined(typeof(EvaluationStatus), request.status))
                {
                    return Result<string>.Failure($"Invalid EvaluationStatus: {request.status}.");
                }
                var status = (EvaluationStatus)request.status;

                var application = await applicationRepository.Where(p => p.Id == request.ApplicationId).Include(p => p.JobPosting).FirstOrDefaultAsync(cancellationToken);

                if (application is null)
                    return Result<string>.Failure(404, "Application not found");

                var applicationEvaluation = new ApplicationEvaluation(
                    request.ApplicationId,
                    request.EvaluationPipelineStageId,
                    userId.Value,
                    status,
                    request.overallComment
                    );

                applicationEvaluationRepository.Add(applicationEvaluation);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                foreach(var value in request.Values)
                {
                    var fieldValue = new ApplicationEvaluationValue(applicationEvaluation.Id,value.FieldDefinitionId,value.Value);

                    applicationEvaluationValueRepository.Add(fieldValue);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);

                await unitOfWork.CommitTransactionAsync(transaction);
                return Result<string>.Succeed("Application evaluation submitted");
            }
            catch(Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync( transaction );
                return Result<string>.Failure(ex.Message);
            }
        }
       
    }
}
