import { RoleDto } from "@/modules/management/models/user-summaries.model";
import api from "../../../services/Axios";
import { RoleAssignmentCreateModel } from "@/modules/management/models/role-assignment-create.model";
import { Result } from "@/models/entities/result.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { TokenInformation } from "../models/token-information.model";

class RoleService {
  toastStore = useToastStore();
  async getAllRoles(): Promise<RoleDto[]> {
    try {
      const res = await api.get<RoleDto[]>(`${import.meta.env.VITE_API_URL}/api/roles`);

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async getTokenInformation(token: string): Promise<Result<TokenInformation>> {
    const res = await api.get<Result<TokenInformation>>(
      `${import.meta.env.VITE_API_URL}/api/token/${token}`
    );
    return res.data;
  }
  async verifyInvitation(token: string): Promise<string> {
    const res = await api.patch<Result<string>>(
      `${import.meta.env.VITE_API_URL}/roles/verify-invitation/${token}`
    );
    return res.data.data;
  }
  async createRoleAssignment(request: RoleAssignmentCreateModel): Promise<string> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/roles/invitation`,
        request
      );

      this.toastStore.addToast({
        message: res.status == 200 ? "Kullanıcıya onay maili gönderildi" : "Hata oluştu",
        type: res.status === 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new RoleService();
