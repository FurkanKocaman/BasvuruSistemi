<script setup lang="ts">
import { defineProps, defineEmits } from "vue";
import { FormFieldResponse } from "@/modules/management/models/form-filed-response.model";

interface Props {
  modelValue: string | undefined;
  field: FormFieldResponse;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);
</script>

<template>
  <div class="mb-4 w-full">
    <label
      :for="`text-${field.id}`"
      class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1 select-none"
    >
      {{ field.label }} <span v-if="field.isRequired" class="text-red-500">*</span>
    </label>
    <textarea
      :id="`text-${field.id}`"
      type="text"
      :placeholder="props.field.placeholder"
      :readonly="props.field.isReadOnly"
      :required="props.field.isRequired"
      :value="props.modelValue"
      @input="e => emit('update:modelValue', (e.target as HTMLInputElement).value)"
      class="w-full px-3 py-2 border rounded-md text-gray-800 dark:text-gray-200 dark:border-gray-600 border-gray-400 outline-none focus:border-indigo-600 dark:focus:border-indigo-600"
    ></textarea>
    <p v-if="field.description" class="text-xs text-gray-500 mt-1">
      {{ field.description }}
    </p>
  </div>
</template>
