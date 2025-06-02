import { FieldValueModel } from "./field-value.model";

export interface ApplicationCreateRequest {
  applicationId?: string;
  jobPostingId: string;
  fieldValues: FieldValueModel[];
}
