export enum JobPostingStatusEnum {
  Draft = 1,
  Published = 2,
  OnHold = 3,
  Closed = 4,
  Expired = 5,
  Archived = 6,
}

export const JobPostingStatusOptions = [
  {
    label: "Taslak",
    value: JobPostingStatusEnum.Draft,
    class: "bg-yellow-600",
    icon: "TextCursorInput",
  },
  {
    label: "Yayında",
    value: JobPostingStatusEnum.Published,
    class: "bg-green-600",
    icon: "TextCursorInput",
  },
  {
    label: "Durdurulmuş",
    value: JobPostingStatusEnum.OnHold,
    class: "bg-orange-600",
    icon: "TextCursorInput",
  },
  {
    label: "Kapatılmış",
    value: JobPostingStatusEnum.Closed,
    class: "bg-red-600",
    icon: "TextCursorInput",
  },
  {
    label: "Son Başvuru Tarihi geçti",
    value: JobPostingStatusEnum.Expired,
    class: "bg-red-600",
    icon: "TextCursorInput",
  },
  {
    label: "Arşivlendi",
    value: JobPostingStatusEnum.Archived,
    class: "bg-blue-600",
    icon: "TextCursorInput",
  },
];

export function getJobPostingStatusOptionByValue(value: number | string) {
  return JobPostingStatusOptions.find((option) => option.value === Number(value));
}

export function getJobPostingStatusOptionByLabel(label: string) {
  return JobPostingStatusOptions.find((option) => option.label === label);
}
