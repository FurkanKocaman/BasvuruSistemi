using BasvuruSistemi.Server.Domain.Abstractions;
using BasvuruSistemi.Server.Domain.Evaluations;
using BasvuruSistemi.Server.Domain.JobPostings;

namespace BasvuruSistemi.Server.Domain.JobPostingEvaluationPipelineStages;
public sealed class JobPostingEvaluationPipelineStage : Entity
{
    public Guid JobPostingId { get; private set; } // Foreign Key: JobPosting
    public JobPosting JobPosting { get; private set; } = default!;

    public Guid EvaluationStageId { get; private set; } // Foreign Key: EvaluationStage (Global şablona referans)
    public EvaluationStage EvaluationStage { get; private set; } = default!;

    public int OrderInPipeline { get; private set; } // Bu aşamanın İŞ İLANINA ÖZEL akıştaki sırası
    public bool IsMandatory { get; private set; } = true; // Bu aşama zorunlu mu?

    public Guid? EvaluationFormId { get; set; } // Foreign Key: EvaluationForm
    public EvaluationForm EvaluationForm { get; set; } = default!;

    public DateTimeOffset? StartDate { get; set; } // Aşamanın başlangıç tarihi (isteğe bağlı)
    public DateTimeOffset? EndDate { get; set; } // Aşamanın bitiş tarihi (isteğe bağlı)

    private JobPostingEvaluationPipelineStage() { }
    public JobPostingEvaluationPipelineStage(Guid jobPostingId, Guid evaluationStageId, int orderInPipeline, bool isMandatory = true, Guid? evaluationFormId = null, DateTimeOffset? startDate = null, DateTimeOffset? endDate = null)
    {
        JobPostingId = jobPostingId;
        EvaluationStageId = evaluationStageId;
        OrderInPipeline = orderInPipeline;
        IsMandatory = isMandatory;
        EvaluationFormId = evaluationFormId;
        CreatedAt = DateTimeOffset.Now;
        StartDate = startDate;
        EndDate = endDate;
    }
    public void UpdateStartAndEndDates(DateTimeOffset? startDate, DateTimeOffset? endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public void UpdateOrderInPipeline(int newOrder)
    {
        if (newOrder < 0)
            throw new ArgumentOutOfRangeException(nameof(newOrder), "Order must be a non-negative integer.");
        OrderInPipeline = newOrder;
    }

    public void SetEvaluationForm(Guid evaluationFormId)
    {
        EvaluationFormId = evaluationFormId;
    }
}
