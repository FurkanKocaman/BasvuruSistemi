// Başvurular için dummy veri
import { jobData } from './jobs';

export interface Application {
  id: number;
  jobId: number;
  jobTitle: string;
  applicantName: string;
  submissionDate: string;
  status: 'Pending' | 'Approved' | 'Rejected';
  resumeFilename: string;
  department: string;
}

export const applicationData: Application[] = [
  {
    id: 1,
    jobId: 1,
    jobTitle: jobData[0].title,
    applicantName: 'Mehmet Yılmaz',
    submissionDate: '2025-04-20',
    status: 'Pending',
    resumeFilename: 'mehmet_yilmaz_cv.pdf',
    department: jobData[0].department
  },
  {
    id: 2,
    jobId: 3,
    jobTitle: jobData[2].title,
    applicantName: 'Ayşe Kaya',
    submissionDate: '2025-04-22',
    status: 'Approved',
    resumeFilename: 'ayse_kaya_portfolio.pdf',
    department: jobData[2].department
  },
  {
    id: 3,
    jobId: 2,
    jobTitle: jobData[1].title,
    applicantName: 'Ali Demir',
    submissionDate: '2025-04-25',
    status: 'Rejected',
    resumeFilename: 'ali_demir_cv.pdf',
    department: jobData[1].department
  },
  {
    id: 4,
    jobId: 5,
    jobTitle: jobData[4].title,
    applicantName: 'Zeynep Şahin',
    submissionDate: '2025-05-01',
    status: 'Pending',
    resumeFilename: 'zeynep_sahin_cv.pdf',
    department: jobData[4].department
  }
];