<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import profileService from "../services/profile.service";
import { Address } from "@/models/entities/address-model";
import { AddressUpdateModel } from "../models/address-update.model";

const props = defineProps<{
  modelValue: boolean;
  address?: Address;
}>();

const emit = defineEmits(["update:modelValue", "save"]);

const isEditing = ref(false);

const formData = ref<AddressUpdateModel>({
  id: undefined,
  name: "",
  country: "Turkey",
});

onMounted(() => {
  resetFormData();
  loadFormData();
});

watch(
  () => props.address,
  () => {
    loadFormData();
  },
  { deep: true }
);

const loadFormData = () => {
  if (props.address) {
    console.log(props.address);
    isEditing.value = true;
    formData.value = {
      id: props.address.id,
      name: props.address.name,
      country: props.address.country,
      city: props.address.city,
      district: props.address.district,
      street: props.address.street,
      fullAddress: props.address.fullAddress,
      postalCode: props.address.postalCode,
    };
  } else {
    isEditing.value = false;
    resetFormData();
  }
};

const resetFormData = () => {
  formData.value = {
    id: undefined,
    name: "",
    country: "Turkey",
    city: undefined,
    district: undefined,
    street: undefined,
    fullAddress: undefined,
    postalCode: undefined,
  };
};

const saveAddress = async () => {
  if (props.address) {
    await profileService.updateAddress(formData.value.id!, formData.value);
    const address: Address = {
      id: formData.value.id!,
      name: formData.value.name,
      country: formData.value.country,
      city: formData.value.city,
      district: formData.value.district,
      street: formData.value.street,
      fullAddress: formData.value.fullAddress,
      postalCode: formData.value.postalCode,
    };
    onSave(address);
    onClose();
  } else {
    // Create
    const res = await profileService.createAddress(formData.value);
    console.log("Address create res", res);
    onSave(res);
    onClose();
  }
};

function onClose() {
  emit("update:modelValue", false);
}

function onSave(address: Address) {
  emit("save", address);
}
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
          {{ isEditing ? "Adres Bilgisini Düzenle" : "Yeni Adres Bilgisi Ekle" }}
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
        <form @submit.prevent="saveAddress">
          <div class="space-y-4">
            <div>
              <label
                for="name"
                class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
              >
                Adres için görüntülenecek ad
              </label>
              <input
                id="name"
                type="text"
                v-model="formData.name"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Ev"
                required
              />
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  İl
                </label>
                <input
                  type="text"
                  v-model="formData.city"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  İlçe
                </label>
                <input
                  type="text"
                  v-model="formData.district"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                />
              </div>
            </div>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Mahalle
                </label>
                <input
                  type="text"
                  v-model="formData.street"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Ulke
                </label>
                <input
                  type="text"
                  v-model="formData.country"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder="Turkey"
                />
              </div>
            </div>
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Posta Kodu
                </label>
                <input
                  type="text"
                  v-model="formData.postalCode"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder="61000"
                />
              </div>
            </div>
            <div class="grid gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Tam Adres
                </label>
                <input
                  type="text"
                  v-model="formData.fullAddress"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder=""
                />
              </div>
            </div>
          </div>

          <div class="mt-6 flex justify-end space-x-3">
            <button
              type="button"
              @click.stop="$emit('update:modelValue', false)"
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
