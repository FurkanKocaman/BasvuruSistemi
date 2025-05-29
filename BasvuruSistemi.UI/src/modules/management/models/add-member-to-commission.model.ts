export interface AddMemberToCommissionModel {
  id?: string;
  commissionId: string;
  email: string;
  role: string;
  isManager: boolean;
}
