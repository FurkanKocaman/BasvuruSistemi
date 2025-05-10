<script setup lang="ts">
import { onMounted, reactive, Ref, ref, watch } from "vue";
import { FieldTypeOptions, getFieldTypeOptionByValue } from "@/models/constants/field-type";
import { VerificationTypeOptions } from "@/models/constants/verification-type";
import { useDropdown } from "../composables/useDropdown";
import * as icons from "lucide-vue-next";
import { FormFieldDefinition } from "../models/form-field.model";
import { UserProfileFieldTypeOptions } from "@/models/constants/profile-field-type";
import formTemplateService from "../services/form-template.service";
import { FormFieldResponse } from "../models/form-filed-response.model";

const formFieldAddingType = ref("NewField");

const formTemplateDropdown = useDropdown();
const fieldTypeDropdown = useDropdown();
const verificationTypeDropdown = useDropdown();
const profileFieldTypeDropdown = useDropdown();

const isVerificationAdded = ref(false);
const isLinkedToUserProfile = ref(false);

const props = defineProps<{
  fieldsInTemplate: FormFieldDefinition[] | undefined;
}>();

const selectedRows = ref<boolean[]>(FieldTypeOptions.map(() => false));
const formTemplateSummaries: Ref<{ id: string; name: string }[]> = ref([]);
const fieldsFromSelectedTemplate: Ref<FormFieldResponse[]> = ref([]);
const selectedFieldsToAdd: Ref<FormFieldResponse[]> = ref([]);

const fields: Ref<FormFieldDefinition[]> = ref([]);

const newField = reactive<FormFieldDefinition>({
  label: "",
  type: Number(fieldTypeDropdown.selectedLabel.value),
  isRequired: false,
  description: undefined,
  placeholder: undefined,
  optionsJson: undefined,
  isReadOnly: false,
  defaultValue: undefined,
  verificationSource: 0,
  verificationParametersJson: undefined,
  allowedFileTypes: undefined,
  maxFileSizeMB: undefined,
});

onMounted(async () => {
  getFormTemplateSummaries();

  if (props.fieldsInTemplate) {
    fields.value = props.fieldsInTemplate;
  }
});

const addFieldToReqeust = () => {
  if (!fields.value.find((p) => p.label == newField.label)) {
    fields.value.push({ ...newField });
    sendField();
    resetNewField();
  }
};

const addSelectedFieldsToRequest = () => {
  selectedFieldsToAdd.value.forEach((field) => {
    console.log(field);
    if (!fields.value.find((p) => p.label == field.label)) {
      console.log(field);
      const fieldToAdd: FormFieldDefinition = {
        label: field.label,
        type: field.type,
        isRequired: field.isRequired,
        description: field.description,
        placeholder: field.placeholder,
        optionsJson: field.optionsJson,
        isReadOnly: field.isReadOnly,
        defaultValue: field.defaultValue,
        verificationSource: field.verificationSource,
        verificationParametersJson: field.verificationParametersJson,
        allowedFileTypes: field.allowedFileTypes,
        maxFileSizeMB: field.maxFileSizeMB,
      };
      fields.value.push({ ...fieldToAdd });
    }
  });
};

const emit = defineEmits<{
  (e: "add:field", payload: FormFieldDefinition): void;
  (e: "formTemplateId", payload: string): void;
}>();

const getFormTemplateSummaries = async () => {
  const res = await formTemplateService.getFormTemplateSummaries();
  if (res) {
    formTemplateSummaries.value = res;
  }
};

const getFormTemplate = async (label: string) => {
  const id = formTemplateSummaries.value.find((p) => p.name == label);
  if (id) {
    setFormTemplateId(id.id);
    const res = await formTemplateService.getFormTemplate(id.id);
    if (res) {
      console.log(res);
      fields.value = res.fields;
    }
  }
};

const sendField = () => {
  emit("add:field", newField);
};

const setFormTemplateId = (id: string) => {
  emit("formTemplateId", id);
};

const getFieldFromSelectedTemplate = async () => {
  selectedFieldsToAdd.value = [];
  var selectedTemplate = formTemplateSummaries.value.find(
    (p) => p.name == formTemplateDropdown.selectedLabel.value
  );
  if (selectedTemplate) {
    const res = await formTemplateService.getFormTemplate(selectedTemplate.id);
    if (res) {
      fieldsFromSelectedTemplate.value = res.fields;
    }
  }
};

watch(fieldTypeDropdown.selectedLabel, (newLabel) => {
  const selectedOption = FieldTypeOptions.find((opt) => opt.label === newLabel);
  if (selectedOption) {
    newField.type = selectedOption.value;
  }
});

