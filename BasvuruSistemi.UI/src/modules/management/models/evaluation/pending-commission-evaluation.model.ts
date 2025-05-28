export interface PendingCommissionEvaluationgetModel {
  id: string;

  userId: string;
  userFullName: string;
  tckn?: string;

  jobPosting: string;
  appliedDate: string;
  status: number;
  reviewDate?: string;
  reviewDescription?: string;

  stageId: string;
  stageName: string;

  evaluationFormId: string;
  evaluationFormName: string;

  evaluationPipelineStageId: string;
}
