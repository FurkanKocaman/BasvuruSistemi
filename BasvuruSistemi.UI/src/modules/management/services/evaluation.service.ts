import api from "@/services/Axios";
import { EvaluationStageCreateModel } from "../models/evaluation/create-requests/evaluation-stage-create.model";
import { Result } from "@/models/entities/result.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { EvaluationFormCreateModel } from "../models/evaluation/create-requests/evaluation-form-create.model";
import { AddFormFieldToEvaluationFormModel } from "../models/evaluation/create-requests/evaluation-form-field-add.model";
import { EvaluationStageDto } from "../models/evaluation/evaluation-stage.model";
import { EvaluationFormDto } from "../models/evaluation/evaluation-form.model";

class EvaluationService {
  toastStore = useToastStore();
  async createEvaluatonStage(request: EvaluationStageCreateModel): Promise<string> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/evaluation-stages`,
        request
      );

      this.toastStore.addToast({
        message: res.status == 200 ? "Değerlendirme adımı oluşturuldu" : "Hata oluştu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async listEvaluationStages(): Promise<EvaluationStageDto[]> {
    try {
      const res = await api.get<EvaluationStageDto[]>(
        `${import.meta.env.VITE_API_URL}/api/evaluation-stages`
      );

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async createEvaluatonForm(request: EvaluationFormCreateModel): Promise<string> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/evaluation-forms`,
        request
      );

      this.toastStore.addToast({
        message: res.status == 200 ? "Değerlendirme formu oluşturuldu" : "Hata oluştu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateEvaluatonForm(request: EvaluationFormCreateModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/evaluation-forms/${request.id}`,
        request
      );

      this.toastStore.addToast({
        message: res.status == 200 ? "Değerlendirme formu güncellendi" : "Hata oluştu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async listEvaluationForms(id: string): Promise<EvaluationFormDto[]> {
    try {
      const res = await api.get<EvaluationFormDto[]>(
        `${import.meta.env.VITE_API_URL}/api/evaluation-stage/forms/${id}`
      );

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async addFieldToForm(request: AddFormFieldToEvaluationFormModel): Promise<string> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/evaluation-forms/fields`,
        request
      );

      this.toastStore.addToast({
        message: res.status == 200 ? "Değerlendirme formuna alan eklendi" : "Hata oluştu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateFieldByForm(request: AddFormFieldToEvaluationFormModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/evaluation-forms/fields/${request.id}`,
        request
      );

      this.toastStore.addToast({
        message: res.status == 200 ? "Form alanı başarıyla güncellendi" : "Hata oluştu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async deleteFieldFromForm(id: string): Promise<string> {
    try {
      const res = await api.delete<Result<string>>(
        `${import.meta.env.VITE_API_URL}/evaluation-forms/fields/${id}`
      );

      this.toastStore.addToast({
        message: res.status == 200 ? "Form alanı başarıyla silindi" : "Hata oluştu",
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

export default new EvaluationService();
