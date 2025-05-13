import { Result } from "@/models/entities/result.model";
import api from "@/services/Axios";
import { ApplicationCreateRequest } from "../models/application-create.model";
import { useToastStore } from "@/modules/toast/store/toast.store";

class ApplicationService {
  toastSore = useToastStore();
  async createApplication(request: ApplicationCreateRequest): Promise<Result<string>> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/applications`,
        request
      );
      if (res.status == 200) {
        this.toastSore.addToast({
          message: res.data.data,
          type: "success",
          duration: 5000,
        });
      } else {
        this.toastSore.addToast({
          message: res.data.data,
          type: "error",
          duration: 5000,
        });
      }
      return res.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }

  async uploadFileByField(formFiledId: string, file: File): Promise<Result<string>> {
    const formData = new FormData();
    formData.append("file", file);

    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/applications/upload-file/${formFiledId}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      return res.data;
    } catch (error) {
      console.error(error);
      throw error;
    }
  }
}

export default new ApplicationService();
