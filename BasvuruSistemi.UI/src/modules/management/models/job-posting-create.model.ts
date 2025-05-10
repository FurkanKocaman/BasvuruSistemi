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
  salaryRange?: string;
  skillsRequired?: string;

  contactInfo?: string;
  isPublic: boolean;

  companyId: string;
  departmentId?: string;

  formTemplateId: string;

  postingGroupId?: string;
}
