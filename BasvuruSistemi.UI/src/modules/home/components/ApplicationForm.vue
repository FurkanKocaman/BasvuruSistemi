<script setup lang="ts">
import { ref, reactive, computed, onMounted, defineProps, defineComponent } from "vue";
import { useJobPostingStore } from "@/stores/job-posting";
import { GetActiveJobPostingsQueryResponse } from "../models/active-job-posting.model";
import formTemplateService from "@/modules/management/services/form-template.service";
import { FormFieldDefinition } from "@/modules/management/models/form-field.model";
import DynamicFormRenderer from "./DynamicFormRenderer.vue";

const props = defineProps<{
  jobId?: string;
}>();

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

const fields = ref<FormFieldDefinition[]>([]);

const form = reactive({
  fullName: "",
  resumeFileName: "",
  resumeFile: null as File | null,
  department: "",
  coverLetter: "",
});

const errors = reactive({
  fullName: "",
  resume: "",
  department: "",
});

const handleFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    const file = target.files[0];

    const allowedTypes = [
      "application/pdf",
      "application/msword",
      "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
    ];
    if (!allowedTypes.includes(file.type)) {
      errors.resume = "Yalnızca PDF, DOC veya DOCX dosyaları yükleyebilirsiniz.";
      return;
    }

    if (file.size > 5 * 1024 * 1024) {
      errors.resume = "Dosya boyutu 5MB'dan küçük olmalıdır.";
      return;
    }

    form.resumeFile = file;
    form.resumeFileName = file.name;
    errors.resume = "";
  }
};

const getFormTemplate = async () => {
  if (job.value) {
    const res = await formTemplateService.getFormTemplate(job.value.formTemplateId);
    if (res) {
      fields.value = res?.fields;
    }
  }
};

onMounted(() => {
  if (props.jobId && jobPostings.value) {
    job.value = jobPostings.value.find((j) => j.id === props.jobId) || null;
    getFormTemplate();
  }
});
</script>

<template>
  <div
    class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-200 dark:border-gray-700"
  >
    <h2 class="text-2xl font-bold text-gray-800 dark:text-white mb-6">Başvuru Formu</h2>

    <!-- Başvuru formu -->
    <div class="space-y-6">
      <div>
        <label
          for="fullName"
          class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
        >
          Ad Soyad <span class="text-red-600">*</span>
        </label>
        <input
          id="fullName"
          v-model="form.fullName"
          type="text"
          required
          aria-required="true"
          class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          placeholder="Adınız ve soyadınız"
        />
        <p v-if="errors.fullName" class="mt-1 text-sm text-red-600">
          {{ errors.fullName }}
        </p>
      </div>
      <DynamicFormRenderer :fields="fields"></DynamicFormRenderer>
    </div>
  </div>
</template>
