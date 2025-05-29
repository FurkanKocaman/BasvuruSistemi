<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useToastStore } from "@/modules/toast/store/toast.store";
import evaluationService from "../../services/evaluation.service";
import { EvaluationFormDto } from "../../models/evaluation/evaluation-form.model";
import { EvaluationFormCreateModel } from "../../models/evaluation/create-requests/evaluation-form-create.model";
import { getFieldTypeOptionByValue } from "@/models/constants/field-type";
import EvaluationFormFieldCreateModal from "../../components/modals/EvaluationFormFieldCreateModal.vue";
import { EvaluationFormFieldDto } from "../../models/evaluation/evaluation-form-field.model";
import ConfirmModal from "@/components/ConfirmModal.vue";

const request = reactive<EvaluationFormCreateModel>({
  name: "",
});

const evaluationForm = ref<EvaluationFormDto>({
  id: "",
  name: "",
  evaluationStageId: "",
  createdAt: "",
  fields: [],
});

const toastStore = useToastStore();

const route = useRoute();
const router = useRouter();

const addFieldModal = ref();
const confirmModal = ref();

const id = route.params.id as string | undefined;
const formId = route.query.formId;

const isUpdate = ref<boolean>(false);

onMounted(async () => {
  if (id) {
    getEvaluationStage(id);
  }
  if (formId) {
    isUpdate.value = true;
    getEvaluationForm(formId.toString());
  }
});

const getEvaluationStage = async (id: string) => {
  const res = await evaluationService.getEvaluationStageById(id);
  if (res) {
    request.evaluationStageId = res.id;
  }
};

const getEvaluationForm = async (id: string) => {
  const res = await evaluationService.getEvaluationFormById(id);
  if (res) {
    evaluationForm.value = res;
    request.id = res.id;
    request.name = res.name;
    request.description = res.description;
  }
};

const handleSubmit = async () => {
  if (request.name.trim() !== "") {
    if (!isUpdate.value) {
      const res = await evaluationService.createEvaluatonForm(request);
      if (res) {
        router.push({ name: "evaluation-stages" });
      }
    } else {
      if (request.id) {
        console.log(request);
        const res = await evaluationService.updateEvaluatonForm(request);
        if (res) {
          router.push({ name: "evaluation-stages" });
        }
      }
    }
  } else {
    toastStore.addToast({
      message: "Ad boş olamaz!",
      type: "error",
      duration: 3000,
    });
  }
};

const addField = async () => {
  if (request.id) {
    const result = await addFieldModal.value.open(
      request.id,
      evaluationForm.value.fields.length,
      undefined
    );
    if (result) {
      if (formId) {
        getEvaluationForm(formId.toString());
      }
    }
  } else {
    toastStore.addToast({
      message: "Önce formu kaydetmelisiniz",
      type: "error",
      duration: 3000,
    });
  }
};
const updateField = async (field: EvaluationFormFieldDto) => {
  const result = await addFieldModal.value.open(
    request.id,
    evaluationForm.value.fields.length,
    field
  );
  if (result) {
    console.log(result);

    if (formId) {
      getEvaluationForm(formId.toString());
    }
  } else {
    console.log(result);
  }
};
const deleteField = async (id: string) => {
  const result = await confirmModal.value.open();
  if (result) {
    const res = await evaluationService.deleteFieldFromForm(id);
    if (res) {
      evaluationForm.value.fields = evaluationForm.value.fields.filter((p) => p.id != id);
    }
  } else {
    console.log(result);
  }
};

// const updateMember = async (member: AddMemberToCommissionModel) => {
//   const result = await addMemberModal.value.open(request.id, member);
//   if (result) {
//     console.log(result);
//   } else {
//     console.log(result);
//   }
// };
</script>

<template>
  <div class="w-full px-50 pt-20 flex pb-20">
    <ConfirmModal
      ref="confirmModal"
      title="Alanı silmek istediğinize emin misiniz?"
      description="Bu işlem geri alınamaz."
    />
    <EvaluationFormFieldCreateModal ref="addFieldModal" />
    <div
      class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
    >
      <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200">
        <span class="text-xl font-base dark:text-gray-50 text-gray-700">
          {{ isUpdate ? "Form Düzenle" : "Form Oluştur" }}
        </span>
      </div>
      <div class="px-10 py-5">
        <div
          class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200 flex flex-col px-20 pt-5"
        >
          <div class="flex flex-col w-full">
            <label for="templateName" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
              >Ad</label
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
              >Açıklama</label
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
                >Alanlar
              </label>
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:text-gray-400 text-gray-700 hover:text-red-600 dark:hover:text-red-400 min-w-[10rem] hover:border-red-600 mr-3 dark:hover:border-red-600"
              >
                Seçili Olanları Sil
              </button>
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
                @click="addField()"
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
                        class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200 cursor-pointer select-none dark:text-gray-400 text-sm w-40"
                      >
                        <div class="flex items-center justify-between">
                          <span>Tür</span>
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
                          <span>Zorunlu mu?</span>
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

                      <!-- <td
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
                      </td> -->

                      <td class="py-3 px-2">İşlemler</td>
                    </tr>
                  </thead>
                  <tbody class="">
                    <tr
                      v-for="field in evaluationForm.fields"
                      :key="field.id"
                      class="border-b dark:border-gray-700/30 border-gray-200"
                    >
                      <td
                        class="py-3 pr-2 pl-4 border-r dark:border-gray-700/30 border-gray-200 w-10"
                      >
                        {{ field.order }}
                      </td>
                      <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                        <span>{{ getFieldTypeOptionByValue(field.type)?.label ?? "-" }}</span>
                      </td>
                      <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                        <div class="flex items-center">
                          <span class="cursor-pointer">{{ field.label }}</span>
                        </div>
                      </td>
                      <td class="py-3 px-2 border-r dark:border-gray-700/30 border-gray-200">
                        <div class="flex items-center">
                          <span>{{ field.isRequired ? "Evet" : "Hayır" }}</span>
                        </div>
                      </td>

                      <!-- <td
                        class="py-3 px-2 border-r text-sm dark:border-gray-700/30 border-gray-200"
                      >
                        {{ form.createdAt ? formatDateTime(form.createdAt) : "-" }}
                      </td> -->

                      <td class="py-3 px-2 w-60">
                        <button class="cursor-pointer pr-1 group" @click.stop="updateField(field)">
                          <svg
                            class="size-6 dark:fill-gray-400 fill-gray-600 group-hover:fill-blue-600 dark:group-hover:fill-blue-600"
                            viewBox="0 0 24 24"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              d="M18.111,2.293,9.384,11.021a.977.977,0,0,0-.241.39L8.052,14.684A1,1,0,0,0,9,16a.987.987,0,0,0,.316-.052l3.273-1.091a.977.977,0,0,0,.39-.241l8.728-8.727a1,1,0,0,0,0-1.414L19.525,2.293A1,1,0,0,0,18.111,2.293ZM11.732,13.035l-1.151.384.384-1.151L16.637,6.6l.767.767Zm7.854-7.853-.768.767-.767-.767.767-.768ZM3,5h8a1,1,0,0,1,0,2H4V20H17V13a1,1,0,0,1,2,0v8a1,1,0,0,1-1,1H3a1,1,0,0,1-1-1V6A1,1,0,0,1,3,5Z"
                            />
                          </svg>
                        </button>
                        <button class="cursor-pointer pr-1" @click="deleteField(field.id)">
                          <svg
                            class="size-6 group"
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
                        <button class="cursor-pointer pr-1 group">
                          <svg
                            class="size-6 group"
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
