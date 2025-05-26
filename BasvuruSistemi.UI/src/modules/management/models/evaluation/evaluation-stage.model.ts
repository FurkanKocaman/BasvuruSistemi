import { EvaluationFormDto } from "./evaluation-form.model";

export interface EvaluationStageDto {
  id: string;
  name: string;
  description?: string;
  order: number;
  defaultEvaluationFormId?: string;
  evaluationForms: EvaluationFormDto[];
  createdAt: string;
}
