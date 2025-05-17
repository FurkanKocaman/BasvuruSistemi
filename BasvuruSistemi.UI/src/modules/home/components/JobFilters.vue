<script setup lang="ts">
import { ref, onMounted, defineEmits } from "vue";
import { GetActiveJobPostingsSummariesQueryResponse } from "../models/active-job-posting-summaries.model";

const emit = defineEmits<{
  (e: "filter", filteredJobs: GetActiveJobPostingsSummariesQueryResponse[]): void;
}>();

const props = defineProps<{
  allJobs: GetActiveJobPostingsSummariesQueryResponse[];
}>();

const searchQuery = ref("");

const resetFilters = () => {
  searchQuery.value = "";

  emit("filter", props.allJobs);
};

onMounted(() => {
  emit("filter", props.allJobs);
});
</script>

<template>
  <div
    class="bg-gray-50 dark:bg-gray-800/30 rounded-lg shadow p-4 mb-6 border border-gray-200 dark:border-gray-700 flex flex-col"
  >
    <div class="mb-3">
      <span class="text-lg font-semibold text-gray-800 dark:text-white">Filtreler</span>
    </div>

    <!-- Arama kutusu -->
    <div class="mb-4">
      <label for="search" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
        >Arama</label
      >
      <div class="relative">
        <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
          <svg
            class="h-5 w-5 text-gray-400"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 20 20"
            fill="currentColor"
          >
            <path
              fill-rule="evenodd"
              d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
              clip-rule="evenodd"
            />
          </svg>
        </div>
        <input
          id="search"
          type="text"
          v-model="searchQuery"
          placeholder="İlan başlığı, şirket veya pozisyon ara"
          class="pl-10 w-full rounded-md border border-gray-300 dark:border-gray-600 shadow-sm dark:focus:border-indigo-600 focus:border-indigo-600 outline-none py-1.5 text-gray-800 dark:text-gray-200"
        />
      </div>
    </div>

    <!-- Filtreleri uygula ve sıfırla butonları -->
    <div class="flex justify-end space-x-2">
      <button
        class="bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200 cursor-pointer"
      >
        Filtreleri Uygula
      </button>
      <button
        @click="resetFilters"
        class="bg-gray-200 dark:bg-gray-600 hover:bg-gray-300 dark:hover:bg-gray-500 text-gray-800 dark:text-white font-medium py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-opacity-50 transition-colors duration-200 cursor-pointer"
      >
        Sıfırla
      </button>
    </div>
  </div>
</template>
