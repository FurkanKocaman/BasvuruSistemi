export interface ActiveJobPosting {
  id: string;

  title: string;
  description: string;
  responsibilities?: string;
  qualifications?: string;
  benefits?: string;

  validFrom?: string; // ISO date string
  validTo?: string; // ISO date string

  isRemote: boolean;
  locationText?: string;

  vacancyCount?: number; // Açık pozisyon sayısı (Kontenjan)
  employmentType?: string;
  experienceLevelRequired?: string;
  salaryRange?: string;
  skillsRequired?: string;

  contactInfo?: string;
  isPublic: boolean;

  company: string;
  department?: string;
}
