export interface ApplicationByUserModel {
  id: string;

  title: string;
  unit?: string;
  appliedDate: string;
  status: number;
  reviewDate?: string;
  reviewDescription?: string;
  reviewer?: string;
}
