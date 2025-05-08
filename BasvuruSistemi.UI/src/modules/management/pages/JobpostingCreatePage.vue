<script setup lang="ts">
import DatePicker from "primevue/datepicker";
import { Editor, EditorContent } from "@tiptap/vue-3";
import StarterKit from "@tiptap/starter-kit";
import Underline from "@tiptap/extension-underline";
import TextAlign from "@tiptap/extension-text-align";
import Color from "@tiptap/extension-color";
import Highlight from "@tiptap/extension-highlight";
import { onBeforeUnmount, onMounted, ref } from "vue";
const modelValue = defineModel<string>();
import {
  Bold,
  Italic,
  Underline as UnderlineIcon,
  AlignLeft,
  AlignCenter,
  AlignRight,
} from "lucide-vue-next";

const editor = ref<Editor | null>(null);
const formFieldAddingType = ref("FormTemplateAll");

onMounted(() => {
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

onBeforeUnmount(() => {
  editor.value?.destroy();
});

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

const activeDate = ref(null);
const endDate = ref(null);

const rows = ref([
  { id: 1, name: "Ali Yılmaz" },
  { id: 2, name: "Zeynep Demir" },
  { id: 3, name: "Ahmet Kaya" },
]);

const selectedRows = ref<boolean[]>(rows.value.map(() => false));
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
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
              />
            </div>
            <div class="flex-1 flex flex-col my-2 items-start 2xl:ml-3">
              <label for="postingName" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                >İlan Adı</label
              >
              <input
                type="text"
                name="postingName"
                id="postingName"
                autocomplete="off"
                class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
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
                v-model="activeDate"
                class="!w-full !border !outline-none !rounded-md dark:!border-gray-700 !border-gray-200 !py-1.5 !px-2 !bg-gray-50 dark:!bg-gray-900/50"
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
                v-model="endDate"
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
                    autocomplete="off"
                    class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
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
                    class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
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
            <label for="editor" class="w-full text-md my-1 mb-5 dark:text-gray-300 text-gray-800"
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
          <div class="flex flex-col mt-5">
            <div class="flex justify-between">
              <label
                for="editor"
                class="w-full text-md my-1 dark:text-gray-300 text-gray-800 min-w-[10rem]"
                >İlan Formu</label
              >
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:text-gray-400 text-gray-700 hover:text-red-600 dark:hover:text-red-400 min-w-[10rem] hover:border-red-600 mr-3 dark:hover:border-red-600"
              >
                Seçili Olanları Sil
              </button>
              <button
                class="text-sm border rounded-md py-1 px-2.5 dark:border-gray-700 border-gray-200 cursor-pointer dark:hover:bg-gray-700/30 hover:bg-gray-400/10 dark:text-gray-400 text-gray-700 dark:hover:text-gray-100 hover:text-gray-900"
              >
                Ekle
              </button>
            </div>
            <div class="flex flex-col border my-5 rounded-lg dark:border-gray-700 border-gray-200">
              <div class="my-1 mx-1">
                <ul
                  class="flex dark:bg-gray-900 bg-gray-200/40 w-fit px-1 py-0.5 rounded-md select-none"
                >
                  <li
                    class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer rounded-md text-sm"
                    :class="
                      formFieldAddingType === 'FormTemplateAll'
                        ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                        : 'text-gray-600 dark:text-gray-400'
                    "
                    @click="
                      () => {
                        formFieldAddingType = 'FormTemplateAll';
                      }
                    "
                  >
                    Form Şablonu Seçimi
                  </li>
                  <li
                    class="px-2 py-1.5 dark:hover:text-gray-50 hover:text-gray-900 cursor-pointer mx-2 rounded-md text-sm"
                    :class="
                      formFieldAddingType === 'FieldFromFormTemplate'
                        ? 'dark:bg-gray-800 bg-gray-50 text-gray-900 dark:text-gray-50'
                        : 'text-gray-600 dark:text-gray-400'
                    "
                    @click="
                      () => {
                        formFieldAddingType = 'FieldFromFormTemplate';
                      }
                    "
                  >
                    Form Şablonundan Alan Seçimi
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
                      }
                    "
                  >
                    Yeni Alan Ekleme
                  </li>
                </ul>
              </div>

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
                      autocomplete="off"
                      class="w-[50%] border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
                    />
                  </div>
                </div>

                <div class="flex justify-end">
                  <button
                    class="text-sm border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
                  >
                    Ekle
                  </button>
                </div>
              </div>

              <div v-if="formFieldAddingType === 'FieldFromFormTemplate'" class="mx-1 my-1">
                <div class="flex flex-col 2xl:flex-row justify-start items-end w-full">
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
                      autocomplete="off"
                      class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
                    />
                  </div>
                  <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
                    <button
                      class="border rounded-md text-sm py-1.5 px-5 cursor-pointer dark:border-gray-700 border-gray-200 text-gray-700 dark:text-gray-200 dark:hover:bg-gray-700/20 hover:bg-gray-400/10 select-none"
                    >
                      Alanları Getir
                    </button>
                  </div>
                </div>
                <div class="flex flex-col 2xl:flex-row justify-between w-full">
                  <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
                    <table class="w-full">
                      <thead></thead>
                      <tbody class="divide-y dark:divide-gray-700 divide-gray-200">
                        <tr v-for="(row, index) in rows" :key="index" class="flex">
                          <td class="w-10 py-2 ml-3 flex items-center">
                            <label
                              class="inline-flex items-center cursor-pointer select-none group"
                            >
                              <input
                                type="checkbox"
                                v-model="selectedRows[index]"
                                class="sr-only"
                              />
                              <span
                                class="w-4 h-4 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                              >
                                <svg
                                  v-if="selectedRows[index]"
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
                          <td class="w-14 py-2 flex items-center">
                            <svg
                              class="size-6"
                              viewBox="0 0 24 24"
                              fill="none"
                              xmlns="http://www.w3.org/2000/svg"
                            >
                              <path
                                fill-rule="evenodd"
                                clip-rule="evenodd"
                                d="M6 1C4.34315 1 3 2.34315 3 4V20C3 21.6569 4.34315 23 6 23H18C19.6569 23 21 21.6569 21 20V8.82843C21 8.03278 20.6839 7.26972 20.1213 6.70711L15.2929 1.87868C14.7303 1.31607 13.9672 1 13.1716 1H6ZM5 4C5 3.44772 5.44772 3 6 3H12V8C12 9.10457 12.8954 10 14 10H19V20C19 20.5523 18.5523 21 18 21H6C5.44772 21 5 20.5523 5 20V4ZM18.5858 8L14 3.41421V8H18.5858Z"
                                class="dark:fill-gray-300 fill-gray-700"
                              />
                            </svg>
                          </td>
                          <td class="flex-1 flex flex-col justify-center py-2">
                            <span class="dark:text-gray-200">Öğrenci Belgesi</span>
                            <span class="text-xs dark:text-gray-400"
                              >E-devlet onaylı öğrenci belgenizi yükleyin</span
                            >
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
                <div class="flex justify-end">
                  <button
                    class="border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
                  >
                    Ekle
                  </button>
                </div>
              </div>

              <div v-if="formFieldAddingType === 'NewField'" class="mx-1 my-1">
                <div class="flex flex-col 2xl:flex-row justify-between w-full">
                  <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
                    <label
                      for="organizaitonName"
                      class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                      >Belge Adı</label
                    >
                    <input
                      type="text"
                      name="organizaitonName"
                      id="organizaitonName"
                      autocomplete="off"
                      class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
                    />
                  </div>
                  <div class="flex-1 flex flex-col my-2 items-start 2xl:ml-3">
                    <label for="postingName" class="text-sm my-1 dark:text-gray-300 text-gray-600"
                      >Belge Tipi</label
                    >
                    <input
                      type="text"
                      name="postingName"
                      id="postingName"
                      autocomplete="off"
                      class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
                    />
                  </div>
                </div>
                <div class="flex flex-col 2xl:flex-row justify-between w-full">
                  <div class="flex-1 flex flex-col my-2 items-start 2xl:mr-3">
                    <label
                      for="description"
                      class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                      >Açıklama</label
                    >
                    <textarea
                      type="text"
                      name="description"
                      id="description"
                      autocomplete="off"
                      class="w-full border outline-none rounded-md py-1.5 px-2 min-h-20 dark:border-gray-700 dark:bg-gray-900/50 text-sm border-gray-200"
                    ></textarea>
                  </div>
                  <div class="flex-1 flex flex-col my-2 items-start 2xl:ml-3">
                    <label class="w-full text-sm my-1 dark:text-gray-300 text-gray-600"
                      >Ayarlar</label
                    >
                    <div class="my-1">
                      <label class="inline-flex items-center cursor-pointer">
                        <input type="checkbox" value="" class="sr-only peer" />
                        <div
                          class="relative w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full rtl:peer-checked:after:-translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:start-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600 dark:peer-checked:bg-blue-600"
                        ></div>
                        <span class="ms-3 text-sm font-medium dark:text-gray-300 text-gray-700"
                          >Zorunlu Mu?</span
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
                          >Onaylama Gerekli Mi?</span
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
                          >Doğrulama Ekle?</span
                        >
                      </label>
                    </div>
                  </div>
                </div>
                <div class="flex justify-end">
                  <button
                    class="border rounded-md py-1 px-5 mr-5 mb-2 border-blue-600 cursor-pointer hover:bg-blue-700 text-gray-700 dark:text-gray-200 hover:text-gray-50 select-none"
                  >
                    Ekle
                  </button>
                </div>
              </div>
            </div>
            <div class="my-2">
              <label class="w-full text-normal mb-2 dark:text-gray-300 text-gray-800"
                >Form Alanları</label
              >
              <table class="w-full">
                <thead></thead>
                <tbody class="divide-y dark:divide-gray-700 divide-gray-200">
                  <tr v-for="(row, index) in rows" :key="index" class="flex">
                    <td class="w-10 py-3 flex items-center">
                      <label class="inline-flex items-center cursor-pointer select-none group">
                        <input type="checkbox" v-model="selectedRows[index]" class="sr-only" />
                        <span
                          class="w-5 h-5 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                        >
                          <svg
                            v-if="selectedRows[index]"
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
                    <td class="w-14 py-3 flex items-center">
                      <svg
                        class="size-7"
                        viewBox="0 0 24 24"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <g id="Interface / Checkbox_Check">
                          <path
                            id="Vector"
                            d="M8 12L11 15L16 9M4 16.8002V7.2002C4 6.08009 4 5.51962 4.21799 5.0918C4.40973 4.71547 4.71547 4.40973 5.0918 4.21799C5.51962 4 6.08009 4 7.2002 4H16.8002C17.9203 4 18.4796 4 18.9074 4.21799C19.2837 4.40973 19.5905 4.71547 19.7822 5.0918C20 5.5192 20 6.07899 20 7.19691V16.8036C20 17.9215 20 18.4805 19.7822 18.9079C19.5905 19.2842 19.2837 19.5905 18.9074 19.7822C18.48 20 17.921 20 16.8031 20H7.19691C6.07899 20 5.5192 20 5.0918 19.7822C4.71547 19.5905 4.40973 19.2842 4.21799 18.9079C4 18.4801 4 17.9203 4 16.8002Z"
                            class="dark:stroke-gray-300 stroke-gray-700"
                            stroke-width="2"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                          />
                        </g>
                      </svg>
                    </td>
                    <td class="flex-1 flex flex-col justify-center py-3">
                      <span class="dark:text-gray-200">Öğrenci Belgesi</span>
                      <span class="text-xs dark:text-gray-400"
                        >E-devlet onaylı öğrenci belgenizi yükleyin</span
                      >
                    </td>

                    <td class="py-3 px-2 flex items-center">
                      <button class="cursor-pointer pr-1 group">
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
                      <button class="cursor-pointer pr-1">
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

                  <tr class="flex">
                    <td class="w-10 py-3">
                      <label class="inline-flex items-center cursor-pointer select-none group">
                        <input type="checkbox" v-model="modelValue" class="sr-only" />
                        <span
                          class="w-5 h-5 flex items-center justify-center rounded border transition-colors border-gray-400 dark:border-gray-600 bg-gray-50 dark:bg-gray-900/50 group-hover:border-blue-500"
                        >
                          <svg
                            v-if="modelValue"
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
                    <td class="w-14 py-3">
                      <svg
                        class="size-7"
                        viewBox="0 0 24 24"
                        fill="none"
                        xmlns="http://www.w3.org/2000/svg"
                      >
                        <path
                          fill-rule="evenodd"
                          clip-rule="evenodd"
                          d="M6 1C4.34315 1 3 2.34315 3 4V20C3 21.6569 4.34315 23 6 23H18C19.6569 23 21 21.6569 21 20V8.82843C21 8.03278 20.6839 7.26972 20.1213 6.70711L15.2929 1.87868C14.7303 1.31607 13.9672 1 13.1716 1H6ZM5 4C5 3.44772 5.44772 3 6 3H12V8C12 9.10457 12.8954 10 14 10H19V20C19 20.5523 18.5523 21 18 21H6C5.44772 21 5 20.5523 5 20V4ZM18.5858 8L14 3.41421V8H18.5858Z"
                          class="dark:fill-gray-300 fill-gray-700"
                        />
                      </svg>
                    </td>
                    <td class="flex-1 flex flex-col py-3">
                      <span class="dark:text-gray-200">KVKK Onayı</span>
                      <span class="text-xs dark:text-gray-400"
                        >Kişisel verilen işlenmesine ilişkin aydınlatma metnini indirmek için
                        tıklayın.</span
                      >
                    </td>

                    <td class="py-3 px-2 w-fit">
                      <button class="cursor-pointer pr-1 group">
                        <svg
                          class="size-6 dark:fill-gray-400 dark:group-hover:fill-blue-600"
                          viewBox="0 0 24 24"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M18.111,2.293,9.384,11.021a.977.977,0,0,0-.241.39L8.052,14.684A1,1,0,0,0,9,16a.987.987,0,0,0,.316-.052l3.273-1.091a.977.977,0,0,0,.39-.241l8.728-8.727a1,1,0,0,0,0-1.414L19.525,2.293A1,1,0,0,0,18.111,2.293ZM11.732,13.035l-1.151.384.384-1.151L16.637,6.6l.767.767Zm7.854-7.853-.768.767-.767-.767.767-.768ZM3,5h8a1,1,0,0,1,0,2H4V20H17V13a1,1,0,0,1,2,0v8a1,1,0,0,1-1,1H3a1,1,0,0,1-1-1V6A1,1,0,0,1,3,5Z"
                          />
                        </svg>
                      </button>
                      <button class="cursor-pointer pr-1">
                        <svg
                          class="size-6 group"
                          viewBox="0 0 24 24"
                          fill="none"
                          xmlns="http://www.w3.org/2000/svg"
                        >
                          <path
                            d="M20.5001 6H3.5"
                            class="dark:stroke-gray-400 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M18.8332 8.5L18.3732 15.3991C18.1962 18.054 18.1077 19.3815 17.2427 20.1907C16.3777 21 15.0473 21 12.3865 21H11.6132C8.95235 21 7.62195 21 6.75694 20.1907C5.89194 19.3815 5.80344 18.054 5.62644 15.3991L5.1665 8.5"
                            class="dark:stroke-gray-400 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M9.5 11L10 16"
                            class="dark:stroke-gray-400 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M14.5 11L14 16"
                            class="dark:stroke-gray-400 dark:group-hover:stroke-red-600"
                            stroke-width="1.5"
                            stroke-linecap="round"
                          />
                          <path
                            d="M6.5 6C6.55588 6 6.58382 6 6.60915 5.99936C7.43259 5.97849 8.15902 5.45491 8.43922 4.68032C8.44784 4.65649 8.45667 4.62999 8.47434 4.57697L8.57143 4.28571C8.65431 4.03708 8.69575 3.91276 8.75071 3.8072C8.97001 3.38607 9.37574 3.09364 9.84461 3.01877C9.96213 3 10.0932 3 10.3553 3H13.6447C13.9068 3 14.0379 3 14.1554 3.01877C14.6243 3.09364 15.03 3.38607 15.2493 3.8072C15.3043 3.91276 15.3457 4.03708 15.4286 4.28571L15.5257 4.57697C15.5433 4.62992 15.5522 4.65651 15.5608 4.68032C15.841 5.45491 16.5674 5.97849 17.3909 5.99936C17.4162 6 17.4441 6 17.5 6"
                            class="dark:stroke-gray-400 dark:group-hover:stroke-red-600"
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
                              class="dark:stroke-gray-400 dark:group-hover:stroke-cyan-600"
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
  </div>
</template>
<style scoped></style>
