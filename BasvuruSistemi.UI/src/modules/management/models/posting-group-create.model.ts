export interface PostingGroupCreateModel {
  id?: string;
  name: string;
  description: string;
  unitId?: string;
  isPublic: boolean;
  status: number;
  announcementDate?: Date;
  overallApplicationStartDate?: Date;
  overallApplicationEndDate?: Date;
}
