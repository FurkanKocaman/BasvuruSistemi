<script setup lang="ts">
import { FormFieldResponse } from "@/modules/management/models/form-filed-response.model";
import { defineProps, defineEmits, ref, onMounted, watch } from "vue";

interface Props {
  modelValue: string[] | undefined;
  field: FormFieldResponse;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);

const options = ref<string[]>([]);
const selectedValues = ref<string[]>([]);

onMounted(() => {
  if (props.field.optionsJson) {
    options.value = props.field.optionsJson.split(",").map((p) => p.trim());
  }

  if (Array.isArray(props.modelValue)) {
    selectedValues.value = [...props.modelValue];
  }
});

watch(
  () => props.modelValue,
  (newVal) => {
    if (Array.isArray(newVal)) {
      selectedValues.value = [...newVal];
    }
  }
);

function toggleOption(option: string) {
  if (selectedValues.value.includes(option)) {
    selectedValues.value = selectedValues.value.filter((o) => o !== option);
  } else {
    selectedValues.value.push(option);
  }

  emit("update:modelValue", [...selectedValues.value]);
}
</script>

<template>
  <div class="mb-4 w-full">
    <span class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1 select-none">
      {{ field.label }}
    </span>

    <div class="flex flex-col gap-2">
      <div v-for="(option, index) in options" :key="option" class="flex items-center gap-2">
        <input
          type="checkbox"
          :id="`checkbox-${field.id}-${index}`"
          :value="option"
          :checked="selectedValues.includes(option)"
          :disabled="field.isReadOnly"
          @change="() => toggleOption(option)"
          class="px-3 py-2 w-4 h-4 border rounded-md dark:bg-gray-900 dark:border-gray-700 dark:text-gray-200 outline-none accent-indigo-600"
        />
        <label
          :for="`checkbox-${field.id}-${index}`"
          class="text-sm font-medium text-gray-700 dark:text-gray-200"
        >
          {{ option }}
        </label>
      </div>
    </div>

    <p v-if="field.description" class="text-xs text-gray-500 mt-1">
      {{ field.description }}
    </p>
  </div>
</template>
