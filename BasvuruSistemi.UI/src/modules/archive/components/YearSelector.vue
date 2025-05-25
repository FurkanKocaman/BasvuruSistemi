<script setup lang="ts">
import { computed } from 'vue';
import { ArchiveEntry } from '../models/archive.model';

const props = defineProps<{
  archives: ArchiveEntry[];
  selectedYear: number | null;
}>();

const emit = defineEmits(['year-selected']);

const years = computed(() => {
  return props.archives
    .filter(archive => archive.status === 'ready')
    .map(archive => archive.year)
    .sort((a, b) => b - a); // Sort years in descending order
});

const selectYear = (year: number) => {
  emit('year-selected', year);
};
</script>

<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-md p-4 mb-6">
    <h3 class="text-lg font-medium text-gray-800 dark:text-white mb-4">Arşiv Yılı Seçin</h3>
    
    <div class="flex flex-wrap gap-2">
      <button
        v-for="year in years"
        :key="year"
        @click="selectYear(year)"
        class="px-4 py-2 rounded-md text-sm font-medium transition-colors"
        :class="[
          selectedYear === year 
            ? 'bg-blue-500 text-white' 
            : 'bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200 hover:bg-gray-200 dark:hover:bg-gray-600'
        ]"
      >
        {{ year }}
      </button>
      
      <div v-if="years.length === 0" class="text-gray-500 dark:text-gray-400 py-2">
        Henüz hazır arşiv bulunmamaktadır.
      </div>
    </div>
  </div>
</template>
