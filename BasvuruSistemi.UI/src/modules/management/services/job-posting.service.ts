import { PaginatedResponse } from "@/models/response/paginated-response.model";
import api from "@/services/Axios";
import { JobPostingCreateModel } from "../models/job-posting-create.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { Result } from "@/models/entities/result.model";
import { PostingGroupCreateModel } from "../models/posting-group-create.model";
import { JobPostingSummariesByTenantResponse } from "../models/job-posting-summaries-by-tenant.model";
import { PostingGroupGetModel } from "../models/posting-group-get.model";
import { PostingGroupSummaryGetModel } from "../models/posting-group-summaries.model";

class JobPostingService {
  toastSore = useToastStore();
  async getJobPostings(): Promise<
    PaginatedResponse<JobPostingSummariesByTenantResponse> | undefined
  > {
    try {
      const res = await api.get(
        `${import.meta.env.VITE_API_URL}/api/job-postings?page=1&pageSize=20&view=summaries`
      );
      console.log("Job posting res", res);
      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async getJobPosting(id: string): Promise<Result<JobPostingCreateModel> | undefined> {
    try {
      const res = await api.get(`${import.meta.env.VITE_API_URL}/api/job-postings/${id}`);
      console.log(res);
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
  async createJobPostingGroup(request: PostingGroupCreateModel): Promise<string | undefined> {
    try {
      const res = await api.post(`${import.meta.env.VITE_API_URL}/api/posting-groups`, request);

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

      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async addJobPostingToGroup(jobPostingId: string, postingGroupId: string): Promise<undefined> {
    try {
      const res = await api.post(
        `${import.meta.env.VITE_API_URL}/api/posting-groups/${postingGroupId}/postings`,
        { jobPostingId: jobPostingId, postingGroupId: postingGroupId }
      );

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
    } catch (err) {
      console.error(err);
    }
  }

  async getPostingGroup(id: string): Promise<PostingGroupGetModel | undefined> {
    try {
      const res = await api.get<PostingGroupGetModel>(
        `${import.meta.env.VITE_API_URL}/api/posting-groups/${id}`
      );

      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async getPostingGroups(): Promise<PostingGroupSummaryGetModel[] | undefined> {
    try {
      const res = await api.get<PostingGroupSummaryGetModel[]>(
        `${import.meta.env.VITE_API_URL}/api/posting-groups`
      );

      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
}

export default new JobPostingService();
