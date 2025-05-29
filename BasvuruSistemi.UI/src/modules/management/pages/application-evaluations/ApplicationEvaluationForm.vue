<script setup lang="ts">
import { getComponentByFieldType } from "@/modules/home/services/getComponentByField.service";
import { onMounted, ref } from "vue";
import evaluationFormService from "../../services/evaluation-form.service";
import { FormFieldResponse } from "../../models/form-filed-response.model";
import { useRoute, useRouter } from "vue-router";
import applicationEvaluationService from "../../services/application-evaluation.service";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { FieldValueModel } from "@/modules/home/models/field-value.model";
import { SubmitEvaluationModel } from "../../models/evaluation/create-requests/application-evaluation-submit.model";

const route = useRoute();
const router = useRouter();

const id = route.params.id as string | undefined;

const values = ref<Record<string, any>>({});
const fields = ref<FormFieldResponse[]>([]);

const toastStore = useToastStore();

const request = ref<SubmitEvaluationModel>({
  id: "",
  status: -1,
  values: [],
});

onMounted(() => {
  getEvaluationForm();
});

const getEvaluationForm = async () => {
  if (id) {
    request.value.id = id;
    const res = await evaluationFormService.getEvaluationForm(id);
    if (res) {
      fields.value = res.fields;
      res.fields.forEach((element) => {
        values.value[element.id] = undefined;
      });
    }
  }
};

const submitForm = async () => {
  let isError = false;

  console.log("Request", request);

  for (const field of fields.value) {
    if (field.type === 6 || field.type === 13 || field.type === 15 || field.type === 16) {
      const file = values.value[field.id];
      if (file && id) {
        try {
          const res = await applicationEvaluationService.uploadEvaluationFile({
            formFieldId: field.id,
            file: file,
            applicationEvaluationId: id,
          });
          if (res.statusCode === 200) {
            values.value[field.id] = res.data;
          } else {
            isError = true;
            console.error(res.errorMessages?.[0] ?? "Bilinmeyen hata");
            return;
          }
        } catch (err) {
          isError = true;
          console.error("Dosya yükleme hatası:", err);
          return;
        }
      }
    }
    if (field.type === 3) {
      values.value[field.id] = values.value[field.id].join(", ");
    }
    if (field.isRequired && !values.value[field.id]) {
      isError = true;
      toastStore.addToast({
        message: "Zorunlu alanları doldurmanız gerekiyor",
        type: "error",
        duration: 5000,
      });
      return;
    }
  }

  const result: FieldValueModel[] = Object.entries(values.value).map(([fieldId, val]) => ({
    fieldDefinitionId: fieldId,
    value: val,
  }));

  request.value.values = result;

  if (isError) {
    return;
  } else {
    console.log("Request", request);
    const res = await applicationEvaluationService.submitEvaluation(request.value);
    console.log(res);
    router.push("/management/pending-evaluations");
  }
};

const submit = () => {
  console.log(values.value);
};
</script>

<template>
  <div
    v-if="id"
    class="w-full border dark:border-gray-800 border-gray-200 rounded-lg mb-5 py-3 px-5 flex flex-col items-start"
  >
    <span class="text-lg dark:text-gray-100 font-semibold" @click="submit"
      >Başvuru Değerlendirme</span
    >
    <div class="mt-5 w-full">
      <div class="space-y-6">
        <div v-for="field in fields" :key="field.id">
          <component
            :is="getComponentByFieldType(field.type)"
            v-model="values[field.id]"
            :field="field"
          />
        </div>
      </div>
      <div class="w-full flex flex-col">
        <label for="reviewDescription" class="text-sm mb-2 dark:text-gray-300 text-gray-700"
          >Değerlendirme Notu
          <span class="ml-1 text-gray-500 dark:text-gray-500">(opsiyonel)</span>
        </label>
        <textarea
          name="reviewDescription"
          id="reviewDescription"
          v-model="request.overallComment"
          class="border rounded-sm border-gray-200 dark:border-gray-800 outline-none dark:focus:border-indigo-600 focus:border-indigo-600 px-2 py-2 text-gray-700 dark:text-gray-200 text-sm"
        ></textarea>
      </div>
      <div class="w-full flex flex-col mt-3">
        <label for="evaluationStatus" class="text-sm mb-2 dark:text-gray-300 text-gray-700"
          >Değerlendirme Durumu
        </label>
        <select
          name="evaluationStatus"
          id="evaluationStatus"
          v-model="request.status"
          class="border rounded-sm border-gray-200 dark:border-gray-800 outline-none dark:focus:border-indigo-600 focus:border-indigo-600 px-2 py-2 text-gray-700 dark:text-gray-200 text-sm bg-gray-50 dark:bg-gray-900"
        >
          <option :value="-1" selected>Durum Seçiniz</option>
          <option :value="0">Beklemede</option>
          <option :value="1">Değerlendirmede</option>
          <option :value="2">Kaydedildi</option>
          <option :value="3">Onaylandı</option>
          <option :value="4">Reddedildi</option>
          <option :value="5">Revize Talep Et</option>
        </select>
      </div>
      <div class="w-full flex justify-end mt-5">
        <button
          class="px-3 py-1 border dark:border-gray-600 border-gray-400 cursor-pointer rounded-md dark:text-gray-200 text-gray-800 ml-3 hover:bg-blue-600 text-sm hover:text-white"
          @click.stop="submitForm"
          :disabled="request.status == -1"
        >
          Değerlendirmeyi Kaydet
        </button>
      </div>
    </div>
  </div>
</template>
