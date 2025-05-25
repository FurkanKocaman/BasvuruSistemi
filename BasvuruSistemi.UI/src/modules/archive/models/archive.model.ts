export interface ArchiveEntry {
  id: number;
  year: number;
  name: string;
  description: string;
  createdAt: string;
  status: 'ready' | 'processing' | 'error';
  fileSize?: string;
}

export interface UserArchiveData {
  id: number;
  fullName: string;
  tcKimlikNo: string;
  email: string;
  phone: string;
  registrationDate: string;
  lastLoginDate: string;
}

export interface ApplicationArchiveData {
  id: number;
  jobTitle: string;
  applicantName: string;
  applicationDate: string;
  status: string;
  score?: number;
}

export interface JobListingArchiveData {
  id: number;
  title: string;
  department: string;
  publishDate: string;
  endDate: string;
  applicationsCount: number;
  status: string;
}
