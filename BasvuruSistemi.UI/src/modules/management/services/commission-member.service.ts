import api from "@/services/Axios";
import { AddMemberToCommissionModel } from "../models/add-member-to-commission.model";
import { useToastStore } from "@/modules/toast/store/toast.store";

class CommissionMember {
  toastStore = useToastStore();
  async addMemberToCommission(request: AddMemberToCommissionModel): Promise<string> {
    try {
      const res = await api.post(`${import.meta.env.VITE_API_URL}/commission-members`, request);

      this.toastStore.addToast({
        message: res.status == 200 ? "Komisyon üyesi eklendi" : "Hata oluştu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new CommissionMember();
