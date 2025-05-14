<script setup lang="ts">
import { ref, computed, onMounted, defineProps, defineComponent } from "vue";
import { useJobPostingStore } from "@/stores/job-posting";
import { GetActiveJobPostingsQueryResponse } from "../models/active-job-posting.model";
import formTemplateService from "@/modules/management/services/form-template.service";
import { getComponentByFieldType } from "../services/getComponentByField.service";
import { useRouter } from "vue-router";
import { FormFieldResponse } from "@/modules/management/models/form-filed-response.model";
import { FieldValueModel } from "../models/field-value.model";
import { ApplicationCreateRequest } from "../models/application-create.model";
import applicationService from "../services/application.service";
import { useToastStore } from "@/modules/toast/store/toast.store";

const props = defineProps<{
  jobId?: string;
}>();

const router = useRouter();

const values = ref<Record<string, any>>({});

const toastStore = useToastStore();

const request = ref<ApplicationCreateRequest>({
  jobPostingId: "",
  fieldValues: [],
});

defineComponent({
  name: "ApplicationForm",
  props: {
    jobId: Number,
  },
  emits: ["submit"],
});

const jobPostingStore = useJobPostingStore();
const job = ref<GetActiveJobPostingsQueryResponse | null>(null);
const jobPostings = computed(() => jobPostingStore.jobPostings);

const fields = ref<FormFieldResponse[]>([]);

const getFormTemplate = async () => {
  if (job.value) {
    request.value.jobPostingId = job.value.id;
    const res = await formTemplateService.getFormTemplate(job.value.formTemplateId);
    if (res) {
      fields.value = res.fields;
      res.fields.forEach((element) => {
        values.value[element.id] = undefined;
      });
    }
  }
};

onMounted(() => {
  if (props.jobId && jobPostings.value) {
    job.value = jobPostings.value.find((j) => j.id === props.jobId) || null;
    getFormTemplate();
  }
});

const goBack = () => {
  router.back();
};
const submitForm = async () => {
  let isError = false;

  for (const field of fields.value) {
    if (field.type === 6 || field.type === 13 || field.type === 15 || field.type === 16) {
      const file = values.value[field.id];
      if (file) {
        try {
          const res = await applicationService.uploadFileByField(field.id, file);
          if (res.statusCode === 200) {
            values.value[field.id] = res.data;
          } else {
            isError = true;
            console.error(res.errorMessages?.[0] ?? "Bilinmeyen hata");
            return;
          }
        } catch (err) {
          isError = true;
          console.error("Dosya yükleme hatası:", err);
          return;
        }
      }
    }
    if (field.isRequired && !values.value[field.id]) {
      isError = true;
      toastStore.addToast({
        message: "Zorunlu alanları doldurmanız gerekiyor",
        type: "error",
        duration: 5000,
      });
      return;
    }
  }

  const result: FieldValueModel[] = Object.entries(values.value).map(([fieldId, val]) => ({
    fieldDefinitionId: fieldId,
    value: val,
  }));

  request.value.fieldValues = result;

  if (isError) {
    return;
  } else {
    await applicationService.createApplication(request.value);

    router.push({ name: "/jobs" });
  }
};
</script>

<template>
  <div
    class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-200 dark:border-gray-700"
  >
    <h2 class="text-2xl font-bold text-gray-800 dark:text-white mb-6">Başvuru Formu</h2>

    <!-- Başvuru formu -->
    <div class="space-y-6">
      <div v-for="field in fields" :key="field.id">
        <component
          :is="getComponentByFieldType(field.type)"
          v-model="values[field.id]"
          :field="field"
        />
      </div>
    </div>

    <div class="flex flex-col sm:flex-row space-y-3 sm:space-y-0 sm:space-x-3">
      <button
        type="button"
        @click="goBack"
        class="w-full sm:w-auto flex-1 bg-gray-200 dark:bg-gray-600 hover:bg-gray-300 dark:hover:bg-gray-500 text-gray-800 dark:text-white font-medium py-2 px-6 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-opacity-50 transition-colors duration-200"
      >
        İptal
      </button>
      <button
        type="button"
        class="w-full sm:w-auto flex-1 bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-6 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200 disabled:opacity-70 disabled:cursor-not-allowed"
        @click="submitForm"
      >
        <span>Başvuruyu Gönder</span>
      </button>
    </div>
  </div>
</template>
