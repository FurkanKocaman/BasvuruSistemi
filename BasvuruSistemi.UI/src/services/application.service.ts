import { PaginatedResponse } from "@/models/response/paginated-response.model";
import { ApplicationGetSummariesModel } from "@/modules/management/models/application-get-summaries.model";
import api from "./Axios";
import { Result } from "@/models/entities/result.model";
import { ApplicationGetDetailModel } from "@/modules/management/models/application-get-detail.model";

class ApplicationService {
  async getAllApplications(
    jobPostingId: string,
    page: number,
    pageSize: number
  ): Promise<PaginatedResponse<ApplicationGetSummariesModel>> {
    try {
      const res = await api.get(
        `${
          import.meta.env.VITE_API_URL
        }/api/applications/${jobPostingId}?page=${page}&pageSize=${pageSize}`
      );
      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async getApplication(applicationId: string): Promise<Result<ApplicationGetDetailModel>> {
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
}
export default new ApplicationService();
