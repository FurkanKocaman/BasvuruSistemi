<script setup lang="ts">
import { defineProps, defineEmits } from "vue";

interface Props {
  modelValue: File | null;
  field: {
    label: string;
    allowedFileTypes?: string; // örn: ".pdf,.jpg"
    maxFileSizeMB?: number;
    isRequired: boolean;
    isReadOnly: boolean;
  };
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);

const handleFileChange = (event: Event) => {
  const file = (event.target as HTMLInputElement).files?.[0] || null;

  if (file && props.field.maxFileSizeMB && file.size > props.field.maxFileSizeMB * 1024 * 1024) {
    alert(`Dosya boyutu ${props.field.maxFileSizeMB}MB'den büyük olamaz.`);
    return;
  }

  emit("update:modelValue", file);
};
</script>

<template>
  <div class="mb-4 w-full">
    <label class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1">
      {{ field.label }} <span v-if="field.isRequired" class="text-red-500">*</span>
    </label>
    <input
      type="date"
      :accept="field.allowedFileTypes"
      :disabled="field.isReadOnly"
      @change="handleFileChange"
      class="w-full file:px-3 file:py-2 file:border-0 file:bg-indigo-600 file:text-white file:rounded-md dark:file:bg-indigo-500"
    />
  </div>
</template>
