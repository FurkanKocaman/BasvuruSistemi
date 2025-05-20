<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { Profile } from "../models/profile.model";
import { UserUpdatemodel } from "../models/user-update.model";
import profileService from "../services/profile.service";

const props = defineProps<{
  modelValue: boolean;
  profile: Profile;
}>();

const emit = defineEmits(["update:modelValue", "profileUpdated"]);

const formData = ref<UserUpdatemodel>({
  id: "",
  firstName: "",
  lastName: "",
  email: "",
  phone: "",
});

onMounted(() => {
  loadFormData();
});

watch(
  () => props.profile,
  () => {
    loadFormData();
  },
  { deep: true }
);

const loadFormData = () => {
  formData.value = {
    id: props.profile.id,
    firstName: props.profile.firstName,
    lastName: props.profile.lastName,
    birthOfDate: props.profile.birthOfDate,
    nationality: props.profile.nationality,
    email: props.profile.email!,
    phone: props.profile.phone,
    tckn: props.profile.tckn,
  };
};

const saveProfile = async () => {
  try {
    const updatedProfile = {
      ...props.profile,
      firstName: formData.value.firstName,
      lastName: formData.value.lastName,
      birthOfDate: formData.value.birthOfDate,
      nationality: formData.value.nationality,
      email: formData.value.email,
      phone: formData.value.phone,
      tckn: formData.value.tckn,
    };
    const res = await profileService.updateUser(updatedProfile);

    if (res) {
      emit("update:modelValue", false);
      emit("profileUpdated", updatedProfile);
    }
  } catch (error) {
    console.error("Profil güncellenirken hata oluştu:", error);
    alert("Profil güncellenirken bir hata oluştu. Lütfen tekrar deneyin.");
  }
};
</script>

<template>
  <div
    class="fixed inset-0 bg-gradient-to-br from-blue-500/20 to-purple-600/30 backdrop-blur-sm flex items-center justify-center z-50"
    v-if="modelValue"
  >
    <div
      class="bg-white dark:bg-gray-800 rounded-lg shadow-xl w-full max-w-2xl max-h-[90vh] overflow-y-auto"
    >
      <div
        class="p-6 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center"
      >
        <h3 class="text-xl font-semibold text-gray-800 dark:text-white">
          Profil Bilgilerini Düzenle
        </h3>
        <button
          @click="$emit('update:modelValue', false)"
          class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-6 w-6"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
        </button>
      </div>

      <div class="p-6">
        <form @submit.prevent="saveProfile">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Ad
              </label>
              <input
                type="text"
                v-model="formData.firstName"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Adınız"
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Soyad
              </label>
              <input
                type="text"
                v-model="formData.lastName"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Soyadınız"
              />
            </div>
            <div>
              <label
                for="email"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
              >
                E-posta
              </label>
              <input
                id="email"
                type="email"
                v-model="formData.email"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="user@example.com"
              />
            </div>

            <div>
              <label
                for="phone"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
              >
                Telefon
              </label>
              <input
                id="phone"
                type="phone"
                v-model="formData.phone"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder=""
              />
            </div>
            <div>
              <label
                for="phone"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
              >
                Doğum Tarihi
              </label>
              <input
                id="phone"
                type="date"
                v-model="formData.birthOfDate"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder=""
              />
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                TC Kimlik No
              </label>
              <input
                type="text"
                v-model="formData.tckn"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="TC Kimlik Numaranız"
                maxlength="11"
              />
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Ülke
              </label>
              <input
                type="text"
                v-model="formData.nationality"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
              />
            </div>
          </div>

          <div class="mt-6 flex justify-end space-x-3">
            <button
              type="button"
              @click="$emit('update:modelValue', false)"
              class="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md text-gray-700 dark:text-gray-300 hover:bg-gray-50 dark:hover:bg-gray-700"
            >
              İptal
            </button>
            <button
              type="submit"
              class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors"
            >
              Kaydet
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>
