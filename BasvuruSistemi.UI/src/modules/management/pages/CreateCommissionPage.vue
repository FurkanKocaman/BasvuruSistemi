<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import { FileSearch } from "lucide-vue-next";
import { useRoute, useRouter } from "vue-router";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { CommissionCreateModel } from "../models/commission-create.model";
import commissionService from "../services/commission.service";
import { CommissionMemberGetModel } from "../models/commission-member-get.model";
import AddMemberToCommissionModal from "../components/modals/AddMemberToCommissionModal.vue";
import { AddMemberToCommissionModel } from "../models/add-member-to-commission.model";
import ConfirmModal from "@/components/ConfirmModal.vue";
import commissionMemberService from "../services/commission-member.service";

const request = reactive<CommissionCreateModel>({
  name: "",
});

const confirmModal = ref();

const commissionMembers = ref<CommissionMemberGetModel[]>([]);

const toastStore = useToastStore();

const route = useRoute();
const router = useRouter();

const addMemberModal = ref();

const id = route.params.id as string | undefined;

const isUpdate = ref<boolean>(false);

onMounted(async () => {
  if (id) {
    isUpdate.value = true;
    getCommission(id);
  }
});

const getCommission = async (id: string) => {
  const res = await commissionService.getCommissionById(id);
  if (res) {
    request.id = res.id;
    request.name = res.name;
    request.description = res.description;
    commissionMembers.value = res.commissionMembers;
  }
};

const handleSubmit = async () => {
  if (request.name.trim() !== "") {
    if (!isUpdate.value) {
      const res = await commissionService.createCommission(request);
      if (res) {
        router.push({ name: "commissions-list" });
      }
    } else {
      if (request.id) {
        console.log(request);
        const res = await commissionService.updateCommission(request);
        if (res) {
          router.push({ name: "commissions-list" });
        }
      }
    }
  } else {
    toastStore.addToast({
      message: "Komisyon adı boş olamaz!",
      type: "error",
      duration: 3000,
    });
  }
};

const addMember = async () => {
  const result = await addMemberModal.value.open(request.id);
  if (result) {
    if (id) getCommission(id);
  }
};

const updateMember = async (member: AddMemberToCommissionModel) => {
  const result = await addMemberModal.value.open(request.id, member);
  if (result) {
    if (id) getCommission(id);
  }
};

const removeMember = async (userId: string) => {
  if (request.id) {
    const result = await confirmModal.value.open();

    if (result) {
      const res = await commissionMemberService.removeMemberFromCommission(userId, request.id);
      if (res) if (id) getCommission(id);
    }
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
  <div class="w-full px-50 pt-20 flex pb-20">
    <ConfirmModal
      ref="confirmModal"
      title="Komisyondan üye çıkarmak istediğinize emin misiniz?"
      description="Bu işlem geri alınamaz."
    />
    <AddMemberToCommissionModal ref="addMemberModal" />
    <div
      class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
    >
      <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200">
        <span class="text-xl font-base dark:text-gray-50 text-gray-700">Komisyon Oluştur</span>
      </div>
      <div class="px-10 py-5">
        <div
          class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200 flex flex-col px-20 pt-5"
        >
          <div class="flex flex-col w-full">
            <label for="templateName" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
              >Komisyon Adı</label
            >
            <input
              type="text"
              name="templateName"
              id="templateName"
              v-model="request.name"
              autocomplete="off"
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            />
          </div>
          <div class="flex flex-col w-full my-3">
            <label
              for="templateDescription"
              class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
              >Komisyon Açıklama</label
            >
            <textarea
              type="text"
              name="templateDescription"
              id="templateDescription"
              v-model="request.description"
              autocomplete="off"
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            ></textarea>
          </div>

          <div class="flex flex-col mt-5">
            <!-- Form Şablon -->
            <div class="flex justify-between">
              <label
                for="editor"
                class="w-full text-md my-1 dark:text-gray-300 text-gray-800 min-w-[10rem]"
                >Komisyon Üyeleri</label
              >
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:text-gray-400 text-gray-700 hover:text-red-600 dark:hover:text-red-400 min-w-[10rem] hover:border-red-600 mr-3 dark:hover:border-red-600"
              >
                Seçili Olanları Sil
              </button>
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
                @click.stop="addMember()"
              >
                Ekle
              </button>
            </div>
            <div class="flex flex-col border my-5 rounded-lg dark:border-gray-700 border-gray-200">
              <div class="px-3">
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
                          <span>Ad - Soyad</span>
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
                          <span>E-posta</span>
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
                          <span>Rol</span>
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
                          <span>Katılma Tarihi</span>
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
                      v-for="(member, index) in commissionMembers"
                      :key="member.userId"
                      class="border-b dark:border-gray-700/30 border-gray-200"
                    >
                      <td
                        class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200 w-10"
                      >
                        {{ index + 1 }}
                      </td>
                      <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                        <div class="flex items-center">
                          <span class="cursor-pointer">{{ member.fullName }}</span>
                        </div>
                      </td>
                      <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                        {{ member.email ?? "-" }}
                      </td>
                      <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                        {{ member.roleInCommission ?? "-" }}
                      </td>

                      <td
                        class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200"
                      >
                        {{ member.createdAt ? formatDateTime(member.createdAt.toString()) : "-" }}
                      </td>

                      <td class="py-3 px-2">
                        <button class="cursor-pointer pr-1 group" title="Önizleme">
                          <FileSearch
                            @click.stop="
                              updateMember({
                                id: member.id,
                                email: member.email,
                                commissionId: request.id!,
                                role: member.roleInCommission,
                                isManager: member.isManager,
                              })
                            "
                            class="size-5 stroke-gray-600 dark:stroke-gray-400 dark:group-hover:stroke-sky-600 group-hover:stroke-sky-600"
                          />
                        </button>
                        <button
                          class="cursor-pointer pr-1"
                          @click.stop="removeMember(member.userId)"
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
              </div>
            </div>
          </div>

          <div class="my-4 flex justify-end">
            <button
              class="border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
              @click="handleSubmit()"
            >
              Kaydet
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped></style>
