import { Address } from "@/models/entities/address-model";

// Profil modeli için temel arayüz
export interface Profile {
  id: string;

  firstName: string;
  lastName: string;
  birthOfDate?: string;
  nationality?: string;
  tckn?: string;
  profileStatus: number;
  avatarUrl?: string;
  email?: string;
  phone?: string;
  addresses: Address[];

  maritalStatus: MaritalStatus;

  educations: Education[];
  certificates: Certificate[];
  experiences: WorkExperience[];
  skills: Skill[];
}

export enum MaritalStatus {
  Single = "Bekar",
  Married = "Evli",
  Divorced = "Boşanmış",
  Widowed = "Dul",
}

export interface Education {
  id: string;
  institution: string;
  department: string;
  degree?: string;
  description?: string;
  startDate: string;
  graduationDate?: string;
  gpa?: number;
}

export interface Certificate {
  id: string;
  title: string;
  issuer: string;
  dateReceived: string;
  expiryDate?: string;
}

export interface WorkExperience {
  id: string;
  companyName: string;
  position: string;
  location?: string;
  startDate: string;
  endDate?: string;
  isCurrentlyWorking: boolean;
  description?: string;
}

export interface Skill {
  id: string;
  name: string;
  description?: string;
  level: number;
}
