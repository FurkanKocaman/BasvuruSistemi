export enum FieldTypeEnum {
  Textbox = 1,
  Textarea = 2,
  Checkbox = 3,
  RadioButton = 4,
  Dropdown = 5,
  File = 6,
  DatePicker = 7,
  Number = 8,
  Email = 9,
  PhoneNumber = 10,
  TCKN = 11,
  IBAN = 12,
  Image = 13,
  URL = 14,
  EDevletVerifiedFile = 15,
  YoksisAlesDocument = 16,
  YoksisAlesScore = 17,
  YoksisKpssScore = 18,
}

export const FieldTypeOptions = [
  { label: "Textbox", value: FieldTypeEnum.Textbox, icon: "TextCursorInput" },
  { label: "Textarea", value: FieldTypeEnum.Textarea, icon: "AlignLeft" },
  { label: "Checkbox", value: FieldTypeEnum.Checkbox, icon: "CheckSquare" },
  { label: "Radio Button", value: FieldTypeEnum.RadioButton, icon: "Dot" },
  { label: "Dropdown", value: FieldTypeEnum.Dropdown, icon: "ChevronsDownUp" },
  { label: "File", value: FieldTypeEnum.File, icon: "File" },
  { label: "Date Picker", value: FieldTypeEnum.DatePicker, icon: "Calendar" },
  { label: "Number", value: FieldTypeEnum.Number, icon: "Hash" },
  { label: "Email", value: FieldTypeEnum.Email, icon: "Mail" },
  { label: "Phone Number", value: FieldTypeEnum.PhoneNumber, icon: "Phone" },
  { label: "TCKN", value: FieldTypeEnum.TCKN, icon: "IdCard" },
  { label: "IBAN", value: FieldTypeEnum.IBAN, icon: "CreditCard" },
  { label: "Image", value: FieldTypeEnum.Image, icon: "Image" },
  { label: "URL", value: FieldTypeEnum.URL, icon: "Link" },
  {
    label: "e-Devlet Doğrulamalı Dosya",
    value: FieldTypeEnum.EDevletVerifiedFile,
    icon: "ShieldCheck",
  },
  { label: "YÖKSİS ALES Belgesi", value: FieldTypeEnum.YoksisAlesDocument, icon: "FileCheck" },
  { label: "YÖKSİS ALES Puanı", value: FieldTypeEnum.YoksisAlesScore, icon: "BarChart2" },
  { label: "YÖKSİS KPSS Puanı", value: FieldTypeEnum.YoksisKpssScore, icon: "BarChartBig" },
];

export function getFieldTypeOptionByValue(value: number | string) {
  return FieldTypeOptions.find((option) => option.value === Number(value));
}
