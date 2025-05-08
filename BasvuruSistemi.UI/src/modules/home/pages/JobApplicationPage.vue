<template>
  <div>
    <div class="mb-6">
      <router-link 
        to="/jobs" 
        class="inline-flex items-center text-sm font-medium text-blue-600 dark:text-blue-400 hover:text-blue-800 dark:hover:text-blue-300"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-1" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
        </svg>
        İş İlanlarına Dön
      </router-link>
    </div>
    
    <!-- İş Detayları -->
    <div v-if="job" class="bg-white dark:bg-gray-800 rounded-lg shadow-md p-6 mb-8 border border-gray-200 dark:border-gray-700">
      <div class="flex flex-col md:flex-row md:justify-between md:items-start gap-4">
        <div>
          <h1 class="text-2xl font-bold text-gray-900 dark:text-white">{{ job.title }}</h1>
          <p class="text-gray-600 dark:text-gray-300 mt-1">{{ job.company }} - {{ job.location }}</p>
          <div class="mt-2 flex items-center">
            <span class="bg-blue-100 dark:bg-blue-900 text-blue-800 dark:text-blue-200 text-xs font-semibold px-2.5 py-0.5 rounded">
              {{ job.department }}
            </span>
            <span class="ml-2 text-gray-500 dark:text-gray-400 text-sm">İlan Tarihi: {{ formatDate(job.postDate) }}</span>
          </div>
        </div>
        <div class="text-right">
          <p class="text-lg font-semibold text-gray-800 dark:text-white">{{ job.salary }}</p>
        </div>
      </div>
      
      <div class="mt-6">
        <h2 class="text-lg font-semibold text-gray-800 dark:text-white mb-2">İş Açıklaması</h2>
        <p class="text-gray-600 dark:text-gray-300">{{ job.description }}</p>
      </div>
      
      <div class="mt-4">
        <h2 class="text-lg font-semibold text-gray-800 dark:text-white mb-2">Gereksinimler</h2>
        <ul class="list-disc list-inside text-gray-600 dark:text-gray-300">
          <li v-for="(req, index) in job.requirements" :key="index">{{ req }}</li>
        </ul>
      </div>
    </div>
    
    <!-- Başvuru Formu -->
    <ApplicationForm v-if="job" :jobId="job.id" @submit="handleSubmit" />
    
    <!-- İş bulunamadı -->
    <div v-else class="bg-white dark:bg-gray-800 rounded-lg shadow-md p-8 text-center">
      <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mx-auto text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
      </svg>
      <h3 class="mt-4 text-lg font-medium text-gray-900 dark:text-white">İş ilanı bulunamadı</h3>
      <p class="mt-1 text-gray-500 dark:text-gray-400">Aradığınız iş ilanı bulunamadı veya kaldırılmış olabilir.</p>
      <router-link 
        to="/jobs" 
        class="mt-4 inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
      >
        İş İlanlarına Dön
      </router-link>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, defineComponent } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import ApplicationForm from '../components/ApplicationForm.vue';
import { jobData } from '../data/jobs';
import type { Job } from '../data/jobs';
import type { Application } from '../data/applications';

// TypeScript için Vue bileşeni tanımlama
defineComponent({
  name: 'JobApplicationPage',
  components: {
    ApplicationForm
  }
});

// Route ve router
const route = useRoute();
const router = useRouter();

// İş verisi
const job = ref<Job | null>(null);

// Tarih formatı fonksiyonu
const formatDate = (dateString: string): string => {
  const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
  return new Date(dateString).toLocaleDateString('tr-TR', options);
};

// Başvuru gönderimi
const handleSubmit = (formData: Partial<Application>) => {
  // Gerçek bir API olmadığı için burada bir şey yapmıyoruz
  // ApplicationForm component'i zaten kullanıcıyı yönlendiriyor
  console.log('Başvuru gönderildi:', formData);
};

// Component yüklendiğinde
onMounted(() => {
  // URL'den iş ID'sini alma
  const jobId = Number(route.params.id);
  
  // İş verisini bulma
  if (!isNaN(jobId)) {
    job.value = jobData.find(j => j.id === jobId) || null;
    
    // Eğer iş bulunamazsa, iş listesine yönlendir
    if (!job.value) {
      setTimeout(() => {
        router.push('/jobs');
      }, 3000);
    }
  } else {
    // Geçersiz ID, iş listesine yönlendir
    router.push('/jobs');
  }
});
</script>
