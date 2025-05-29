using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Applications;
using BasvuruSistemi.Server.Domain.Enums;
using BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
using BasvuruSistemi.Server.Domain.Users;

namespace BasvuruSistemi.Server.Domain.ApplicationEvaluations;
public sealed class ApplicationEvaluation : Entity
{
    public Guid ApplicationId { get; private set; } // Foreign Key: Application (Mevcut Application modeliniz)
    public Application Application { get; private set; } = default!;

    //public Guid EvaluationStageId { get; private set; } // Foreign Key: EvaluationStage
    //public EvaluationStage EvaluationStage { get; private set; } = default!;

    //public Guid? AssignedCommissionId { get; private set; } // Foreign Key: ApprovalCommission (Bu değerlendirmeyi hangi komisyon yapıyor?)
    //public ApprovalCommission? AssignedCommission { get; private set; }

    public Guid JobPostingEvaluationPipelineStageId { get; private set; } // Foreign Key: JobPostingEvaluationPipelineStage (Bu değerlendirme hangi aşamada yapılıyor?)
    public JobPostingEvaluationPipelineStage JobPostingEvaluationPipelineStage { get; private set; } = default!;

    public Guid? EvaluatorId { get; private set; } // Foreign Key: User (Komisyon üyesi veya bireysel değerlendirici)
    public AppUser? Evaluator { get; private set; }

    public EvaluationStatus Status { get; private set; } = EvaluationStatus.Pending;
    public DateTimeOffset? EvaluationDate { get; private set; }
    public string? OverallComment { get; private set; } // Değerlendiricinin genel yorumu
    public decimal? Score { get; private set; } // Eğer bir puanlama sistemi varsa

    public ICollection<ApplicationEvaluationValue> EvaluationValues { get; private set; } = new List<ApplicationEvaluationValue>();

    private ApplicationEvaluation() { }
    public ApplicationEvaluation(Guid applicationId, Guid jobPostingEvaluationPipelineStageId, Guid? evaluatorId, EvaluationStatus status, string? overallComment)
    {
        Id = Guid.CreateVersion7();
        ApplicationId = applicationId;
        JobPostingEvaluationPipelineStageId = jobPostingEvaluationPipelineStageId;
        Status = status;
        OverallComment = overallComment;
        EvaluatorId = evaluatorId;
        CreatedAt = DateTimeOffset.Now;
    }
    public void Update(EvaluationStatus status, string? overallComment)
    {
        Status = status;
        OverallComment = overallComment;
    }

    public void SetActive(bool active)
    {
        IsActive = active;
    }
}
