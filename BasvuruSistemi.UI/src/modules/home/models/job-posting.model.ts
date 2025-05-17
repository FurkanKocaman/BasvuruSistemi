export interface JobPostingModel {
  id: string;

  title: string;
  description: string;
  responsibilities?: string;
  qualifications?: string;
  benefits?: string;

  datePosted: string;
  applicationDeadline: string;
  validFrom?: string;
  validTo?: string;

  status: number; // Enum
  isRemote: boolean;
  locationText?: string;

  vacancyCount?: number;
  employmentType?: number; // Enum
  experienceLevelRequired?: number; // Enum
  skillsRequired?: string;

  allowedNationalIds?: string[];

  contactInfo?: string;
  isPublic: boolean;

  minSalary?: number;
  maxSalary?: number;
  currency?: string;

  unitId?: string;

  formTemplateId: string;

  postingGroupId?: string;
}
