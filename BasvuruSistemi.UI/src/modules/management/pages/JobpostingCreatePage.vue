<script setup lang="ts">
import DatePicker from "primevue/datepicker";
import { Editor, EditorContent } from "@tiptap/vue-3";
import StarterKit from "@tiptap/starter-kit";
import Underline from "@tiptap/extension-underline";
import TextAlign from "@tiptap/extension-text-align";
import Color from "@tiptap/extension-color";
import Highlight from "@tiptap/extension-highlight";
import { onBeforeUnmount, onMounted, reactive, Ref, ref } from "vue";
import FormTemplateCreateComponent from "../components/FormTemplateCreateComponent.vue";
import {
  Bold,
  Italic,
  Underline as UnderlineIcon,
  AlignLeft,
  AlignCenter,
  AlignRight,
} from "lucide-vue-next";
import { useRoute } from "vue-router";
import { useDropdown } from "../composables/useDropdown";
import { JobPostingCreateModel } from "../models/job-posting-create.model";
import organizationService from "../services/organization.service";
import jobPostingService from "../services/job-posting.service";
import DOMPurify from "dompurify";

const editor = ref<Editor | null>(null);

const organizations: Ref<{ name: string; companyId: string; departmentId?: string }[]> = ref([]);

const organizationsDropdown = useDropdown();

const request = reactive<JobPostingCreateModel>({
  title: "",
  description: "",
  datePosted: new Date(),
  applicationDeadLine: new Date(),
  validFrom: new Date(),
  validTo: undefined,
  status: 1,
  isRemote: false,
  isPublic: false,
  companyId: "",
  departmentId: "",
  formTemplateId: "",
});

const route = useRoute();
const id = route.params.id as string | undefined;

onMounted(() => {
  //update eklemek için kullanılacak
  if (id) {
    // const res = await formTemplateService.getFormTemplate(id);
    // if (res) {
    //   request.name = res.name;
    //   request.description = res.description;
    //   request.fields = res.fields;
    // }
  }

  getOrganizations();
  editor.value = new Editor({
    content: "",
    extensions: [
      StarterKit,
      Underline,
      Color,
      Highlight,
      TextAlign.configure({
        types: ["heading", "paragraph"],
      }),
    ],
    editorProps: {
      attributes: {
        class: "focus:outline-none w-full prose max-w-none dark:prose-invert min-h-[200px] p-4",
      },
    },
  });
});

const getOrganizations = async () => {
  const res = await organizationService.getOrganizationsByTenant();
  if (res) {
    organizations.value = res;
  }
};

const selectOrganization = (organization: {
  name: string;
  companyId: string;
  departmentId?: string;
}) => {
  request.companyId = organization.companyId;
  request.departmentId = organization.departmentId;
};

onBeforeUnmount(() => {
  editor.value?.destroy();
});

const getEditorText = () => {
  console.log(editor.value?.getJSON());
};

const getFormTemplateId = (id: string) => {
  console.log(id);
  request.formTemplateId = id;
};

const handleSubmit = async () => {
  if (editor.value?.getHTML()) {
    request.description = DOMPurify.sanitize(editor.value.getHTML());
  }

  const res = await jobPostingService.createJobPostings(request);

  if (res) {
    console.log("RES", res);
  }
};

const commands = [
  {
    icon: Bold,
    action: () => editor.value?.chain().focus().toggleBold().run(),
    isActive: () => editor.value?.isActive("bold"),
  },
  {
    icon: Italic,
    action: () => editor.value?.chain().focus().toggleItalic().run(),
    isActive: () => editor.value?.isActive("italic"),
  },
  {
    icon: UnderlineIcon,
    action: () => editor.value?.chain().focus().toggleUnderline().run(),
    isActive: () => editor.value?.isActive("underline"),
  },
  {
    icon: AlignLeft,
    action: () => editor.value?.chain().focus().setTextAlign("left").run(),
    isActive: () => editor.value?.isActive({ textAlign: "left" }),
  },
  {
    icon: AlignCenter,
    action: () => editor.value?.chain().focus().setTextAlign("center").run(),
    isActive: () => editor.value?.isActive({ textAlign: "center" }),
  },
  {
    icon: AlignRight,
    action: () => editor.value?.chain().focus().setTextAlign("right").run(),
    isActive: () => editor.value?.isActive({ textAlign: "right" }),
  },
];
</script>

