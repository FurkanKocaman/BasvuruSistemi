export interface CertificateCreateModel {
  id?: string;
  title: string;
  issuer: string;
  dateReceived: string;
  expiryDate?: string;
}
