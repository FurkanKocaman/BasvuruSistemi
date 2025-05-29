<script setup lang="ts">
import { onMounted, ref } from "vue";
import PipelineStageModal from "./modals/PipelineStageModal.vue";
import { PipelineStageDto } from "../../models/evaluation/pipeline-stage.model";
import evaluationService from "../../services/evaluation.service";
import { EvaluationStageDto } from "../../models/evaluation/evaluation-stage.model";
import commissionService from "../../services/commission.service";
import { CommissionGetModel } from "../../models/commission-get.model";

// defineProps(["modelValue"]);
// defineEmits(["update:modelValue"]);

const pipelineStageModal = ref();

const pipelineStages = ref<PipelineStageDto[]>([]);

const evaluationStages = ref<EvaluationStageDto[]>([]);
const commissions = ref<CommissionGetModel[]>([]);

onMounted(() => {
  getEvaluationStages();
  getCommissions();
});

const emit = defineEmits(["update:modelValue"]);
const props = defineProps({
  modelValue: {
    type: Array,
    required: true,
  },
});

const addPipelineStage = async () => {
  const result = await pipelineStageModal.value.open();
  if (result) {
    result.orderInPipeline = pipelineStages.value.length + 1;
    pipelineStages.value.push(result);
    emit("update:modelValue", [...pipelineStages.value]);
  }
};
const updatePipelineStage = async (pipelineStage: PipelineStageDto) => {
  const result = await pipelineStageModal.value.open(pipelineStage);
  if (result) {
    // pipelineStages.value.find((p) => p.evaluationStageId == result.evaluationStageId) = result;
    console.log(result);
    emit("update:modelValue", [...pipelineStages.value]);
  }
};

const removePipelineStage = async (pipelineStage: PipelineStageDto) => {
  pipelineStages.value = pipelineStages.value.filter((p) => p != pipelineStage);
};

const getEvaluationStages = async () => {
  const res = await evaluationService.listEvaluationStages();
  if (res) {
    evaluationStages.value = res;
  }
};
const getCommissions = async () => {
  const res = await commissionService.getCommissions();
  if (res) {
    commissions.value = res;
  }
};

function formatDateTime(value: string): string {
  const date = new Date(value);

  const day = date.getDate().toString().padStart(2, "0");
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const year = date.getFullYear();

  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");

  return `${day}.${month}.${year} ${hours}:${minutes}`;
}
</script>

<template>
  <!-- Commission or SingleUser start -->
  <div class="mt-5 flex flex-col">
    <PipelineStageModal
      ref="pipelineStageModal"
      :evaluation-stages="evaluationStages"
      :commissions="commissions"
    />
    <div>
      <span class="dark:text-gray-300 text-gray-800">Değerlendirme</span>
    </div>
    <div
      class="border rounded-md border-gray-200 dark:border-gray-700 mt-2 flex xl:flex-row lg:flex-col divide-x divide-gray-200 dark:divide-gray-700 text-gray-800 dark:text-gray-50"
    >
      <div class="flex-1 px-3 py-3 flex flex-col">
        <div class="flex justify-between">
          <span class="text-sm">Değerlendirme Adımları</span>
          <button
            class="border rounded-md dark:border-gray-700 border-gray-200 text-sm dark:text-gray-200 text-gray-700 dark:hover:bg-gray-700/20 hover:bg-gray-200/20 cursor-pointer px-3 py-1.5"
            @click.stop="addPipelineStage()"
          >
            Ekle
          </button>
        </div>
        <div class="mt-2 w-full">
          <table
            v-if="pipelineStages.length"
            class="w-full text-sm text-gray-800 dark:text-gray-100"
          >
            <thead>
              <tr class="border-b border-gray-200 dark:border-gray-700">
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700">Sıra</td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700">Adım</td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700">Form</td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700">Komisyon</td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700">Başlangıç</td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700">Bitiş</td>
                <td class="py-3 px-2 border-gray-200 dark:border-gray-700">İşlemler</td>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="pipelineStage in pipelineStages"
                :key="pipelineStage.evaluationStageId"
                class="border-b border-t border-gray-200 dark:border-gray-700 hover:bg-gray-100 dark:hover:bg-gray-900/30"
              >
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700">1</td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700 wrap-break-word">
                  {{ evaluationStages.find((p) => p.id == pipelineStage.evaluationStageId)?.name }}
                </td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700 wrap-break-word">
                  {{
                    evaluationStages
                      .find((p) => p.id == pipelineStage.evaluationStageId)
                      ?.evaluationForms.find((p) => p.id == pipelineStage.evaluationFormId)?.name
                  }}
                </td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700 wrap-break-word">
                  {{ commissions.find((p) => p.id == pipelineStage.responsibleCommissionId)?.name }}
                </td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700 wrap-break-word">
                  {{
                    pipelineStage.startDate
                      ? formatDateTime(pipelineStage.startDate.toString())
                      : "-"
                  }}
                </td>
                <td class="py-3 px-2 border-r border-gray-200 dark:border-gray-700 wrap-break-word">
                  {{
                    pipelineStage.endDate ? formatDateTime(pipelineStage.endDate.toString()) : "-"
                  }}
                </td>
                <td class="py-3 px-2 w-60">
                  <button
                    class="cursor-pointer pr-1 group"
                    @click.stop="updatePipelineStage(pipelineStage)"
                  >
                    <svg
                      class="size-5 dark:fill-gray-400 fill-gray-600 group-hover:fill-blue-600 dark:group-hover:fill-blue-600"
                      viewBox="0 0 24 24"
                      xmlns="http://www.w3.org/2000/svg"
                    >
                      <path
                        d="M18.111,2.293,9.384,11.021a.977.977,0,0,0-.241.39L8.052,14.684A1,1,0,0,0,9,16a.987.987,0,0,0,.316-.052l3.273-1.091a.977.977,0,0,0,.39-.241l8.728-8.727a1,1,0,0,0,0-1.414L19.525,2.293A1,1,0,0,0,18.111,2.293ZM11.732,13.035l-1.151.384.384-1.151L16.637,6.6l.767.767Zm7.854-7.853-.768.767-.767-.767.767-.768ZM3,5h8a1,1,0,0,1,0,2H4V20H17V13a1,1,0,0,1,2,0v8a1,1,0,0,1-1,1H3a1,1,0,0,1-1-1V6A1,1,0,0,1,3,5Z"
                      />
                    </svg>
                  </button>
                  <button
                    class="cursor-pointer pr-1"
                    @click.stop="removePipelineStage(pipelineStage)"
                  >
                    <svg
                      class="size-5 group"
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
                      class="size-5 group"
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
          <div
            v-else
            class="w-full text-center border rounded-md py-3 border-blue-500 text-blue-600"
          >
            Herhangi bir değerlendirme eklenmemiş
          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Commission or SingleUser end -->
</template>
