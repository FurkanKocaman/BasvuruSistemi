<script setup lang="ts">
import { FormFieldResponse } from "@/modules/management/models/form-filed-response.model";
import { defineProps, defineEmits, ref, onMounted } from "vue";

interface Props {
  modelValue: string | undefined;
  field: FormFieldResponse;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);

const options = ref<string[]>([]);

onMounted(() => {
  if (props.field.optionsJson) {
    options.value = props.field.optionsJson.split(",").map((p) => p.trim());
  }
});
</script>

<template>
  <div class="mb-4 w-full">
    <span class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1 select-none">{{
      field.label
    }}</span>
    <div v-for="(option, index) in options" :key="option" class="flex items-center">
      <input
        :id="`radio-${field.id}-${index}`"
        type="radio"
        :name="`radio-${field.id}-${index}`"
        :placeholder="props.field.placeholder"
        :readonly="props.field.isReadOnly"
        :required="props.field.isRequired"
        :value="option"
        :checked="option == props.modelValue"
        @change="e => emit('update:modelValue', (e.target as HTMLInputElement).value)"
        class="px-3 py-2 w-4 h-4 border rounded-md dark:bg-gray-900 dark:border-gray-700 dark:text-gray-200 mr-3 outline-none accent-indigo-600"
      />
      <label
        :for="`radio-${field.id}-${index}`"
        class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1 select-none"
      >
        {{ option }}
      </label>
    </div>
    <p v-if="field.description" class="text-xs text-gray-500 mt-1">
      {{ field.description }}
    </p>
  </div>
</template>
