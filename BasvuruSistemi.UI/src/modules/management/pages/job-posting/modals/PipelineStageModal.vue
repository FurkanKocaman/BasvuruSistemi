<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
    <div
      class="bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-2xl w-full p-6 animate-fadeIn"
      @click.stop
    >
      <div class="flex items-center">
        <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-200">Değerlendirme Ekle</h3>
      </div>

      <div class="mt-6 flex flex-col justify-start space-x-3">
        <label for="name" class="text-sm text-gray-700 dark:text-gray-300 mb-1"
          >Değerlendirme Adımı</label
        >
        <select
          name="name"
          id="name"
          v-model="request.evaluationStageId"
          @change="handleChange"
          class="outline-none px-1.5 py-1.5 border border-gray-200 dark:border-gray-700 rounded-md text-gray-700 dark:text-gray-300 bg-transparent text-sm dark:focus:border-indigo-600 focus:border-indigo-600"
        >
          <option value="" selected>Değerlendirme adımı seçin</option>
          <option
            v-for="stage in props.evaluationStages"
            :key="stage.id"
            :value="stage.id"
            class="dark:bg-gray-800 bg-gray-50"
          >
            {{ stage.name }}
          </option>
        </select>
      </div>
      <div class="mt-6 flex flex-col justify-start space-x-3">
        <label for="name" class="text-sm text-gray-700 dark:text-gray-300 mb-1"
          >Değerlendirme Formu</label
        >
        <select
          name="name"
          id="name"
          v-model="request.evaluationFormId"
          class="outline-none px-1.5 py-1 border border-gray-200 dark:border-gray-700 rounded-md text-gray-700 dark:text-gray-300 bg-transparent text-sm dark:focus:border-indigo-600 focus:border-indigo-600"
          :disabled="selectedStage == undefined"
        >
          <option value="" selected>Değerlendirme formu seçin</option>
          <option
            v-for="form in selectedStage?.evaluationForms"
            :key="form.id"
            :value="form.id"
            class="dark:bg-gray-800 bg-gray-50"
          >
            {{ form.name }}
          </option>
        </select>
      </div>

      <div class="mt-6 flex flex-col justify-start space-x-3">
        <label for="name" class="text-sm text-gray-700 dark:text-gray-300">Komisyon</label>
        <select
          name="name"
          id="name"
          v-model="request.responsibleCommissionId"
          class="outline-none px-1.5 py-1 border border-gray-200 dark:border-gray-700 rounded-md text-gray-700 dark:text-gray-300 bg-transparent text-sm dark:focus:border-indigo-600 focus:border-indigo-600"
        >
          <option value="" selected>Komisyon seçin</option>
          <option
            v-for="commission in commissions"
            :key="commission.id"
            :value="commission.id"
            class="dark:bg-gray-800 bg-gray-50"
          >
            {{ commission.name }}
          </option>
        </select>
      </div>
      <div class="mt-6">
        <label class="inline-flex items-center cursor-pointer">
          <input
            type="checkbox"
            v-model="isEvaluationDate"
            @change="toggleEvaluationDate"
            class="sr-only peer"
          />
          <div
            class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
          ></div>
          <span class="ms-3 text-sm dark:text-gray-300 text-gray-700"
            >Değerlendirme süreci el ile başlatılacak</span
          >
        </label>
      </div>
      <div v-if="!isEvaluationDate" class="flex flex-col my-2 items-start w-full">
        <label for="endDate" class="text-sm my-1 dark:text-gray-300 text-gray-600"
          >Değerlendirme Adımı Başlangıç Tarihi</label
        >
        <DatePicker
          input-id="endDate"
          v-model="request.startDate"
          class="!w-full !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !py-1.5 !px-2 focus:!border-indigo-600"
          inputClass="!text-start !text-sm  "
          panel-class="dark:!bg-gray-900 !bg-gray-50 dark:!text-gray-200 !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !p-3"
          showTime
          hourFormat="24"
          showButtonBar
          showIcon
        />
      </div>
      <div v-if="!isEvaluationDate" class="flex flex-col my-2 items-start w-full">
        <label for="endDate" class="text-sm my-1 dark:text-gray-300 text-gray-600"
          >Değerlendirme Adımı Bitiş Tarihi</label
        >
        <DatePicker
          input-id="endDate"
          v-model="request.endDate"
          class="!w-full !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !py-1.5 !px-2 focus:!border-indigo-600"
          inputClass="!text-start !text-sm  "
          panel-class="dark:!bg-gray-900 !bg-gray-50 dark:!text-gray-200 !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !p-3"
          showTime
          hourFormat="24"
          showButtonBar
          showIcon
        />
      </div>
      <div class="mt-6 flex justify-end items-center gap-3">
        <button
          class="px-4 py-2 text-sm bg-gray-600 dark:bg-gray-600 hover:bg-gray-400 dark:hover:bg-gray-500/10 text-white rounded-md cursor-pointer"
          @click="cancel"
        >
          İptal
        </button>
        <button
          class="px-4 py-2 text-sm bg-indigo-600 hover:bg-indigo-500 text-white rounded-md cursor-pointer"
          @click="confirm"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, defineExpose } from "vue";
import { PipelineStageDto } from "@/modules/management/models/evaluation/pipeline-stage.model";
import { EvaluationStageDto } from "@/modules/management/models/evaluation/evaluation-stage.model";
import { CommissionGetModel } from "@/modules/management/models/commission-get.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import DatePicker from "primevue/datepicker";

const visible = ref(false);
const resolveFn = ref(null);

const isEdit = ref(false);

const toastStore = useToastStore();

const isEvaluationDate = ref(false);

const props = defineProps<{
  evaluationStages: EvaluationStageDto[];
  commissions: CommissionGetModel[];
}>();

const selectedStage = ref<EvaluationStageDto | undefined>(undefined);

const request = ref<PipelineStageDto>({
  evaluationStageId: "",
  orderInPipeline: 0,
  isMandatory: false,
  evaluationFormId: "",
  responsibleCommissionId: "",
});

function open(pipelineStage?: PipelineStageDto) {
  if (pipelineStage) {
    isEdit.value = true;

    request.value = pipelineStage;
  } else {
    isEdit.value = false;
    request.value = {
      evaluationStageId: "",
      orderInPipeline: 0,
      isMandatory: true,
      evaluationFormId: "",
      responsibleCommissionId: "",
      startDate: undefined,
      endDate: undefined,
    };
  }

  visible.value = true;
  return new Promise((resolve) => {
    resolveFn.value = resolve;
  });
}

const handleChange = (event) => {
  selectedStage.value = props.evaluationStages.find((p) => p.id == request.value.evaluationStageId);
  request.value.evaluationFormId = "";
};

function confirm() {
  if (
    request.value.evaluationFormId &&
    request.value.evaluationStageId &&
    request.value.responsibleCommissionId
  ) {
    visible.value = false;
    resolveFn.value?.(request.value);
  } else {
    toastStore.addToast({
      message: "Tüm alanları doldurun",
      type: "error",
      duration: 3000,
    });
  }
}

const toggleEvaluationDate = () => {
  request.value.startDate = undefined;
  request.value.endDate = undefined;
};

function cancel() {
  visible.value = false;
  resolveFn.value?.(false);
}

defineExpose({ open }); // Parent çağırabilsin diye
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
</style>
