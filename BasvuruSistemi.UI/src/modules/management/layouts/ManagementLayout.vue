<script setup lang="ts">
import { ref, watch } from "vue";
import ManagementHeader from "../components/ManagementHeader.vue";
import { useRoute } from "vue-router";

const isRouteChanging = ref(false);
const route = useRoute();

watch(
  () => route.fullPath,
  () => {
    isRouteChanging.value = true;
    setTimeout(() => {
      isRouteChanging.value = false;
    }, 300);
  }
);
</script>

<template>
  <div class="dark:bg-gray-900 bg-gray-100 w-screen h-screen overflow-hidden flex flex-col">
    <!-- Sabit Header -->
    <div class="flex-shrink-0">
      <ManagementHeader />
    </div>

    <!-- Scrollable ve Animasyonlu İçerik -->
    <div class="flex-1 overflow-y-auto overflow-x-hidden relative">
      <router-view v-slot="{ Component }">
        <transition name="fade" mode="out-in">
          <div v-if="isRouteChanging" key="loading" class="w-full flex justify-center py-10">
            <span
              class="animate-spin h-8 w-8 border-4 border-blue-500 border-t-transparent rounded-full"
            ></span>
          </div>
          <component v-else :is="Component" :key="route.fullPath" />
        </transition>
      </router-view>
    </div>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
