// Profil modeli için temel arayüz
export interface Profile {
  id: string;
  firstName: string;
  lastName: string;
  tcKimlikNo: string;
  maritalStatus: MaritalStatus;
  profilePicture: string;
  education: Education[];
  certificates: Certificate[];
  workExperience: WorkExperience[];
  skills: Skill[];
}

// Medeni durum için enum
export enum MaritalStatus {
  Single = 'Bekar',
  Married = 'Evli',
  Divorced = 'Boşanmış',
  Widowed = 'Dul'
}

// Eğitim bilgileri için arayüz
export interface Education {
  id: string;
  schoolName: string;
  degree: string;
  fieldOfStudy: string;
  startDate: string;
  endDate: string | null;
  isCurrentlyStudying: boolean;
  description: string;
}

// Sertifikalar için arayüz
export interface Certificate {
  id: string;
  name: string;
  issuingOrganization: string;
  issueDate: string;
  expirationDate: string | null;
  credentialId: string;
  credentialUrl: string;
}

// İş deneyimi için arayüz
export interface WorkExperience {
  id: string;
  title: string;
  companyName: string;
  location: string;
  startDate: string;
  endDate: string | null;
  isCurrentlyWorking: boolean;
  description: string;
}

// Yetenekler için arayüz
export interface Skill {
  id: string;
  name: string;
  level: SkillLevel;
}

// Yetenek seviyesi için enum
export enum SkillLevel {
  Beginner = 'Başlangıç',
  Intermediate = 'Orta',
  Advanced = 'İleri',
  Expert = 'Uzman'
}
