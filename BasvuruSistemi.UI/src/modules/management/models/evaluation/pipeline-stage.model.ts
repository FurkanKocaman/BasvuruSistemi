export interface PipelineStageDto {
  evaluationStageId: string;
  orderInPipeline: number;
  isMandatory: boolean;
  evaluationFormId: string;
  responsibleCommissionId: string;
  startDate?: Date;
  endDate?: Date;
}
