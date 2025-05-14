<script setup lang="ts">
import { ref, Ref } from "vue";
import { TenantCreateModel } from "../models/tenant-create.model";
import tenantService from "../services/tenant.service";

const emit = defineEmits(["onClose"]);

const request: Ref<TenantCreateModel> = ref({
  name: "",
  code: undefined,
});

const createTenant = async () => {
  const res = await tenantService.createTenant(request.value);
  if (res) {
    window.location.reload();
  }
};

function closeModal() {
  emit("onClose");
}
</script>
<template>
  <div class="fixed z-[100] flex justify-center items-center w-full h-full bg-gray-500/60">
    <div class="rounded-md bg-gray-50 dark:bg-gray-900 min-w-[40rem] px-5 py-8">
      <span class="text-xl dark:text-gray-100 font-semibold">Kurum Oluştur</span>
      <div class="flex flex-row mt-5">
        <div class="flex flex-col w-full">
          <div class="flex flex-col mb-3">
            <label for="tenantName" class="text-sm dark:text-gray-400 text-gray-700 mb-1.5"
              >Ad</label
            >
            <input
              type="text"
              name="tenantName"
              id="tenantName"
              v-model="request.name"
              autocomplete="off"
              placeholder="Five LTD. ŞTİ."
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm text-gray-800 dark:text-gray-200 border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            />
          </div>
          <div class="flex flex-col mb-3">
            <label for="tenantCode" class="text-sm dark:text-gray-400 text-gray-700 mb-1.5"
              >Kod</label
            >
            <input
              type="text"
              name="tenantCode"
              id="tenantCode"
              v-model="request.code"
              autocomplete="off"
              placeholder="FV"
              class="w-full border outline-none rounded-md py-1.5 px-2 dark:border-gray-700 dark:bg-gray-900/50 text-sm text-gray-800 dark:text-gray-200 border-gray-200 dark:focus:border-indigo-600 focus:border-indigo-600"
            />
          </div>
          <div class="flex flex-row justify-end mt-5">
            <button
              class="px-2 py-1 border rounded-md border-gray-200 dark:border-gray-700 text-gray-700 dark:text-gray-300 cursor-pointer hover:bg-gray-400/10 mr-5 text-sm"
              @click="closeModal()"
            >
              İptal
            </button>
            <button
              class="px-2 py-1 border rounded-md border-blue-600 dark:border-blue-700 text-gray-700 dark:text-gray-300 cursor-pointer hover:bg-blue-600 hover:text-white text-sm"
              @click="createTenant()"
            >
              Oluştur
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
