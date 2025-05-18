export interface GetActiveJobPostingsSummariesQueryResponse {
  id: string;

  type: number;
  title: string;
  description: string;
  qualifications?: string;

  validTo?: string;

  locationText?: string;

  vacancyCount?: number;
  salaryRange?: string;

  isPublic: boolean;

  tenant: string;
  unit?: string;
}
