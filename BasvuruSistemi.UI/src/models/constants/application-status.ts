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
  JobPostingClosed = 9,
}

export const ApplicationStatusOptions = [
  {
    label: "Beklemede",
    value: ApplicationStatusEnum.Pending,
    class: "bg-yellow-500",
    icon: "TextCursorInput",
  },
  {
    label: "Değerlendirmede",
    value: ApplicationStatusEnum.InReview,
    class: "bg-yellow-500",
    icon: "TextCursorInput",
  },
  {
    label: "Onaylandı",
    value: ApplicationStatusEnum.Approved,
    class: "bg-green-500",
    icon: "TextCursorInput",
  },
  {
    label: "Reddedildi",
    value: ApplicationStatusEnum.Rejected,
    class: "bg-red-500",
    icon: "TextCursorInput",
  },
  {
    label: "Geri Çekildi",
    value: ApplicationStatusEnum.Withdrawn,
    class: "bg-gray-500",
    icon: "TextCursorInput",
  },
  {
    label: "Süresi Doldu",
    value: ApplicationStatusEnum.Expired,
    class: "bg-red-600",
    icon: "TextCursorInput",
  },
  {
    label: "Görüşme Planlandı",
    value: ApplicationStatusEnum.InterviewScheduled,
    class: "bg-blue-500",
    icon: "TextCursorInput",
  },
  {
    label: "Teklif Yapıldı",
    value: ApplicationStatusEnum.OfferMade,
    class: "bg-blue-500",
    icon: "TextCursorInput",
  },
  {
    label: "İşe Alındı",
    value: ApplicationStatusEnum.Hired,
    class: "bg-red-500",
    icon: "TextCursorInput",
  },
  {
    label: "İlan Kapatıldı",
    value: ApplicationStatusEnum.JobPostingClosed,
    class: "bg-red-500",
    icon: "TextCursorInput",
  },
];

export function getApplicationStatusOptionByValue(value: number | string) {
  return ApplicationStatusOptions.find((option) => option.value === Number(value));
}

export function getApplicationStatusOptionByLabel(label: string) {
  return ApplicationStatusOptions.find((option) => option.label === label);
}
