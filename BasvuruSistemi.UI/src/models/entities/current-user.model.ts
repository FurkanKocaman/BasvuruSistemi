export interface CurrentUserModel {
  id: string;
  firstName: string;
  lastName: string;
  fullName: string;

  nationality?: string;
  tckn?: string;
  profileStatus: string;

  address: Address;
  contact: Contact;
}

export interface Address {
  country?: string;
  city?: string;
  district?: string;
  street?: string;
  fullAddress?: string;
  postalCode?: string;
}

export interface Contact {
  email?: string;
  phone?: string;
}
