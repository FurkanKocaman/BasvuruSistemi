import { GetActiveJobPostingsQueryResponse } from "@/modules/home/models/active-job-posting.model";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useJobPostingStore = defineStore("job-posting", () => {
  const jobPostings = ref<GetActiveJobPostingsQueryResponse[] | null>(null);

  function setJobPostings(payload: GetActiveJobPostingsQueryResponse[]) {
    jobPostings.value = payload;
  }

  function clearJobPostings() {
    jobPostings.value = null;
  }

  return {
    jobPostings,
    setJobPostings,
    clearJobPostings,
  };
});
