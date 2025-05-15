export enum ApplicationStatusEnum {
  Pending = 0,
  InReview = 1,
  Approved = 2,
  Rejected = 3,
  Withdrawn = 4,
  Expired = 5,
  InterviewScheduled = 6,
  OfferMade = 7,
  Hired = 8,
}

export const ApplicationStatusOptions = [
  { label: "Beklemede", value: ApplicationStatusEnum.Pending, icon: "TextCursorInput" },
  { label: "Değerlendirmede", value: ApplicationStatusEnum.InReview, icon: "TextCursorInput" },
  { label: "Onaylandı", value: ApplicationStatusEnum.Approved, icon: "TextCursorInput" },
  { label: "Reddedildi", value: ApplicationStatusEnum.Rejected, icon: "TextCursorInput" },
  { label: "Geri Çekildi", value: ApplicationStatusEnum.Withdrawn, icon: "TextCursorInput" },
  { label: "Süresi Doldu", value: ApplicationStatusEnum.Expired, icon: "TextCursorInput" },
  {
    label: "Görüşme Planlandı",
    value: ApplicationStatusEnum.InterviewScheduled,
    icon: "TextCursorInput",
  },
  { label: "Teklif Yapıldı", value: ApplicationStatusEnum.OfferMade, icon: "TextCursorInput" },
  { label: "İşe ALındı", value: ApplicationStatusEnum.Hired, icon: "TextCursorInput" },
];

export function getApplicationStatusOptionByValue(value: number | string) {
  return ApplicationStatusOptions.find((option) => option.value === Number(value));
}

export function getApplicationStatusOptionByLabel(label: string) {
  return ApplicationStatusOptions.find((option) => option.label === label);
}
