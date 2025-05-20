export enum SkillLevelEnum {
  None = 0,
  Beginner = 1,
  Intermediate = 2,
  Advanced = 3,
  Expert = 4,
  Master = 5,
}

export const SkillLeveloptions = [
  { label: "Hiçbiri", value: SkillLevelEnum.None, icon: "TextCursorInput" },
  { label: "Başlangıç", value: SkillLevelEnum.Beginner, icon: "TextCursorInput" },
  { label: "Ortalama", value: SkillLevelEnum.Intermediate, icon: "TextCursorInput" },
  { label: "İleri Seviye", value: SkillLevelEnum.Advanced, icon: "TextCursorInput" },
  { label: "Uzman", value: SkillLevelEnum.Expert, icon: "TextCursorInput" },
  { label: "Master", value: SkillLevelEnum.Master, icon: "TextCursorInput" },
];

export function getSkillLevelOptionByValue(value: number | string) {
  return SkillLeveloptions.find((option) => option.value === Number(value));
}

export function getSkillLevelOptionByLabel(label: string) {
  return SkillLeveloptions.find((option) => option.label === label);
}
