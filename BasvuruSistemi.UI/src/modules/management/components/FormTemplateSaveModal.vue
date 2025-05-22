<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
    <div
      class="bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-md w-full p-6 animate-fadeIn"
      @click.stop
    >
      <div class="flex items-center">
        <div class="bg-red-100 rounded-full p-2 mr-4">
          <svg
            class="w-6 h-6 text-red-600"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z"
            />
          </svg>
        </div>
        <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-200">{{ title }}</h3>
      </div>
      <p class="mt-4 text-sm text-gray-600">{{ description }}</p>

      <div class="mt-6 flex flex-col justify-start space-x-3">
        <label for="name" class="text-sm text-gray-700 dark:text-gray-300">Şablon Adı</label>
        <input
          id="name"
          type="text"
          autocomplete="off"
          v-model="templateName"
          class="outline-none px-1.5 py-1 border border-gray-200 dark:border-gray-700 rounded-md text-gray-700 dark:text-gray-300 bg-transparent text-sm dark:focus:border-indigo-600 focus:border-indigo-600"
        />
      </div>
      <div class="mt-6 flex justify-between items-center">
        <button
          class="py-0.5 text-sm cursor-pointer text-gray-700 dark:text-gray-300 hover:text-gray-900 dark:hover:text-gray-50 hover:border-gray-500 dark:hover:border-gray-500 border-b border-transparent flex items-center"
          @click="cancel"
        >
          Kaydetmeden devam et<MoveRight class="ml-1 text-sm" />
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
import { MoveRight } from "lucide-vue-next";
defineProps({
  title: String,
  description: String,
});

const visible = ref(false);
const resolveFn = ref(null);

const templateName = ref("");

function open() {
  visible.value = true;
  return new Promise((resolve) => {
    resolveFn.value = resolve;
  });
}

function confirm() {
  visible.value = false;
  resolveFn.value?.(templateName.value);
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
