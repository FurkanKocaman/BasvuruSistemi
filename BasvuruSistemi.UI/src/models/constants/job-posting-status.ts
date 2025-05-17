export enum JobPostingStatusEnum {
  Draft = 1,
  Published = 2,
  OnHold = 3,
  Closed = 4,
  Expired = 5,
  Archived = 6,
}

export const JobPostingStatusOptions = [
  { label: "Taslak", value: JobPostingStatusEnum.Draft, icon: "TextCursorInput" },
  { label: "Yayında", value: JobPostingStatusEnum.Published, icon: "TextCursorInput" },
  { label: "Durdurulmuş", value: JobPostingStatusEnum.OnHold, icon: "TextCursorInput" },
  { label: "Kapatılmış", value: JobPostingStatusEnum.Closed, icon: "TextCursorInput" },
  {
    label: "Son Başvuru Tarihi geçti",
    value: JobPostingStatusEnum.Expired,
    icon: "TextCursorInput",
  },
  { label: "Arşivlendi", value: JobPostingStatusEnum.Archived, icon: "TextCursorInput" },
];

export function getJobPostingStatusOptionByValue(value: number | string) {
  return JobPostingStatusOptions.find((option) => option.value === Number(value));
}

export function getJobPostingStatusOptionByLabel(label: string) {
  return JobPostingStatusOptions.find((option) => option.label === label);
}
