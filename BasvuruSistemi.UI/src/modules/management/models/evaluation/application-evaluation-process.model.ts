export interface ApplicationEvaluationProcessModel {
  stageId: string;
  stageName: string;

  evaluationFormId?: string;
  evaluationFormName?: string;

  commissionId?: string;
  commissionName?: string;

  commissionEvaluationSummaries: ApplicationEvaluationSummary[];
}

export interface ApplicationEvaluationSummary {
  userId?: string;
  userName?: string;

  status: number;

  statusDescription?: string;
}
