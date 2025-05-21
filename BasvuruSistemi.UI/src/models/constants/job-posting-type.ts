export enum JobPostingTypeEnum {
  JobPosting = 1,
  PostingGroup = 2,
}

export const JobPostingTypeOptions = [
  { label: "İlan", value: JobPostingTypeEnum.JobPosting, icon: "TextCursorInput" },
  { label: "İlan Grubu", value: JobPostingTypeEnum.PostingGroup, icon: "TextCursorInput" },
];

export function getJobPostingtypeOptionByValue(value: number | string) {
  return JobPostingTypeOptions.find((option) => option.value === Number(value));
}

export function getJobPostingTypeOptionByLabel(label: string) {
  return JobPostingTypeOptions.find((option) => option.label === label);
}
