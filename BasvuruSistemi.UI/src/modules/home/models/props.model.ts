export interface Props {
  modelValue: string;
  field: {
    label: string;
    placeholder?: string;
    description?: string;
    isRequired: boolean;
    isReadOnly: boolean;
  };
}
