<script setup lang="ts">
// Archive layout component
</script>

<template>
  <div class="min-h-screen bg-gray-100 dark:bg-gray-900">
    <header class="bg-white dark:bg-gray-800 shadow">
      <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
        <h1 class="text-2xl font-bold text-gray-900 dark:text-white">Dijital Arşiv Sistemi</h1>
        <p class="mt-1 text-sm text-gray-600 dark:text-gray-400">
          Yıllık başvuru ve ilan arşivlerine buradan erişebilirsiniz
        </p>
      </div>
    </header>
    
    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      <router-view />
    </main>
  </div>
</template>
