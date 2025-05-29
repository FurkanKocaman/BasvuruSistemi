export enum EvaluationStatus {
  Pending = 0,
  InProgress = 1,
  Submitted = 2,
  Accepted = 3,
  Rejected = 4,
  NeedsRevision = 5,
}

export const EvaluationStatusOptions = [
  { label: "Beklemede", value: EvaluationStatus.Pending, icon: "TextCursorInput" },
  { label: "Değerlendirmede", value: EvaluationStatus.InProgress, icon: "TextCursorInput" },
  { label: "Kaydedildi", value: EvaluationStatus.Submitted, icon: "TextCursorInput" },
  { label: "Onaylandı", value: EvaluationStatus.Accepted, icon: "TextCursorInput" },
  { label: "Reddedildi", value: EvaluationStatus.Rejected, icon: "TextCursorInput" },
  { label: "Revize İstendi", value: EvaluationStatus.NeedsRevision, icon: "TextCursorInput" },
];

export function getEvaluationStatusByValue(value: number | string) {
  return EvaluationStatusOptions.find((option) => option.value === Number(value));
}
