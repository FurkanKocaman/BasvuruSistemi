import { PaginatedResponse } from "@/models/response/paginated-response.model";
import api from "@/services/Axios";
import { JobPostingsByTenantResponse } from "../models/job-posting-by-tenant.model";
import { JobPostingCreateModel } from "../models/job-posting-create.model";
import { useToastStore } from "@/modules/toast/store/toast.store";

class JobPostingService {
  toastSore = useToastStore();
  async getJobPostings(): Promise<PaginatedResponse<JobPostingsByTenantResponse> | undefined> {
    try {
      const res = await api.get(
        `${import.meta.env.VITE_API_URL}/api/job-postings?page=1&pageSize=20`
      );
      console.log("Job posting get response", res);
      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async createJobPostings(request: JobPostingCreateModel): Promise<undefined> {
    try {
      const res = await api.post(`${import.meta.env.VITE_API_URL}/api/job-postings`, request);

      if (res.status == 200) {
        this.toastSore.addToast({
          message: res.data.data,
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastSore.addToast({
          message: res.data.data,
          type: "error",
          duration: 3000,
        });
      }

      console.log("Job posting response", res);
    } catch (err) {
      console.error(err);
    }
  }
}

export default new JobPostingService();
