<script setup lang="ts">
import { defineProps, defineEmits } from "vue";

interface Props {
  modelValue: string;
  field: {
    label: string;
    placeholder?: string;
    description?: string;
    isRequired: boolean;
    isReadOnly: boolean;
  };
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);
</script>

<template>
  <div class="mb-4 w-full">
    <label
      for="text"
      class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1 select-none"
    >
      {{ field.label }} <span v-if="field.isRequired" class="text-red-500">*</span>
    </label>
    <input
      id="text"
      type="text"
      :placeholder="props.field.placeholder"
      :readonly="props.field.isReadOnly"
      :required="props.field.isRequired"
      :value="props.modelValue"
      @input="e => emit('update:modelValue', (e.target as HTMLInputElement).value)"
      class="w-full px-3 py-2 border rounded-md dark:bg-gray-900 dark:border-gray-700 dark:text-white focus:outline-none focus:ring focus:ring-indigo-300"
    />
    <p v-if="field.description" class="text-xs text-gray-500 mt-1">
      {{ field.description }}
    </p>
  </div>
</template>
