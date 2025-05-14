import { FormFieldDefinition } from "./form-field.model";

export interface FormTemplateCreateReqeust {
  id?: string;
  name: string;
  description?: string;
  fields: FormFieldDefinition[];
}
