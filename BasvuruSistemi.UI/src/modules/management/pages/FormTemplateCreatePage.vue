<script setup lang="ts">
import { onMounted, reactive } from "vue";
import { FormTemplateCreateReqeust } from "../models/form-template-create.model";
import formTemplateService from "../services/form-template.service";
import { useRoute } from "vue-router";
import FormTemplateCreateComponent from "../components/FormTemplateCreateComponent.vue";
import { FormFieldDefinition } from "../models/form-field.model";

const request = reactive<FormTemplateCreateReqeust>({
  name: "",
  description: undefined,
  fields: [],
});

const route = useRoute();
const id = route.params.id as string | undefined;

onMounted(async () => {
  if (id) {
    const res = await formTemplateService.getFormTemplate(id);
    if (res) {
      request.name = res.name;
      request.description = res.description;
      request.fields = res.fields;
    }
  }
});

const addFieldToRequest = (field: FormFieldDefinition) => {
  if (!request.fields.find((p) => p.label == field.label)) {
    request.fields.push({ ...field });
    console.log(request);
  }
};
</script>

<template>
  <div class="w-full px-50 pt-20 flex pb-20">
    <div
      class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
    >
      <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200">
        <span class="text-xl font-base dark:text-gray-50 text-gray-700">Form Şablonu Oluştur</span>
      </div>
      <div class="px-10 py-5">
        <div
          class="dark:bg-gray-700/40 dark:text-gray-300 rounded-md border dark:border-gray-800 border-gray-200 flex flex-col px-20"
        >
          <div class="flex flex-col w-full">
            <label for="templateName" class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
              >Şablon Adı</label
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
              >Şablon Açıklama</label
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
          <!-- FormTemplateCreateComponent -->
          <FormTemplateCreateComponent
            @add:field="addFieldToRequest"
            :fields-in-template="request.fields"
          ></FormTemplateCreateComponent>
          <div class="my-4 flex justify-end">
            <button
              class="border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
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
