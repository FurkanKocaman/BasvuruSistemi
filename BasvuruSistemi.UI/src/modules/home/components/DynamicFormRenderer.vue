<script setup lang="ts">
import { FormFieldDefinition } from "@/modules/management/models/form-field.model";
import { getComponentByFieldType } from "../services/getComponentByField.service";
import { ref } from "vue";
import { useRouter } from "vue-router";

const values = ref([]);

const router = useRouter();

const props = defineProps<{ fields: FormFieldDefinition[] }>();

const submit = () => {
  console.log(values.value);
};

const isSubmitting = ref(false);

const goBack = () => {
  router.back();
};
</script>

<template>
  <form @submit.prevent="submit">
    <div v-for="(field, index) in props.fields" :key="index">
      <component :is="getComponentByFieldType(field.type)" v-model="values[index]" :field="field" />
    </div>
    <div class="flex flex-col sm:flex-row space-y-3 sm:space-y-0 sm:space-x-3">
      <button
        type="submit"
        :disabled="isSubmitting"
        class="w-full sm:w-auto flex-1 bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-6 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200 disabled:opacity-70 disabled:cursor-not-allowed"
      >
        <span v-if="isSubmitting" class="flex items-center justify-center">
          <svg
            class="animate-spin -ml-1 mr-2 h-4 w-4 text-white"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
          >
            <circle
              class="opacity-25"
              cx="12"
              cy="12"
              r="10"
              stroke="currentColor"
              stroke-width="4"
            ></circle>
            <path
              class="opacity-75"
              fill="currentColor"
              d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
            ></path>
          </svg>
          Gönderiliyor...
        </span>
        <span v-else>Başvuruyu Gönder</span>
      </button>
      <button
        type="button"
        @click="goBack"
        class="w-full sm:w-auto flex-1 bg-gray-200 dark:bg-gray-600 hover:bg-gray-300 dark:hover:bg-gray-500 text-gray-800 dark:text-white font-medium py-2 px-6 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-opacity-50 transition-colors duration-200"
      >
        İptal
      </button>
    </div>
  </form>
</template>
