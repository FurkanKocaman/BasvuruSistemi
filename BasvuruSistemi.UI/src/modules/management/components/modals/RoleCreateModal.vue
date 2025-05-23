<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
    <div
      class="bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-xl w-full p-6 animate-fadeIn"
      @click.stop
    >
      <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-200 mb-4">
        {{ request.id ? "Rol Düzenle" : "Rol Oluştur" }}
      </h3>

      <div class="my-3 text-gray-800 dark:text-gray-50">
        <label for="name">Rol Adı</label>
        <input
          type="text"
          name="name"
          id="name"
          autocomplete="off"
          v-model="request.name"
          class="outline-none w-full border border-gray-200 dark:border-gray-800 bg-transparent focus:border-indigo-600 rounded-md px-1.5 py-1 mt-1"
        />
      </div>
      <div class="my-3 text-gray-800 dark:text-gray-50">
        <label for="description">Açıklama</label>
        <input
          type="text"
          name="description"
          id="description"
          autocomplete="off"
          v-model="request.description"
          class="outline-none w-full border border-gray-200 dark:border-gray-800 bg-transparent focus:border-indigo-600 rounded-md px-1.5 py-1 mt-1"
        />
      </div>
      <div class="text-gray-800 dark:text-gray-50 my-3">
        <span>Yetkiler</span>
        <div class="grid grid-cols-2 gap-2 mt-2">
          <div
            v-for="(claim, index) in claimOptions"
            :key="claim.value"
            class="flex items-center space-x-2"
          >
            <input
              type="checkbox"
              :id="`claim-${index}`"
              :value="claim.value"
              v-model="selectedClaims"
              class="form-checkbox"
            />
            <label :for="`claim-${index}`">{{ claim.label }}</label>
          </div>
        </div>
      </div>

      <div class="mt-6 flex justify-end">
        <button
          class="px-4 py-2 text-sm bg-gray-200 dark:bg-gray-700 dark:text-white text-gray-800 rounded hover:bg-gray-400/50 cursor-pointer"
          @click="close"
        >
          Kapat
        </button>
        <button
          class="px-4 py-2 text-sm bg-sky-600 text-gray-50 rounded hover:bg-sky-500 ml-3 cursor-pointer"
          @click="request.id ? updateRole() : createRole()"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, defineExpose } from "vue";
import { RoleCreateModel } from "../../models/role-create.model";
import roleService from "../../services/role.service";
import { claimOptions } from "@/models/constants/claim-type";

const emit = defineEmits<{
  (e: "addRole", role: RoleCreateModel): void;
  (e: "updateRole", role: RoleCreateModel): void;
}>();

const request = ref<RoleCreateModel>({
  name: "",
  permissions: [],
});

const selectedClaims = ref<string[]>([]);

const visible = ref(false);

function open(role?: RoleCreateModel) {
  visible.value = true;
  if (role) {
    request.value = role;
    selectedClaims.value = role.permissions;
  }
}

async function createRole() {
  request.value.permissions = selectedClaims.value;
  const res = await roleService.createRole(request.value);
  if (res) {
    request.value.id = res;
    emit("addRole", request.value);
    close();
  }
}
async function updateRole() {
  request.value.permissions = selectedClaims.value;
  const res = await roleService.updateRole(request.value);
  if (res) {
    request.value.id = res;
    emit("updateRole", request.value);
    close();
  }
}

function close() {
  visible.value = false;
}

defineExpose({ open });
</script>

<style scoped>
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.95);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
.animate-fadeIn {
  animation: fadeIn 0.2s ease-out;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
