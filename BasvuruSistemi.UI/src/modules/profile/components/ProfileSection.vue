<template>
  <div class="p-6 border-b border-gray-200 dark:border-gray-700 last:border-b-0">
    <!-- Bölüm başlığı ve düzenleme butonu -->
    <div class="flex justify-between items-center mb-4">
      <h3 class="text-xl font-semibold text-gray-800 dark:text-white flex items-center gap-2">
        <slot name="icon"></slot>
        {{ title }}
      </h3>
      
      <button 
        v-if="editable"
        @click="$emit('edit')" 
        class="px-3 py-1.5 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors flex items-center gap-1 text-sm"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <path d="M12 20h9"></path>
          <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
        </svg>
        Düzenle
      </button>
    </div>
    
    <!-- Bölüm içeriği -->
    <div v-if="!isEmpty" class="space-y-4">
      <slot></slot>
    </div>
    
    <!-- Boş içerik durumu -->
    <div v-else class="py-4 text-center">
      <p class="text-gray-500 dark:text-gray-400">{{ emptyMessage }}</p>
      
      <button 
        v-if="editable"
        @click="$emit('add')" 
        class="mt-3 px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors flex items-center gap-2 mx-auto"
      >
        <svg xmlns="http://www.w3.org/2000/svg" class="h-4 w-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <line x1="12" y1="5" x2="12" y2="19"></line>
          <line x1="5" y1="12" x2="19" y2="12"></line>
        </svg>
        {{ addButtonText }}
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
// Props tanımı
defineProps({
  title: {
    type: String,
    required: true
  },
  isEmpty: {
    type: Boolean,
    default: false
  },
  emptyMessage: {
    type: String,
    default: 'Henüz bilgi eklenmemiş'
  },
  addButtonText: {
    type: String,
    default: 'Yeni Ekle'
  },
  editable: {
    type: Boolean,
    default: true
  }
});

// Emit tanımları
defineEmits(['edit', 'add']);
</script>
