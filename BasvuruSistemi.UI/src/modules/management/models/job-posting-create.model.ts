export interface JobPostingCreateModel {
  title: string;
  description: string;
  responsibilities?: string;
  qualifications?: string;
  benefits?: string;

  datePosted: Date;
  applicationDeadLine: Date;
  validFrom?: Date;
  validTo?: Date;

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

  companyId: string;
  departmentId?: string;

  formTemplateId: string;

  postingGroupId?: string;
}
