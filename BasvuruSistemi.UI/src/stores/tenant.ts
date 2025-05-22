import { Tenant } from "@/models/entities/tenant.model";
import { defineStore } from "pinia";
import { ref, computed } from "vue";
import Cookies from "js-cookie";
import { refreshJwtByTenant } from "@/services/tenant.service";

const TENANT_COOKIE_KEY = "tenantId";

export const useTenantStore = defineStore("tenant", () => {
  const tenant = ref<Tenant | null>(null);

  const tenantId = computed(() => tenant.value?.id || Cookies.get("tenantId"));

  async function setTenant(payload: Tenant) {
    tenant.value = payload;

    Cookies.set(TENANT_COOKIE_KEY, payload.id.toString(), { expires: 1 });
    await refreshJwtByTenant();
  }

  function clearTenant() {
    tenant.value = null;
    Cookies.remove(TENANT_COOKIE_KEY);
  }

  return {
    tenant,
    tenantId,
    setTenant,
    clearTenant,
  };
});
