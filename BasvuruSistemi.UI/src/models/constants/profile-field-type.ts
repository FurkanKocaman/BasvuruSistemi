export enum UserProfileFieldTypeEnum {
  Email = 1,
  PhoneNumber = 2,
  TCKN = 3,
  IBAN = 4,
  FirstName = 5,
  LastName = 6,
  DateOfBirth = 7,
  Gender = 8,
  Country = 9,
  City = 10,
  Address = 11,
}

export const UserProfileFieldTypeOptions = [
  { label: "Email", value: UserProfileFieldTypeEnum.Email },
  { label: "PhoneNumber", value: UserProfileFieldTypeEnum.PhoneNumber },
  { label: "TCKN", value: UserProfileFieldTypeEnum.TCKN },
  { label: "IBAN", value: UserProfileFieldTypeEnum.IBAN },
  { label: "FirstName", value: UserProfileFieldTypeEnum.FirstName },
  { label: "LastName", value: UserProfileFieldTypeEnum.LastName },
  { label: "DateOfBirth", value: UserProfileFieldTypeEnum.DateOfBirth },
  { label: "Gender", value: UserProfileFieldTypeEnum.Gender },
  { label: "Country", value: UserProfileFieldTypeEnum.Country },
  { label: "City", value: UserProfileFieldTypeEnum.City },
  { label: "Address", value: UserProfileFieldTypeEnum.Address },
];
