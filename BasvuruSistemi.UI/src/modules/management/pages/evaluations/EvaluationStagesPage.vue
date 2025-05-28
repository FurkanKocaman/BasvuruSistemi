<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { FileSearch, KeyRound } from "lucide-vue-next";
import { useVisiblePages } from "@/services/pagination.service";
import { useRouter } from "vue-router";
import evaluationService from "../../services/evaluation.service";
import { EvaluationStageDto } from "../../models/evaluation/evaluation-stage.model";

const evaluationStages = ref<EvaluationStageDto[]>([]);
const page = ref(1);
const pageSize = ref(20);
const totalCount = ref(0);

const router = useRouter();

const totalPages = computed(() => {
  return Math.ceil(totalCount.value / pageSize.value);
});

const visiblePages = useVisiblePages(totalPages, page);

onMounted(() => {
  getEvaluationStages();
});

const getEvaluationStages = async () => {
  const res = await evaluationService.listEvaluationStages();
  if (res) {
    evaluationStages.value = res;
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

const changePage = (pageNumber: number) => {
  if (!(pageNumber <= 0 || pageNumber > totalPages.value)) {
    page.value = pageNumber;
    getEvaluationStages();
  }
};

const updateStage = (id: string) => {
  router.push({ name: "evaluations-stage-update", params: { id: id } });
};
</script>
<template>
  <main class="w-full h-full px-10 pt-20 pb-10">
    <div class="w-full flex">
      <!-- Filtreler -->
      <div></div>
      <!-- İlan Listesi -->
      <div
        class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
      >
        <div
          class="border-b px-5 py-3 dark:border-gray-800 border-gray-200 flex justify-between items-center"
        >
          <span class="text-xl font-base dark:text-gray-50 text-gray-700"
            >Değerlendirme Adımları</span
          >
          <router-link
            to="/management/evaluation-stages/create"
            class="border rounded-md dark:border-gray-700 border-gray-200 text-sm dark:text-gray-200 text-gray-700 dark:hover:bg-gray-700/20 hover:bg-gray-200/20 cursor-pointer px-3 py-1.5"
          >
            Değerlendirme Adımı Oluştur
          </router-link>
        </div>
        <div class="px-5 py-5">
          <div
            class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200"
          >
            <div class="ml-5 py-4">
              <select
                name="pageSize"
                id="pageSize"
                v-model.number="pageSize"
                @change="getEvaluationStages()"
                class="text-sm dark:text-gray-300 text-gray-700 dark:bg-gray-800 px-3 py-1 outline-none focus:border-indigo-600 rounded-md border dark:border-gray-700 border-gray-300"
              >
                <option :value="10">10</option>
                <option :value="20" selected>20</option>
                <option :value="30">30</option>
              </select>
              <span class="ml-2 dark:text-gray-400 text-gray-600"> kayıt göster</span>
            </div>
            <table class="w-full text-sm">
              <thead class="">
                <tr class="border-b border-t dark:border-gray-700/30 border-gray-200">
                  <td
                    class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm w-10"
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
                    class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
                  >
                    <div class="flex items-center justify-between">
                      <span>Ad</span>
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
                    class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
                  >
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
                  <td
                    class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
                  >
                    <div class="flex items-center justify-between">
                      <span>Formlar</span>
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
                    class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
                  >
                    <div class="flex items-center justify-between">
                      <span>Oluşturulma</span>
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

                  <td class="py-3 px-2">İşlemler</td>
                </tr>
              </thead>
              <tbody class="">
                <tr
                  v-for="(evaluationStage, index) in evaluationStages"
                  :key="evaluationStage.id"
                  class="border-b dark:border-gray-700/30 border-gray-200"
                >
                  <td class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200 w-10">
                    {{ index + 1 }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    <div class="flex items-center">
                      <span class="cursor-pointer" @click="updateStage(evaluationStage.id)">{{
                        evaluationStage.name
                      }}</span>
                    </div>
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ evaluationStage.description ?? "-" }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ evaluationStage.evaluationForms.length ?? "-" }}
                  </td>

                  <td class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200">
                    {{
                      evaluationStage.createdAt
                        ? formatDateTime(evaluationStage.createdAt.toString())
                        : "-"
                    }}
                  </td>

                  <td class="py-3 px-2">
                    <button class="cursor-pointer pr-1 group" title="Önizleme">
                      <FileSearch
                        @click="updateStage(evaluationStage.id)"
                        class="size-5 stroke-gray-600 dark:stroke-gray-400 dark:group-hover:stroke-sky-600 group-hover:stroke-sky-600"
                      />
                    </button>
                    <button class="cursor-pointer pr-1 group" title="Rolleri Düzenle">
                      <KeyRound
                        class="size-5 stroke-gray-600 dark:stroke-gray-400 dark:group-hover:stroke-orange-600 group-hover:stroke-orange-600"
                      />
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
            <div class="flex flex-row justify-between py-4 px-5">
              <div>
                <span class="text-sm"
                  >Toplam {{ totalCount }} kayıttan {{ page }} ile
                  {{
                    totalCount - page * pageSize <= 0 ? totalCount : totalCount - page * pageSize
                  }}
                  arası gösteriliyor</span
                >
              </div>
              <div class="flex">
                <button
                  class="border rounded-md p-2 text-sm dark:border-gray-700 border-gray-200 dark:text-gray-300 cursor-pointer select-none"
                  @click.stop="changePage(page - 1)"
                >
                  Önceki
                </button>
                <div class="mx-3">
                  <button
                    v-for="pageNumber in visiblePages"
                    :key="pageNumber.label"
                    class="rounded-md p-2 text-sm cursor-pointer mx-1 w-8 select-none"
                    @click.stop="() => pageNumber.type === 'page' && changePage(pageNumber.page)"
                    :class="
                      pageNumber.label == page
                        ? 'text-blue-700 dark:text-blue-500 hover:bg-blue-600/10 dark:hover:text-blue-500 hover:text-blue-700 bg-blue-600/10'
                        : 'text-gray-800 dark:text-gray-300 hover:bg-blue-600/10 dark:hover:text-blue-500 hover:text-blue-700'
                    "
                  >
                    {{ pageNumber.label }}
                  </button>
                </div>
                <button
                  class="border rounded-md p-2 text-sm dark:border-gray-700 border-gray-200 dark:text-gray-300 cursor-pointer select-none"
                  @click.stop="changePage(page + 1)"
                >
                  Sonraki
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>
