export interface ExperienceCreateModel {
  id?: string;
  companyName: string;
  position: string;
  startDate: string;
  endDate?: string;
  description?: string;
  location?: string;
}
