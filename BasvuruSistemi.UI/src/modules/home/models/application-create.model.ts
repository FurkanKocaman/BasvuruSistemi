import { FieldValueModel } from "./field-value.model";

export interface ApplicationCreateRequest {
  jobPostingId: string;
  fieldValues: FieldValueModel[];
}
