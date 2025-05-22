<script setup lang="ts">
import DatePicker from "primevue/datepicker";
import { onMounted, reactive, Ref, ref } from "vue";
import { useDropdown } from "../composables/useDropdown";
import organizationService from "../services/unit.service";
import DOMPurify from "dompurify";
import EditorComponent from "../components/job-posting-components/EditorComponent.vue";

import { Unit } from "../models/unit-node.model";
import { useRoute, useRouter } from "vue-router";
import { PostingGroupCreateModel } from "../models/posting-group-create.model";
import ConfirmModal from "@/components/ConfirmModal.vue";
import { useToastStore } from "@/modules/toast/store/toast.store";
import jobPostingService from "../services/job-posting.service";
import { JobPostingSummaryDto } from "../models/posting-group-get.model";
import { getJobPostingStatusOptionByValue } from "@/models/constants/job-posting-status";

const toastStore = useToastStore();

const router = useRouter();

const units: Ref<Unit[]> = ref([]);
const confirmModal = ref();
const unitsDropdown = useDropdown();

const isGroupCreated = ref(false);
const jobPostings = ref<JobPostingSummaryDto[]>([]);
const isPostingGroupPublished = ref(false);

const request = reactive<PostingGroupCreateModel>({
  name: "",
  description: "",
  isPublic: false,
  status: 0,
});

const route = useRoute();
const id = route.params.id as string | undefined;

onMounted(async () => {
  if (id) {
    getPostingGroup(id);
  }
  getUnits();
});

const getUnits = async () => {
  const res = await organizationService.getUnitsByTenant();
  if (res) {
    units.value = res;

    if (request.unitId) {
      var unit = units.value.find((p) => p.id == request.unitId);
      if (unit) {
        unitsDropdown.selectOption(unit.name);
      }
    }
  }
};

const getPostingGroup = async (id: string) => {
  const res = await jobPostingService.getPostingGroup(id);
  if (res) {
    request.id = res.id;
    request.name = res.name;
    request.description = res.description ? res.description : "";

    request.isPublic = res.isPublic;
    isPostingGroupPublished.value = res.status == 2;
    request.status = res.status;

    request.announcementDate = res.announcementdate ? new Date(res.announcementdate) : undefined;
    request.overallApplicationStartDate = res.overallApplicationStartDate
      ? new Date(res.overallApplicationStartDate)
      : undefined;
    request.overallApplicationEndDate = res.overallApplicationEndDate
      ? new Date(res.overallApplicationEndDate)
      : undefined;

    request.unitId = res.unitId;

    jobPostings.value = res.jobPostings;

    isGroupCreated.value = true;
    if (res.unit) unitsDropdown.selectOption(res.unit);
  }
};

const handleSubmit = async () => {
  if (unitsDropdown.selectedLabel) {
    const unit = units.value.find((p) => p.name == unitsDropdown.selectedLabel.value);
    if (unit) {
      request.unitId = unit.id;
    }
  }
  if (request.name.length == 0 || request.name.trim() == "") {
    console.error("name cannot be null");
    return;
  }
  request.description = DOMPurify.sanitize(request.description);

  request.status = isPostingGroupPublished.value ? 2 : 1;

  const res = await jobPostingService.createJobPostingGroup(request);
  if (res) {
    request.id = res;
    isGroupCreated.value = true;
  }
};

const editJobPosting = (id: string) => {
  router.push({
    name: "job-posting-update",
    params: { id },
    query: { postingGroupId: request.id },
  });
  console.error("navigate to job posting edit", id);
};

const addJobPostingToGroup = () => {
  if (!isGroupCreated.value) {
    toastStore.addToast({
      message: "Bir ilan eklemeden önce grubu kaydetmelisiniz.",
      type: "warning",
      duration: 3000,
    });
  } else {
    router.push({
      name: "job-posting-create",
      query: { postingGroupId: request.id },
    });
  }
};

const removeJobPostingFromGroup = async (id: string) => {
  const result = await confirmModal.value.open();
  if (result) {
    console.log("Cofirm modal onaylandı");
  } else {
    console.log("Kullanıcı iptal etti.");
  }
  console.error("implement removeJobPostingFromGroup", id);
};

const selectUnit = (unit: Unit) => {
  request.unitId = unit.id;
};
</script>

