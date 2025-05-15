<script setup lang="ts">
import { ref, Ref } from "vue";
import * as icons from "lucide-vue-next";
import { useDropdown } from "../../../composables/useDropdown";
import { FormFieldResponse } from "../../../models/form-filed-response.model";
import formTemplateService from "../../../services/form-template.service";
import { getFieldTypeOptionByValue } from "@/models/constants/field-type";
import { FormFieldDefinition } from "../../../models/form-field.model";

const formTemplateDropdown = useDropdown();

const fields: Ref<FormFieldDefinition[]> = ref([]);
const fieldsFromSelectedTemplate: Ref<FormFieldResponse[]> = ref([]);
const selectedFieldIndices = ref<Set<number>>(new Set());
const selectedFieldsToAdd: Ref<FormFieldResponse[]> = ref([]);

const props = defineProps<{
  formTemplateSummaries: { id: string; name: string }[];
}>();

const getFieldFromSelectedTemplate = async () => {
  selectedFieldsToAdd.value = [];
  var selectedTemplate = props.formTemplateSummaries.find(
    (p) => p.name == formTemplateDropdown.selectedLabel.value
  );
  if (selectedTemplate) {
    const res = await formTemplateService.getFormTemplate(selectedTemplate.id);
    if (res) {
      fieldsFromSelectedTemplate.value = res.fields;
    }
  }
};

const toggleSelection = (index: number) => {
  if (selectedFieldIndices.value.has(index)) {
    selectedFieldIndices.value.delete(index);
  } else {
    selectedFieldIndices.value.add(index);
  }
};

const emit = defineEmits<{
  (e: "add:field", payload: FormFieldDefinition): void;
}>();

const addSelectedFieldsToRequest = () => {
  selectedFieldIndices.value.forEach((index) => {
    const field = fieldsFromSelectedTemplate.value[index];
    if (!fields.value.find((p) => p.label === field.label)) {
      const fieldToAdd: FormFieldDefinition = {
        label: field.label,
        type: field.type,
        isRequired: field.isRequired,
        description: field.description,
        placeholder: field.placeholder,
        optionsJson: field.optionsJson,
        isReadOnly: field.isReadOnly,
        defaultValue: field.defaultValue,
        allowedFileTypes: field.allowedFileTypes,
        maxFileSizeMB: field.maxFileSizeMB,
      };
      fields.value.push({ ...fieldToAdd });
      emit("add:field", field);
    }
  });

  selectedFieldIndices.value.clear();
};
</script>

<template>
  <div class="mx-1 my-1">
    <div class="flex flex-col 2xl:flex-row justify-start items-end w-full">
      <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
        <label for="formTeplate" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
          >Form Şablonu</label
        >
        <input
          type="text"
          name="formTeplate"
          id="formTeplate"
          autocomplete="off"
          :ref="formTemplateDropdown.inputRef"
          v-model="formTemplateDropdown.selectedLabel.value"
          @focus="formTemplateDropdown.handleFocus"
          @blur="formTemplateDropdown.handleBlur"
          readonly
          placeholder="Form şablonu seçin..."
          class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
        />
        <div
          v-if="formTemplateDropdown.isOpen.value"
          class="absolute w-fit mt-16 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
        >
          <div
            v-for="(option, index) in props.formTemplateSummaries"
            :key="index"
            @mousedown.prevent="
              () => {
                formTemplateDropdown.selectOption(option.name);
              }
            "
            class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
          >
            {{ option.name }}
          </div>
        </div>
      </div>
      <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
        <button
          class="border rounded-md text-sm py-1.5 px-5 cursor-pointer dark:border-gray-700 border-gray-200 text-gray-700 dark:text-gray-200 dark:hover:bg-gray-700/20 hover:bg-gray-400/10 select-none"
          @click="getFieldFromSelectedTemplate()"
        >
          Alanları Getir
        </button>
      </div>
    </div>
    <div class="flex flex-col 2xl:flex-row justify-between w-full">
      <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
        <table class="w-full">
          <thead></thead>
          <tbody class="divide-y dark:divide-gray-700 divide-gray-200">
            <tr v-for="(row, index) in fieldsFromSelectedTemplate" :key="index" class="flex">
              <td class="w-10 py-2 ml-3 flex items-center">
                <label class="inline-flex items-center cursor-pointer select-none group">
                  <input
                    type="checkbox"
                    :checked="selectedFieldIndices.has(index)"
                    @change="toggleSelection(index)"
                    class="sr-only"
                  />
                  <span
                    class="w-4 h-4 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                  >
                    <svg
                      v-if="selectedFieldIndices.has(index)"
                      xmlns="http://www.w3.org/2000/svg"
                      class="w-4 h-4 text-indigo-400"
                      fill="none"
                      viewBox="0 0 24 24"
                      stroke="currentColor"
                      stroke-width="4"
                    >
                      <path stroke-linecap="round" stroke-linejoin="round" d="M5 13l4 4L19 7" />
                    </svg>
                  </span>
                </label>
              </td>
              <td class="w-10 py-2 flex items-center">
                <!-- <svg
                      class="size-6"
                      viewBox="0 0 24 24"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        fill-rule="evenodd"
                        clip-rule="evenodd"
                        d="M6 1C4.34315 1 3 2.34315 3 4V20C3 21.6569 4.34315 23 6 23H18C19.6569 23 21 21.6569 21 20V8.82843C21 8.03278 20.6839 7.26972 20.1213 6.70711L15.2929 1.87868C14.7303 1.31607 13.9672 1 13.1716 1H6ZM5 4C5 3.44772 5.44772 3 6 3H12V8C12 9.10457 12.8954 10 14 10H19V20C19 20.5523 18.5523 21 18 21H6C5.44772 21 5 20.5523 5 20V4ZM18.5858 8L14 3.41421V8H18.5858Z"
                        class="dark:fill-gray-300 fill-gray-700"
                      /> -->
                <!-- </svg> -->
                <component
                  :is="icons[getFieldTypeOptionByValue(row.type)!.icon]"
                  class="w-4 h-4 text-gray-500"
                />
              </td>
              <td class="flex-1 flex flex-col justify-center py-2">
                <span class="dark:text-gray-200"> {{ row.label }} </span>
                <span class="text-xs dark:text-gray-400"> {{ row.description }} </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="flex justify-end">
      <button
        class="text-sm border rounded-md py-1.5 px-3 mr-3 mb-3 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
        @click="addSelectedFieldsToRequest()"
      >
        Ekle
      </button>
    </div>
  </div>
</template>
