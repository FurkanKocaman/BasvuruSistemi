<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
    <div
      class="bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-md w-full p-6 animate-fadeIn"
      @click.stop
    >
      <div class="flex items-center">
        <div class="bg-yellow-100 rounded-full p-2 mr-4">
          <svg class="size-6" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path
              d="M15.4998 5.49994L18.3282 8.32837M3 20.9997L3.04745 20.6675C3.21536 19.4922 3.29932 18.9045 3.49029 18.3558C3.65975 17.8689 3.89124 17.4059 4.17906 16.9783C4.50341 16.4963 4.92319 16.0765 5.76274 15.237L17.4107 3.58896C18.1918 2.80791 19.4581 2.80791 20.2392 3.58896C21.0202 4.37001 21.0202 5.63634 20.2392 6.41739L8.37744 18.2791C7.61579 19.0408 7.23497 19.4216 6.8012 19.7244C6.41618 19.9932 6.00093 20.2159 5.56398 20.3879C5.07171 20.5817 4.54375 20.6882 3.48793 20.9012L3 20.9997Z"
              class="stroke-yellow-600"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
          </svg>
        </div>
        <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-200">{{ props.title }}</h3>
      </div>

      <div class="mt-6 flex flex-col justify-start space-x-3">
        <label for="name" class="text-sm text-gray-700 dark:text-gray-300">Durum</label>
        <select
          name="name"
          id="name"
          v-model="newStatus"
          class="outline-none px-1.5 py-1 border border-gray-200 dark:border-gray-700 rounded-md text-gray-700 dark:text-gray-300 bg-transparent text-sm dark:focus:border-indigo-600 focus:border-indigo-600"
        >
          <option
            v-for="opt in JobPostingStatusOptions"
            :key="opt.label"
            :value="opt.value"
            class="dark:bg-gray-800 bg-gray-50"
          >
            {{ opt.label }}
          </option>
        </select>
      </div>
      <div class="mt-6 flex justify-end items-center gap-3">
        <button
          class="px-4 py-2 text-sm bg-gray-200 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500 text-white rounded-md cursor-pointer"
          @click="cancel"
        >
          İptal
        </button>
        <button
          class="px-4 py-2 text-sm bg-indigo-600 hover:bg-indigo-500 text-white rounded-md cursor-pointer"
          @click="confirm"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, defineExpose, defineProps } from "vue";
import { JobPostingStatusOptions } from "@/models/constants/job-posting-status";
const props = defineProps({
  title: String,
});

const visible = ref(false);
const resolveFn = ref(null);

const newStatus = ref(0);

function open() {
  visible.value = true;
  return new Promise((resolve) => {
    resolveFn.value = resolve;
  });
}

function confirm() {
  visible.value = false;
  resolveFn.value?.(newStatus.value);
}

function cancel() {
  visible.value = false;
  resolveFn.value?.(false);
}

defineExpose({ open }); // Parent çağırabilsin diye
</script>

<style scoped>
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
.animate-fadeIn {
  animation: fadeIn 0.2s ease-out;
}
</style>
