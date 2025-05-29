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
  async getJobPostings(
    page: number,
    pageSize: number
  ): Promise<PaginatedResponse<JobPostingSummariesByTenantResponse> | undefined> {
    try {
      const res = await api.get(
        `${
          import.meta.env.VITE_API_URL
        }/api/job-postings?page=${page}&pageSize=${pageSize}&view=summaries`
      );
      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async getJobPosting(id: string): Promise<JobPostingCreateModel | undefined> {
    try {
      const res = await api.get<JobPostingCreateModel>(
        `${import.meta.env.VITE_API_URL}/api/job-postings/${id}`
      );
      return res.data;
    } catch (err) {
      console.error(err);
    }
  }
  async createJobPostings(request: JobPostingCreateModel): Promise<string> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/api/job-postings`,
        request
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

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateJobPostings(request: JobPostingCreateModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/api/job-postings/${request.id}`,
        request
      );

      if (res.status == 200) {
        this.toastSore.addToast({
          message: "İlan başarıyla güncellendi",
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastSore.addToast({
          message: "İlan güncellenirken hata oluştu",
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
          message: "İlan başarıyla gruba eklendi",
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastSore.addToast({
          message: "İlan gruba eklenirken hata oluştu",
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

  async deleteJobPosting(id: string): Promise<string> {
    try {
      const res = await api.delete<Result<string>>(
        `${import.meta.env.VITE_API_URL}/api/job-postings/${id}`
      );
      this.toastSore.addToast({
        message:
          res.data.statusCode == 200 ? "İlan başarıyla silindi" : "İlan silinirken hata oluştu",
        type: res.data.statusCode == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async jobPostingChangeStatus(
    id: string,
    status: number,
    publishStartDate?: Date
  ): Promise<string> {
    const dto: { newStatus: number; publishStartDate?: Date } = {
      newStatus: status,
      publishStartDate: publishStartDate,
    };

    try {
      const res = await api.patch<Result<string>>(
        `${import.meta.env.VITE_API_URL}/api/job-postings/${id}/status`,
        dto
      );
      this.toastSore.addToast({
        message:
          res.data.statusCode == 200
            ? "İlan başarıyla güncellendi"
            : "İlan güncellenirken hata oluştu",
        type: res.data.statusCode == 200 ? "success" : "error",
        duration: 3000,
      });
      console.log("hata yok");
      return res.data.data;
    } catch (err) {
      console.error("Hata");
      throw err;
    }
  }
}

export default new JobPostingService();
