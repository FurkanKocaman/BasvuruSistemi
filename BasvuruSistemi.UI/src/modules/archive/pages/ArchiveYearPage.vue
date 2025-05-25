<script setup lang="ts">
import { ref, onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import YearSelector from '../components/YearSelector.vue';
import ArchiveTable from '../components/ArchiveTable.vue';
import { ArchiveEntry, UserArchiveData, ApplicationArchiveData, JobListingArchiveData } from '../models/archive.model';
import { archiveService } from '../services/archive.service';

const route = useRoute();
const router = useRouter();
const year = computed(() => parseInt(route.params.year as string));

const archives = ref<ArchiveEntry[]>([]);
const currentArchive = ref<ArchiveEntry | null>(null);
const loading = ref({
  archives: true,
  users: true,
  applications: true,
  jobListings: true
});
const error = ref<string | null>(null);

const userData = ref<UserArchiveData[]>([]);
const applicationData = ref<ApplicationArchiveData[]>([]);
const jobListingData = ref<JobListingArchiveData[]>([]);

// Format date strings
const formatDate = (dateString: string) => {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('tr-TR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(date);
};

// Column definitions for tables
const userColumns = [
  { key: 'fullName', label: 'Ad Soyad' },
  { key: 'tcKimlikNo', label: 'TC Kimlik No' },
  { key: 'email', label: 'E-posta' },
  { key: 'phone', label: 'Telefon' },
  { 
    key: 'registrationDate', 
    label: 'Kayıt Tarihi',
    formatter: formatDate
  },
  { 
    key: 'lastLoginDate', 
    label: 'Son Giriş Tarihi',
    formatter: formatDate
  }
];

const applicationColumns = [
  { key: 'jobTitle', label: 'İlan Başlığı' },
  { key: 'applicantName', label: 'Başvuran' },
  { 
    key: 'applicationDate', 
    label: 'Başvuru Tarihi',
    formatter: formatDate
  },
  { key: 'status', label: 'Durum' },
  { 
    key: 'score', 
    label: 'Puan',
    formatter: (value: number | undefined) => value ? value.toString() : '-'
  }
];

const jobListingColumns = [
  { key: 'title', label: 'İlan Başlığı' },
  { key: 'department', label: 'Departman' },
  { 
    key: 'publishDate', 
    label: 'Yayın Tarihi',
    formatter: formatDate
  },
  { 
    key: 'endDate', 
    label: 'Bitiş Tarihi',
    formatter: formatDate
  },
  { key: 'applicationsCount', label: 'Başvuru Sayısı' },
  { key: 'status', label: 'Durum' }
];

// Load all data
onMounted(async () => {
  try {
    // Load archives
    loading.value.archives = true;
    archives.value = await archiveService.getArchiveEntries();
    
    // Find current archive
    const archive = archives.value.find(a => a.year === year.value);
    if (!archive) {
      error.value = 'Belirtilen yıla ait arşiv bulunamadı.';
      return;
    }
    
    if (archive.status !== 'ready') {
      error.value = 'Bu arşiv henüz hazır değil.';
      return;
    }
    
    currentArchive.value = archive;
    
    // Load user data
    loading.value.users = true;
    userData.value = await archiveService.getUserDataByYear(year.value);
    
    // Load application data
    loading.value.applications = true;
    applicationData.value = await archiveService.getApplicationDataByYear(year.value);
    
    // Load job listing data
    loading.value.jobListings = true;
    jobListingData.value = await archiveService.getJobListingDataByYear(year.value);
    
  } catch (err) {
    error.value = 'Arşiv verileri yüklenirken bir hata oluştu.';
    console.error(err);
  } finally {
    loading.value.archives = false;
    loading.value.users = false;
    loading.value.applications = false;
    loading.value.jobListings = false;
  }
});

// Handle year selection
const onYearSelected = (selectedYear: number) => {
  router.push({ name: 'archive-year', params: { year: selectedYear.toString() } });
};

// Download archive
const downloadArchive = async () => {
  if (!currentArchive.value) return;
  
  try {
    await archiveService.downloadArchive(currentArchive.value.year);
    alert(`${currentArchive.value.year} yılına ait arşiv indiriliyor...`);
  } catch (err) {
    alert('Arşiv indirme işlemi sırasında bir hata oluştu.');
    console.error(err);
  }
};
</script>

<template>
  <div>
    <div class="mb-6 flex justify-between items-center">
      <div>
        <h2 class="text-xl font-semibold text-gray-800 dark:text-white">
          {{ year }} Yılı Arşivi
        </h2>
        <p class="mt-1 text-gray-600 dark:text-gray-400">
          {{ year }} yılına ait tüm başvuru ve ilan kayıtları
        </p>
      </div>
      
      <button 
        @click="router.push({ name: 'archive' })"
        class="px-4 py-2 bg-gray-200 text-gray-800 dark:bg-gray-700 dark:text-white rounded-md hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors flex items-center gap-1 text-sm"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <line x1="19" y1="12" x2="5" y2="12"></line>
          <polyline points="12 19 5 12 12 5"></polyline>
        </svg>
        Tüm Arşivler
      </button>
    </div>

    <div v-if="loading.archives" class="flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
    </div>

    <div v-else-if="error" class="bg-red-100 dark:bg-red-900 text-red-800 dark:text-red-300 p-4 rounded-md mb-6">
      <p>{{ error }}</p>
      <button 
        @click="router.push({ name: 'archive' })"
        class="mt-4 px-4 py-2 bg-red-200 dark:bg-red-800 text-red-800 dark:text-red-200 rounded-md hover:bg-red-300 dark:hover:bg-red-700 transition-colors text-sm"
      >
        Arşiv Listesine Dön
      </button>
    </div>

    <template v-else-if="currentArchive">
      <YearSelector 
        :archives="archives" 
        :selected-year="year"
        @year-selected="onYearSelected"
      />
      
      <div class="mb-6 flex justify-end">
        <button 
          @click="downloadArchive"
          class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors flex items-center gap-1 text-sm"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
            <polyline points="7 10 12 15 17 10"></polyline>
            <line x1="12" y1="15" x2="12" y2="3"></line>
          </svg>
          Tüm Arşivi İndir
        </button>
      </div>
      
      <ArchiveTable 
        title="Kullanıcılar"
        :data="userData"
        :columns="userColumns"
        :loading="loading.users"
      />
      
      <ArchiveTable 
        title="Başvurular"
        :data="applicationData"
        :columns="applicationColumns"
        :loading="loading.applications"
      />
      
      <ArchiveTable 
        title="İş İlanları"
        :data="jobListingData"
        :columns="jobListingColumns"
        :loading="loading.jobListings"
      />
    </template>
  </div>
</template>
