<script setup lang="ts">
import { getComponentByFieldType } from "@/modules/home/services/getComponentByField.service";
import { onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import applicationEvaluationService from "../services/application-evaluation.service";
import { ApplicationEvaluationProcessModel } from "../models/evaluation/application-evaluation-process.model";
import { getEvaluationStatusByValue } from "@/models/constants/evaluation-status";

const route = useRoute();

const id = route.params.id as string | undefined;
const evaluationProcess = ref<ApplicationEvaluationProcessModel[]>([]);

onMounted(() => {
  getEvaluationProcess();
});

const getEvaluationProcess = async () => {
  if (id) {
    const res = await applicationEvaluationService.getApplicationEvaluationsProcess(id);
    if (res) {
      evaluationProcess.value = res;
      console.log(res);
    }
  }
};
</script>

<template>
  <div
    v-if="id"
    class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
  >
    <span class="text-lg dark:text-gray-100 font-semibold">Değerlendirme Süreci</span>

    <div
      v-for="evaluation in evaluationProcess"
      :key="evaluation.evaluationFormId"
      class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start mt-5"
    >
      <div class="flex flex-row mt-5">
        <div class="flex flex-col mr-20">
          <div class="flex flex-col mb-3">
            <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5"
              >Değerlendirme Adımı</label
            >
            <span class="dark:text-gray-200 text-gray-900">{{ evaluation.stageName ?? "-" }} </span>
          </div>
          <div class="flex flex-col mb-3">
            <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Komisyon</label>
            <span class="dark:text-gray-200 text-gray-800">{{
              evaluation.commissionName ?? "-"
            }}</span>
          </div>
        </div>
        <div class="flex flex-col mr-20">
          <div class="flex flex-col mb-3">
            <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5"
              >Değerlendirme Formu</label
            >
            <span class="dark:text-gray-200 text-gray-800">
              {{ evaluation.evaluationFormName }}
            </span>
          </div>
          <div class="flex flex-col mb-3">
            <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5"
              >Toplam Başvuru Sayısı</label
            >
            <span class="dark:text-gray-200 text-gray-800">- </span>
          </div>
        </div>
      </div>
      <!-- EvaluationTable -->
      <div class="mt-5 w-full">
        <table class="w-full text-sm table-auto">
          <thead class="">
            <tr class="border-b dark:border-gray-700/30 border-gray-200">
              <td
                class="w-12 py-3 pr-2 pl-4 border-r dark:border-gray-800 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
              >
                <div class="flex items-center justify-between">
                  <span>Sıra</span>
                  <svg
                    class="size-6 dark:fill-gray-500"
                    viewBox="0 0 1024 1024"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M620.6 562.3l36.2 36.2L512 743.3 367.2 598.5l36.2-36.2L512 670.9l108.6-108.6zM512 353.1l108.6 108.6 36.2-36.2L512 280.7 367.2 425.5l36.2 36.2L512 353.1z"
                    />
                  </svg>
                </div>
              </td>
              <td
                class="py-3 px-2 border-r dark:border-gray-800 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
              >
                <div class="flex items-center justify-between">
                  <span>Değerlendiren</span>
                  <svg
                    class="size-6 dark:fill-gray-500"
                    viewBox="0 0 1024 1024"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M620.6 562.3l36.2 36.2L512 743.3 367.2 598.5l36.2-36.2L512 670.9l108.6-108.6zM512 353.1l108.6 108.6 36.2-36.2L512 280.7 367.2 425.5l36.2 36.2L512 353.1z"
                    />
                  </svg>
                </div>
              </td>
              <td
                class="py-3 px-2 border-r dark:border-gray-800 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
              >
                <div class="flex items-center justify-between">
                  <span>Durum</span>
                  <svg
                    class="size-6 dark:fill-gray-500"
                    viewBox="0 0 1024 1024"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M620.6 562.3l36.2 36.2L512 743.3 367.2 598.5l36.2-36.2L512 670.9l108.6-108.6zM512 353.1l108.6 108.6 36.2-36.2L512 280.7 367.2 425.5l36.2 36.2L512 353.1z"
                    />
                  </svg>
                </div>
              </td>
              <td class="py-3 px-2 cursor-pointer select-none dark:text-gray-400 text-sm">
                <div class="flex items-center justify-between">
                  <span>Açıklama</span>
                  <svg
                    class="size-6 dark:fill-gray-500"
                    viewBox="0 0 1024 1024"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      d="M620.6 562.3l36.2 36.2L512 743.3 367.2 598.5l36.2-36.2L512 670.9l108.6-108.6zM512 353.1l108.6 108.6 36.2-36.2L512 280.7 367.2 425.5l36.2 36.2L512 353.1z"
                    />
                  </svg>
                </div>
              </td>
            </tr>
          </thead>
          <tbody class="dark:text-gray-200 text-gray-800">
            <tr
              v-for="(field, index) in evaluation.commissionEvaluationSummaries"
              :key="field.userId"
              class="border-b dark:border-gray-700/30 border-gray-200"
            >
              <td class="py-3 pr-2 pl-4 border-r dark:border-gray-800 border-gray-200">
                {{ index }}
              </td>
              <td class="py-3 px-2 border-r dark:border-gray-800 border-gray-200">
                {{ field.userName }}
              </td>
              <td class="py-3 px-2 border-r dark:border-gray-800 border-gray-200">
                <span
                  class="text-sm px-2 py-1 rounded-md text-white"
                  :class="
                    field.status == 0
                      ? 'bg-yellow-600'
                      : field.status == 1 || field.status == 2
                      ? 'bg-blue-600'
                      : field.status == 3
                      ? 'bg-green-600'
                      : field.status == 4
                      ? 'bg-red-500'
                      : 'bg-indigo-500'
                  "
                  >{{ getEvaluationStatusByValue(field.status)?.label }}</span
                >
              </td>
              <td class="py-3 px-2 text-sm">
                <span>
                  {{ field.statusDescription }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
