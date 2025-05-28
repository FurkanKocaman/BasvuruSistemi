<script setup lang="ts">
import { onMounted, ref } from "vue";
import applicationService from "@/services/application.service";
import { useRoute } from "vue-router";
import { ApplicationGetDetailModel } from "../models/application-get-detail.model";
import { getFieldTypeOptionByValue } from "@/models/constants/field-type";
import ConfirmModal from "@/components/ConfirmModal.vue";
import { getApplicationStatusOptionByValue } from "@/models/constants/application-status";
import profileService from "@/modules/profile/services/profile.service";
import { Profile } from "@/modules/profile/models/profile.model";
import ApplicationEvaluationForm from "./application-evaluations/ApplicationEvaluationForm.vue";

const route = useRoute();
const id = route.params.id as string | undefined;

const confirmModal = ref();
const modalTitle = ref("");

const userData = ref<Profile>({
  id: "",
  firstName: "",
  lastName: "",
  profileStatus: 0,
  addresses: [],
  educations: [],
  certificates: [],
  experiences: [],
  skills: [],
});

const apiUrl = import.meta.env.VITE_API_PUBLIC_URL;

const windowType = ref(0);

const application = ref<ApplicationGetDetailModel>({
  id: "",
  jobPostingId: "",
  jobPostingTitle: "",
  userId: "",
  firstName: "",
  lastName: "",
  appliedDate: "",
  status: 0,
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

    if (res) {
      application.value = res;
      userData.value = await profileService.getUserProfile(res.userId);
    }
  }
};

