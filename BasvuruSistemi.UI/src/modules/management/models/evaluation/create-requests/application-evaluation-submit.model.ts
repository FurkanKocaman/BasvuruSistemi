import { FieldValueModel } from "@/modules/home/models/field-value.model";

export interface SubmitEvaluationModel {
  id: string;
  status: number;
  overallComment?: string;
  values: FieldValueModel[];
}
