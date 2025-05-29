<script setup lang="ts">
import { onMounted, reactive, Ref, ref } from "vue";
import { FormTemplateCreateReqeust } from "../../models/form-template-create.model";
import formTemplateService from "../../services/form-template.service";
import { useRoute, useRouter } from "vue-router";
import { FormFieldDefinition } from "../../models/form-field.model";
import { useDropdown } from "../../composables/useDropdown";
import FieldsFromFormTemplate from "../../components/form-template-components/form-template-create/FieldsFromFormTemplate.vue";
import NewFormField from "../../components/form-template-components/form-template-create/NewFormField.vue";
import SelectedFormFields from "../../components/form-template-components/form-template-create/SelectedFormFields.vue";
import { useToastStore } from "@/modules/toast/store/toast.store";

const formFieldAddingType = ref("FormTemplateAll");

const formTemplateDropdown = useDropdown();

const formTemplateSummaries: Ref<{ id: string; name: string }[]> = ref([]);

const request = reactive<FormTemplateCreateReqeust>({
  id: undefined,
  name: "",
  description: undefined,
  isSaved: true,
  fields: [],
});

const toastStore = useToastStore();

const route = useRoute();
const router = useRouter();
const id = route.params.id as string | undefined;

const isUpdate = ref<boolean>(false);

onMounted(async () => {
  if (id) {
    isUpdate.value = true;
    const res = await formTemplateService.getFormTemplate(id);
    if (res) {
      request.id = res.id;
      request.name = res.name;
      request.description = res.description;
      request.fields = res.fields;
    }
  }
  getFormTemplateSummaries();
});

const handleSubmit = async () => {
  if (request.name.trim() !== "" && request.fields.length != 0) {
    if (!isUpdate.value) {
      const res = await formTemplateService.createFormTemplate(request);
      if (res) {
        router.push({ name: "form-templates-list" });
      }
    } else {
      if (request.id) {
        console.log(request);
        const res = await formTemplateService.updateFormTemplate(request.id, request);
        if (res) {
          router.push({ name: "form-templates-list" });
        }
      }
    }
  } else {
    toastStore.addToast({
      message: "Şablon adı boş olamaz ve en az 1 alan olmak zorunda",
      type: "error",
      duration: 3000,
    });
  }
};

const addFieldToRequest = (field: FormFieldDefinition) => {
  if (!request.fields.find((p) => p.label == field.label)) {
    request.fields.push({ ...field });
  }
};

const getFormTemplateSummaries = async () => {
  const res = await formTemplateService.getFormTemplateSummaries();
  if (res) {
    formTemplateSummaries.value = res;
  }
};

const removeFieldFromRequest = (label: string) => {
  request.fields = request.fields.filter((p) => p.label != label);
};

const getFormTemplate = async (label: string) => {
  const id = formTemplateSummaries.value.find((p) => p.name == label);
  if (id) {
    const res = await formTemplateService.getFormTemplate(id.id);
    if (res) {
      request.fields = res.fields;
    }
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

          <div class="flex flex-col mt-5">
            <!-- Form Şablon -->
            <div class="flex justify-between">
              <label
                for="editor"
                class="w-full text-md my-1 dark:text-gray-300 text-gray-800 min-w-[10rem]"
                >Form Şablon Alanları</label
              >
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:text-gray-400 text-gray-700 hover:text-red-600 dark:hover:text-red-400 min-w-[10rem] hover:border-red-600 mr-3 dark:hover:border-red-600"
              >
                Seçili Olanları Sil
              </button>
              <!-- <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
              >
                Ekle
              </button> -->
            </div>
            <div class="flex flex-col border my-5 rounded-lg dark:border-gray-700 border-gray-200">
              <div class="my-1 mx-1">
                <ul
                  class="flex dark:bg-gray-900 bg-gray-200/40 w-fit px-1 py-0.5 rounded-md select-none"
                >
                  <li
                    class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mr-2 rounded-md text-sm"
                    :class="
                      formFieldAddingType === 'FormTemplateAll'
                        ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                        : 'text-gray-600 dark:text-gray-400'
                    "
                    @click="
                      () => {
                        formFieldAddingType = 'FormTemplateAll';
                        formTemplateDropdown.selectOption('');
                      }
                    "
                  >
                    Form Şablonu Seçimi
                  </li>
                  <li
                    class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mr-2 rounded-md text-sm"
                    :class="
                      formFieldAddingType === 'FieldFromFormTemplate'
                        ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                        : 'text-gray-600 dark:text-gray-400'
                    "
                    @click="
                      () => {
                        formFieldAddingType = 'FieldFromFormTemplate';
                        formTemplateDropdown.selectOption('');
                      }
                    "
                  >
                    Mevcut Şablondan Alanlar Ekle
                  </li>
                  <li
                    class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer rounded-md text-sm"
                    :class="
                      formFieldAddingType === 'NewField'
                        ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                        : 'text-gray-600 dark:text-gray-400'
                    "
                    @click="
                      () => {
                        formFieldAddingType = 'NewField';
                        formTemplateDropdown.selectOption('');
                      }
                    "
                  >
                    Yeni Alan Ekleme
                  </li>
                </ul>
              </div>
              <!-- Form Şablonu Seç -->
              <div v-if="formFieldAddingType === 'FormTemplateAll'" class="mx-1 my-1">
                <div class="flex flex-col 2xl:flex-row justify-between w-full">
                  <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
                    <label
                      for="formTeplate"
                      class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                      >Form Şablonu</label
                    >
                    <input
                      type="text"
                      name="formTeplate"
                      id="formTeplate"
                      v-model="formTemplateDropdown.selectedLabel.value"
                      @focus="formTemplateDropdown.handleFocus"
                      @blur="formTemplateDropdown.handleBlur"
                      readonly
                      placeholder="Form şablonu seçin..."
                      autocomplete="off"
                      class="w-[50%] border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
                    />
                  </div>
                  <div
                    v-if="formTemplateDropdown.isOpen.value"
                    class="absolute w-fit mt-18 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
                  >
                    <div
                      v-for="option in formTemplateSummaries"
                      :key="option.id"
                      @mousedown.prevent="formTemplateDropdown.selectOption(option.name)"
                      class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
                    >
                      {{ option.name }}
                    </div>
                  </div>
                </div>

                <div class="flex justify-end">
                  <button
                    class="text-sm border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
                    @click="getFormTemplate(formTemplateDropdown.selectedLabel.value)"
                  >
                    Ekle
                  </button>
                </div>
              </div>
              <!-- Field From Existing Template -->
              <FieldsFromFormTemplate
                v-if="formFieldAddingType === 'FieldFromFormTemplate'"
                :form-template-summaries="formTemplateSummaries"
                @add:field="addFieldToRequest"
              ></FieldsFromFormTemplate>

              <!-- New Field -->
              <NewFormField
                v-if="formFieldAddingType === 'NewField'"
                @add:field="addFieldToRequest"
              ></NewFormField>
            </div>

            <!-- Form Alanları -->
            <SelectedFormFields
              :fields="request.fields"
              @remove:field="removeFieldFromRequest"
            ></SelectedFormFields>
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
