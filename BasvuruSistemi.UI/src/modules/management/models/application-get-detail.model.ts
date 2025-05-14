import { FieldValueResponseModel } from "@/modules/home/models/field-value-response.model";

export interface ApplicationGetDetailModel {
  id: string;
  jobPostingId: string;
  jobPostingTitle: string;
  userId: string;
  firstName: string;
  lastName: string;

  appliedDate: string;
  status: string;
  reviewDate?: string;
  reviewDescription?: string;

  fieldValues: FieldValueResponseModel[];
}
