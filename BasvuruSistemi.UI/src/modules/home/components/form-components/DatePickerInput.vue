<script setup lang="ts">
import { FormFieldResponse } from "@/modules/management/models/form-filed-response.model";
import { defineProps, defineEmits, ref, watch } from "vue";
import DatePicker from "primevue/datepicker";

interface Props {
  modelValue: Date | undefined;
  field: FormFieldResponse;
}

const props = defineProps<Props>();
const emit = defineEmits(["update:modelValue"]);

const selectedDate = ref<Date | null>(props.modelValue ? new Date(props.modelValue) : null);

watch(
  () => props.modelValue,
  (val) => {
    selectedDate.value = val ? new Date(val) : null;
  }
);

function onDateChange(val: Date | Date[] | (Date | null)[] | null | undefined) {
  let selected: Date | null = null;

  if (val instanceof Date) {
    selected = val;
  } else if (Array.isArray(val) && val[0] instanceof Date) {
    selected = val[0]; // Eğer tarih aralığı desteği varsa
  }

  emit("update:modelValue", selected ? selected.toISOString() : "");
}
</script>

<template>
  <div class="mb-4 w-full relative">
    <span class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1 select-none">
      {{ field.label }}
    </span>

    <DatePicker
      :model-value="selectedDate"
      @update:model-value="onDateChange"
      :placeholder="field.placeholder"
      :required="field.isRequired"
      :readonly="field.isReadOnly"
      showButtonBar
      showIcon
      class="!w-full !border !outline-none !rounded-md !text-gray-800 dark:!text-gray-200 dark:!border-gray-700 !border-gray-200 !py-2 !px-2 !bg-gray-50 dark:!bg-gray-900 dark:!focus:border-indigo-600 focus:border-indigo-600"
      inputClass="!text-start !text-sm"
      panel-class="dark:!bg-gray-900 !bg-gray-50 dark:!text-gray-200 !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !p-3"
    />

    <p v-if="field.description" class="text-xs text-gray-500 mt-1">
      {{ field.description }}
    </p>
  </div>
</template>
