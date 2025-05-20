import { Address } from "./address-model";
import { Contact } from "./contact-model";

export interface CurrentUserModel {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;

  avatarUrl?: string;

  nationality?: string;
  tckn?: string;
  profileStatus: string;

  address: Address;
  contact: Contact;
}
