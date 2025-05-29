<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import applicationService from "@/services/application.service";
import { FileSearch } from "lucide-vue-next";
import { useRouter } from "vue-router";
import { useVisiblePages } from "@/services/pagination.service";
import { PendingCommissionEvaluationgetModel } from "../models/evaluation/pending-commission-evaluation.model";
import { getEvaluationStatusByValue } from "@/models/constants/evaluation-status";
import { useDropdown } from "../composables/useDropdown";

const pendingEvaluations = ref<PendingCommissionEvaluationgetModel[]>([]);
const page = ref(1);
const pageSize = ref(20);
const totalCount = ref(0);

const request = ref<{ JobPostinId?: string; EvaluationStageId?: string }>({});

const jobPostingDropdown = useDropdown();
const evaluationStageDropdown = useDropdown();

const totalPages = computed(() => {
  return Math.ceil(totalCount.value / pageSize.value);
});

const evaluationStages = ref<{ id: string; name: string }[]>([]);
const jobPostings = ref<{ id: string; name: string }[]>([]);

const visiblePages = useVisiblePages(totalPages, page);

const router = useRouter();

onMounted(() => {
  jobPostings.value.push({ id: "", name: "Tümü" });
  evaluationStages.value.push({ id: "", name: "Tümü" });
  getPendingEvaluations();
});

