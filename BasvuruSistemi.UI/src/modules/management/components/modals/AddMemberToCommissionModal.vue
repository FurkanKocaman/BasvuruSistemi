<template>
  <div v-if="visible" class="fixed inset-0 z-50 flex items-center justify-center bg-black/50">
    <div
      class="bg-white dark:bg-gray-900 rounded-lg shadow-xl max-w-md w-full p-6 animate-fadeIn"
      @click.stop
    >
      <h3 class="text-lg font-semibold text-gray-900 dark:text-gray-200 mb-4">
        Komisyona üye ekle
      </h3>

      <div class="space-y-2 flex flex-col text-gray-800 dark:text-gray-200 text-sm">
        <label for="email">E-posta</label>
        <input
          id="email"
          name="email"
          type="text"
          autocomplete="off"
          v-model="request.email"
          class="outline-none bg-transparent border rounded-md border-gray-200 dark:border-gray-800 px-2 py-1"
          :disabled="isUpdate"
        />
      </div>
      <div class="space-y-2 flex flex-col text-gray-800 dark:text-gray-200 my-3 text-sm">
        <label for="role">Rol</label>
        <input
          id="role"
          name="role"
          type="text"
          autocomplete="off"
          v-model="request.role"
          class="outline-none bg-transparent border rounded-md border-gray-200 dark:border-gray-800 px-2 py-1"
        />
      </div>

      <div class="mt-6 flex justify-end">
        <button
          class="px-4 py-2 text-sm bg-gray-200 dark:bg-gray-700 dark:text-white text-gray-800 rounded hover:bg-gray-300/40 cursor-pointer"
          @click="close"
        >
          Kapat
        </button>
        <button
          class="px-4 py-2 text-sm bg-sky-600 text-gray-50 rounded hover:bg-sky-500 ml-3 cursor-pointer"
          @click="onSubmit()"
        >
          Kaydet
        </button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, defineExpose } from "vue";
import { AddMemberToCommissionModel } from "../../models/add-member-to-commission.model";
import commissionMemberService from "../../services/commission-member.service";

const request = ref<AddMemberToCommissionModel>({
  commissionId: "",
  email: "",
  role: "",
});
const resolveFn = ref(null);

const isUpdate = ref(false);

const onSubmit = async () => {
  if (isUpdate) {
    const res = await commissionMemberService.addMemberToCommission(request.value);
    if (res) {
      resolveFn.value?.(true);
      visible.value = false;
    }
  } else {
    const res = await commissionMemberService.addMemberToCommission(request.value);
    if (res) {
      resolveFn.value?.(true);
      visible.value = false;
    }
  }
};

const visible = ref(false);

function open(commissionId: string, existingMember?: AddMemberToCommissionModel) {
  request.value.commissionId = commissionId;
  visible.value = true;
  isUpdate.value = false;
  if (existingMember) {
    request.value.email = existingMember.email;
    request.value.role = existingMember.role;
    isUpdate.value = true;
  }
  return new Promise((resolve) => {
    resolveFn.value = resolve;
  });
}

function close() {
  visible.value = false;
  resolveFn.value?.(false);
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
