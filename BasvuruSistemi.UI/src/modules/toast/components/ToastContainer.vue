<script setup lang="ts">
import { CheckCircle, Info, AlertTriangle, XCircle, X } from "lucide-vue-next";
import { ToastType, useToastStore } from "../store/toast.store";

const toastStore = useToastStore();

function getToastClasses(type: ToastType) {
  switch (type) {
    case "success":
      return "bg-gray-50 dark:bg-gray-800 border-green-600 text-gray-800 dark:text-gray-200";
    case "error":
      return "bg-gray-50 dark:bg-gray-800 border-red-600 text-gray-800 dark:text-gray-200";
    case "info":
      return "bg-gray-50 dark:bg-gray-800 border-blue-600 text-gray-800 dark:text-gray-200";
    case "warning":
      return "bg-gray-50 dark:bg-gray-800 border-yellow-600 text-gray-800 dark:text-gray-200";
    default:
      return "";
  }
}

function getToastIconClasses(type: ToastType) {
  switch (type) {
    case "success":
      return "text-green-600";
    case "error":
      return "text-red-600";
    case "info":
      return "text-blue-600";
    case "warning":
      return "text-yellow-600";
    default:
      return "";
  }
}

function getToastIcon(type: ToastType) {
  switch (type) {
    case "success":
      return CheckCircle;
    case "error":
      return XCircle;
    case "info":
      return Info;
    case "warning":
      return AlertTriangle;
    default:
      return Info;
  }
}
</script>
<template>
  <div class="fixed top-5 right-5 space-y-3 z-[99] w-[320px]">
    <div
      v-for="toast in toastStore.toasts"
      :key="toast.id"
      class="flex items-start p-3 rounded-lg shadow border animate-fade-in-down"
      :class="getToastClasses(toast.type)"
    >
      <div class="mr-3" :class="getToastIconClasses(toast.type)">
        <component :is="getToastIcon(toast.type)" class="w-5 h-5 mt-1" />
      </div>
      <div class="flex-1">
        <p class="text-sm font-small">{{ toast.message }}</p>
      </div>
      <button
        @click="toastStore.removeToast(toast.id)"
        class="ml-3 text-gray-400 hover:text-gray-600 dark:hover:text-white"
      >
        <X class="w-4 h-4" />
      </button>
    </div>
  </div>
</template>

<style scoped>
@keyframes fade-in-down {
  from {
    opacity: 0;
    transform: translateY(-10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
.animate-fade-in-down {
  animation: fade-in-down 0.3s ease-out;
}
</style>
