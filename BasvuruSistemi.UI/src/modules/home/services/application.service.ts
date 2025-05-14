import { Result } from "@/models/entities/result.model";
import api from "@/services/Axios";
import { ApplicationCreateRequest } from "../models/application-create.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { PaginatedResponse } from "@/models/response/paginated-response.model";
import { ApplicationByUserModel } from "../models/application-by-user.model";

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

  async getApplicationsByUser(
    page: number,
    pageSize: number
  ): Promise<PaginatedResponse<ApplicationByUserModel>> {
    try {
      const res = await api.get(
        `${import.meta.env.VITE_API_URL}/api/applications-by-user?page=${page}&pageSize=${pageSize}`
      );

      return await res.data;
    } catch (err) {
      console.log(err);
      throw err;
    }
  }
  async withdrawnApplication(id: string): Promise<Result<string>> {
    try {
      const res = await api.patch<Result<string>>(
        `${import.meta.env.VITE_API_URL}/applications/withdrawn/${id}`
      );

      this.toastSore.addToast({
        message: res.data.data,
        type: res.data.statusCode == 200 ? "success" : "error",
        duration: 5000,
      });

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new ApplicationService();
