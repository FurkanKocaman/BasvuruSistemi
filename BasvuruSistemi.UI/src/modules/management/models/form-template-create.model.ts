import { FormFieldDefinition } from "./form-field.model";

export interface FormTemplateCreateReqeust {
  name: string;
  description?: string;
  fields: FormFieldDefinition[];
}
