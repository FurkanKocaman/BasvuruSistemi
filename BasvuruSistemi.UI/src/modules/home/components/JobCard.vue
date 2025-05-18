<script setup lang="ts">
import { defineProps, defineComponent } from "vue";
import applicationService from "../services/application.service";
import { useRouter } from "vue-router";
import { GetActiveJobPostingsSummariesQueryResponse } from "../models/active-job-posting-summaries.model";

// TypeScript için Vue bileşeni tanımlama
defineComponent({
  name: "JobCard",
  props: {
    job: {
      type: Object,
      required: true,
    },
  },
});

const router = useRouter();

// Component prop tanımlaması
const props = defineProps<{
  job: GetActiveJobPostingsSummariesQueryResponse;
}>();

// Tarih formatı fonksiyonu
const formatDate = (dateString: string): string => {
  const options: Intl.DateTimeFormatOptions = { year: "numeric", month: "long", day: "numeric" };
  return new Date(dateString).toLocaleDateString("tr-TR", options);
};

const navigateApplication = async () => {
  const res = await applicationService.checkApplicationExist(props.job.id);
  if (res.data) {
    if (props.job.type == 0) {
      router.push({
        name: "JobApplication",
        params: { id: props.job.id },
        query: { type: props.job.type },
      });
    } else if (props.job.type == 1) {
      router.push({
        name: "PostingGroupApplication",
        params: { id: props.job.id },
        query: { type: props.job.type },
      });
    }
  }
};
</script>

<template>
  <div
    class="bg-gray-50 dark:bg-gray-800/40 rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow duration-300 border border-gray-200 dark:border-gray-700 min-h-[25rem] flex flex-col justify-between"
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
      <span class="text-gray-700 dark:text-gray-200 text-sm font-medium">{{
        job.salaryRange
      }}</span>
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
      <h4 class="text-sm font-semibold text-gray-700 dark:text-gray-200 mb-2">Gereksinimler:</h4>
      <div v-if="job.qualifications" class="flex flex-wrap gap-1">
        <div
          class="bg-gray-100 dark:bg-gray-700 text-gray-800 dark:text-gray-200 text-xs px-2 py-1 rounded"
          v-html="job.qualifications"
        ></div>
      </div>
    </div>

    <!-- Başvur butonu -->
    <div class="flex justify-end">
      <button
        class="inline-flex items-center px-4 py-2 bg-blue-600 dark:bg-blue-700 text-white text-sm font-medium rounded-lg hover:bg-blue-700 dark:hover:bg-blue-800 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200"
        @click="navigateApplication()"
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
</template>