<template>
  <div class="w-full px-50 pt-20 flex pb-20">
    <ConfirmModal
      ref="confirmModal"
      title="Bu ilanı gruptan çıkarmak istediğinize emin misiniz?"
      description="Çıkarılan ilan ilanlar sayfasında gözükecektir."
    />
    <div
      class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
    >
      <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200">
        <span class="text-xl font-base dark:text-gray-50 text-gray-700">İlan Grubu Oluştur</span>
      </div>

      <div class="px-10 py-5">
        <div
          class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200 flex flex-col px-20"
        >
          <div class="flex flex-col 2xl:flex-row justify-between w-full">
            <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
              <label
                for="organizaitonName"
                class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                >Birim <span class="text-gray-500 dark:text-gray-400 ml-1">(Opsiyonel)</span></label
              >
              <input
                type="text"
                name="organizaitonName"
                id="organizaitonName"
                autocomplete="off"
                :ref="unitsDropdown.inputRef"
                v-model="unitsDropdown.selectedLabel.value"
                @focus="unitsDropdown.handleFocus"
                @blur="unitsDropdown.handleBlur"
                readonly
                placeholder="Birim seçin..."
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
              <span class="text-gray-600 dark:text-gray-400 text-xs"
                >* Bu ilan grubuna ekleyeceğiniz her ilan bu gruba atanan birimin altındaki bir
                birimde veya kendisi olmalıdır.</span
              >
              <div
                v-if="unitsDropdown.isOpen.value"
                class="absolute w-fit mt-16 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
              >
                <div
                  v-for="(option, index) in units"
                  :key="index"
                  @mousedown.prevent="
                    () => {
                      unitsDropdown.selectOption(option.name);
                      selectUnit(option);
                    }
                  "
                  class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
                >
                  {{ option.name }}
                </div>
              </div>
            </div>
            <div class="flex-1 flex flex-col my-2 items-start 2xl:ml-3">
              <label for="postingName" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >Grup Adı</label
              >
              <input
                type="text"
                name="postingName"
                id="postingName"
                v-model="request.name"
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
            </div>
          </div>
          <div class="flex flex-col xl:flex-row justify-start w-full">
            <div class="flex flex-col my-2 items-start w-full 2xl:mr-3">
              <label for="activeDate" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >İlk Başvuru Tarihi<span class="text-gray-500 dark:text-gray-400 ml-1"
                  >(Opsiyonel)</span
                ></label
              >
              <DatePicker
                input-id="activeDate"
                v-model="request.overallApplicationStartDate"
                class="!w-full !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !py-1.5 !px-2 !bg-gray-50 dark:!bg-gray-900/50 dark:!focus:border-indigo-600 focus:border-indigo-600"
                inputClass="!text-start !text-sm  "
                panel-class="dark:!bg-gray-900 !bg-gray-50 dark:!text-gray-200 !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !p-3"
                showTime
                hourFormat="24"
                showButtonBar
                showIcon
              />
            </div>
            <div class="flex flex-col my-2 items-start w-full 2xl:ml-3">
              <label for="endDate" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >Son Başvuru Tarihi<span class="text-gray-500 dark:text-gray-400 ml-1"
                  >(Opsiyonel)</span
                ></label
              >
              <DatePicker
                input-id="endDate"
                v-model="request.overallApplicationEndDate"
                class="!w-full !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !py-1.5 !px-2 !bg-gray-50 dark:!bg-gray-900/50 focus:!border-indigo-600"
                inputClass="!text-start !text-sm  "
                panel-class="dark:!bg-gray-900 !bg-gray-50 dark:!text-gray-200 !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !p-3"
                showTime
                hourFormat="24"
                showButtonBar
                showIcon
              />
            </div>
          </div>
          <div class="flex flex-col justify-start w-full">
            <div class="my-1">
              <label class="inline-flex items-center cursor-pointer select-none">
                <input type="checkbox" v-model="request.isPublic" class="sr-only peer" />
                <div
                  class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                ></div>
                <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                  >İlan grubu herkese açık mı?</span
                >
              </label>
            </div>
            <div class="my-1">
              <label class="inline-flex items-center cursor-pointer select-none">
                <input type="checkbox" v-model="isPostingGroupPublished" class="sr-only peer" />
                <div
                  class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                ></div>
                <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                  >İlan Yayında Mı?</span
                >
              </label>
            </div>
          </div>

          <EditorComponent
            v-if="id ? request.description : true"
            v-model="request.description"
            :label="'Açıklama'"
          ></EditorComponent>

          <!-- FormTemplate Edit and Select start -->

          <div class="flex flex-col mt-5">
            <!-- Form Şablon -->
            <div class="flex justify-between">
              <label class="w-full text-md my-1 dark:text-gray-300 text-gray-800 min-w-[10rem]"
                >İlanlar</label
              >
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:text-gray-400 text-gray-700 hover:text-red-600 dark:hover:text-red-400 min-w-[10rem] hover:border-red-600 mr-3 dark:hover:border-red-600"
              >
                Seçili Olanları Çıkar
              </button>
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
                @click.stop="addJobPostingToGroup()"
              >
                Ekle
              </button>
            </div>

            <!-- Form Alanları -->
            <div class="my-2">
              <table class="w-full">
                <thead class="">
                  <tr class="border-b dark:border-gray-700 border-gray-200">
                    <td class="w-10 py-3">
                      <label class="inline-flex items-center cursor-pointer select-none group">
                        <input type="checkbox" class="sr-only" />
                        <span
                          class="w-5 h-5 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="w-4 h-4 text-indigo-400"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                            stroke-width="4"
                          >
                            <path
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              d="M5 13l4 4L19 7"
                            />
                          </svg>
                        </span>
                      </label>
                    </td>
                    <td class="text-gray-700 dark:text-gray-300 text-sm">İlan Adı</td>

                    <td class="text-gray-700 dark:text-gray-300 text-sm">Birim</td>
                    <td class="text-gray-700 dark:text-gray-300 text-sm">Durum</td>
                    <td class="text-gray-700 dark:text-gray-300 text-sm w-20">Başvurular</td>
                    <td class="text-gray-700 dark:text-gray-300 text-sm w-30">İşlemler</td>
                  </tr>
                </thead>
                <tbody class="">
                  <tr v-for="jobPosting in jobPostings" :key="jobPosting.id">
                    <td class="py-2">
                      <label class="inline-flex items-center cursor-pointer select-none group">
                        <input type="checkbox" class="sr-only" />
                        <span
                          class="w-5 h-5 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="w-4 h-4 text-indigo-400"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                            stroke-width="4"
                          >
                            <path
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              d="M5 13l4 4L19 7"
                            />
                          </svg>
                        </span>
                      </label>
                    </td>
                    <td class="py-2 text-sm cursor-pointer hover:text-blue-500">
                      {{ jobPosting.title }}
                    </td>

                    <td class="py-2 text-sm">
                      {{ jobPosting.unit ?? "-" }}
                    </td>
                    <td class="py-2 text-sm">
                      {{ getJobPostingStatusOptionByValue(jobPosting.status)?.label }}
                    </td>
                    <td class="py-2 text-sm w-20">
                      {{ jobPosting.totalApplicationsCount }}
                    </td>
                    <td class="py-2">
                      <button
                        class="cursor-pointer mx-1 group"
                        title="İlanı düzenle"
                        @click.stop="editJobPosting(jobPosting.id)"
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
                      <button
                        class="cursor-pointer mr-1"
                        title="Gruptan çıkar"
                        @click.stop="removeJobPostingFromGroup('12')"
                      >
                        <svg
                          class="size-5 group"
                          viewBox="0 0 24 24"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M20.5001 6H3.5"
                            class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
                            class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M9.5 11L10 16"
                            class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M14.5 11L14 16"
                            class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65651 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
                            class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-red-600 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                          />
                        </svg>
                      </button>
                      <button class="cursor-pointer pr-1 group" title="Sıralamayı değiştir">
                        <svg
                          class="size-5 group"
                          viewBox="0 0 24 24"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <g id="Edit / Move_Vertical">
                            <path
                              id="Vector"
                              d="M12 21V3M12 21L15 18M12 21L9 18M12 3L9 6M12 3L15 6"
                              class="dark:stroke-gray-400 stroke-gray-700 group-hover:stroke-cyan-600 dark:group-hover:stroke-cyan-600"
                              stroke-width="1.5"
                              stroke-linecap="round"
                              stroke-linejoin="round"
                            />
                          </g>
                        </svg>
                      </button>
                    </td>
                  </tr>
                </tbody>
              </table>
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
