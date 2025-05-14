import { Result } from "@/models/entities/result.model";
import { TenantCreateModel } from "../models/tenant-create.model";
import api from "@/services/Axios";
import { useToastStore } from "@/modules/toast/store/toast.store";

class TenantService {
  toastSore = useToastStore();
  async createTenant(reqeust: TenantCreateModel): Promise<Result<string>> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/tenants`,
        reqeust
      );

      if (res.status == 200) {
        this.toastSore.addToast({
          message: res.data.data,
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastSore.addToast({
          message: res.data.data,
          type: "error",
          duration: 3000,
        });
      }
      return res.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}
export default new TenantService();
