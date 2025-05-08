<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow p-4 mb-6 border border-gray-200 dark:border-gray-700">
    <h2 class="text-lg font-semibold text-gray-800 dark:text-white mb-4">İş Filtreleri</h2>
    
    <!-- Arama kutusu -->
    <div class="mb-4">
      <label for="search" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Arama</label>
      <div class="relative">
        <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
          <svg class="h-5 w-5 text-gray-400" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor">
            <path fill-rule="evenodd" d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" clip-rule="evenodd" />
          </svg>
        </div>
        <input 
          id="search" 
          type="text" 
          v-model="searchQuery"
          placeholder="İş başlığı, şirket veya pozisyon ara" 
          class="pl-10 w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
        />
      </div>
    </div>
    
    <!-- Departman filtresi -->
    <div class="mb-4">
      <label for="department" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Departman</label>
      <select 
        id="department" 
        v-model="selectedDepartment"
        class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
      >
        <option value="">Tüm Departmanlar</option>
        <option v-for="department in departments" :key="department" :value="department">
          {{ department }}
        </option>
      </select>
    </div>
    
    <!-- Lokasyon filtresi -->
    <div class="mb-4">
      <label for="location" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Lokasyon</label>
      <select 
        id="location" 
        v-model="selectedLocation"
        class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
      >
        <option value="">Tüm Lokasyonlar</option>
        <option v-for="location in locations" :key="location" :value="location">
          {{ location }}
        </option>
      </select>
    </div>
    
    <!-- Filtreleri uygula ve sıfırla butonları -->
    <div class="flex space-x-2">
      <button 
        @click="applyFilters"
        class="flex-1 bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200"
      >
        Filtreleri Uygula
      </button>
      <button 
        @click="resetFilters"
        class="flex-1 bg-gray-200 dark:bg-gray-600 hover:bg-gray-300 dark:hover:bg-gray-500 text-gray-800 dark:text-white font-medium py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-opacity-50 transition-colors duration-200"
      >
        Sıfırla
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, defineEmits } from 'vue';
import { jobData } from '../data/jobs';

// Emits tanımlaması
const emit = defineEmits<{
  (e: 'filter', filteredJobs: typeof jobData): void
}>();

// Filtre durumları
const searchQuery = ref('');
const selectedDepartment = ref('');
const selectedLocation = ref('');

// Departman ve lokasyon listelerini hesaplama
const departments = computed(() => {
  const deptSet = new Set(jobData.map(job => job.department));
  return Array.from(deptSet);
});

const locations = computed(() => {
  const locSet = new Set(jobData.map(job => {
    // Şehir kısmını çıkarma (örn. "İstanbul, Türkiye" -> "İstanbul")
    return job.location.split(',')[0].trim();
  }));
  return Array.from(locSet);
});

// Filtreleri uygulama fonksiyonu
const applyFilters = () => {
  const filteredJobs = jobData.filter(job => {
    // Arama sorgusu filtresi
    const matchesSearch = searchQuery.value 
      ? job.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        job.company.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
        job.description.toLowerCase().includes(searchQuery.value.toLowerCase())
      : true;
    
    // Departman filtresi
    const matchesDepartment = selectedDepartment.value 
      ? job.department === selectedDepartment.value
      : true;
    
    // Lokasyon filtresi (şehir kısmı)
    const matchesLocation = selectedLocation.value
      ? job.location.includes(selectedLocation.value)
      : true;
    
    return matchesSearch && matchesDepartment && matchesLocation;
  });
  
  // Filtrelenmiş sonuçları emit etme
  emit('filter', filteredJobs);
};

// Filtre sıfırlama fonksiyonu
const resetFilters = () => {
  searchQuery.value = '';
  selectedDepartment.value = '';
  selectedLocation.value = '';
  
  // Tüm işleri göster
  emit('filter', jobData);
};

// Component yüklendiğinde 
onMounted(() => {
  // Başlangıçta tüm iş ilanlarını göster
  emit('filter', jobData);
});
</script>