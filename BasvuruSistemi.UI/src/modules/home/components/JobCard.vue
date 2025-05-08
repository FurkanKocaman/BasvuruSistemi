<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow duration-300 border border-gray-200 dark:border-gray-700">
    <!-- İş kartı başlık bölümü -->
    <div class="flex justify-between items-start mb-4">
      <h3 class="text-xl font-bold text-gray-800 dark:text-white">{{ job.title }}</h3>
      <span class="bg-blue-100 dark:bg-blue-900 text-blue-800 dark:text-blue-200 text-xs font-semibold px-2.5 py-0.5 rounded">
        {{ job.department }}
      </span>
    </div>
    
    <!-- Şirket ve lokasyon bilgisi -->
    <div class="mb-4">
      <p class="text-gray-600 dark:text-gray-300 font-medium">{{ job.company }}</p>
      <p class="text-gray-500 dark:text-gray-400 text-sm flex items-center">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
        </svg>
        {{ job.location }}
      </p>
    </div>
    
    <!-- Maaş ve tarih bilgisi -->
    <div class="flex justify-between items-center mb-4">
      <span class="text-gray-700 dark:text-gray-200 text-sm font-medium">{{ job.salary }}</span>
      <span class="text-gray-500 dark:text-gray-400 text-xs">İlan Tarihi: {{ formatDate(job.postDate) }}</span>
    </div>
    
    <!-- İş açıklaması -->
    <p class="text-gray-600 dark:text-gray-300 text-sm mb-4 line-clamp-3">{{ job.description }}</p>
    
    <!-- Gereksinimler -->
    <div class="mb-6">
      <h4 class="text-sm font-semibold text-gray-700 dark:text-gray-200 mb-2">Gereksinimler:</h4>
      <div class="flex flex-wrap gap-1">
        <span 
          v-for="(req, index) in job.requirements.slice(0, 3)" 
          :key="index" 
          class="bg-gray-100 dark:bg-gray-700 text-gray-800 dark:text-gray-200 text-xs px-2 py-1 rounded"
        >
          {{ req }}
        </span>
        <span v-if="job.requirements.length > 3" class="text-xs text-gray-500 dark:text-gray-400 px-2 py-1">
          +{{ job.requirements.length - 3 }} daha
        </span>
      </div>
    </div>
    
    <!-- Başvur butonu -->
    <div class="flex justify-end">
      <router-link 
        :to="{ name: 'JobApplication', params: { id: job.id } }" 
        class="inline-flex items-center px-4 py-2 bg-blue-600 dark:bg-blue-700 text-white text-sm font-medium rounded-lg hover:bg-blue-700 dark:hover:bg-blue-800 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200"
      >
        Başvur
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 ml-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3" />
        </svg>
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { defineProps, defineComponent } from 'vue';
import type { Job } from '../data/jobs';

// TypeScript için Vue bileşeni tanımlama
defineComponent({
  name: 'JobCard',
  props: {
    job: {
      type: Object,
      required: true
    }
  }
});

// Component prop tanımlaması
defineProps<{
  job: Job
}>();

// Tarih formatı fonksiyonu
const formatDate = (dateString: string): string => {
  const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
  return new Date(dateString).toLocaleDateString('tr-TR', options);
};
</script>