import { Result } from "@/models/entities/result.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import api from "@/services/Axios";

class ApplicationManagementService {
  toastStore = useToastStore();
  async reviewApplication(
    id: string,
    status: number,
    reviewDescription?: string
  ): Promise<Result<string>> {
    try {
      const res = await api.patch<Result<string>>(
        `${import.meta.env.VITE_API_URL}/applications/review/${id}`,
        {
          id: id,
          status: status,
          reviewDescription: reviewDescription,
        }
      );
      this.toastStore.addToast({
        message: res.data.data,
        type: res.data.statusCode == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new ApplicationManagementService();