watch(
  () => props.fieldsInTemplate,
  (newVal) => {
    if (newVal) {
      fields.value = newVal;
    }
  },
  { immediate: true }
);

const resetNewField = () => {
  Object.assign(newField, {
    label: "",
    description: undefined,
    type: 0,
    isRequired: false,
    isReadOnly: false,
    verificationSource: 0,
    isLinkedToUserProfile: false,
    linkedUserProfileField: undefined,
  });
  fieldTypeDropdown.selectOption("");
};
// watch(fieldTypeDropdown.selectedLabel, () => handleSubmit());
</script>

<template>
  <div class="flex flex-col mt-5">
    <div class="flex justify-between">
      <label for="editor" class="w-full text-md my-1 dark:text-gray-300 text-gray-800 min-w-[10rem]"
        >Form Şablon Alanları</label
      >
      <button
        class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:text-gray-400 text-gray-700 hover:text-red-600 dark:hover:text-red-400 min-w-[10rem] hover:border-red-600 mr-3 dark:hover:border-red-600"
      >
        Seçili Olanları Sil
      </button>
      <!-- <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
              >
                Ekle
              </button> -->
    </div>
    <div class="flex flex-col border my-5 rounded-lg dark:border-gray-700 border-gray-200">
      <div class="my-1 mx-1">
        <ul class="flex dark:bg-gray-900 bg-gray-200/40 w-fit px-1 py-0.5 rounded-md select-none">
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mr-2 rounded-md text-sm"
            :class="
              formFieldAddingType === 'FormTemplateAll'
                ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                formFieldAddingType = 'FormTemplateAll';
                formTemplateDropdown.selectOption('');
              }
            "
          >
            Form Şablonu Seçimi
          </li>
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mr-2 rounded-md text-sm"
            :class="
              formFieldAddingType === 'FieldFromFormTemplate'
                ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                formFieldAddingType = 'FieldFromFormTemplate';
                formTemplateDropdown.selectOption('');
              }
            "
          >
            Mevcut Şablondan Alanlar Ekle
          </li>
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer rounded-md text-sm"
            :class="
              formFieldAddingType === 'NewField'
                ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                formFieldAddingType = 'NewField';
                formTemplateDropdown.selectOption('');
              }
            "
          >
            Yeni Alan Ekleme
          </li>
        </ul>
      </div>
      <!-- Form Şablonu Seç -->
      <div v-if="formFieldAddingType === 'FormTemplateAll'" class="mx-1 my-1">
        <div class="flex flex-col 2xl:flex-row justify-between w-full">
          <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
            <label for="formTeplate" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
              >Form Şablonu</label
            >
            <input
              type="text"
              name="formTeplate"
              id="formTeplate"
              v-model="formTemplateDropdown.selectedLabel.value"
              @focus="formTemplateDropdown.handleFocus"
              @blur="formTemplateDropdown.handleBlur"
              readonly
              placeholder="Alan tipi seçin..."
              autocomplete="off"
              class="w-[50%] border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            />
          </div>
          <div
            v-if="formTemplateDropdown.isOpen.value"
            class="absolute w-fit mt-18 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
          >
            <div
              v-for="option in formTemplateSummaries"
              :key="option.id"
              @mousedown.prevent="formTemplateDropdown.selectOption(option.name)"
              class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
            >
              {{ option.name }}
            </div>
          </div>
        </div>

        <div class="flex justify-end">
          <button
            class="text-sm border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
            @click="getFormTemplate(formTemplateDropdown.selectedLabel.value)"
          >
            Ekle
          </button>
        </div>
      </div>
      <!-- Field From Existing Template -->
      <div v-if="formFieldAddingType === 'FieldFromFormTemplate'" class="mx-1 my-1">
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
                v-for="(option, index) in formTemplateSummaries"
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
                      <input type="checkbox" v-model="selectedFieldsToAdd[index]" class="sr-only" />
                      <span
                        class="w-4 h-4 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                      >
                        <svg
                          v-if="selectedFieldsToAdd[index]"
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
      <!-- New Field -->
      <div v-if="formFieldAddingType === 'NewField'" class="mx-1 my-1">
        <div class="flex flex-col 2xl:flex-row justify-between w-full">
          <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
            <label
              for="organizaitonName"
              class="w-full text-sm my-1 dark:text-gray-300 text-gray-600 select-none"
              >Alan Adı</label
            >
            <input
              type="text"
              name="organizaitonName"
              id="organizaitonName"
              v-model="newField.label"
              autocomplete="off"
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            />
          </div>
          <div class="flex-1 flex flex-col my-2 items-start 2xl:ml-3">
            <label
              for="postingName"
              class="text-sm my-1 dark:text-gray-300 text-gray-600 select-none"
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
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
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
        </div>
        <div class="flex flex-col 2xl:flex-row justify-between w-full">
          <div class="flex-1 flex flex-col">
            <div class="flex flex-col my-2 items-start 2xl:mr-3">
              <label
                for="description"
                class="w-full text-sm my-1 dark:text-gray-300 text-gray-600 select-none"
                >Açıklama</label
              >
              <textarea
                type="text"
                name="description"
                id="description"
                v-model="newField.description"
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 min-h-20 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              ></textarea>
            </div>
            <div
              v-if="newField.type == 6 || newField.type == 13 || newField.type == 15"
              class="flex flex-col my-2 items-start 2xl:mr-3"
            >
              <label
                for="allowedFileTypes"
                class="w-full text-sm my-1 dark:text-gray-300 text-gray-600 select-none"
                >Desteklenen Dosya Türleri
                <span class="text-xs">(Virgül ile ayırarak giriniz)</span></label
              >
              <input
                type="text"
                name="allowedFileTypes"
                id="allowedFileTypes"
                v-model="newField.allowedFileTypes"
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
            </div>
            <div
              v-if="newField.type == 6 || newField.type == 13 || newField.type == 15"
              class="flex flex-col my-2 items-start 2xl:mr-3"
            >
              <label
                for="maxFileSizeMB"
                class="w-full text-sm my-1 dark:text-gray-300 text-gray-600 select-none"
                >Maksimum Dosya Boyutu</label
              >
              <input
                type="text"
                name="maxFileSizeMB"
                id="maxFileSizeMB"
                v-model="newField.maxFileSizeMB"
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
            </div>
          </div>
          <div class="flex-1 flex flex-col my-2 items-start 2xl:ml-3">
            <label class="w-full text-sm my-1 dark:text-gray-300 text-gray-600 select-none"
              >Ayarlar</label
            >
            <div class="my-1">
              <label class="inline-flex items-center cursor-pointer">
                <input
                  type="checkbox"
                  :value="false"
                  v-model="newField.isRequired"
                  class="sr-only peer"
                />
                <div
                  class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                ></div>
                <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                  >Zorunlu Mu?</span
                >
              </label>
            </div>

            <div class="my-1">
              <label class="inline-flex items-center cursor-pointer">
                <input
                  type="checkbox"
                  :value="true"
                  v-model="isLinkedToUserProfile"
                  class="sr-only peer"
                />
                <div
                  class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                ></div>
                <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                  >Kullanıcı profilinden al?</span
                >
              </label>
            </div>
            <div v-if="isLinkedToUserProfile" class="flex-1 flex flex-col items-start w-full mb-2">
              <label
                for="verificationType"
                class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                >Profildeki Alan Adı</label
              >
              <input
                :ref="profileFieldTypeDropdown.inputRef"
                type="text"
                name="postingName"
                id="postingName"
                autocomplete="off"
                v-model="profileFieldTypeDropdown.selectedLabel.value"
                @focus="profileFieldTypeDropdown.handleFocus"
                @blur="profileFieldTypeDropdown.handleBlur"
                readonly
                placeholder="Doğrulama tipi seçin..."
                class="flex-1 w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
              <div
                v-if="profileFieldTypeDropdown.isOpen.value"
                class="absolute w-fit mt-16 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
              >
                <div
                  v-for="option in UserProfileFieldTypeOptions"
                  :key="option.value"
                  @mousedown.prevent="profileFieldTypeDropdown.selectOption(option.label)"
                  class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer"
                >
                  {{ option.label }}
                </div>
              </div>
            </div>

            <div class="my-1">
              <label class="inline-flex items-center cursor-pointer">
                <input
                  type="checkbox"
                  value=""
                  v-model="isVerificationAdded"
                  class="sr-only peer"
                />
                <div
                  class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                ></div>
                <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                  >Doğrulama Ekle?</span
                >
              </label>
            </div>
            <div v-if="isVerificationAdded" class="flex flex-col items-start w-full">
              <label
                for="verificationType"
                class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                >Doğrulama Tipi</label
              >
              <input
                :ref="verificationTypeDropdown.inputRef"
                type="text"
                name="postingName"
                id="postingName"
                autocomplete="off"
                v-model="verificationTypeDropdown.selectedLabel.value"
                @focus="verificationTypeDropdown.handleFocus"
                @blur="verificationTypeDropdown.handleBlur"
                readonly
                placeholder="Doğrulama tipi seçin..."
                class="flex-1 w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
              <div
                v-if="verificationTypeDropdown.isOpen.value"
                class="absolute w-fit mt-16 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
              >
                <div
                  v-for="option in VerificationTypeOptions"
                  :key="option.value"
                  @mousedown.prevent="verificationTypeDropdown.selectOption(option.label)"
                  class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer"
                >
                  {{ option.label }}
                </div>
              </div>
            </div>
          </div>
        </div>
        <div
          v-if="newField.type == 4 || newField.type == 5"
          class="flex-1 flex flex-col my-2 items-start 2xl:mr-3"
        >
          <label for="options" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
            >Seçenekler <span class="text-xs">(Virgül ile ayırarak seçenekleri girin)</span></label
          >
          <textarea
            type="text"
            name="options"
            id="options"
            v-model="newField.optionsJson"
            autocomplete="off"
            class="w-full border outline-none rounded-md py-1.5 px-2 min-h-20 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
          ></textarea>
        </div>
        <div class="flex justify-end">
          <button
            @click="addFieldToReqeust()"
            class="text-sm border rounded-md py-1.5 px-3 mr-3 mb-3 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
          >
            Ekle
          </button>
        </div>
      </div>
    </div>
    <div class="my-2">
      <label class="w-full text-normal mb-2 dark:text-gray-300 text-gray-800">Form Alanları</label>
      <table class="w-full">
        <thead></thead>
        <tbody class="divide-y dark:divide-gray-700 divide-gray-200">
          <tr v-for="(row, index) in fields" :key="index" class="flex">
            <td class="w-10 py-3 flex items-center">
              <label class="inline-flex items-center cursor-pointer select-none group">
                <input type="checkbox" v-model="selectedRows[index]" class="sr-only" />
                <span
                  class="w-5 h-5 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                >
                  <svg
                    v-if="selectedRows[index]"
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
            <td class="w-14 py-3 flex items-center">
              <component
                :is="icons[getFieldTypeOptionByValue(row.type)?.icon]"
                class="w-4 h-4 text-gray-500"
              />
            </td>
            <td class="flex-1 flex flex-col justify-center py-3">
              <span class="dark:text-gray-200">{{ row.label }}</span>
              <span class="text-xs dark:text-gray-400">{{ row.description }}</span>
            </td>

            <td class="py-3 px-2 flex items-center">
              <button class="cursor-pointer pr-1 group">
                <svg
                  class="size-6 dark:fill-gray-400 fill-gray-600 group-hover:fill-blue-600 dark:group-hover:fill-blue-600"
                  viewBox="0 0 24 24"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M18.111,2.293,9.384,11.021a.977.977,0,0,0-.241.39L8.052,14.684A1,1,0,0,0,9,16a.987.987,0,0,0,.316-.052l3.273-1.091a.977.977,0,0,0,.39-.241l8.728-8.727a1,1,0,0,0,0-1.414L19.525,2.293A1,1,0,0,0,18.111,2.293ZM11.732,13.035l-1.151.384.384-1.151L16.637,6.6l.767.767Zm7.854-7.853-.768.767-.767-.767.767-.768ZM3,5h8a1,1,0,0,1,0,2H4V20H17V13a1,1,0,0,1,2,0v8a1,1,0,0,1-1,1H3a1,1,0,0,1-1-1V6A1,1,0,0,1,3,5Z"
                  />
                </svg>
              </button>
              <button class="cursor-pointer pr-1">
                <svg
                  class="size-6 group"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <path
                    d="M20.5001 6H3.5"
                    class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
                    class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M9.5 11L10 16"
                    class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M14.5 11L14 16"
                    class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                    stroke-width="1.5"
                    stroke-linecap="round"
                  />
                  <path
                    d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65651 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
                    class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                    stroke-width="1.5"
                  />
                </svg>
              </button>
              <button class="cursor-pointer pr-1 group">
                <svg
                  class="size-6 group"
                  viewBox="0 0 24 24"
                  fill="none"
                  xmlns="http://www.w3.org/2000/svg"
                >
                  <g id="Edit / Move_Vertical">
                    <path
                      id="Vector"
                      d="M12 21V3M12 21L15 18M12 21L9 18M12 3L9 6M12 3L15 6"
                      class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
                      stroke-width="1.5"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                    />
                  </g>
                </svg>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped></style>
