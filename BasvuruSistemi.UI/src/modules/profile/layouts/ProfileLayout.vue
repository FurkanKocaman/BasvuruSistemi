<template>
  <div class="min-h-screen bg-gray-50 dark:bg-gray-900">
    <HeaderComponent />
    <!-- Profil sayfası düzeni -->
    <div class="container mx-auto px-4 py-8">
      <h1 class="text-2xl font-bold text-gray-900 dark:text-white mb-6">
        {{ title }}
      </h1>

      <!-- İçerik alanı -->
      <div class="bg-white dark:bg-gray-800 rounded-lg shadow-md overflow-hidden mt-10">
        <slot></slot>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import HeaderComponent from "@/modules/home/components/HeaderComponent.vue";

// Sayfa başlığı için prop tanımı
defineProps({
  title: {
    type: String,
    default: "Profil",
  },
});
</script>
