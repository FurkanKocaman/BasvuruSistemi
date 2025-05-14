import { Address } from "@/models/entities/address-model";
import { Contact } from "@/models/entities/contact-model";

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  nationality?: string;
  tckn?: string;
  birthOfDate: Date;
  address: Address;
  contact: Contact;
}
