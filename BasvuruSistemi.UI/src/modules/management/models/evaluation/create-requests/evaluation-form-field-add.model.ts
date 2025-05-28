export interface AddFormFieldToEvaluationFormModel {
  id?: string;
  evaluationFormId: string;

  label: string;
  type: number;
  isRequired: boolean;
  order: number;
  description?: string;
  placeholder?: string;
  optionsJson?: string;

  isReadOnly: boolean;
  defaultValue?: string;

  verificationSource: number;
  verificationParametersJson?: string;

  allowedFileTypes?: string;
  maxFileSizeMB?: number;
}
