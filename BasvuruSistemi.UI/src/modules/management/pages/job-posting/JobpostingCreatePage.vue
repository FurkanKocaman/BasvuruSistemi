<script setup lang="ts">
import DatePicker from "primevue/datepicker";
import { onMounted, reactive, Ref, ref, watch } from "vue";
import { useDropdown } from "../../composables/useDropdown";
import { JobPostingCreateModel } from "../../models/job-posting-create.model";
import organizationService from "../../services/unit.service";
import jobPostingService from "../../services/job-posting.service";
import DOMPurify from "dompurify";
import EditorComponent from "../../components/job-posting-components/EditorComponent.vue";
import formTemplateService from "../../services/form-template.service";
import { FormFieldDefinition } from "../../models/form-field.model";
import { Unit } from "../../models/unit-node.model";
import { useRoute, useRouter } from "vue-router";
import FieldsFromFormTemplate from "../../components/form-template-components/form-template-create/FieldsFromFormTemplate.vue";
import NewFormField from "../../components/form-template-components/form-template-create/NewFormField.vue";
import SelectedFormFields from "../../components/form-template-components/form-template-create/SelectedFormFields.vue";
import JobPostingGroupComponent from "../../components/job-posting-components/JobPostingGroupComponent.vue";
import { useToastStore } from "@/modules/toast/store/toast.store";
import FormTemplateSaveModal from "../../components/modals/FormTemplateSaveModal.vue";
import { FormTemplateCreateReqeust } from "../../models/form-template-create.model";
import { getJobPostingStatusOptionByValue } from "@/models/constants/job-posting-status";
import { Pen } from "lucide-vue-next";
import JobPostingStatusUpdateModal from "../../components/modals/JobPostingStatusUpdateModal.vue";
import EvaluationStagesPipelineComponent from "./EvaluationStagesPipelineComponent.vue";

const organizations: Ref<Unit[]> = ref([]);

const organizationsDropdown = useDropdown();

const toastStore = useToastStore();
const router = useRouter();

const isQuota = ref<boolean>(false);
const onlyDefinedUsers = ref<boolean>(false);
const isJobPostingPublished = ref<boolean>(false);

const formFieldAddingType = ref("FormTemplateAll");
const formTemplateDropdown = useDropdown();
const formTemplateSummaries: Ref<{ id: string; name: string }[]> = ref([]);

const fields = ref<FormFieldDefinition[]>([]);

const allowedNationalIdsText = ref<string>("");
const selectedFormTemplate = ref("");
const isNewTemplate = ref(false);

const confirmModal = ref();
const statusModal = ref();

const request = reactive<JobPostingCreateModel>({
  id: undefined,
  title: "",
  description: "",
  responsibilities: "",
  qualifications: "",
  benefits: "",

  datePosted: new Date(),
  applicationDeadline: new Date(),
  validFrom: new Date(),
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
  isAnonymous: false,
  minSalary: undefined,
  maxSalary: undefined,
  currency: undefined,

  unitId: undefined,

  formTemplateId: "",

  postingGroupId: undefined,

  evaluationPipelineStages: [],
});

const response = ref<JobPostingCreateModel | undefined>(undefined);

const route = useRoute();
const id = route.params.id as string | undefined;

onMounted(async () => {
  getOrganizations();
  getFormTemplateSummaries();
  if (id) {
    await getJobPosting(id);
  }
});

const getJobPosting = async (id: string) => {
  const res = await jobPostingService.getJobPosting(id);
  if (res) {
    response.value = res.data;

    request.id = res.data.id;
    request.title = res.data.title;
    request.description = res.data.description ?? "";
    request.responsibilities = res.data.responsibilities ?? "";
    request.qualifications = res.data.qualifications ?? "";
    request.benefits = res.data.benefits ?? "";

    request.datePosted = res.data.datePosted;
    request.applicationDeadline = new Date(res.data.applicationDeadline);
    request.validFrom = res.data.validFrom ? new Date(res.data.validFrom) : undefined;
    request.validTo = res.data.validTo ? new Date(res.data.validTo) : undefined;

    request.status = res.data.status;
    if (res.data.status == 2) isJobPostingPublished.value = true;
    request.isRemote = res.data.isRemote;
    request.locationText = res.data.locationText;

    request.vacancyCount = res.data.vacancyCount;
    if (res.data.vacancyCount) isQuota.value = true;
    request.employmentType = res.data.employmentType;
    request.experienceLevelRequired = res.data.experienceLevelRequired;
    request.skillsRequired = res.data.skillsRequired;

    request.allowedNationalIds = res.data.allowedNationalIds;
    if (res.data.allowedNationalIds?.length != 0) onlyDefinedUsers.value = true;

    request.contactInfo = res.data.contactInfo;
    request.isPublic = res.data.isPublic;
    request.isAnonymous = res.data.isAnonymous;
    request.minSalary = res.data.minSalary;
    request.maxSalary = res.data.maxSalary;
    request.currency = res.data.currency;

    request.unitId = res.data.unitId;

    request.formTemplateId = res.data.formTemplateId;

    request.postingGroupId = res.data.postingGroupId;

    if (organizations.value) {
      var organization = organizations.value.find((p) => p.id == request.unitId);
      if (organization) {
        organizationsDropdown.selectOption(organization.name);
      }
    }
    getExistingTemplate(res.data.formTemplateId);
  }
};

