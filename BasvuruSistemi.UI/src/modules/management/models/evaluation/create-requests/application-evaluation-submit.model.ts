import { FieldValueModel } from "@/modules/home/models/field-value.model";

export interface SubmitEvaluationModel {
  applicationId: string;
  evaluationPipelineStageId: string;
  status: number;
  overallComment?: string;
  values: FieldValueModel[];
}
