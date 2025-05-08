<template>
  <div>
    <h1 class="text-2xl font-bold text-gray-900 dark:text-white mb-6">Başvurularım</h1>
    
    <!-- Başvuru Filtreleri -->
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow p-4 mb-6 border border-gray-200 dark:border-gray-700">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <!-- Arama -->
        <div class="w-full md:w-1/3">
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
              placeholder="İş başlığı ara" 
              class="pl-10 w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
            />
          </div>
        </div>
        
        <!-- Durum Filtresi -->
        <div class="w-full md:w-1/3">
          <label for="status" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Durum</label>
          <select 
            id="status" 
            v-model="statusFilter"
            class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          >
            <option value="">Tüm Durumlar</option>
            <option value="Pending">Beklemede</option>
            <option value="Approved">Onaylandı</option>
            <option value="Rejected">Reddedildi</option>
          </select>
        </div>
        
        <!-- Sıralama -->
        <div class="w-full md:w-1/3">
          <label for="sort" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">Sıralama</label>
          <select 
            id="sort" 
            v-model="sortOrder"
            class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          >
            <option value="newest">En Yeni</option>
            <option value="oldest">En Eski</option>
          </select>
        </div>
      </div>
    </div>
    
    <!-- Başvurular Tablosu -->
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow border border-gray-200 dark:border-gray-700 overflow-hidden">
      <ApplicationsTable 
        :applications="filteredApplications" 
        @view-details="viewApplicationDetails" 
        @withdraw="withdrawApplication"
      />
    </div>
    
    <!-- Yeni Başvuru Butonu -->
    <div class="mt-6 flex justify-center">
      <router-link 
        to="/jobs" 
        class="inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 mr-2" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4v16m8-8H4" />
        </svg>
        Yeni Başvuru Yap
      </router-link>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ApplicationsTable from '../components/ApplicationsTable.vue';
import { applicationData } from '../data/applications';
import type { Application } from '../data/applications';

export default defineComponent({
  name: 'MyApplicationsPage',
  components: {
    ApplicationsTable
  },
  data() {
    return {
      searchQuery: '',
      statusFilter: '',
      sortOrder: 'newest',
      applicationData
    };
  },
  computed: {
    filteredApplications(): Application[] {
      // Önce filtreleme
      let filtered = this.applicationData.filter(app => {
        // Arama sorgusu filtresi
        const matchesSearch = this.searchQuery
          ? app.jobTitle.toLowerCase().includes(this.searchQuery.toLowerCase())
          : true;
        
        // Durum filtresi
        const matchesStatus = this.statusFilter
          ? app.status === this.statusFilter
          : true;
        
        return matchesSearch && matchesStatus;
      });
      
      // Sonra sıralama
      filtered = [...filtered].sort((a, b) => {
        const dateA = new Date(a.submissionDate).getTime();
        const dateB = new Date(b.submissionDate).getTime();
        
        return this.sortOrder === 'newest' ? dateB - dateA : dateA - dateB;
      });
      
      return filtered;
    }
  },
  methods: {
    // Başvuru detaylarını görüntüleme
    viewApplicationDetails(id: number): void {
      // Gerçek uygulamada başvuru detay sayfasına yönlendirme yapılabilir
      alert(`Başvuru #${id} detayları görüntüleniyor.`);
    },
    
    // Başvuruyu geri çekme
    withdrawApplication(id: number): void {
      // Gerçek uygulamada API çağrısı yapılır
      const index = this.applicationData.findIndex(app => app.id === id);
      if (index !== -1) {
        // Başvuruyu silmek yerine durumunu güncelliyoruz
        this.applicationData[index].status = 'Rejected';
        alert('Başvurunuz geri çekildi.');
      }
    }
  },
  mounted() {
    // Başvuru yoksa uyarı göster
    if (this.applicationData.length === 0) {
      setTimeout(() => {
        alert('Henüz bir başvurunuz bulunmamaktadır. İş ilanlarına göz atabilirsiniz.');
      }, 500);
    }
  }
});
</script>
