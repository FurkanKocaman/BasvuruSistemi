<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import ApplicationForm from "../components/ApplicationForm.vue";
import type { Application } from "../data/applications";
import jobPostingService from "../services/job-posting.service";
import { PostingGroupGetModel } from "@/modules/management/models/posting-group-get.model";
import applicationService from "../services/application.service";

const route = useRoute();
const router = useRouter();

const postingGroup = ref<PostingGroupGetModel | null>(null);

const formatDate = (dateString: string): string => {
  const options: Intl.DateTimeFormatOptions = { year: "numeric", month: "long", day: "numeric" };
  return new Date(dateString).toLocaleDateString("tr-TR", options);
};

const handleSubmit = (formData: Partial<Application>) => {
  // Gerçek bir API olmadığı için burada bir şey yapmıyoruz
  // ApplicationForm component'i zaten kullanıcıyı yönlendiriyor
  console.log("Başvuru gönderildi:", formData);
};

onMounted(() => {
  const jobId = String(route.params.id);
  const jobType = Number(route.query.type);
  if (jobId && (jobType == 0 || jobType == 1)) {
    getPostingGroup(jobId);
  } else {
    router.push("/jobs");
  }
});

const getPostingGroup = async (id: string) => {
  const res = await jobPostingService.getPostingGroup(id);
  if (res) {
    console.log("PostingGroup res", res);
    postingGroup.value = res;
  }
};

const navigateJobPosting = async (id: string) => {
  const res = await applicationService.checkApplicationExist(id);
  if (res.data) {
    router.push({
      name: "JobApplication",
      params: { id: id },
      query: { type: 0 },
    });
  }
};
</script>

<template>
  <div class="px-10 pt-26 pb-10">
    <div class="mb-6">
      <router-link
        to="/jobs"
        class="inline-flex items-center text-sm font-medium text-blue-600 dark:text-blue-400 hover:text-blue-800 dark:hover:text-blue-300"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-5 w-5 mr-1"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M10 19l-7-7m0 0l7-7m-7 7h18"
          />
        </svg>
        İş İlanlarına Dön
      </router-link>
    </div>

    <!-- İş Detayları -->
    <div
      v-if="postingGroup"
      class="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6 mb-8 border border-gray-200 dark:border-gray-700"
    >
      <div class="flex flex-col md:flex-row md:justify-between md:items-start gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900 dark:text-white">{{ postingGroup.name }}</h1>
          <p class="text-gray-600 dark:text-gray-300 mt-1">
            {{ postingGroup.unit ?? "-" }}
          </p>
          <div class="mt-2 flex items-center">
            <span class="ml-2 text-gray-500 dark:text-gray-400 text-sm"
              >İlan Tarihi:
              {{
                postingGroup.overallApplicationStartDate
                  ? formatDate(postingGroup.overallApplicationStartDate)
                  : "-"
              }}</span
            >
          </div>
        </div>
      </div>

      <div class="mt-6">
        <h2 class="text-lg font-semibold text-gray-800 dark:text-white mb-2">İş Açıklaması</h2>
        <div class="text-gray-600 dark:text-gray-300" v-html="postingGroup.description"></div>
      </div>
    </div>

    <!-- Başvuru Formu -->
    <!-- <ApplicationForm v-if="job" :job="job" @submit="handleSubmit" /> -->

    <div
      class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-200 dark:border-gray-700"
    >
      <h2 class="text-2xl font-bold text-gray-800 dark:text-white mb-6">İlanlar</h2>
      <div class="flex flex-col w-full">
        <div
          v-for="jobPosting in postingGroup?.jobPostings"
          :key="jobPosting.id"
          class="w-full border my-2 p-2 rounded-md border-gray-200 dark:border-gray-700 cursor-pointer hover:bg-gray-400/10 transition-all ease-in-out duration-300"
          @click.stop="navigateJobPosting(jobPosting.id)"
        >
          <span class="text-gray-700 dark:text-gray-200 w-full">
            {{ jobPosting.title }}
          </span>
        </div>
      </div>
    </div>
  </div>
</template>
