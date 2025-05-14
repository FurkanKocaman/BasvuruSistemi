<script setup lang="ts">
import { onMounted, ref } from "vue";
import applicationService from "@/services/application.service";
import { useRoute } from "vue-router";
import { ApplicationGetDetailModel } from "../models/application-get-detail.model";
import { getFieldTypeOptionByValue } from "@/models/constants/field-type";

const route = useRoute();
const id = route.params.id as string | undefined;

const isReviewModalOpen = ref<boolean>(false);
const isApplicationApproved = ref<boolean>(false);

const apiUrl = import.meta.env.VITE_API_PUBLIC_URL;

const application = ref<ApplicationGetDetailModel>({
  id: "",
  jobPostingId: "",
  jobPostingTitle: "",
  userId: "",
  firstName: "",
  lastName: "",
  appliedDate: "",
  status: "",
  fieldValues: [],
  jobPostingCreateDate: "",
  unit: "",
});

onMounted(() => {
  getApplicationDetail();
});

const getApplicationDetail = async () => {
  if (id) {
    const res = await applicationService.getApplication(id);
    if (res.statusCode == 200) {
      application.value = res.data;
      console.log("Res.data", application.value);
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
  <main class="w-full h-full px-30 pt-20 pb-20">
    <!-- Modal start -->
    <div
      class="relative ease-in-out transition-all duration-1000"
      :class="!isReviewModalOpen ? 'hidden' : 'block'"
    >
      <div
        class="fixed z-[50] inset-0 bg-gray-500/75 transition-opacity duration-500 ease-in-out"
        :class="isReviewModalOpen ? 'opacity-100 visible' : 'opacity-0 invisible'"
      ></div>

      <div
        class="fixed inset-0 z-[99] w-screen overflow-y-auto transition-all duration-500 ease-in-out"
        :class="
          isReviewModalOpen ? 'opacity-100 scale-100 visible' : 'opacity-0 scale-95 invisible'
        "
        @click="
          () => {
            isReviewModalOpen = false;
          }
        "
      >
        <div
          class="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0"
        >
          <div
            class="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg"
          >
            <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
              <div class="sm:flex sm:items-start">
                <div
                  class="mx-auto flex size-12 shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:size-10"
                >
                  <svg
                    class="size-6 text-red-600"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke-width="1.5"
                    stroke="currentColor"
                    aria-hidden="true"
                    data-slot="icon"
                  >
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      d="M12 9v3.75m-9.303 3.376c-.866 1.5.217 3.374 1.948 3.374h14.71c1.73 0 2.813-1.874 1.948-3.374L13.949 3.378c-.866-1.5-3.032-1.5-3.898 0L2.697 16.126ZM12 15.75h.007v.008H12v-.008Z"
                    />
                  </svg>
                </div>
                <div class="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
                  <h3 class="text-base font-semibold text-gray-900" id="modal-title">
                    {{
                      isApplicationApproved
                        ? "Başvuruyu Onaylamak İstediğinize Emin Misiniz?"
                        : "Başvuruyu Reddetmek İstediğinize Emin Misiniz?"
                    }}
                  </h3>
                  <div class="mt-2">
                    <p class="text-sm text-gray-500">
                      Başvuruyu sonradan tekrar değerlendirebilirsiniz fakat kullanıcıya
                      değerlendirilme e-postası gönderilecektir.
                    </p>
                  </div>
                </div>
              </div>
            </div>
            <div class="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
              <button
                type="button"
                class="inline-flex w-full justify-center rounded-md px-3 py-2 text-sm font-semibold text-white shadow-xs sm:ml-3 sm:w-auto cursor-pointer"
                :class="
                  isApplicationApproved
                    ? 'bg-green-600 hover:bg-green-500'
                    : 'bg-red-600 hover:bg-red-500'
                "
                @click.stop="
                  () => {
                    isReviewModalOpen = false;
                  }
                "
              >
                {{ isApplicationApproved ? "Onayla" : "Reddet" }}
              </button>
              <button
                type="button"
                class="mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-xs ring-1 ring-gray-300 ring-inset hover:bg-gray-50 sm:mt-0 sm:w-auto cursor-pointer"
                @click.stop="
                  () => {
                    isReviewModalOpen = false;
                  }
                "
              >
                İptal
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Modal end -->
    <div class="w-full h-full flex flex-col dark:bg-gray-800/40 bg-gray-50 rounded-lg px-10 py-10">
      <div
        class="w-full h-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex items-center"
      >
        <img
          class="size-16 rounded-full object-cover"
          src="https://picsum.photos/200/300"
          alt="profile image"
        />
        <div class="flex flex-col">
          <span class="text-base dark:text-gray-100 font-semibold ml-5">{{
            application.firstName + " " + application.lastName
          }}</span>
          <span class="text-sm dark:text-gray-400 font-normal ml-5">{{ application.email }}</span>
        </div>
      </div>
      <div
        class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
      >
        <span class="text-lg dark:text-gray-100 font-semibold">Kişisel Bilgiler</span>
        <div class="flex flex-row mt-5">
          <div class="flex flex-col mr-20">
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Ad</label>
              <span class="dark:text-gray-200 text-gray-800">{{ application.firstName }} </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">E-posta</label>
              <span class="dark:text-gray-200 text-gray-800">{{ application.email ?? "-" }}</span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">TC Kimlik No</label>
              <span class="dark:text-gray-200 text-gray-800">{{ application.tckn ?? "-" }} </span>
            </div>
          </div>
          <div class="flex flex-col">
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Soyad</label>
              <span class="dark:text-gray-200 text-gray-800">{{ application.lastName }} </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Telefon</label>
              <span class="dark:text-gray-200 text-gray-800">{{ application.phone ?? "-" }}</span>
            </div>
          </div>
        </div>
      </div>
      <div
        class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
      >
        <span class="text-lg dark:text-gray-100 font-semibold">Adres Bilgileri</span>
        <div class="flex flex-row mt-5">
          <div class="flex flex-col mr-20">
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Ülke</label>
              <span class="dark:text-gray-200 text-gray-800"
                >{{ application.country ?? "-" }}
              </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">İlçe</label>
              <span class="dark:text-gray-200 text-gray-800">{{
                application.district ?? "-"
              }}</span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Tam Adres</label>
              <span class="dark:text-gray-200 text-gray-800">{{
                application.fullAddress ?? "-"
              }}</span>
            </div>
          </div>
          <div class="flex flex-col">
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">İl</label>
              <span class="dark:text-gray-200 text-gray-800">{{ application.city ?? "-" }} </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Posta Kodu</label>
              <span class="dark:text-gray-200 text-gray-800"
                >{{ application.postalCode ?? "-" }}
              </span>
            </div>
          </div>
        </div>
      </div>
      <div
        class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
      >
        <span class="text-lg dark:text-gray-100 font-semibold">Başvurulan İlan Bilgileri</span>
        <div class="flex flex-row mt-5">
          <div class="flex flex-col mr-20">
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Başlık</label>
              <span class="dark:text-gray-200 text-gray-800"
                >{{ application.jobPostingTitle ?? "-" }}
              </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5"
                >Kontenjan Sayısı</label
              >
              <span class="dark:text-gray-200 text-gray-800">{{
                application.jobPostingVacancyCount ?? "-"
              }}</span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Birim</label>
              <span class="dark:text-gray-200 text-gray-800">{{ application.unit ?? "-" }}</span>
            </div>
          </div>
          <div class="flex flex-col mr-20">
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5"
                >Oluşturulma Tarihi</label
              >
              <span class="dark:text-gray-200 text-gray-800"
                >{{ formatDateTime(application.jobPostingCreateDate) ?? "-" }}
              </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5"
                >Toplam Başvuru Sayısı</label
              >
              <span class="dark:text-gray-200 text-gray-800">150 </span>
            </div>
          </div>
        </div>
      </div>
      <div
        class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
      >
        <span class="text-lg dark:text-gray-100 font-semibold">Başvuru Bilgileri</span>
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
                    <span>Alan Adı</span>
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
                    <span>Alan Türü</span>
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
                    <span>İçerik</span>
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
                v-for="(field, index) in application.fieldValues"
                :key="field.fieldDefinitionId"
                class="border-b dark:border-gray-700/30 border-gray-200"
              >
                <td class="py-3 pr-2 pl-4 border-r dark:border-gray-800 border-gray-200">
                  {{ index }}
                </td>
                <td class="py-3 px-2 border-r dark:border-gray-800 border-gray-200">
                  {{ field.title }}
                </td>
                <td class="py-3 px-2 border-r dark:border-gray-800 border-gray-200">
                  {{ getFieldTypeOptionByValue(field.type)?.label }}
                </td>
                <td class="py-3 px-2 text-sm">
                  <a
                    v-if="
                      field.type == 6 || field.type == 13 || field.type == 15 || field.type == 16
                    "
                    :href="apiUrl + field.value"
                    class="text-blue-500"
                    target="_blank"
                    >{{ field.value ?? "-" }}</a
                  >

                  <span v-else>{{ field.value ?? "-" }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div
        class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
      >
        <span class="text-lg dark:text-gray-100 font-semibold">Başvuru Değerlendirme</span>
        <div class="mt-5 w-full">
          <div class="w-full flex flex-col">
            <label for="reviewDescription" class="text-sm mb-2 dark:text-gray-300 text-gray-700"
              >Değerlendirme Notu
              <span class="ml-1 text-gray-500 dark:text-gray-500">(opsiyonel)</span>
            </label>
            <textarea
              name="reviewDescription"
              id="reviewDescription"
              v-model="application.reviewDescription"
              class="border rounded-sm border-gray-200 dark:border-gray-800 outline-none dark:focus:border-indigo-600 focus:border-indigo-600 px-2 py-2 text-gray-700 dark:text-gray-200 text-sm"
            ></textarea>
          </div>
          <div class="w-full flex justify-end mt-5">
            <button
              class="px-3 py-1 border dark:border-gray-600 border-gray-400 cursor-pointer rounded-md dark:text-gray-200 text-gray-800 mr-3 hover:bg-red-600 text-sm hover:text-white"
              @click="
                () => {
                  isReviewModalOpen = true;
                  isApplicationApproved = false;
                }
              "
            >
              Başvuruyu Reddet
            </button>
            <button
              class="px-3 py-1 border dark:border-gray-600 border-gray-400 cursor-pointer rounded-md dark:text-gray-200 text-gray-800 ml-3 hover:bg-green-600 text-sm hover:text-white"
              @click="
                () => {
                  isReviewModalOpen = true;
                  isApplicationApproved = true;
                }
              "
            >
              Başvuruyu Onayla
            </button>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>
<style scoped></style>
