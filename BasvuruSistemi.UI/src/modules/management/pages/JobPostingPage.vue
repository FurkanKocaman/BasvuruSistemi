<script setup lang="ts">
import { onMounted, ref, Ref } from "vue";
import jobPostingService from "../services/job-posting.service";
import { JobPostingsByTenantResponse } from "../models/job-posting-by-tenant.model";
import { useRouter } from "vue-router";

const jobPostings: Ref<JobPostingsByTenantResponse[]> = ref([]);
const page = ref(1);
const pageSize = ref(10);
const totalCount = ref(0);

const router = useRouter();

onMounted(async () => {
  getJobPostings();
});

const getJobPostings = async () => {
  const res = await jobPostingService.getJobPostings();
  if (res) {
    jobPostings.value = res.items;
    totalCount.value = res.totalCount;
    page.value = res.page;
    pageSize.value = res.pageSize;
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

const goToJobPostingEdit = (id: string) => {
  router.push({ name: "job-posting-update", params: { id } });
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
        <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200">
          <span class="text-xl font-base dark:text-gray-50 text-gray-700">İlan Listesi</span>
        </div>
        <div class="px-5 py-5">
          <div
            class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200"
          >
            <div class="ml-5 py-4">
              <select
                name="pageSize"
                id="pageSize"
                class="text-sm dark:text-gray-300 text-gray-700 dark:bg-gray-800 px-3 py-1 outline-none focus:border-indigo-600 rounded-md border dark:border-gray-700 border-gray-300"
              >
                <option value="10">10</option>
                <option value="20">20</option>
                <option value="30">30</option>
              </select>
              <span class="ml-2 dark:text-gray-400 text-gray-600"> kayıt göster</span>
            </div>
            <table class="w-full text-sm">
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
                      <span>İlan Adı</span>
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
                      <span>İlan Tipi</span>
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
                      <span>İlk Başvuru Tarihi</span>
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
                      <span>Son Başvuru Tarihi</span>
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
                      <span>Kontenjan Var Mı</span>
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
                      <span>Başvuru sayısı</span>
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
                  <td class="py-3 px-2">İşlemler</td>
                </tr>
              </thead>
              <tbody class="">
                <tr
                  v-for="(jobPosting, index) in jobPostings"
                  :key="jobPosting.id"
                  class="border-b dark:border-gray-700/30 border-gray-200"
                >
                  <td class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200">
                    {{ index + 1 }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    <span class="cursor-pointer">{{ jobPosting.title }}</span>
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">Type</td>
                  <td class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200">
                    {{ formatDateTime(jobPosting.validFrom!) }}
                  </td>
                  <td class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200">
                    {{ formatDateTime(jobPosting.validTo!) }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    <span
                      class="py-0.5 px-1.5 rounded-lg"
                      :class="
                        jobPosting.vacancyCount
                          ? 'dark:bg-blue-500/60 bg-blue-500/20 dark:text-gray-200 text-gray-600'
                          : 'dark:bg-red-500/60 bg-red-500/20 dark:text-red-200 text-red-600'
                      "
                      >{{ jobPosting.vacancyCount ?? "yok" }}</span
                    >
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ jobPosting.totalApplicationsCount }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    <span class="bg-blue-600 py-0.5 px-1.5 rounded-lg text-gray-100">Status</span>
                  </td>
                  <td class="py-3 px-2">
                    <button
                      class="cursor-pointer pr-1 group"
                      @click="goToJobPostingEdit(jobPosting.id)"
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
                    <button class="cursor-pointer pr-1">
                      <svg
                        class="size-5 group"
                        viewBox="0 0 24 24"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          d="M20.5001 6H3.5"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                          stroke-width="1.5"
                          stroke-linecap="round"
                        />
                        <path
                          d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                          stroke-width="1.5"
                          stroke-linecap="round"
                        />
                        <path
                          d="M9.5 11L10 16"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                          stroke-width="1.5"
                          stroke-linecap="round"
                        />
                        <path
                          d="M14.5 11L14 16"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                          stroke-width="1.5"
                          stroke-linecap="round"
                        />
                        <path
                          d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65651 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
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
                        <path
                          d="M3 10C3 6.22876 3 4.34315 4.17157 3.17157C5.34315 2 7.22876 2 11 2H13C16.7712 2 18.6569 2 19.8284 3.17157C21 4.34315 21 6.22876 21 10V14C21 17.7712 21 19.6569 19.8284 20.8284C18.6569 22 16.7712 22 13 22H11C7.22876 22 5.34315 22 4.17157 20.8284C3 19.6569 3 17.7712 3 14V10Z"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
                          stroke-width="1.5"
                        />
                        <path
                          d="M8 12H16"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
                          stroke-width="1.5"
                          stroke-linecap="round"
                        />
                        <path
                          d="M8 8H16"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
                          stroke-width="1.5"
                          stroke-linecap="round"
                        />
                        <path
                          d="M8 16H13"
                          class="dark:stroke-gray-400 stroke-gray-600 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
                          stroke-width="1.5"
                          stroke-linecap="round"
                        />
                      </svg>
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
                  class="border rounded-md p-2 text-sm dark:border-gray-700 border-gray-200 dark:text-gray-300 cursor-pointer"
                >
                  Önceki
                </button>
                <div class="mx-3">
                  <button
                    class="rounded-md p-2 text-sm dark:text-blue-500 bg-blue-600/10 cursor-pointer mx-1 w-8 hover:bg-blue-600/10 dark:hover:text-blue-500 hover:text-blue-700"
                  >
                    1
                  </button>
                  <button
                    class="rounded-md p-2 text-sm dark:text-gray-300 cursor-pointer mx-1 w-8 hover:bg-blue-600/10 dark:hover:text-blue-500 hover:text-blue-700"
                  >
                    2
                  </button>
                </div>
                <button
                  class="border rounded-md p-2 text-sm dark:border-gray-700 border-gray-200 dark:text-gray-300 cursor-pointer"
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
