import { FormTemplateGetModel } from "../models/form-template-get.model";
import api from "@/services/Axios";

class EvaluationFormService {
  async getEvaluationForm(
    id: string,
    evaluationPipelineStageId: string
  ): Promise<FormTemplateGetModel> {
    try {
      const res = await api.get(
        `${
          import.meta.env.VITE_API_URL
        }/api/evaluation-form?applicationId=${id}&evaluationPipelineStageId=${evaluationPipelineStageId}`
      );

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new EvaluationFormService();
