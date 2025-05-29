<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
    <div
      class="bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-xl w-full p-6 animate-fadeIn"
      @click.stop
    >
      <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-200 mb-4">Forma Alan Ekle</h3>

      <div class="my-3 flex flex-col">
        <span
          v-for="error in errors"
          :key="error"
          class="text-sm border rounded-md w-full px-1 py-1 border-red-600 text-red-500"
          >{{ error }}</span
        >
      </div>

      <div class="flex-1 flex flex-col my-2 items-start">
        <label for="postingName" class="text-sm my-1 dark:text-gray-300 text-gray-600 select-none"
          >Alan Tipi</label
        >
        <input
          type="text"
          name="postingName"
          id="postingName"
          autocomplete="off"
          :ref="fieldTypeDropdown.inputRef"
          v-model="fieldTypeDropdown.selectedLabel.value"
          @focus="fieldTypeDropdown.handleFocus"
          @blur="fieldTypeDropdown.handleBlur"
          readonly
          placeholder="Alan tipi seçin..."
          class="w-full border outline-none rounded-md py-1.5 px-2 dark:text-gray-50 text-gray-900 placeholder:text-gray-700 dark:placeholder:text-gray-200 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
        />
        <div
          v-if="fieldTypeDropdown.isOpen.value"
          class="absolute w-fit mt-16 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
        >
          <div
            v-for="option in FieldTypeOptions"
            :key="option.value"
            @mousedown.prevent="fieldTypeDropdown.selectOption(option.label)"
            class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer"
          >
            {{ option.label }}
          </div>
        </div>
      </div>

      <div class="space-y-2 flex flex-col text-gray-800 dark:text-gray-200 text-sm my-3">
        <label for="label">Alan Adı</label>
        <input
          id="label"
          name="label"
          type="text"
          autocomplete="off"
          v-model="request.label"
          class="outline-none bg-transparent border rounded-md border-gray-200 dark:border-gray-800 px-2 py-1"
        />
      </div>
      <div class="my-1">
        <label class="inline-flex items-center cursor-pointer">
          <input type="checkbox" :value="false" v-model="request.isRequired" class="sr-only peer" />
          <div
            class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
          ></div>
          <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700">Zorunlu Mu?</span>
        </label>
      </div>

      <div
        v-if="request.type == 3 || request.type == 4 || request.type == 5"
        class="flex-1 flex flex-col my-2 items-start 2xl:mr-3"
      >
        <label for="options" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
          >Seçenekler <span class="text-xs">(Virgül ile ayırarak seçenekleri girin)</span></label
        >
        <textarea
          type="text"
          name="options"
          id="options"
          v-model="request.optionsJson"
          autocomplete="off"
          class="w-full border outline-none rounded-md py-1.5 px-2 min-h-20 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600 text-gray-900 dark:text-gray-100"
        ></textarea>
      </div>
      <div
        v-if="request.type == 6 || request.type == 13 || request.type == 15 || request.type == 16"
        class="flex-1 flex flex-col my-2 items-start 2xl:mr-3"
      >
        <label for="options" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
          >Dosya Türleri <span class="text-xs">(Virgül ile ayırarak seçenekleri girin)</span></label
        >
        <textarea
          type="text"
          name="options"
          id="options"
          v-model="request.allowedFileTypes"
          autocomplete="off"
          class="w-full border outline-none rounded-md py-1.5 px-2 min-h-20 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600 text-gray-900 dark:text-gray-100"
        ></textarea>
        <label for="options" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
          >Dosya Boyutu <span class="text-xs">(MB olarak girin)</span></label
        >
        <input
          type="text"
          name="options"
          id="options"
          v-model="request.maxFileSizeMB"
          autocomplete="off"
          class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600 text-gray-900 dark:text-gray-100"
        />
      </div>

      <div class="mt-6 flex justify-end">
        <button
          class="px-4 py-2 text-sm bg-gray-200 dark:bg-gray-700 dark:text-white text-gray-800 rounded hover:bg-gray-300/40 cursor-pointer"
          @click="close"
        >
          Kapat
        </button>
        <button
          class="px-4 py-2 text-sm bg-sky-600 text-gray-50 rounded hover:bg-sky-500 ml-3 cursor-pointer"
          @click="onSubmit()"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, defineExpose, watch } from "vue";
