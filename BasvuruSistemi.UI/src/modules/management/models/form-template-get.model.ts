import { EntityDto } from "@/models/entities/entity.model";
import { FormFieldResponse } from "./form-filed-response.model";

export interface FormTemplateGetModel extends EntityDto {
  name: string;
  description?: string;
  fields: FormFieldResponse[];
}
