import { FieldValueResponseModel } from "@/modules/home/models/field-value-response.model";

export interface ApplicationGetDetailModel {
  id: string;

  userId: string;
  firstName: string;
  lastName: string;
  email?: string;
  phone?: string;
  tckn?: string;

  country?: string;
  city?: string;
  district?: string;
  street?: string;
  fullAddress?: string;
  postalCode?: string;

  jobPostingId: string;
  jobPostingTitle: string;
  jobPostingCreateDate: string; //Date
  jobPostingVacancyCount?: number;
  totalApplicationCount?: number;
  unit: string;

  appliedDate: string;
  status: number;
  reviewDate?: string;
  reviewDescription?: string;

  fieldValues: FieldValueResponseModel[];
}
