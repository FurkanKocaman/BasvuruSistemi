<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import { FileSearch, KeyRound } from "lucide-vue-next";
import { useRoute, useRouter } from "vue-router";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { CommissionCreateModel } from "../models/commission-create.model";
import commissionService from "../services/commission.service";
import { CommissionMemberGetModel } from "../models/commission-member-get.model";
import AddMemberToCommissionModal from "../components/modals/AddMemberToCommissionModal.vue";
import { AddMemberToCommissionModel } from "../models/add-member-to-commission.model";

const request = reactive<CommissionCreateModel>({
  name: "",
});

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
    console.log(result);
    if (id) getCommission(id);
  } else {
    console.log(result);
  }
};

const updateMember = async (member: AddMemberToCommissionModel) => {
  const result = await addMemberModal.value.open(request.id, member);
  if (result) {
    console.log(result);
  } else {
    console.log(result);
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
                                email: member.email,
                                commissionId: request.id!,
                                role: member.roleInCommission,
                              })
                            "
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
