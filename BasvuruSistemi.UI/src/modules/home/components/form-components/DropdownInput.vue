<script setup lang="ts">
import { useDropdown } from "@/modules/management/composables/useDropdown";
import { FormFieldResponse } from "@/modules/management/models/form-filed-response.model";
import { defineProps, defineEmits, ref, onMounted, watch } from "vue";

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

const { isOpen, selectedLabel, inputRef, selectOption, handleFocus, handleBlur } = useDropdown(
  props.modelValue
);

watch(
  () => props.modelValue,
  (val) => {
    if (val) selectedLabel.value = val;
  }
);

watch(
  () => selectedLabel.value,
  (val) => {
    emit("update:modelValue", val);
  }
);
</script>

<template>
  <div class="mb-4 w-full relative">
    <span class="block text-sm font-medium text-gray-700 dark:text-gray-200 mb-1 select-none">
      {{ field.label }}
    </span>

    <div class="relative">
      <input
        type="text"
        readonly
        ref="inputRef"
        :value="selectedLabel"
        @focus="handleFocus"
        @blur="handleBlur"
        :placeholder="field.placeholder"
        :required="field.isRequired"
        class="w-full px-3 py-2 border rounded-md text-gray-800 dark:text-gray-200 dark:border-gray-600 border-gray-400 outline-none focus:border-indigo-600 dark:focus:border-indigo-600 focus:shadow-lg"
      />
      <ul
        v-if="isOpen && options.length"
        class="absolute z-10 w-full bg-white dark:bg-gray-800 shadow-lg border dark:border-gray-700 rounded-md mt-1 max-h-48 overflow-auto"
      >
        <li
          v-for="(option, index) in options"
          :key="index"
          @mousedown.prevent="selectOption(option)"
          class="px-4 py-2 text-sm text-gray-800 dark:text-gray-200 hover:bg-gray-100 dark:hover:bg-gray-900/40 cursor-pointer ease-in-out duration-100 transition-all"
        >
          {{ option }}
        </li>
      </ul>
    </div>

    <p v-if="field.description" class="text-xs text-gray-500 mt-1">
      {{ field.description }}
    </p>
  </div>
</template>
