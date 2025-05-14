<script setup lang="ts">
import { Tenant } from "@/models/entities/tenant.model";
import { fetchTenants } from "@/services/tenant.service";
import { useTenantStore } from "@/stores/tenant";
import { onMounted, ref, Ref } from "vue";
import { useRouter } from "vue-router";

import HeaderComponent from "@/modules/home/components/HeaderComponent.vue";
import TenantCreateModal from "../modals/TenantCreateModal.vue";

const tenantStore = useTenantStore();
const router = useRouter();

const isModalOpen = ref(false);

const tenants: Ref<Tenant[] | undefined> = ref(undefined);

onMounted(async () => {
  const storedTenantId = tenantStore.tenantId;

  if (tenantStore.tenant) {
    router.push("/management");
  } else if (storedTenantId) {
    tenants.value = await fetchTenants();
    if (tenants.value) {
      const tenant = tenants.value.find((t) => t.id.toString() === storedTenantId);
      if (tenant) {
        tenantStore.setTenant(tenant);
        router.push("/management");
        return;
      }
    }
  }

  // Tenant set edilmemişse liste göster
  tenants.value = await fetchTenants();
});
function setTenant(tenant: Tenant) {
  tenantStore.setTenant(tenant);
  if (tenantStore.tenant) {
    router.push("/management");
  }
}

const closeModal = () => {
  isModalOpen.value = false;
};
</script>

<template>
  <main class="w-full min-h-[100dvh] h-full overflow-auto">
    <TenantCreateModal v-if="isModalOpen" @on-close="closeModal" />
    <HeaderComponent />
    <div class="min-h-[100dvh] w-full h-full flex justify-center items-center px-10 pt-20 pb-10">
      <div
        class="min-w-sm dark:bg-gray-900/40 bg-gray-50 rounded-xl border dark:border-gray-800 border-gray-200"
      >
        <div
          class="border-b px-5 py-3 dark:border-gray-800 border-gray-200 flex items-center justify-between"
        >
          <span class="flex-1 text-center text-xl font-base dark:text-gray-50 text-gray-700"
            >Kurum Seç</span
          >
          <button
            class="px-2 py-1 border rounded-md border-gray-200 dark:border-gray-700 text-gray-700 dark:text-gray-300 cursor-pointer hover:bg-gray-400/10"
            @click="
              () => {
                isModalOpen = true;
              }
            "
          >
            Kurum oluştur
          </button>
        </div>
        <div class="px-5 py-3">
          <div
            v-for="tenant in tenants"
            :key="tenant.id"
            class="border rounded-md dark:border-gray-700 border-gray-200 px-3 py-2 text-center cursor-pointer dark:hover:bg-gray-700/20 hover:bg-gray-200/20"
            @click="setTenant(tenant)"
          >
            <span class="text-base text-gray-700 dark:text-gray-200">{{ tenant.name }}</span>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>
<style scoped></style>
