<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
    <div
      class="bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-md w-full p-6 animate-fadeIn"
      @click.stop
    >
      <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-200 mb-4">
        {{ user.firstName }} {{ user.lastName }} kullanıcısının rolleri
      </h3>

      <div class="space-y-2">
        <div v-if="userRoles.length === 0" class="text-sm text-gray-500">
          Kullanıcının henüz bir rolü yok.
        </div>
        <div
          v-for="role in userRoles"
          :key="role.id"
          class="flex justify-between items-center text-sm text-gray-700 dark:text-gray-300 bg-gray-100 dark:bg-gray-800 px-2 py-1 rounded"
        >
          <span>{{ role.name }}</span>
          <button @click="removeRole(role.id)" class="text-red-500 hover:text-red-700 text-xs">
            ✕
          </button>
        </div>
      </div>

      <div class="mt-4">
        <div class="flex items-center relative">
          <label for="role" class="text-sm mb-1 text-gray-700 dark:text-gray-300"
            >Yeni Rol Ekle</label
          >
          <Info
            @mouseover="showInfo = true"
            @mouseleave="showInfo = false"
            class="text-lg ml-1 text-blue-600 p-0.5"
          />
          <transition
            enter-active-class="transition-opacity duration-200 ease-out"
            enter-from-class="opacity-0 "
            enter-to-class="opacity-100 translate-y-0"
            leave-active-class="transition-opacity duration-150 ease-in"
            leave-from-class="opacity-100 translate-y-0"
            leave-to-class="opacity-0 "
          >
            <div
              v-if="showInfo"
              @mouseover="showInfo = true"
              @mouseleave="showInfo = false"
              class="absolute left-1/2 -translate-x-1/2 mt-2 w-48 p-2 bg-gray-100 dark:bg-gray-800 text-gray-600 dark:text-gray-300 text-sm rounded shadow-lg z-10"
            >
              Seçilen kullanıcıya rol atandığında e-posta gönderilir ve eğer kullanıcı rolü kabul
              ederse rol atanır.
            </div>
          </transition>
        </div>
        <select
          id="role"
          v-model="selectedRole"
          class="w-full border border-gray-200 dark:border-gray-700 px-2 py-1 rounded text-sm text-gray-700 dark:text-gray-200"
        >
          <option disabled :value="undefined" class="dark:bg-gray-900 bg-gray-50 outline-none">
            Bir rol seçin
          </option>
          <option
            v-for="role in availableRoles"
            :key="role.id"
            :value="role"
            :disabled="userRoles.includes(role)"
            class="dark:bg-gray-900 bg-gray-50 outline-none"
          >
            {{ role.name }}
          </option>
        </select>
      </div>
      <div v-if="selectedRole?.name == 'UnitManager'" class="mt-4">
        <div class="flex items-center relative">
          <label for="role" class="text-sm mb-1 text-gray-700 dark:text-gray-300">Birim</label>
        </div>
        <select
          id="role"
          v-model="request.unitId"
          class="w-full border border-gray-200 dark:border-gray-700 px-2 py-1 rounded text-sm text-gray-700 dark:text-gray-200"
        >
          <option disabled :value="undefined" class="dark:bg-gray-900 bg-gray-50 outline-none">
            Bir birim seçin
          </option>
          <option
            v-for="unit in units"
            :key="unit.id"
            :value="unit.id"
            class="dark:bg-gray-900 bg-gray-50 outline-none"
          >
            {{ unit.name }}
          </option>
        </select>
      </div>

      <div class="mt-6 flex justify-end">
        <button
          class="px-4 py-2 text-sm bg-gray-200 dark:bg-gray-700 dark:text-white text-gray-800 rounded hover:bg-gray-300 cursor-pointer"
          @click="close"
        >
          Kapat
        </button>
        <button
          class="px-4 py-2 text-sm bg-sky-600 text-gray-50 rounded hover:bg-sky-500 ml-3 cursor-pointer"
          @click="assignRole"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, defineExpose, watch } from "vue";
import { RoleDto, UserSummariesModel } from "../models/user-summaries.model";
import roleService from "@/modules/management/services/role.service";
import { Info } from "lucide-vue-next";
import unitService from "../services/unit.service";
import { Unit } from "../models/unit-node.model";
import { RoleAssignmentCreateModel } from "../models/role-assignment-create.model";

const user = ref<UserSummariesModel>({
  id: "",
  firstName: "",
  lastName: "",
  roles: [],
  createdAt: "",
});
const request = ref<RoleAssignmentCreateModel>({
  inviteeId: "",
  roleId: "",
});
const userRoles = ref<RoleDto[]>([]);
const units = ref<Unit[]>([]);

const showInfo = ref(false);

const visible = ref(false);
const selectedRole = ref<RoleDto | undefined>(undefined);
const availableRoles = ref<RoleDto[]>([]);

async function fetchRoles() {
  const roles = await roleService.getAllRoles();
  availableRoles.value = roles;
}

async function assignRole() {
  if (!selectedRole.value) return;
  request.value.roleId = selectedRole.value.id;
  await roleService.createRoleAssignment(request.value);
  selectedRole.value = undefined;
  close();
}

async function removeRole(roleId: string) {
  // await removeRoleFromUser(props.user.id, role);
  userRoles.value = userRoles.value.filter((r) => r.id !== roleId);
}

function open(userData: UserSummariesModel) {
  visible.value = true;
  user.value = userData;
  userRoles.value = [...userData.roles];
  selectedRole.value = undefined;
  request.value.inviteeId = userData.id;
  fetchRoles();
}

function close() {
  visible.value = false;
}

async function getUnits() {
  const res = await unitService.getUnitsByTenant();
  if (res) units.value = res;
  console.log("Res", res);
}

watch(selectedRole, () => {
  if (selectedRole.value?.name == "UnitManager") {
    getUnits();
  }
});

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
