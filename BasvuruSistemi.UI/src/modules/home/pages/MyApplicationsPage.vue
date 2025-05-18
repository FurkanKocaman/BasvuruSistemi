<script setup lang="ts">
import { useDropdown } from "@/modules/management/composables/useDropdown";
import ApplicationsTable from "../components/ApplicationsTable.vue";

const statusFilterDropdown = useDropdown();

const options = [
  { name: "Tümü" },
  { name: "Onaylandı" },
  { name: "Reddedildi" },
  { name: "Beklemede" },
  { name: "Geri Çekildi" },
];
</script>

<template>
  <div class="px-10 pt-26 pb-10">
    <h1 class="text-2xl font-bold text-gray-900 dark:text-white mb-6">Başvurularım</h1>

    <!-- Başvuru Filtreleri -->
    <div
      class="bg-gray-50 dark:bg-gray-800/40 rounded-lg p-4 mb-6 border border-gray-200 dark:border-gray-700"
    >
      <div class="flex flex-col md:flex-row md:items-center md:justify-start gap-4">
        <!-- Arama -->
        <div class="w-full md:w-1/3">
          <label
            for="search"
            class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
            >Arama</label
          >
          <div class="relative">
            <div class="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg
                class="h-5 w-5 text-gray-400"
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 20 20"
                fill="currentColor"
              >
                <path
                  fill-rule="evenodd"
                  d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                  clip-rule="evenodd"
                />
              </svg>
            </div>
            <input
              id="search"
              type="text"
              placeholder="İş başlığı ara"
              class="pl-10 w-full rounded-md border-gray-200 dark:border-gray-700 outline-none dark:focus:border-indigo-600 focus:border-indigo-600 border py-1.5 text-gray-800 dark:text-gray-100"
            />
          </div>
        </div>

        <!-- Durum Filtresi -->
        <div class="w-full md:w-1/3">
          <label
            for="status"
            class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
            >Durum</label
          >
          <input
            type="text"
            name="formTeplate"
            id="formTeplate"
            :ref="statusFilterDropdown.inputRef"
            v-model="statusFilterDropdown.selectedLabel.value"
            @focus="statusFilterDropdown.handleFocus"
            @blur="statusFilterDropdown.handleBlur"
            readonly
            placeholder="Durum seçin..."
            autocomplete="off"
            class="w-[50%] border outline-none rounded-md py-2 px-2 text-gray-700 dark:text-gray-200 dark:border-gray-700 text-sm border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600 cursor-pointer"
          />
          <div
            v-if="statusFilterDropdown.isOpen.value"
            class="absolute w-fit text-gray-700 dark:text-gray-200 bg-gray-50 dark:bg-gray-800 border border-gray-200 dark:border-gray-700 rounded shadow z-10 max-h-60 overflow-auto"
          >
            <div
              v-for="option in options"
              :key="option.name"
              @mousedown.prevent="statusFilterDropdown.selectOption(option.name)"
              class="px-4 py-2 hover:bg-gray-300/30 dark:hover:bg-gray-700/40 cursor-pointer text-sm"
            >
              {{ option.name }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Başvurular Tablosu -->
    <div
      class="bg-gray-50 dark:bg-gray-800/40 rounded-lg shadow border border-gray-200 dark:border-gray-700 overflow-hidden"
    >
      <ApplicationsTable />
    </div>

    <!-- Yeni Başvuru Butonu -->
    <div class="mt-6 flex justify-center">
      <router-link
        to="/jobs"
        class="inline-flex items-center px-4 py-2 border border-transparent rounded-md shadow-sm text-sm font-medium text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-5 w-5 mr-2"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M12 4v16m8-8H4"
          />
        </svg>
        Yeni Başvuru Yap
      </router-link>
    </div>
  </div>
</template>
