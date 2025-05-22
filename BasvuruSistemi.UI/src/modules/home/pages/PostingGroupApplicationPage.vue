<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
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
      <div class="w-full grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        <div
          v-for="job in postingGroup?.jobPostings"
          :key="job.id"
          class="w-full border my-2 p-2 rounded-md border-gray-200 dark:border-gray-700 hover:bg-gray-400/10 transition-all ease-in-out duration-300"
        >
          <div
            class="p-6 transition-shadow duration-300 min-h-[15rem] flex flex-col justify-between"
          >
            <!-- İş kartı başlık bölümü -->
            <div class="flex justify-between items-start mb-4">
              <h3 class="text-xl font-bold text-gray-800 dark:text-white">{{ job.title }}</h3>
            </div>

            <!-- Şirket ve lokasyon bilgisi -->
            <div class="mb-4">
              <p class="text-gray-600 dark:text-gray-300 font-medium">{{ job.unit }}</p>
            </div>

            <!-- Maaş ve tarih bilgisi -->
            <div class="flex justify-between items-center mb-4">
              <span class="text-gray-500 dark:text-gray-400 text-xs"
                >Son Başvuru Tarihi: {{ formatDate(job.validTo!) }}</span
              >
            </div>

            <!-- İş açıklaması -->
            <div
              class="text-gray-600 dark:text-gray-300 text-sm mb-4 line-clamp-3"
              v-html="job.description"
            ></div>

            <!-- Gereksinimler -->
            <div class="mb-6">
              <h4 class="text-sm font-semibold text-gray-700 dark:text-gray-200 mb-2">
                Gereksinimler:
              </h4>
              <div v-if="job.qualifications" class="flex flex-wrap gap-1">
                <div
                  class="bg-gray-100 dark:bg-gray-700 text-gray-800 dark:text-gray-200 text-xs px-2 py-1 rounded"
                  v-html="job.qualifications"
                ></div>
              </div>
            </div>

            <div class="flex justify-end">
              <button
                class="cursor-pointer inline-flex items-center px-4 py-2 bg-blue-600 dark:bg-blue-700 text-white text-sm font-medium rounded-lg hover:bg-blue-700 dark:hover:bg-blue-800 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200"
                @click.stop="navigateJobPosting(job.id)"
              >
                Başvur
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-4 w-4 ml-1"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M14 5l7 7m0 0l-7 7m7-7H3"
                  />
                </svg>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
