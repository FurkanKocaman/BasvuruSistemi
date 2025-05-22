export interface JobPostingCreateModel {
  id?: string;

  title: string;
  description: string;
  responsibilities?: string;
  qualifications?: string;
  benefits?: string;

  datePosted: Date;
  applicationDeadline: Date;
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
  isAnonymous: boolean;

  minSalary?: number;
  maxSalary?: number;
  currency?: string;

  unitId?: string;

  formTemplateId: string;

  postingGroupId?: string;
}
