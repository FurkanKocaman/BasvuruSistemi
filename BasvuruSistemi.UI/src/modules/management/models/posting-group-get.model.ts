export interface PostingGroupGetModel {
  id: string;

  name: string;
  description?: string;

  isPublic: boolean;
  status: number;

  announcementdate?: string;
  overallApplicationStartDate?: string;
  overallApplicationEndDate?: string;

  parentPostingGroupId?: string;
  parentPostingGroup?: string;

  tenantId: string;
  tenant: string;

  unitId?: string;
  unit?: string;

  jobPostings: JobPostingSummaryDto[];
}

export interface JobPostingSummaryDto {
  id: string;
  title: string;
  description: string;
  responsibilities?: string;
  qualifications?: string;
  validFrom?: string;
  validTo?: string;
  totalApplicationsCount: number;
  status: number;
  unit?: string;
}
