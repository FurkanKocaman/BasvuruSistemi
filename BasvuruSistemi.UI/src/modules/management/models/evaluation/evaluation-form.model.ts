import { EvaluationFormFieldDto } from "./evaluation-form-field.model";

export interface EvaluationFormDto {
  id: string;
  name: string;
  evaluationStageId: string;
  createdAt: string;
  fields: EvaluationFormFieldDto[];
  description?: string;
}