const getExistingTemplate = async (id: string | undefined) => {
  if (id) {
    const template = await formTemplateService.getFormTemplate(id);
    if (template) {
      if (formTemplateSummaries.value.find((p) => p.id == template.id)) {
        formTemplateDropdown.selectOption(template.name);
        selectedFormTemplate.value = template.name;
      } else {
        selectedFormTemplate.value = "Özel";
      }

      fields.value = template.fields;
      isNewTemplate.value = false;
    }
  }
};

const getOrganizations = async () => {
  const res = await organizationService.getUnitsByTenant();
  if (res) {
    organizations.value = res;

    if (request.unitId) {
      var organization = organizations.value.find((p) => p.id == request.unitId);
      if (organization) {
        organizationsDropdown.selectOption(organization.name);
      }
    }
  }
};

const selectOrganization = (organization: Unit) => {
  request.unitId = organization.id;
};

const setAllowedNationalIds = () => {
  if (allowedNationalIdsText.value) {
    request.allowedNationalIds = allowedNationalIdsText.value
      .split(",")
      .map((id) => id.trim())
      .filter((id) => id.length === 11);
  }
};

const handleSubmit = async () => {
  console.log(request.evaluationPipelineStages);

  request.description = DOMPurify.sanitize(request.description);
  if (request.responsibilities)
    request.responsibilities = DOMPurify.sanitize(request.responsibilities);
  if (request.qualifications) request.qualifications = DOMPurify.sanitize(request.qualifications);

  if (isJobPostingPublished.value) request.status = 2;

  setAllowedNationalIds();

  if (request.unitId && request.title && request.validFrom) {
    if (id) {
      //Update
      if (isNewTemplate.value) {
        const result = await confirmModal.value.open();
        if (result) {
          //Eğer template kaydedilmişse
          const req: FormTemplateCreateReqeust = {
            name: result,
            description: undefined,
            fields: fields.value,
            isSaved: true,
          };
          const formTemplateRes = await formTemplateService.createFormTemplate(req);
          if (formTemplateRes) {
            request.formTemplateId = formTemplateRes;
            const res = await jobPostingService.updateJobPostings(request);
            if (res) {
              router.push({ name: "job-posting-list" });
            }
          }
        } else {
          //Eğer template keydedilmemişse
          const req: FormTemplateCreateReqeust = {
            name: request.title + " (" + new Date().toLocaleDateString("tr-TR") + ")",
            description: undefined,
            fields: fields.value,
            isSaved: false,
          };
          const formTemplateRes = await formTemplateService.createFormTemplate(req);
          if (formTemplateRes) {
            request.formTemplateId = formTemplateRes;
            const res = await jobPostingService.createJobPostings(request);
            if (res) {
              router.push({ name: "job-posting-list" });
            }
          }
        }
      } else {
        console.log("Request", request);
        const res = await jobPostingService.updateJobPostings(request);
        if (res) {
          console.log("Response", res);
          router.push({ name: "job-posting-list" });
        }
      }
    } else {
      //Create
      if (isNewTemplate.value) {
        const result = await confirmModal.value.open();
        if (result) {
          //Eğer template kaydedilmişse
          const req: FormTemplateCreateReqeust = {
            name: result,
            description: undefined,
            fields: fields.value,
            isSaved: true,
          };
          const formTemplateRes = await formTemplateService.createFormTemplate(req);
          if (formTemplateRes) {
            request.formTemplateId = formTemplateRes;
            const res = await jobPostingService.createJobPostings(request);
            if (res) {
              router.push({ name: "job-posting-list" });
            }
          }
        } else {
          //Eğer template keydedilmemişse
          const req: FormTemplateCreateReqeust = {
            name: request.title + " (" + new Date().toLocaleDateString("tr-TR") + ")",
            description: undefined,
            fields: fields.value,
            isSaved: false,
          };
          const formTemplateRes = await formTemplateService.createFormTemplate(req);
          if (formTemplateRes) {
            request.formTemplateId = formTemplateRes;
            const res = await jobPostingService.createJobPostings(request);
            if (res) {
              router.push({ name: "job-posting-list" });
            }
          }
        }
      } else {
        console.log("Request", request);
        const res = await jobPostingService.createJobPostings(request);
        if (res) {
          console.log("Response", res);
          router.push({ name: "job-posting-list" });
        }
      }
    }
  } else {
    toastStore.addToast({
      message: "İlan oluşturmak için formu eksiksiz doldurmanız gerekmektedir.",
      type: "error",
      duration: 4000,
    });
  }
};