const getPendingEvaluations = async () => {
  const res = await applicationService.getPendingCommissionEvaluations(request.value);
  if (res) {
    pendingEvaluations.value = res;
    res.forEach((evaluation) => {
      if (!jobPostings.value.find((p) => p.id == evaluation.jobPostingId)) {
        jobPostings.value.push({ id: evaluation.jobPostingId, name: evaluation.jobPosting });
      }
      if (!evaluationStages.value.find((p) => p.id == evaluation.stageId)) {
        evaluationStages.value.push({ id: evaluation.stageId, name: evaluation.stageName });
      }
    });
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
const goToApplicationDetail = (id: string) => {
  router.push({
    name: "pending-evaluations-evaluate",
    params: { id },
  });
};

const changePage = (pageNumber: number) => {
  if (!(pageNumber <= 0 || pageNumber > totalPages.value)) {
    page.value = pageNumber;
    getPendingEvaluations();
  }
};

const selectJobPostingFilter = async (option: { id: string; name: string }) => {
  jobPostingDropdown.selectOption(option.name);
  request.value.JobPostinId = option.id === "" ? undefined : option.id;
  getPendingEvaluations();
};
const selectEvaluationStageFilter = async (option: { id: string; name: string }) => {
  evaluationStageDropdown.selectOption(option.name);
  request.value.EvaluationStageId = option.id === "" ? undefined : option.id;
  getPendingEvaluations();
};
</script>

<template>
  <main class="w-full h-full px-10 pt-20 pb-10 overflow-x-auto">
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
            >Değerlendirme Bekleyen Başvurular</span
          >
        </div>
        <!-- Başvuru Filtreleri -->
        <div
          class="bg-gray-50 dark:bg-gray-800/40 rounded-lg p-4 mb-6 border border-gray-200 dark:border-gray-700 my-5 mx-3"
        >
          <div class="flex flex-col md:flex-row md:items-center md:justify-start gap-4">
            <!-- Arama -->
            <div class="w-full md:w-1/3">
              <label
                for="search"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Arama</label
              >
              <div class="relative">
                <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                  <svg
                    class="h-5 w-5 text-gray-400"
                    xmlns="http://www.w3.org/2000/svg"
                    viewBox="0 0 20 20"
                    fill="currentColor"
                  >
                    <path
                      fill-rule="evenodd"
                      d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                      clip-rule="evenodd"
                    />
                  </svg>
                </div>
                <input
                  id="search"
                  type="text"
                  placeholder="İlan başlığı ara"
                  class="pl-10 w-full rounded-md border-gray-200 dark:border-gray-700 outline-none dark:focus:border-indigo-600 focus:border-indigo-600 border py-1.5 text-gray-800 dark:text-gray-100"
                />
              </div>
            </div>

            <!-- Durum Filtresi -->
            <div class="w-1/3">
              <label
                for="status"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >İlan</label
              >
              <input
                type="text"
                name="formTeplate"
                id="formTeplate"
                :ref="jobPostingDropdown.inputRef"
                v-model="jobPostingDropdown.selectedLabel.value"
                @focus="jobPostingDropdown.handleFocus"
                @blur="jobPostingDropdown.handleBlur"
                readonly
                placeholder="Durum seçin..."
                autocomplete="off"
                class="w-full border outline-none rounded-md py-2 px-2 text-gray-700 dark:text-gray-200 dark:border-gray-700 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600 cursor-pointer"
              />
              <div
                v-if="jobPostingDropdown.isOpen.value"
                class="absolute w-fit text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
              >
                <div
                  v-for="option in jobPostings"
                  :key="option.name"
                  @mousedown.prevent="selectJobPostingFilter(option)"
                  class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
                >
                  {{ option.name }}
                </div>
              </div>
            </div>
            <div class="w-full md:w-1/3">
              <label
                for="status"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >Değerlendirme Adımı</label
              >
              <input
                type="text"
                name="formTeplate"
                id="formTeplate"
                :ref="evaluationStageDropdown.inputRef"
                v-model="evaluationStageDropdown.selectedLabel.value"
                @focus="evaluationStageDropdown.handleFocus"
                @blur="evaluationStageDropdown.handleBlur"
                readonly
                placeholder="Durum seçin..."
                autocomplete="off"
                class="w-full border outline-none rounded-md py-2 px-2 text-gray-700 dark:text-gray-200 dark:border-gray-700 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600 cursor-pointer"
              />
              <div
                v-if="evaluationStageDropdown.isOpen.value"
                class="absolute w-fit text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
              >
                <div
                  v-for="option in evaluationStages"
                  :key="option.name"
                  @mousedown.prevent="selectEvaluationStageFilter(option)"
                  class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
                >
                  {{ option.name }}
                </div>
              </div>
            </div>
          </div>
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
                @change="getPendingEvaluations()"
                class="text-sm dark:text-gray-300 text-gray-700 dark:bg-gray-800 px-3 py-1 outline-none focus:border-indigo-600 rounded-md border dark:border-gray-700 border-gray-300"
              >
                <option :value="10">10</option>
                <option :value="20" selected>20</option>
                <option :value="30">30</option>
              </select>
              <span class="ml-2 dark:text-gray-400 text-gray-600"> kayıt göster</span>
            </div>
            <table class="w-full text-sm overflow-x-auto">
              <thead class="">
                <tr class="border-b border-t dark:border-gray-700/30 border-gray-200">
                  <td
                    class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm"
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
                      <span>Adı - Soyadı</span>
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
                      <span>TCKN</span>
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
                      <span>İlan</span>
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
                      <span>Başvuru Tarihi</span>
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
                      <span>Değerlendirilme Durumu</span>
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
                      <span>Değerlendirme Adımı</span>
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
                      <span>Değerlendirme Formu</span>
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
                      <span>Değerlendirme Tarihi</span>
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
                  v-for="(application, index) in pendingEvaluations"
                  :key="application.id"
                  class="border-b dark:border-gray-700/30 border-gray-200"
                >
                  <td class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200">
                    {{ index + 1 }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    <span class="cursor-pointer">{{ application.userFullName }}</span>
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ application.tckn ?? "-" }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ application.jobPosting ?? "-" }}
                  </td>
                  <td class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200">
                    {{ formatDateTime(application.appliedDate) }}
                  </td>

                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    <span
                      class="text-sm px-2 py-1 rounded-md text-white"
                      :class="
                        application.status == 0
                          ? 'bg-yellow-600'
                          : application.status == 1 || application.status == 2
                          ? 'bg-blue-600'
                          : application.status == 3
                          ? 'bg-green-600'
                          : application.status == 4
                          ? 'bg-red-500'
                          : 'bg-indigo-500'
                      "
                      >{{ getEvaluationStatusByValue(application.status)?.label }}</span
                    >
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ application.stageName ?? "-" }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ application.evaluationFormName ?? "-" }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{
                      application.reviewDate
                        ? formatDateTime(application.reviewDate)
                        : "Bu başvuruyu değerlendirmediniz"
                    }}
                  </td>

                  <td class="py-3 px-2">
                    <button
                      class="cursor-pointer pr-1 group"
                      title="Düzenle"
                      @click="goToApplicationDetail(application.id)"
                    >
                      <FileSearch
                        class="size-5 stroke-gray-600 dark:stroke-gray-400 dark:group-hover:stroke-sky-600 group-hover:stroke-sky-600"
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
<style scoped></style>
