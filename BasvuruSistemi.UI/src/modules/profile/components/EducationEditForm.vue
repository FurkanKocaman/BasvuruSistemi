<script setup lang="ts">
import { ref, onMounted, watch } from "vue";
import { Education } from "../models/profile.model";
import { EducationCreateModel } from "../models/education-create.model";
import profileService from "../services/profile.service";

const props = defineProps<{
  modelValue: boolean;
  education?: Education;
}>();

const emit = defineEmits(["update:modelValue", "save"]);

// Düzenleme modunda mı yoksa yeni ekleme modunda mı?
const isEditing = ref(false);

// Form verisi
const formData = ref<EducationCreateModel>({
  institution: "",
  department: "",
  startDate: "",
});

// Eğitim verisini forma yükle
onMounted(() => {
  resetFormData();
  loadFormData();
});

watch(
  () => props.education,
  () => {
    loadFormData();
  },
  { deep: true }
);

const loadFormData = () => {
  if (props.education) {
    isEditing.value = true;
    formData.value = {
      id: props.education.id,
      institution: props.education.institution,
      department: props.education.department,
      startDate: props.education.startDate,
      graduationDate: props.education.graduationDate,
      gpa: props.education.gpa,
      degree: props.education.degree,
      description: props.education.description,
    };
  } else {
    isEditing.value = false;
    resetFormData();
  }
};

const resetFormData = () => {
  formData.value = {
    id: undefined,
    institution: "",
    department: "",
    startDate: "",
    graduationDate: undefined,
    gpa: undefined,
    degree: undefined,
    description: undefined,
  };
};

const saveEducation = async () => {
  if (props.education) {
    await profileService.updateEducation(formData.value);
    const education: Education = {
      id: formData.value.id!,
      institution: formData.value.institution,
      department: formData.value.department,
      degree: formData.value.degree,
      description: formData.value.description,
      startDate: formData.value.startDate,
      graduationDate: formData.value.graduationDate,
      gpa: formData.value.gpa,
    };
    onSave(education);
    onClose();
  } else {
    // Create
    const res = await profileService.createEducation(formData.value);
    onSave(res);
    onClose();
  }
};

function onClose() {
  emit("update:modelValue", false);
}

function onSave(education: Education) {
  emit("save", education);
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
          {{ isEditing ? "Eğitim Bilgisini Düzenle" : "Yeni Eğitim Bilgisi Ekle" }}
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
        <form @submit.prevent="saveEducation">
          <div class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Okul Adı
              </label>
              <input
                type="text"
                v-model="formData.institution"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Okul adını giriniz"
                required
              />
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Derece
                </label>
                <select
                  v-model="formData.degree"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  required
                >
                  <option :value="undefined">Derece Seçiniz</option>
                  <option value="Lise">Lise</option>
                  <option value="Ön Lisans">Ön Lisans</option>
                  <option value="Lisans">Lisans</option>
                  <option value="Yüksek Lisans">Yüksek Lisans</option>
                  <option value="Doktora">Doktora</option>
                </select>
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Bölüm
                </label>
                <input
                  type="text"
                  v-model="formData.department"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder="Bölüm adını giriniz"
                  required
                />
              </div>
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label
                  for="activeDate"
                  class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1"
                >
                  Başlangıç Tarihi
                </label>
                <input
                  type="date"
                  v-model="formData.startDate"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  required
                />
              </div>

              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Bitiş Tarihi
                </label>

                <div class="relative flex items-center justify-end">
                  <input
                    type="date"
                    v-model="formData.graduationDate"
                    class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  />
                </div>
              </div>
            </div>

            <div class="flex items-center">
              <input
                type="checkbox"
                id="isCurrentlyStudying"
                class="h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
              />
              <label
                for="isCurrentlyStudying"
                class="ml-2 block text-sm text-gray-700 dark:text-gray-300"
              >
                Halen öğrenim görüyorum
              </label>
            </div>

            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Açıklama <span class="text-gray-400 dark:text-gray-60">(Opsiyonel)</span>
              </label>
              <textarea
                v-model="formData.description"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Eğitiminiz hakkında kısa bir açıklama giriniz"
                rows="3"
              ></textarea>
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
