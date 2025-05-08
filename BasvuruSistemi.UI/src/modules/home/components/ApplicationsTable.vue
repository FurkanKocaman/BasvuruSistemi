<template>
  <div class="overflow-x-auto">
    <table class="min-w-full divide-y divide-gray-200 dark:divide-gray-700">
      <thead class="bg-gray-50 dark:bg-gray-800">
        <tr>
          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
            İş Başlığı
          </th>
          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
            Departman
          </th>
          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
            Başvuru Tarihi
          </th>
          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
            Durum
          </th>
          <th scope="col" class="px-6 py-3 text-left text-xs font-medium text-gray-500 dark:text-gray-400 uppercase tracking-wider">
            İşlemler
          </th>
        </tr>
      </thead>
      <tbody class="bg-white dark:bg-gray-900 divide-y divide-gray-200 dark:divide-gray-800">
        <tr v-for="application in applications" :key="application.id" class="hover:bg-gray-50 dark:hover:bg-gray-800 transition-colors duration-150">
          <td class="px-6 py-4 whitespace-nowrap">
            <div class="text-sm font-medium text-gray-900 dark:text-white">{{ application.jobTitle }}</div>
          </td>
          <td class="px-6 py-4 whitespace-nowrap">
            <div class="text-sm text-gray-700 dark:text-gray-300">{{ application.department }}</div>
          </td>
          <td class="px-6 py-4 whitespace-nowrap">
            <div class="text-sm text-gray-700 dark:text-gray-300">{{ formatDate(application.submissionDate) }}</div>
          </td>
          <td class="px-6 py-4 whitespace-nowrap">
            <status-badge :status="application.status" />
          </td>
          <td class="px-6 py-4 whitespace-nowrap text-sm font-medium">
            <button 
              @click="viewDetails(application.id)"
              class="text-indigo-600 dark:text-indigo-400 hover:text-indigo-900 dark:hover:text-indigo-300 mr-3"
            >
              Detaylar
            </button>
            <button 
              v-if="application.status === 'Pending'"
              @click="withdrawApplication(application.id)"
              class="text-red-600 dark:text-red-400 hover:text-red-900 dark:hover:text-red-300"
            >
              Geri Çek
            </button>
          </td>
        </tr>
        <tr v-if="applications.length === 0">
          <td colspan="5" class="px-6 py-10 text-center text-gray-500 dark:text-gray-400">
            <div class="flex flex-col items-center">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12 mb-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
              <p class="text-lg font-medium">Henüz başvuru bulunamadı</p>
              <p class="mt-1">İş ilanlarını inceleyip başvuru yapabilirsiniz.</p>
              <router-link 
                to="/jobs" 
                class="mt-4 inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                İş İlanlarına Git
              </router-link>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';
import StatusBadge from './StatusBadge.vue';
import type { Application } from '../data/applications';

// Props tanımlaması
defineProps<{
  applications: Application[]
}>();

// Emits tanımlaması
const emit = defineEmits<{
  (e: 'view-details', id: number): void
  (e: 'withdraw', id: number): void
}>();

// Tarih formatı fonksiyonu
const formatDate = (dateString: string): string => {
  const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
  return new Date(dateString).toLocaleDateString('tr-TR', options);
};

// Detay görüntüleme fonksiyonu
const viewDetails = (id: number) => {
  emit('view-details', id);
};

// Başvuruyu geri çekme fonksiyonu
const withdrawApplication = (id: number) => {
  if (confirm('Bu başvuruyu geri çekmek istediğinizden emin misiniz?')) {
    emit('withdraw', id);
  }
};
</script>