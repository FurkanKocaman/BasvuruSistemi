export interface UserSummariesModel {
  id: string;
  avatarUrl?: string;
  firstName: string;
  lastName: string;
  email?: string;
  tckn?: string;
  roles: RoleDto[];
  createdAt: string;
}

export interface RoleDto {
  id: string;
  name?: string;
}
