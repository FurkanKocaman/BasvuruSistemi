export interface EducationCreateModel {
  id?: string;
  institution: string;
  department: string;
  startDate: string;
  graduationDate?: string;
  gpa?: number;
  degree?: string;
  description?: string;
}
