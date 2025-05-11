//Create form template de kullanılıyor
export interface FormFieldDefinition {
  label: string;
  type: number;
  isRequired: boolean;
  description?: string;
  placeholder?: string;
  optionsJson?: string;
  isReadOnly: boolean;
  defaultValue?: string;
  allowedFileTypes?: string;
  maxFileSizeMB?: number;
}
