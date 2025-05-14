<script setup lang="ts">
import DatePicker from "primevue/datepicker";
import { onMounted, reactive, Ref, ref } from "vue";
import { useDropdown } from "../composables/useDropdown";
import { JobPostingCreateModel } from "../models/job-posting-create.model";
import organizationService from "../services/organization.service";
import jobPostingService from "../services/job-posting.service";
import DOMPurify from "dompurify";
import EditorComponent from "../components/EditorComponent.vue";
import formTemplateService from "../services/form-template.service";
import { FormFieldDefinition } from "../models/form-field.model";
import NewFormField from "../components/form-template-create/NewFormField.vue";
import FieldsFromFormTemplate from "../components/form-template-create/FieldsFromFormTemplate.vue";
import SelectedFormFields from "../components/form-template-create/SelectedFormFields.vue";

const organizations: Ref<{ name: string; unitId: string }[]> = ref([]);

const organizationsDropdown = useDropdown();

const isQuota = ref<boolean>(false);
const onlyDefinedUsers = ref<boolean>(false);
const isJobPostingPublished = ref<boolean>(false);

const formFieldAddingType = ref("FormTemplateAll");
const formTemplateDropdown = useDropdown();
const formTemplateSummaries: Ref<{ id: string; name: string }[]> = ref([]);

const fields = ref<FormFieldDefinition[]>([]);

const allowedNationalIdsText = ref<string>("");

const request = reactive<JobPostingCreateModel>({
  title: "",
  description: "",
  responsibilities: undefined,
  qualifications: undefined,
  benefits: undefined,

  datePosted: new Date(),
  applicationDeadLine: new Date(),
  validFrom: undefined,
  validTo: undefined,

  status: 1,
  isRemote: false,
  locationText: undefined,

  vacancyCount: undefined,
  employmentType: undefined,
  experienceLevelRequired: undefined,
  skillsRequired: undefined,

  allowedNationalIds: undefined,

  contactInfo: undefined,
  isPublic: true,

  unitId: undefined,

  formTemplateId: "",

  postingGroupId: undefined,
});

//update eklemek için kullanılacak
// const route = useRoute();
// const id = route.params.id as string | undefined;

onMounted(() => {
  getOrganizations();
  getFormTemplateSummaries();
});

const getOrganizations = async () => {
  const res = await organizationService.getOrganizationsByTenant();
  if (res) {
    organizations.value = res;
  }
};

const selectOrganization = (organization: { name: string; unitId: string }) => {
  request.unitId = organization.unitId;
};

const setAllowedNationalIds = () => {
  if (allowedNationalIdsText.value) {
    request.allowedNationalIds = allowedNationalIdsText.value
      .split(",")
      .map((id) => id.trim())
      .filter((id) => id.length === 11);
  }
};

//Form şablonu seçme kısmında var olan şablonun id değerini alıyor ilerde job posting oluşturma sırasında form template oluşturma da eklenir şuanda aktif değiller.

const handleSubmit = async () => {
  request.description = DOMPurify.sanitize(request.description);

  if (isJobPostingPublished.value) request.status = 2;

  setAllowedNationalIds();

  console.log("Job posting create request", request);

  await jobPostingService.createJobPostings(request);
};

const getFormTemplate = async (label: string) => {
  const id = formTemplateSummaries.value.find((p) => p.name == label);
  if (id) {
    const res = await formTemplateService.getFormTemplate(id.id);
    if (res) {
      request.formTemplateId = res.id;
      fields.value = res.fields;
    }
  }
};

const getFormTemplateSummaries = async () => {
  const res = await formTemplateService.getFormTemplateSummaries();
  if (res) {
    formTemplateSummaries.value = res;
  }
};

const addFieldToRequest = (field: FormFieldDefinition) => {
  if (!fields.value.find((p) => p.label == field.label)) {
    fields.value.push({ ...field });
  }
};

const removeFieldFromRequest = (label: string) => {
  fields.value = fields.value.filter((p) => p.label != label);
};
</script>