import { AddFormFieldToEvaluationFormModel } from "../../models/evaluation/create-requests/evaluation-form-field-add.model";
import { EvaluationFormFieldDto } from "../../models/evaluation/evaluation-form-field.model";
import evaluationService from "../../services/evaluation.service";
import { useDropdown } from "../../composables/useDropdown";
import {
  FieldTypeOptions,
  getFieldTypeOptionByLabel,
  getFieldTypeOptionByValue,
} from "@/models/constants/field-type";

const request = ref<AddFormFieldToEvaluationFormModel>({
  id: "",
  evaluationFormId: "",
  label: "",
  type: 0,
  isRequired: false,
  order: 0,
  isReadOnly: false,
  verificationSource: 0,
});

const fieldTypeDropdown = useDropdown();
const resolveFn = ref(null);
const isUpdate = ref(false);

const errors = ref<string[]>([]);

const onSubmit = async () => {
  request.value.type = getFieldTypeOptionByLabel(fieldTypeDropdown.selectedLabel.value)?.value ?? 0;
  console.log("Request", request.value);
  if (request.value.type != 0) {
    if (isUpdate.value) {
      const res = await evaluationService.updateFieldByForm(request.value);
      if (res) {
        resolveFn.value?.(true);
        visible.value = false;
      }
    } else {
      const res = await evaluationService.addFieldToForm(request.value);
      if (res) {
        resolveFn.value?.(true);
        visible.value = false;
      }
    }
  } else {
    errors.value.push("Alan türü boş olamaz");
  }
};

const visible = ref(false);

function open(formId: string, currentLenght: number, field?: EvaluationFormFieldDto) {
  errors.value = [];
  fieldTypeDropdown.selectOption("");

  request.value.evaluationFormId = formId;
  request.value.order = currentLenght + 1;

  visible.value = true;
  if (field) {
    isUpdate.value = true;

    request.value.id = field.id;
    request.value.evaluationFormId = field.evaluationFormId;

    request.value.label = field.label;
    request.value.type = field.type;
    request.value.isRequired = field.isRequired;
    request.value.order = field.order;
    request.value.description = field.description;
    request.value.placeholder = field.placeholder;
    request.value.optionsJson = field.optionsJson;

    request.value.isReadOnly = field.isReadOnly;
    request.value.defaultValue = field.defaultValue;

    request.value.verificationSource = field.verificationSource;
    request.value.verificationParametersJson = field.verificationParametersJson;

    request.value.allowedFileTypes = field.allowedFileTypes;
    request.value.maxFileSizeMB = field.maxFileSizeMB;

    fieldTypeDropdown.selectOption(getFieldTypeOptionByValue(request.value.type)?.label ?? "");
  } else {
    isUpdate.value = false;

    request.value.id = undefined;

    request.value.label = "";
    request.value.type = 0;
    request.value.isRequired = false;
    request.value.description = undefined;
    request.value.placeholder = undefined;
    request.value.optionsJson = undefined;

    request.value.isReadOnly = false;
    request.value.defaultValue = undefined;

    request.value.verificationSource = 0;
    request.value.verificationParametersJson = undefined;

    request.value.allowedFileTypes = undefined;
    request.value.maxFileSizeMB = undefined;
  }

  return new Promise((resolve) => {
    resolveFn.value = resolve;
  });
}

function close() {
  visible.value = false;
  resolveFn.value?.(false);
}

defineExpose({ open });

watch(fieldTypeDropdown.selectedLabel, () => {
  request.value.type = getFieldTypeOptionByLabel(fieldTypeDropdown.selectedLabel.value)?.value ?? 0;
  request.value.allowedFileTypes = undefined;
  request.value.maxFileSizeMB = undefined;
  request.value.optionsJson = undefined;
});
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

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
