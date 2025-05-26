import api from "@/services/Axios";
import { CommissionGetModel } from "../models/commission-get.model";
import { CommissionCreateModel } from "../models/commission-create.model";
import { Result } from "@/models/entities/result.model";
import { useToastStore } from "@/modules/toast/store/toast.store";

class CommissionService {
  toastStore = useToastStore();
  async getCommissionById(id: string): Promise<CommissionGetModel> {
    try {
      const res = await api.get<CommissionGetModel>(
        `${import.meta.env.VITE_API_URL}/api/commissions/${id}`
      );

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async getCommissions(): Promise<CommissionGetModel[]> {
    try {
      const res = await api.get<CommissionGetModel[]>(
        `${import.meta.env.VITE_API_URL}/api/commissions`
      );

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async createCommission(request: CommissionCreateModel): Promise<string> {
    const res = await api.post<Result<string>>(
      `${import.meta.env.VITE_API_URL}/commissions`,
      request
    );

    this.toastStore.addToast({
      message: res.status == 200 ? "Komisyon oluşturuldu" : "Hata oluştu",
      type: res.status == 200 ? "success" : "error",
      duration: 3000,
    });

    return res.data.data;
  }
  async updateCommission(request: CommissionCreateModel): Promise<string> {
    const res = await api.put<Result<string>>(
      `${import.meta.env.VITE_API_URL}/commissions/${request.id}`,
      request
    );

    this.toastStore.addToast({
      message: res.status == 200 ? "Komisyon oluşturuldu" : "Hata oluştu",
      type: res.status == 200 ? "success" : "error",
      duration: 3000,
    });

    return res.data.data;
  }
}
export default new CommissionService();