<template>
  <div class="w-full px-50 pt-20 flex pb-20">
    <div
      class="w-full dark:bg-gray-800/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
    >
      <div class="border-b px-5 py-3 dark:border-gray-800 border-gray-200">
        <span class="text-xl font-base dark:text-gray-50 text-gray-700">İlan Oluştur</span>
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
                >Birim</label
              >
              <input
                type="text"
                name="organizaitonName"
                id="organizaitonName"
                autocomplete="off"
                :ref="organizationsDropdown.inputRef"
                v-model="organizationsDropdown.selectedLabel.value"
                @focus="organizationsDropdown.handleFocus"
                @blur="organizationsDropdown.handleBlur"
                readonly
                placeholder="Birim seçin..."
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
              <div
                v-if="organizationsDropdown.isOpen.value"
                class="absolute w-fit mt-16 text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
              >
                <div
                  v-for="(option, index) in organizations"
                  :key="index"
                  @mousedown.prevent="
                    () => {
                      organizationsDropdown.selectOption(option.name);
                      selectOrganization(option);
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
                >İlan Adı</label
              >
              <input
                type="text"
                name="postingName"
                id="postingName"
                v-model="request.title"
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
              />
            </div>
          </div>
          <div class="flex flex-col xl:flex-row justify-start w-full">
            <div class="flex flex-col my-2 items-start w-full 2xl:mr-3">
              <label for="activeDate" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >İlk Başvuru Tarihi</label
              >
              <DatePicker
                input-id="activeDate"
                v-model="request.validFrom"
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
                >Son Başvuru Tarihi</label
              >
              <DatePicker
                input-id="endDate"
                v-model="request.applicationDeadLine"
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
          <div class="flex xl:flex-row flex-col justify-between">
            <div class="flex-1 flex flex-col 2xl:mr-3">
              <div>
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" value="" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >Kontenjan var mı?</span
                  >
                </label>
                <div class="flex-1 flex flex-col mb-2 items-start">
                  <label for="quota" class="text-sm dark:text-gray-300 text-gray-600"
                    >Kontenjan Limiti</label
                  >
                  <input
                    type="number"
                    name="quota"
                    id="quota"
                    v-model="request.vacancyCount"
                    autocomplete="off"
                    class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
                  />
                </div>
              </div>
              <div>
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" value="" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >Sadece Tanımlı Kişilere Göster?</span
                  >
                </label>
                <div class="flex-1 flex flex-col mb-2 items-start">
                  <label for="tckns" class="text-sm dark:text-gray-300 text-gray-600"
                    >Tanımlı Kişilerin TC Kimlik Numaralarını ',' ile ayırarak girin</label
                  >
                  <input
                    type="text"
                    name="tckns"
                    id="tckns"
                    autocomplete="off"
                    class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
                  />
                </div>
              </div>
            </div>
            <div class="flex-1 flex flex-col 2xl:ml-3">
              <div class="my-1">
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" value="" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >Kabul Edilenlere Belge Oluştur Butonu Çıkar?</span
                  >
                </label>
              </div>
              <div class="my-1">
                <label class="inline-flex items-center cursor-pointer">
                  <input type="checkbox" value="" class="sr-only peer" />
                  <div
                    class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                  ></div>
                  <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                    >İlan Yayında Mı?</span
                  >
                </label>
              </div>
            </div>
          </div>
          <div class="flex flex-col">
            <label
              for="editor"
              class="w-full text-md my-1 mb-5 dark:text-gray-300 text-gray-800"
              @click="getEditorText()"
              >Şartlar</label
            >
            <div
              class="dark:bg-gray-900/50 bg-gray-50 border rounded-md dark:border-gray-700 border-gray-200"
            >
              <div class="flex flex-wrap gap-2 p-2 border-b dark:border-gray-700 border-gray-200">
                <button
                  v-for="(cmd, index) in commands"
                  :key="index"
                  @click="cmd.action"
                  :class="[
                    'p-1 rounded-md hover:bg-gray-200 dark:hover:bg-gray-700',
                    cmd.isActive?.()
                      ? 'bg-blue-500 text-white'
                      : 'text-gray-700 dark:text-gray-300',
                  ]"
                >
                  <component :is="cmd.icon" class="w-5 h-5" />
                </button>
              </div>

              <EditorContent v-if="editor" :editor="editor" class="overflow-x-auto" />
            </div>
          </div>

          <FormTemplateCreateComponent
            @add:field="undefined"
            :fields-in-template="undefined"
            @form-template-id="getFormTemplateId"
          ></FormTemplateCreateComponent>
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
