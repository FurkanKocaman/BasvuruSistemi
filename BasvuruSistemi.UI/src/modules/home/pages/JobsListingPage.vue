<template>
  <div>
    <h1 class="text-2xl font-bold text-gray-900 dark:text-white mb-6">İş İlanları</h1>
    
    <!-- Filtreler -->
    <JobFilters @filter="handleFilter" />
    
    <!-- İş İlanları Listesi -->
    <div v-if="filteredJobs.length > 0" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <JobCard 
        v-for="job in filteredJobs" 
        :key="job.id" 
        :job="job" 
      />
    </div>
    
    <!-- Sonuç bulunamadı -->
    <div v-else class="bg-white dark:bg-gray-800 rounded-lg shadow-md p-8 text-center">
      <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mx-auto text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
      <h3 class="mt-4 text-lg font-medium text-gray-900 dark:text-white">Sonuç bulunamadı</h3>
      <p class="mt-1 text-gray-500 dark:text-gray-400">Arama kriterlerinize uygun iş ilanı bulunamadı. Lütfen filtreleri değiştirerek tekrar deneyin.</p>
      <button 
        @click="resetFilters" 
        class="mt-4 inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
      >
        Filtreleri Sıfırla
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import JobFilters from '../components/JobFilters.vue';
import JobCard from '../components/JobCard.vue';
import { jobData } from '../data/jobs';
import type { Job } from '../data/jobs';

// Filtrelenmiş işler listesi
const filteredJobs = ref<Job[]>(jobData);

// Filtre işlemi
const handleFilter = (jobs: Job[]) => {
  filteredJobs.value = jobs;
};

// Filtreleri sıfırlama
const resetFilters = () => {
  filteredJobs.value = jobData;
};
</script>
