import { FieldTypeEnum } from "@/models/constants/field-type";

export interface EvaluationFormFieldDto {
  id: string;
  evaluationFormId: string;
  fieldtype: FieldTypeEnum;
  label: string;
  options?: string;
  isRequired: boolean;
  order: number;
  placeholder?: string;
  helpText?: string;
  validationRules?: string;
}
