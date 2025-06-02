export interface ApplicationByUserModel {
  id: string;

  jobPostingId: string;
  type: number;

  title: string;
  unit?: string;
  appliedDate: string;
  status: number;
  reviewDate?: string;
  reviewDescription?: string;
  reviewer?: string;
}
