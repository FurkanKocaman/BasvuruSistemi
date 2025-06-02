<script setup lang="ts">
import { ref, onMounted, defineProps, defineComponent } from "vue";
import { GetActiveJobPostingsQueryResponse } from "../models/active-job-posting.model";
import formTemplateService from "@/modules/management/services/form-template.service";
import { getComponentByFieldType } from "../services/getComponentByField.service";
import { useRoute, useRouter } from "vue-router";
import { FormFieldResponse } from "@/modules/management/models/form-filed-response.model";
import { FieldValueModel } from "../models/field-value.model";
import { ApplicationCreateRequest } from "../models/application-create.model";
import applicationService from "../services/application.service";
import { useToastStore } from "@/modules/toast/store/toast.store";

const props = defineProps<{
  job?: GetActiveJobPostingsQueryResponse;
}>();

const router = useRouter();
const route = useRoute();

const applicationId = route.query.applicationId;

const values = ref<Record<string, any>>({});

const toastStore = useToastStore();

const request = ref<ApplicationCreateRequest>({
  jobPostingId: "",
  fieldValues: [],
});

const existingFiles = ref<Record<string, any>>({});

defineComponent({
  name: "ApplicationForm",
  props: {
    jobId: Number,
  },
  emits: ["submit"],
});

const fields = ref<FormFieldResponse[]>([]);

const getFormTemplate = async () => {
  if (props.job) {
    request.value.jobPostingId = props.job.id;
    const res = await formTemplateService.getFormTemplate(props.job.formTemplateId);
    if (res) {
      fields.value = res.fields;
      res.fields.forEach((element) => {
        values.value[element.id] = undefined;
      });
      if (applicationId) {
        getApplicationFieldValues(applicationId.toString());
      }
    }
  }
};

onMounted(async () => {
  if (props.job) {
    getFormTemplate();
  }
});

const getApplicationFieldValues = async (id: string) => {
  const res = await applicationService.getApplicationForUpdate(id);
  if (res) {
    res.fieldValues.forEach(async (element) => {
      if (element.type == 6 || element.type == 13 || element.type == 15 || element.type == 16) {
        if (element.value) {
          const file = await urlToFile(element.value);

          values.value[element.id] = file;
          existingFiles.value[element.id] = file;
        }
      } else {
        values.value[element.id] = element.value;
      }
    });
  }
};

async function urlToFile(url: string): Promise<File> {
  const response = await fetch(url);
  const blob = await response.blob();
  return new File([blob], url);
}

const goBack = () => {
  router.back();
};
const submitForm = async () => {
  let isError = false;

  for (const field of fields.value) {
    if (field.type === 6 || field.type === 13 || field.type === 15 || field.type === 16) {
      const file = values.value[field.id];
      if (file && props.job && existingFiles.value[field.id] != file) {
        try {
          const res = await applicationService.uploadFileByField(props.job.id, field.id, file);
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
    if (field.type === 3) {
      if (Array.isArray(values.value[field.id]))
        values.value[field.id] = values.value[field.id].join(", ");
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
    if (applicationId != undefined) {
      request.value.applicationId = applicationId.toString();
      await applicationService.upteApplication(request.value);
    } else {
      await applicationService.createApplication(request.value);
    }

    router.push("/jobs");
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

    <div v-if="applicationId" class="flex flex-col sm:flex-row space-y-3 sm:space-y-0 sm:space-x-3">
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