// const reviewApplication = async (id: string, status: number) => {
//   if (status == 2) {
//     modalTitle.value = "Başvuruyu onaylamak istediğinize emin misiniz?";
//   } else {
//     modalTitle.value = "Başvuruyu reddetmek istediğinize emin misiniz?";
//   }
//   const result = await confirmModal.value.open();
//   if (result) {
//     await ApplicationManagementService.reviewApplication(
//       id,
//       status,
//       application.value.reviewDescription
//     );
//     getApplicationDetail();
//   } else {
//     console.log("Kullanıcı iptal etti.");
//   }
// };

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
    <ConfirmModal ref="confirmModal" :title="modalTitle" description="Bu işlem geri alınamaz." />

    <div class="w-full h-full flex flex-col dark:bg-gray-800/40 bg-gray-50 rounded-lg px-10 py-10">
      <div
        class="w-full h-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex items-center"
      >
        <img
          class="size-16 rounded-full object-cover"
          :src="userData.avatarUrl ?? apiUrl + '/user.png'"
          alt="profile image"
        />
        <div class="flex flex-col">
          <span class="text-base dark:text-gray-100 font-semibold ml-5">{{
            userData.firstName + " " + userData.lastName
          }}</span>
          <span class="text-sm dark:text-gray-400 font-normal ml-5">{{ userData.email }}</span>
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
              <span class="dark:text-gray-200 text-gray-800">{{ userData.firstName }} </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">E-posta</label>
              <span class="dark:text-gray-200 text-gray-800">{{ userData.email ?? "-" }}</span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">TC Kimlik No</label>
              <span class="dark:text-gray-200 text-gray-800">{{ userData.tckn ?? "-" }} </span>
            </div>
          </div>
          <div class="flex flex-col">
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Soyad</label>
              <span class="dark:text-gray-200 text-gray-800">{{ userData.lastName }} </span>
            </div>
            <div class="flex flex-col mb-3">
              <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Telefon</label>
              <span class="dark:text-gray-200 text-gray-800">{{ userData.phone ?? "-" }}</span>
            </div>
          </div>
        </div>
      </div>
      <div class="w-full border dark:border-gray-800 border-gray-200 rounded-md mb-5">
        <ul
          class="flex dark:bg-gray-800/50 bg-gray-100 w-fit px-1 py-0.5 rounded-md select-none mb-3"
        >
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mr-2 rounded-md text-sm"
            :class="
              windowType === 0
                ? 'dark:bg-gray-700 bg-gray-200 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                windowType = 0;
              }
            "
          >
            Adres
          </li>
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mr-2 rounded-md text-sm"
            :class="
              windowType === 1
                ? 'dark:bg-gray-700 bg-gray-200 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                windowType = 1;
              }
            "
          >
            Eğitim
          </li>
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mr-2 rounded-md text-sm"
            :class="
              windowType === 2
                ? 'dark:bg-gray-700 bg-gray-200 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                windowType = 2;
              }
            "
          >
            Sertifika
          </li>
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer rounded-md text-sm"
            :class="
              windowType === 3
                ? 'dark:bg-gray-700 bg-gray-200 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                windowType = 3;
              }
            "
          >
            Deneyim
          </li>
          <li
            class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer rounded-md text-sm"
            :class="
              windowType === 4
                ? 'dark:bg-gray-700 bg-gray-200 text-gray-900 dark:text-gray-50'
                : 'text-gray-600 dark:text-gray-400'
            "
            @click="
              () => {
                windowType = 4;
              }
            "
          >
            Yetenekler
          </li>
        </ul>
        <!-- Address -->
        <div v-if="windowType === 0" class="flex gap-3 px-3">
          <div
            v-for="address in userData.addresses"
            :key="address.id"
            class="rounded-lg mb-5 py-3 px-5 flex flex-col items-start border dark:border-gray-800 border-gray-200 w-fit"
          >
            <span class="text-lg dark:text-gray-100 font-semibold"
              >Adres Bilgileri
              <span class="text-gray-500 dark:text-gray-600">({{ address.name }})</span></span
            >
            <div class="flex flex-row mt-5">
              <div class="flex flex-col mr-20">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Ülke</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ address.country ?? "-" }}
                  </span>
                </div>
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">İlçe</label>
                  <span class="dark:text-gray-200 text-gray-800">{{
                    address.district ?? "-"
                  }}</span>
                </div>
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Tam Adres</label>
                  <span class="dark:text-gray-200 text-gray-800">{{
                    address.fullAddress ?? "-"
                  }}</span>
                </div>
              </div>
              <div class="flex flex-col">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">İl</label>
                  <span class="dark:text-gray-200 text-gray-800">{{ address.city ?? "-" }} </span>
                </div>
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Posta Kodu</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ address.postalCode ?? "-" }}
                  </span>
                </div>
              </div>
            </div>
          </div>
          <div
            v-if="userData.addresses.length == 0"
            class="w-full flex justify-center items-center py-5 border rounded-md my-3 border-gray-200 dark:border-gray-800 text-gray-700 dark:text-gray-300"
          >
            Adres Bulunamadı
          </div>
        </div>
        <!-- Education  -->
        <div v-if="windowType === 1" class="flex gap-3 px-3">
          <div
            v-for="education in userData.educations"
            :key="education.id"
            class="rounded-lg mb-5 py-3 px-5 flex flex-col items-start border dark:border-gray-800 border-gray-200 w-fit"
          >
            <span class="text-lg dark:text-gray-100 font-semibold">Eğitim Bilgileri</span>
            <div class="flex flex-row mt-5">
              <div class="flex flex-col mr-20">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Kurum</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ education.institution ?? "-" }}
                  </span>
                </div>
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Seviye</label>
                  <span class="dark:text-gray-200 text-gray-800">{{
                    education.degree ?? "-"
                  }}</span>
                </div>
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Dönem</label>

                  <span class="dark:text-gray-200 text-gray-800">{{
                    new Date(education.startDate).toLocaleDateString("tr-TR") +
                    "  -  " +
                    (education.graduationDate
                      ? new Date(education.graduationDate).toLocaleDateString("tr-TR")
                      : "Devam Ediyor")
                  }}</span>
                </div>
              </div>
              <div class="flex flex-col">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Bölüm</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ education.department ?? "-" }}
                  </span>
                </div>
              </div>
            </div>
          </div>
          <div
            v-if="userData.educations.length == 0"
            class="w-full flex justify-center items-center py-5 border rounded-md my-3 border-gray-200 dark:border-gray-800 text-gray-700 dark:text-gray-300"
          >
            Eğitim Bulunamadı
          </div>
        </div>
        <!-- Certificate  -->
        <div v-if="windowType === 2" class="flex gap-3 px-3">
          <div
            v-for="certificate in userData.certificates"
            :key="certificate.id"
            class="rounded-lg mb-5 py-3 px-5 flex flex-col items-start border dark:border-gray-800 border-gray-200 w-fit"
          >
            <span class="text-lg dark:text-gray-100 font-semibold">Sertifika Bilgileri</span>
            <div class="flex flex-row mt-5">
              <div class="flex flex-col mr-20">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Kurum</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ certificate.title ?? "-" }}
                  </span>
                </div>

                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Dönem</label>

                  <span class="dark:text-gray-200 text-gray-800">{{
                    new Date(certificate.dateReceived).toLocaleDateString("tr-TR") +
                    " - " +
                    (certificate.expiryDate
                      ? new Date(certificate.expiryDate).toLocaleDateString("tr-TR")
                      : "Devam Ediyor")
                  }}</span>
                </div>
              </div>
              <div class="flex flex-col">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Bölüm</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ certificate.issuer ?? "-" }}
                  </span>
                </div>
              </div>
            </div>
          </div>
          <div
            v-if="userData.certificates.length == 0"
            class="w-full flex justify-center items-center py-5 border rounded-md my-3 border-gray-200 dark:border-gray-800 text-gray-700 dark:text-gray-300"
          >
            Sertifika Bulunamadı
          </div>
        </div>
        <!-- Experience  -->
        <div v-if="windowType === 3" class="flex gap-3 px-3">
          <div
            v-for="experience in userData.experiences"
            :key="experience.id"
            class="rounded-lg mb-5 py-3 px-5 flex flex-col items-start border dark:border-gray-800 border-gray-200 w-fit"
          >
            <span class="text-lg dark:text-gray-100 font-semibold">Deneyim Bilgileri</span>
            <div class="flex flex-row mt-5">
              <div class="flex flex-col mr-20">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Kurum</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ experience.companyName ?? "-" }}
                  </span>
                </div>

                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Dönem</label>

                  <span class="dark:text-gray-200 text-gray-800">{{
                    new Date(experience.startDate).toLocaleDateString("tr-TR") +
                    " - " +
                    (experience.endDate
                      ? new Date(experience.endDate).toLocaleDateString("tr-TR")
                      : "Devam Ediyor")
                  }}</span>
                </div>
              </div>
              <div class="flex flex-col">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Departman</label>
                  <span class="dark:text-gray-200 text-gray-800"
                    >{{ experience.position ?? "-" }}
                  </span>
                </div>
              </div>
            </div>
          </div>
          <div
            v-if="userData.experiences.length == 0"
            class="w-full flex justify-center items-center py-5 border rounded-md my-3 border-gray-200 dark:border-gray-800 text-gray-700 dark:text-gray-300"
          >
            Deneyim Bulunamadı
          </div>
        </div>
        <!-- Skill  -->
        <div v-if="windowType === 4" class="flex gap-3 px-3">
          <div
            v-for="skill in userData.skills"
            :key="skill.id"
            class="rounded-lg mb-5 py-3 px-5 flex flex-col items-start border dark:border-gray-800 border-gray-200 w-fit"
          >
            <span class="text-lg dark:text-gray-100 font-semibold">Yetenek Bilgileri</span>
            <div class="flex flex-row mt-5">
              <div class="flex flex-col mr-20">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Ad</label>
                  <span class="dark:text-gray-200 text-gray-800">{{ skill.name ?? "-" }} </span>
                </div>

                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Açıklama</label>

                  <span class="dark:text-gray-200 text-gray-800">{{ skill.description }}</span>
                </div>
              </div>
              <div class="flex flex-col">
                <div class="flex flex-col mb-3">
                  <label class="text-xs dark:text-gray-400 text-gray-700 mb-1.5">Seviye</label>
                  <span class="dark:text-gray-200 text-gray-800">{{ skill.level ?? "-" }} </span>
                </div>
              </div>
            </div>
          </div>
          <div
            v-if="userData.skills.length == 0"
            class="w-full flex justify-center items-center py-5 border rounded-md my-3 border-gray-200 dark:border-gray-800 text-gray-700 dark:text-gray-300"
          >
            Yetenek Bulunamadı
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
              <span class="dark:text-gray-200 text-gray-800"
                >{{ application.totalApplicationCount }}
              </span>
            </div>
          </div>
        </div>
      </div>
      <div
        class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
      >
        <div class="flex justify-between w-full items-center">
          <span class="flex-1 text-lg dark:text-gray-100 font-semibold">Başvuru Bilgileri</span>

          <span
            class="text-sm px-2 py-1 rounded-md text-white"
            :class="
              application.status == 0
                ? 'bg-yellow-600'
                : application.status == 1
                ? 'bg-blue-600'
                : application.status == 2
                ? 'bg-green-600'
                : application.status == 3
                ? 'bg-red-500'
                : 'bg-indigo-500'
            "
            >{{ getApplicationStatusOptionByValue(application.status)?.label }}</span
          >
        </div>

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
                    :href="field.value"
                    class="text-blue-500"
                    target="_blank"
                    >{{ field.value ? field.value : "-" }}</a
                  >
                  <span v-else-if="field.type == 7">
                    {{ field.value ? formatDateTime(field.value) : "-" }}
                  </span>

                  <span v-else>{{ field.value ?? "-" }}</span>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <ApplicationEvaluationForm />
      <!-- <div
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
              :disabled="application.status != 0"
              @click="reviewApplication(application.id, 3)"
            >
              Başvuruyu Reddet
            </button>
            <button
              class="px-3 py-1 border dark:border-gray-600 border-gray-400 cursor-pointer rounded-md dark:text-gray-200 text-gray-800 ml-3 hover:bg-green-600 text-sm hover:text-white"
              :disabled="application.status != 0"
              @click="reviewApplication(application.id, 2)"
            >
              Başvuruyu Onayla
            </button>
          </div>
        </div>
      </div> -->
    </div>
  </main>
</template>
<style scoped></style>
