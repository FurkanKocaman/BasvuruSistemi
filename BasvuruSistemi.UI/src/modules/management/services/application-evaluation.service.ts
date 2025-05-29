import api from "@/services/Axios";
import { SubmitEvaluationModel } from "../models/evaluation/create-requests/application-evaluation-submit.model";
import { Result } from "@/models/entities/result.model";
import { UploadEvaluationFileModel } from "../models/evaluation/create-requests/upload-evaluation-file.model";
import { ApplicationGetDetailModel } from "../models/application-get-detail.model";
import { ApplicationEvaluationProcessModel } from "../models/evaluation/application-evaluation-process.model";

class ApplicationEvaluationService {
  async getApplicationDetail(applicationId: string): Promise<ApplicationGetDetailModel> {
    try {
      const res = await api.get(
        `${import.meta.env.VITE_API_URL}/api/applications/detail/${applicationId}`
      );
      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async getApplicationEvaluationsProcess(
    applicationId: string
  ): Promise<ApplicationEvaluationProcessModel[]> {
    try {
      const res = await api.get<ApplicationEvaluationProcessModel[]>(
        `${
          import.meta.env.VITE_API_URL
        }/api/application-evaluations/process?applicationId=${applicationId}`
      );
      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

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
        }?applicationEvaluationId=${request.applicationEvaluationId}`,
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
