import api from "@/services/Axios";
import { SubmitEvaluationModel } from "../models/evaluation/create-requests/application-evaluation-submit.model";
import { Result } from "@/models/entities/result.model";
import { UploadEvaluationFileModel } from "../models/evaluation/create-requests/upload-evaluation-file.model";

class ApplicationEvaluationService {
  async submitEvaluation(request: SubmitEvaluationModel): Promise<string> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/application-evaluations`,
        request
      );

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async uploadEvaluationFile(request: UploadEvaluationFileModel): Promise<Result<string>> {
    try {
      const formData = new FormData();
      formData.append("file", request.file);

      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/application-evaluations/upload-file/${
          request.formFieldId
        }?evaluationPipelineStageId=${request.evaluationPipelineStageId}`,
        formData,
        {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        }
      );

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new ApplicationEvaluationService();
