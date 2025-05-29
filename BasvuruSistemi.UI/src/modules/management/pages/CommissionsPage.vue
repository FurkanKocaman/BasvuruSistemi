<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { FileSearch } from "lucide-vue-next";
import { useVisiblePages } from "@/services/pagination.service";
import commissionService from "../services/commission.service";
import { CommissionGetModel } from "../models/commission-get.model";
import { useRouter } from "vue-router";
import ConfirmModal from "@/components/ConfirmModal.vue";

const commissions = ref<CommissionGetModel[]>([]);
const page = ref(1);
const pageSize = ref(20);
const totalCount = ref(0);

const confirmModal = ref();

const router = useRouter();

const totalPages = computed(() => {
  return Math.ceil(totalCount.value / pageSize.value);
});

const visiblePages = useVisiblePages(totalPages, page);

onMounted(() => {
  getCommissions();
});

const getCommissions = async () => {
  const res = await commissionService.getCommissions();
  commissions.value = res;
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
    getCommissions();
  }
};

const updateCommission = (id: string) => {
  router.push({ name: "commission-update", params: { id: id } });
};

const deleteCommission = async (id: string) => {
  const result = await confirmModal.value.open();

  if (result) {
    const res = await commissionService.deleteCommission(id);
    if (res) {
      commissions.value = commissions.value.filter((p) => p.id != id);
    }
  }
};
</script>
<template>
  <main class="w-full h-full px-10 pt-20 pb-10">
    <ConfirmModal
      ref="confirmModal"
      title="Komisyonu silmek istediğinize emin misiniz?"
      description="Bu işlem geri alınamaz."
    />
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
          <span class="text-xl font-base dark:text-gray-50 text-gray-700">Komisyonlar</span>
          <router-link
            to="/management/commissions/create"
            class="border rounded-md dark:border-gray-700 border-gray-200 text-sm dark:text-gray-200 text-gray-700 dark:hover:bg-gray-700/20 hover:bg-gray-200/20 cursor-pointer px-3 py-1.5"
          >
            Komisyon Oluştur
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
                @change="getCommissions()"
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
                      <span>Komisyon Adı</span>
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
                      <span>Üyeler</span>
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
                  v-for="(commission, index) in commissions"
                  :key="commission.id"
                  class="border-b dark:border-gray-700/30 border-gray-200"
                >
                  <td class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200 w-10">
                    {{ index + 1 }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    <div class="flex items-center">
                      <span class="cursor-pointer" @click="updateCommission(commission.id)">{{
                        commission.name
                      }}</span>
                    </div>
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ commission.description ?? "-" }}
                  </td>
                  <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                    {{ commission.commissionMembers.length ?? "-" }}
                  </td>

                  <td class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200">
                    {{
                      commission.createdAt ? formatDateTime(commission.createdAt.toString()) : "-"
                    }}
                  </td>

                  <td class="py-3 px-2">
                    <button class="cursor-pointer pr-1 group" title="Önizleme">
                      <FileSearch
                        @click="updateCommission(commission.id)"
                        class="size-5 stroke-gray-600 dark:stroke-gray-400 dark:group-hover:stroke-sky-600 group-hover:stroke-sky-600"
                      />
                    </button>
                    <button
                      class="cursor-pointer pr-1"
                      @click.stop="deleteCommission(commission.id)"
                    >
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
