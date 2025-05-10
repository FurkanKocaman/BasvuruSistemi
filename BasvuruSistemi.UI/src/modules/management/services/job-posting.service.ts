import { PaginatedResponse } from "@/models/response/paginated-response.model";
import api from "@/services/Axios";
import { JobPostingsByTenantResponse } from "../models/job-posting-by-tenant.model";
import { JobPostingCreateModel } from "../models/job-posting-create.model";

class JobPostingService {
  async getJobPostings(): Promise<PaginatedResponse<JobPostingsByTenantResponse> | undefined> {
    try {
      const res = await api.get(
        `${import.meta.env.VITE_API_URL}/api/job-postings?page=1&pageSize=20`
      );
      console.log(res);
      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async createJobPostings(request: JobPostingCreateModel): Promise<string | undefined> {
    try {
      const res = await api.post(`${import.meta.env.VITE_API_URL}/api/job-postings`, request);
      console.log(res);
      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
}

export default new JobPostingService();
