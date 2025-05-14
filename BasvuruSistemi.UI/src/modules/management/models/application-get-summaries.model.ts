export interface ApplicationGetSummariesModel {
  id: string;
  userId: string;
  userFullName: string;
  tckn?: string;
  appliedDate: string;
  status: string;
  reviewDate?: string;
  reviewDescription?: string;
}
