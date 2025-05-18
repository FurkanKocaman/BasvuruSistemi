import { PaginatedResponse } from "@/models/response/paginated-response.model";
import axios from "axios";
import { useJobPostingStore } from "@/stores/job-posting";
import { GetActiveJobPostingsSummariesQueryResponse } from "../models/active-job-posting-summaries.model";
import { GetActiveJobPostingsQueryResponse } from "../models/active-job-posting.model";
import { PostingGroupGetModel } from "@/modules/management/models/posting-group-get.model";

class JobPostingService {
  async getActiveJobPostings(): Promise<
    PaginatedResponse<GetActiveJobPostingsSummariesQueryResponse> | undefined
  > {
    try {
      const res = await axios.get(`${import.meta.env.VITE_API_URL}/api/job-postings/active`);
      const jobPostingStore = useJobPostingStore();

      jobPostingStore.setJobPostings(res.data.items);

      return res.data;
    } catch (ex) {
      console.error("Exception ", ex);
    }
  }
  async getActiveJobPosting(id: string): Promise<GetActiveJobPostingsQueryResponse | undefined> {
    try {
      const res = await axios.get(`${import.meta.env.VITE_API_URL}/api/job-postings/active/${id}`);

      return res.data;
    } catch (ex) {
      console.error("Exception ", ex);
    }
  }
  async getPostingGroup(id: string): Promise<PostingGroupGetModel | undefined> {
    try {
      const res = await axios.get(`${import.meta.env.VITE_API_URL}/api/posting-groups/${id}`);
      return res.data;
    } catch (ex) {
      console.error("Exception ", ex);
    }
  }
}
export default new JobPostingService();
