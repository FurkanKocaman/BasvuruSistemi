<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import ArchiveCard from '../components/ArchiveCard.vue';
import { ArchiveEntry } from '../models/archive.model';
import { archiveService } from '../services/archive.service';

const router = useRouter();
const archives = ref<ArchiveEntry[]>([]);
const loading = ref(true);
const error = ref<string | null>(null);

onMounted(async () => {
  try {
    loading.value = true;
    archives.value = await archiveService.getArchiveEntries();
  } catch (err) {
    error.value = 'Arşiv verileri yüklenirken bir hata oluştu.';
    console.error(err);
  } finally {
    loading.value = false;
  }
});

const viewArchive = (year: number) => {
  router.push({ name: 'archive-year', params: { year: year.toString() } });
};

const downloadArchive = async (year: number) => {
  try {
    await archiveService.downloadArchive(year);
    // In a real application, this would trigger a file download
    alert(`${year} yılına ait arşiv indiriliyor...`);
  } catch (err) {
    alert('Arşiv indirme işlemi sırasında bir hata oluştu.');
    console.error(err);
  }
};
</script>

<template>
  <div>
    <div class="mb-6">
      <h2 class="text-xl font-semibold text-gray-800 dark:text-white">Dijital Arşivler</h2>
      <p class="mt-1 text-gray-600 dark:text-gray-400">
        Yıllık başvuru ve ilan arşivlerinizi görüntüleyin ve indirin
      </p>
    </div>

    <div v-if="loading" class="flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
    </div>

    <div v-else-if="error" class="bg-red-100 dark:bg-red-900 text-red-800 dark:text-red-300 p-4 rounded-md">
      {{ error }}
    </div>

    <div v-else-if="archives.length === 0" class="bg-gray-100 dark:bg-gray-800 p-8 rounded-lg text-center">
      <svg xmlns="http://www.w3.org/2000/svg" class="h-16 w-16 mx-auto text-gray-400" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round">
        <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
        <polyline points="14 2 14 8 20 8"></polyline>
        <line x1="16" y1="13" x2="8" y2="13"></line>
        <line x1="16" y1="17" x2="8" y2="17"></line>
        <polyline points="10 9 9 9 8 9"></polyline>
      </svg>
      <h3 class="mt-4 text-lg font-medium text-gray-800 dark:text-white">Henüz Arşiv Bulunmuyor</h3>
      <p class="mt-2 text-gray-600 dark:text-gray-400">Arşivler yıl sonunda otomatik olarak oluşturulacaktır.</p>
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      <ArchiveCard 
        v-for="archive in archives" 
        :key="archive.id" 
        :archive="archive"
        @view="viewArchive"
        @download="downloadArchive"
      />
    </div>
  </div>
</template>
