<script setup lang="ts">
import { ref, onMounted } from "vue";
import applicationService from "../services/application.service";
import { ApplicationByUserModel } from "../models/application-by-user.model";
import { CircleX } from "lucide-vue-next";
import { FileSearch } from "lucide-vue-next";
import ConfirmModal from "@/components/ConfirmModal.vue";

const applications = ref<ApplicationByUserModel[]>([]);
const page = ref(1);
const pageSize = ref(20);
const totalCount = ref(0);

const confirmModal = ref();

onMounted(() => {
  getApplications();
});

const getApplications = async () => {
  const res = await applicationService.getApplicationsByUser(page.value, pageSize.value);
  if (res) {
    applications.value = res.items;
    totalCount.value = res.totalCount;
    pageSize.value = res.pageSize;
    page.value = res.page;
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

const withdrawApplication = async (id: string) => {
  const result = await confirmModal.value.open();
  if (result) {
    await applicationService.withdrawnApplication(id);
    getApplications();
  } else {
    console.log("Kullanıcı iptal etti.");
  }
};
</script>
<template>
  <div class="px-5 py-5">
    <ConfirmModal
      ref="confirmModal"
      title="Başvurunuzu Geri Çekmek İstediğinize Emin Misiniz?"
      description="Bu işlem geri alınamaz."
    />
    <div class="my-2">
      <span class="text-gray-800 dark:text-gray-200 text-lg font-semibold">Başvurularım</span>
    </div>
    <div
      class="bg-transparent dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200"
    >
      <div class="ml-5 py-4">
        <select
          name="pageSize"
          id="pageSize"
          v-model.number="pageSize"
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
                <span>İlan Başlığı</span>
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
                <span>Birim</span>
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
                <span>Değerlendirilme Tarihi</span>
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
                <span>Değerlendirme Açıklaması</span>
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
            v-for="(application, index) in applications"
            :key="application.id"
            class="border-b dark:border-gray-700/30 border-gray-200"
          >
            <td class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200">
              {{ index + 1 }}
            </td>
            <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
              <span class="cursor-pointer">{{ application.title }}</span>
            </td>
            <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
              {{ application.unit ?? "-" }}
            </td>
            <td class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200">
              {{ formatDateTime(application.appliedDate) }}
            </td>
            <td class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200">
              {{ application.reviewDate ? formatDateTime(application.reviewDate) : "-" }}
            </td>
            <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
              {{ application.status }}
            </td>
            <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
              {{ application.reviewDescription ?? "-" }}
            </td>

            <td class="py-3 px-2">
              <button class="cursor-pointer mr-2 group" title="Düzenle">
                <FileSearch
                  class="size-6 stroke-gray-600 dark:stroke-gray-400 dark:group-hover:stroke-sky-600 group-hover:stroke-sky-600"
                />
              </button>
              <button class="cursor-pointer group">
                <CircleX
                  class="size-6 stroke-gray-600 dark:stroke-gray-400 dark:group-hover:stroke-red-600 group-hover:stroke-red-600"
                  @click="withdrawApplication(application.id)"
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
            {{ totalCount - page * pageSize <= 0 ? totalCount : totalCount - page * pageSize }}
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
</template>
