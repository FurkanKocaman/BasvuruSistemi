export interface ApplicationByUserModel {
  id: string;

  title: string;
  unit?: string;
  appliedDate: string;
  status: string;
  reviewDate?: string;
  reviewDescription?: string;
  reviewer?: string;
}
