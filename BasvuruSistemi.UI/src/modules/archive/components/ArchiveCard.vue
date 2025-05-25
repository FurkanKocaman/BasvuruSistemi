<script setup lang="ts">
import { computed } from 'vue';
import { ArchiveEntry } from '../models/archive.model';

const props = defineProps<{
  archive: ArchiveEntry;
}>();

const emit = defineEmits(['view', 'download']);

const statusColor = computed(() => {
  switch (props.archive.status) {
    case 'ready':
      return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-300';
    case 'processing':
      return 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-300';
    case 'error':
      return 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-300';
    default:
      return 'bg-gray-100 text-gray-800 dark:bg-gray-800 dark:text-gray-300';
  }
});

const statusText = computed(() => {
  switch (props.archive.status) {
    case 'ready':
      return 'Hazır';
    case 'processing':
      return 'İşleniyor';
    case 'error':
      return 'Hata';
    default:
      return 'Bilinmiyor';
  }
});

const formattedDate = computed(() => {
  const date = new Date(props.archive.createdAt);
  return new Intl.DateTimeFormat('tr-TR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  }).format(date);
});
</script>

<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden transition-all hover:shadow-lg">
    <div class="p-6">
      <div class="flex justify-between items-start">
        <div>
          <h3 class="text-xl font-semibold text-gray-800 dark:text-white">{{ archive.name }}</h3>
          <p class="text-gray-600 dark:text-gray-300 mt-1">{{ archive.description }}</p>
        </div>
        <span 
          class="px-3 py-1 rounded-full text-sm font-medium" 
          :class="statusColor"
        >
          {{ statusText }}
        </span>
      </div>
      
      <div class="mt-4 flex items-center text-sm text-gray-500 dark:text-gray-400">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect>
          <line x1="16" y1="2" x2="16" y2="6"></line>
          <line x1="8" y1="2" x2="8" y2="6"></line>
          <line x1="3" y1="10" x2="21" y2="10"></line>
        </svg>
        <span>{{ formattedDate }}</span>
      </div>
      
      <div v-if="archive.fileSize" class="mt-2 flex items-center text-sm text-gray-500 dark:text-gray-400">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4 mr-1" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"></path>
          <polyline points="14 2 14 8 20 8"></polyline>
          <line x1="16" y1="13" x2="8" y2="13"></line>
          <line x1="16" y1="17" x2="8" y2="17"></line>
          <polyline points="10 9 9 9 8 9"></polyline>
        </svg>
        <span>{{ archive.fileSize }}</span>
      </div>
      
      <div class="mt-6 flex justify-between">
        <button 
          @click="emit('view', archive.year)"
          class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors flex items-center gap-1 text-sm"
          :disabled="archive.status !== 'ready'"
          :class="{'opacity-50 cursor-not-allowed': archive.status !== 'ready'}"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
            <circle cx="12" cy="12" r="3"></circle>
          </svg>
          Görüntüle
        </button>
        
        <button 
          @click="emit('download', archive.year)"
          class="px-4 py-2 bg-gray-200 text-gray-800 dark:bg-gray-700 dark:text-white rounded-md hover:bg-gray-300 dark:hover:bg-gray-600 transition-colors flex items-center gap-1 text-sm"
          :disabled="archive.status !== 'ready'"
          :class="{'opacity-50 cursor-not-allowed': archive.status !== 'ready'}"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
            <polyline points="7 10 12 15 17 10"></polyline>
            <line x1="12" y1="15" x2="12" y2="3"></line>
          </svg>
          İndir
        </button>
      </div>
    </div>
  </div>
</template>
