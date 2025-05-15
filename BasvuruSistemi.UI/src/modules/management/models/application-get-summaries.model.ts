export interface ApplicationGetSummariesModel {
  id: string;
  userId: string;
  userFullName: string;
  tckn?: string;
  jobPosting: string;
  appliedDate: string;
  status: number;
  reviewDate?: string;
  reviewDescription?: string;
}
