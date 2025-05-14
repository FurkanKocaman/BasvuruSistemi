export interface GetActiveJobPostingsQueryResponse {
  id: string;

  title: string;
  description: string;
  responsibilities?: string;
  qualifications?: string;
  benefits?: string;

  validFrom?: string;
  validTo?: string;

  isRemote: boolean;
  locationText?: string;

  vacancyCount?: number;
  employmentType?: string;
  experienceLevelRequired?: string;
  salaryRange?: string;
  skillsRequired?: string;

  contactInfo?: string;
  isPublic: boolean;

  unit: string;

  formTemplateId: string;
}