const getFormTemplate = async (label: string) => {
  const id = formTemplateSummaries.value.find((p) => p.name == label);
  if (id) {
    const res = await formTemplateService.getFormTemplate(id.id);
    if (res) {
      request.formTemplateId = res.id;
      fields.value = res.fields;

      selectedFormTemplate.value = label;
      isNewTemplate.value = false;
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
    selectedFormTemplate.value = "Özel";
    isNewTemplate.value = true;
  }
};

const removeFieldFromRequest = (label: string) => {
  fields.value = fields.value.filter((p) => p.label != label);
};

const selectPostingGroupId = (id?: string) => {
  request.postingGroupId = id;
};

const updateStatus = async () => {
  const result = await statusModal.value.open();
  if (result) {
    await jobPostingService.jobPostingChangeStatus(
      request.id!,
      result,
      result == 2 ? new Date() : undefined
    );
  }
};

watch(request.evaluationPipelineStages, () => console.log(request.evaluationPipelineStages));
</script>

<template>
  <div class="w-full px-50 pt-20 flex pb-20">
    <FormTemplateSaveModal
      ref="confirmModal"
      title="Oluşturduğunuz şablonu daha sonra kullanmak için kaydetmek ister misiniz?"
      description=""
    />
    <JobPostingStatusUpdateModal ref="statusModal" title="İlanın durumunu güncelleyin" />

    <div
      class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
    >
      <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200 flex justify-between">
        <span class="text-xl font-base dark:text-gray-50 text-gray-700">
          {{ id ? "İlan Düzenle" : "İlan Oluştur" }}
        </span>
        <div v-if="id" class="flex items-center gap-3">
          <span
            class="text-base font-base text-gray-50 px-2 py-1 rounded-lg"
            :class="getJobPostingStatusOptionByValue(request.status)?.class"
          >
            {{ getJobPostingStatusOptionByValue(request.status)?.label }}
          </span>
          <button class="cursor-pointer group" @click.stop="updateStatus()">
            <Pen
              class="text-gray-600 dark:text-gray-400 group-hover:text-gray-900 dark:group-hover:text-gray-50"
            />
          </button>
        </div>
      </div>
      <JobPostingGroupComponent
        @group-id-selected="selectPostingGroupId"
        :group-id="request.postingGroupId"
      />
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
                v-model="request.applicationDeadline"
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
              <div class="my-1">
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" v-model="request.isAnonymous" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >Anonim olarak başvuru kabul edilsin mi?</span
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

          <!-- Evaluation start -->
          <EvaluationStagesPipelineComponent v-model="request.evaluationPipelineStages" />
          <!-- Evaluation end -->
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
                <div class="flex items-center">
                  <div
                    v-if="selectedFormTemplate"
                    class="my-1 bg-gray-400/20 text-gray-700 dark:text-gray-300 rounded-md w-fit px-3 py-1 flex"
                  >
                    <span class="text-sm">Seçili Şablon :</span>
                    <span class="text-sm ml-1">{{ selectedFormTemplate }}</span>
                  </div>
                  <button
                    v-if="id"
                    class="cursor-pointer group ml-3 outline-none"
                    @click.stop="getExistingTemplate(response?.formTemplateId)"
                    title="Sıfırla"
                  >
                    <svg
                      class="size-7 group-hover:rotate-90 transition-all duration-200"
                      viewBox="0 0 24 24"
                      fill="none"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        d="M4.06189 13C4.02104 12.6724 4 12.3387 4 12C4 7.58172 7.58172 4 12 4C14.5006 4 16.7332 5.14727 18.2002 6.94416M19.9381 11C19.979 11.3276 20 11.6613 20 12C20 16.4183 16.4183 20 12 20C9.61061 20 7.46589 18.9525 6 17.2916M9 17H6V17.2916M18.2002 4V6.94416M18.2002 6.94416V6.99993L15.2002 7M6 20V17.2916"
                        class="stroke-gray-700 dark:stroke-gray-300"
                        stroke-width="2"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                      />
                    </svg>
                  </button>
                </div>
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
                      :ref="formTemplateDropdown.inputRef"
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
