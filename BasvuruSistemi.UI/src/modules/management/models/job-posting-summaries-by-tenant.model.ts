export interface JobPostingSummariesByTenantResponse {
  id: string;

  title: string;
  type: number;

  validFrom?: string;
  validTo?: string;

  isRemote: boolean;
  locationText?: string;

  vacancyCount?: number;
  totalApplicationsCount?: number;

  isPublic: boolean;
  status: number;

  unit: string;
  createdAt: string;
}