<template>
  <div class="w-full px-50 pt-20 flex pb-20">
    <div
      class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
    >
      <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200">
        <span class="text-xl font-base dark:text-gray-50 text-gray-700">İlan Oluştur</span>
      </div>
      <div class="px-10 py-5">
        <div
          class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200 flex flex-col px-20"
        >
          <div class="flex flex-col 2xl:flex-row justify-between w-full">
            <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
              <label
                for="organizaitonName"
                class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                >Birim</label
              >
              <input
                type="text"
                name="organizaitonName"
                id="organizaitonName"
                autocomplete="off"
                :ref="organizationsDropdown.inputRef"
                v-model="organizationsDropdown.selectedLabel.value"
                @focus="organizationsDropdown.handleFocus"
                @blur="organizationsDropdown.handleBlur"
                readonly
                placeholder="Birim seçin..."
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
              <div
                v-if="organizationsDropdown.isOpen.value"
                class="absolute w-fit mt-16 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
              >
                <div
                  v-for="(option, index) in organizations"
                  :key="index"
                  @mousedown.prevent="
                    () => {
                      organizationsDropdown.selectOption(option.name);
                      selectOrganization(option);
                    }
                  "
                  class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
                >
                  {{ option.name }}
                </div>
              </div>
            </div>
            <div class="flex-1 flex flex-col my-2 items-start 2xl:ml-3">
              <label for="postingName" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >İlan Adı</label
              >
              <input
                type="text"
                name="postingName"
                id="postingName"
                v-model="request.title"
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
            </div>
          </div>
          <div class="flex flex-col xl:flex-row justify-start w-full">
            <div class="flex flex-col my-2 items-start w-full 2xl:mr-3">
              <label for="activeDate" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >İlk Başvuru Tarihi</label
              >
              <DatePicker
                input-id="activeDate"
                v-model="request.validFrom"
                class="!w-full !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !py-1.5 !px-2 !bg-gray-50 dark:!bg-gray-900/50 dark:!focus:border-indigo-600 focus:border-indigo-600"
                inputClass="!text-start !text-sm  "
                panel-class="dark:!bg-gray-900 !bg-gray-50 dark:!text-gray-200 !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !p-3"
                showTime
                hourFormat="24"
                showButtonBar
                showIcon
              />
            </div>
            <div class="flex flex-col my-2 items-start w-full 2xl:ml-3">
              <label for="endDate" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >Son Başvuru Tarihi</label
              >
              <DatePicker
                input-id="endDate"
                v-model="request.applicationDeadLine"
                class="!w-full !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !py-1.5 !px-2 !bg-gray-50 dark:!bg-gray-900/50 focus:!border-indigo-600"
                inputClass="!text-start !text-sm  "
                panel-class="dark:!bg-gray-900 !bg-gray-50 dark:!text-gray-200 !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !p-3"
                showTime
                hourFormat="24"
                showButtonBar
                showIcon
              />
            </div>
          </div>
          <div class="flex xl:flex-row flex-col justify-between">
            <div class="flex-1 flex flex-col 2xl:mr-3">
              <div>
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" v-model="isQuota" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >Kontenjan var mı?</span
                  >
                </label>
                <div v-if="isQuota" class="flex-1 flex flex-col mb-2 items-start">
                  <label for="quota" class="text-sm dark:text-gray-300 text-gray-600"
                    >Kontenjan Limiti</label
                  >
                  <input
                    type="number"
                    name="quota"
                    id="quota"
                    v-model="request.vacancyCount"
                    autocomplete="off"
                    class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
                  />
                </div>
              </div>
              <div>
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" v-model="onlyDefinedUsers" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >Sadece Tanımlı Kişilere Göster?</span
                  >
                </label>
                <div v-if="onlyDefinedUsers" class="flex-1 flex flex-col mb-2 items-start">
                  <label for="tckns" class="text-sm dark:text-gray-300 text-gray-600"
                    >Tanımlı Kişilerin TC Kimlik Numaralarını ',' ile ayırarak girin</label
                  >
                  <input
                    type="text"
                    name="tckns"
                    id="tckns"
                    autocomplete="off"
                    class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
                  />
                </div>
              </div>
            </div>
            <div class="flex-1 flex flex-col 2xl:ml-3">
              <div class="my-1">
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" value="" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >Kabul Edilenlere Belge Oluştur Butonu Çıkar?</span
                  >
                </label>
              </div>
              <div class="my-1">
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" v-model="isJobPostingPublished" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >İlan Yayında Mı?</span
                  >
                </label>
              </div>
            </div>
          </div>
          <EditorComponent v-model="request.description" :label="'Açıklama'"></EditorComponent>

          <EditorComponent
            v-model="request.responsibilities!"
            :label="'Sorumluluklar'"
          ></EditorComponent>

          <EditorComponent
            v-model="request.qualifications!"
            :label="'Gereken Özellikler '"
          ></EditorComponent>

          <!-- FormTemplate Edit and Select start -->

          <div class="flex flex-col mt-5">
            <!-- Form Şablon -->
            <div class="flex justify-between">
              <label
                for="editor"
                class="w-full text-md my-1 dark:text-gray-300 text-gray-800 min-w-[10rem]"
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
                <ul
                  class="flex dark:bg-gray-900 bg-gray-200/40 w-fit px-1 py-0.5 rounded-md select-none"
                >
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
                    <label
                      for="formTeplate"
                      class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
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
                      placeholder="Form şablonu seçin..."
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
              <FieldsFromFormTemplate
                v-if="formFieldAddingType === 'FieldFromFormTemplate'"
                :form-template-summaries="formTemplateSummaries"
                @add:field="addFieldToRequest"
              ></FieldsFromFormTemplate>

              <!-- New Field -->
              <NewFormField
                v-if="formFieldAddingType === 'NewField'"
                @add:field="addFieldToRequest"
              ></NewFormField>
            </div>

            <!-- Form Alanları -->
            <SelectedFormFields
              :fields="fields"
              @remove:field="removeFieldFromRequest"
            ></SelectedFormFields>
          </div>
          <!-- FormTemplate Edit and Select end -->

          <div class="my-4 flex justify-end">
            <button
              class="border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
              @click="handleSubmit()"
            >
              Kaydet
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped></style>
