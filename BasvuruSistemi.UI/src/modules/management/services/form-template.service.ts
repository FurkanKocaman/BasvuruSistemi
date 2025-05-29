import api from "@/services/Axios";
import { FormTemplateCreateReqeust } from "../models/form-template-create.model";
import { FormTemplateGetModel } from "../models/form-template-get.model";
import { PaginatedResponse } from "@/models/response/paginated-response.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { Result } from "@/models/entities/result.model";

class FormTeplateSerive {
  toastStore = useToastStore();

  async createFormTemplate(request: FormTemplateCreateReqeust): Promise<string> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/form-templates`,
        request
      );

      if (res.status == 200) {
        this.toastStore.addToast({
          message: "Form şablonu başarıyla oluşturuldu",
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastStore.addToast({
          message: "Hata oluştu",
          type: "error",
          duration: 3000,
        });
      }

      return res.data.data;
    } catch (err) {
      this.toastStore.addToast({
        message: "Hata oluştu",
        type: "error",
        duration: 3000,
      });
      console.error(err);
      throw err;
    }
  }
  async deleteFormTemplate(id: string): Promise<string> {
    try {
      const res = await api.delete<Result<string>>(
        `${import.meta.env.VITE_API_URL}/form-templates/${id}`
      );

      if (res.status == 200) {
        this.toastStore.addToast({
          message: "Form şablonu silindi",
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastStore.addToast({
          message: "Hata oluştu",
          type: "error",
          duration: 3000,
        });
      }

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateFormTemplate(
    id: string,
    request: FormTemplateCreateReqeust
  ): Promise<string | undefined> {
    try {
      const res = await api.put(
        `${import.meta.env.VITE_API_URL}/form-templates/update/${id}`,
        request
      );
      console.log("Update res", res);
      if (res.status == 200) {
        this.toastStore.addToast({
          message: "Form şablonu başarıyla güncellendi",
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastStore.addToast({
          message: "Hata oluştu",
          type: "error",
          duration: 3000,
        });
      }

      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async getFormTemplate(id: string): Promise<FormTemplateGetModel | undefined> {
    try {
      const res = await api.get(`${import.meta.env.VITE_API_URL}/api/form-templates/${id}`);

      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async getFormTemplates(
    page: number,
    pageSize: number
  ): Promise<PaginatedResponse<FormTemplateGetModel> | undefined> {
    try {
      const res = await api.get(
        `${
          import.meta.env.VITE_API_URL
        }/api/form-templates?view=details&page?${page}&pageSize=${pageSize}`
      );
      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async getFormTemplateSummaries(): Promise<{ id: string; name: string }[] | undefined> {
    try {
      const res = await api.get(
        `${import.meta.env.VITE_API_URL}/api/form-templates?view=summaries`
      );

      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
}

export default new FormTeplateSerive();
