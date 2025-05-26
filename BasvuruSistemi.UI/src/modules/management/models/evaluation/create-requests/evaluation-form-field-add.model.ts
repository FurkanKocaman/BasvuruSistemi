export interface AddFormFieldToEvaluationFormModel {
  id?: string;
  evaluationFormId?: string;
  label: string;
  fieldType: number;
  options?: string;
  isRequired: boolean;
  order: number;
  placeholder?: string;
  helpText?: string;
  validationRules?: string;
}
