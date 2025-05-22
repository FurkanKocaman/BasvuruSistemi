export interface RoleDetailsModel {
  id: string;
  name?: string;
  description?: string;
  claims: string[];
  usersCount: number;
  createdAt: string;
}
