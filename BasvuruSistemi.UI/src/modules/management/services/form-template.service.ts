import api from "@/services/Axios";
import { FormTemplateCreateReqeust } from "../models/form-template-create.model";
import { FormTemplateGetModel } from "../models/form-template-get.model";
import { PaginatedResponse } from "@/models/response/paginated-response.model";

class FormTeplateSerive {
  async createFormTemplate(request: FormTemplateCreateReqeust): Promise<string | undefined> {
    try {
      const res = await api.post(`${import.meta.env.VITE_API_URL}/form-templates`, request);

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
  async getFormTemplates(): Promise<PaginatedResponse<FormTemplateGetModel> | undefined> {
    try {
      const res = await api.get(`${import.meta.env.VITE_API_URL}/api/form-templates?view=details`);

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
