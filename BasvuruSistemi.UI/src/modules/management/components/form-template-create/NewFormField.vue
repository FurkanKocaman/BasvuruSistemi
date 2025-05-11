<script setup lang="ts">
import { reactive, ref, watch } from "vue";
import { FormFieldDefinition } from "../../models/form-field.model";
import { useDropdown } from "../../composables/useDropdown";
import { FieldTypeOptions, getFieldTypeOptionByLabel } from "@/models/constants/field-type";
import { UserProfileFieldTypeOptions } from "@/models/constants/profile-field-type";

const fieldTypeDropdown = useDropdown();
const profileFieldTypeDropdown = useDropdown();

const isLinkedToUserProfile = ref(false);

const newField = reactive<FormFieldDefinition>({
  label: "",
  type: 0,
  isRequired: false,
  description: undefined,
  placeholder: undefined,
  optionsJson: undefined,
  isReadOnly: false,
  defaultValue: undefined,
  allowedFileTypes: undefined,
  maxFileSizeMB: undefined,
});

const emit = defineEmits<{
  (e: "add:field", payload: FormFieldDefinition): void;
  (e: "formTemplateId", payload: string): void;
}>();

const sendField = () => {
  emit("add:field", newField);
};

const addFieldToReqeust = () => {
  // if (!fields.value.find((p) => p.label == newField.label)) {
  //   fields.value.push({ ...newField });
  sendField();
  resetNewField();
  // }
};

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

const printField = () => {
  console.log("Field", newField);
  console.log("SelectedLabel", fieldTypeDropdown.selectedLabel.value);
};

watch(fieldTypeDropdown.selectedLabel, () => {
  newField.type = getFieldTypeOptionByLabel(fieldTypeDropdown.selectedLabel.value)?.value ?? 0;
});
</script>

<template>
  <div class="mx-1 my-1">
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
          @click="printField()"
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
          <label for="verificationType" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
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
</template>
