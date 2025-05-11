<script setup lang="ts">
import { ref } from "vue";
import { FormFieldDefinition } from "../../models/form-field.model";
import * as icons from "lucide-vue-next";
import { getFieldTypeOptionByValue } from "@/models/constants/field-type";

const props = defineProps<{ fields: FormFieldDefinition[] }>();

const emit = defineEmits<{
  (e: "remove:field", payload: string): void;
}>();

const removeField = (label: string) => {
  emit("remove:field", label);
};

const selectedRows = ref([]);
</script>

<template>
  <div class="my-2">
    <label class="w-full text-normal mb-2 dark:text-gray-300 text-gray-800">Form Alanları</label>
    <table class="w-full">
      <thead></thead>
      <tbody class="divide-y dark:divide-gray-700 divide-gray-200">
        <tr v-for="(row, index) in props.fields" :key="index" class="flex">
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
                  <path stroke-linecap="round" stroke-linejoin="round" d="M5 13l4 4L19 7" />
                </svg>
              </span>
            </label>
          </td>
          <td class="w-14 py-3 flex items-center">
            <component
              :is="icons[getFieldTypeOptionByValue(row.type)?.icon]"
              class="w-4 h-4 text-gray-500"
            />
          </td>
          <td class="flex-1 flex flex-col justify-center py-3">
            <span class="dark:text-gray-200">{{ row.label }}</span>
            <span class="text-xs dark:text-gray-400">{{ row.description }}</span>
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
            <button class="cursor-pointer pr-1" @click="removeField(row.label)">
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
</template>
