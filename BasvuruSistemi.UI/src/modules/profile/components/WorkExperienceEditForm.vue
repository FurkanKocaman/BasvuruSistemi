<template>
  <div class="fixed inset-0 bg-gradient-to-br from-blue-500/20 to-purple-600/30 backdrop-blur-sm flex items-center justify-center z-50" v-if="modelValue">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-xl w-full max-w-2xl max-h-[90vh] overflow-y-auto">
      <div class="p-6 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center">
        <h3 class="text-xl font-semibold text-gray-800 dark:text-white">
          {{ isEditing ? 'İş Deneyimi Düzenle' : 'Yeni İş Deneyimi Ekle' }}
        </h3>
        <button @click="$emit('update:modelValue', false)" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
      
      <div class="p-6">
        <form @submit.prevent="saveWorkExperience">
          <div class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Pozisyon
              </label>
              <input 
                type="text" 
                v-model="formData.title"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Pozisyon adını giriniz"
                required
              />
            </div>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Şirket Adı
                </label>
                <input 
                  type="text" 
                  v-model="formData.companyName"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder="Şirket adını giriniz"
                  required
                />
              </div>
              
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Lokasyon
                </label>
                <input 
                  type="text" 
                  v-model="formData.location"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder="Şehir, Ülke"
                  required
                />
              </div>
            </div>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
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
                <input 
                  type="date" 
                  v-model="formData.endDate"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  :disabled="formData.isCurrentlyWorking"
                />
              </div>
            </div>
            
            <div class="flex items-center">
              <input 
                type="checkbox" 
                id="isCurrentlyWorking" 
                v-model="formData.isCurrentlyWorking"
                class="h-4 w-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
              />
              <label for="isCurrentlyWorking" class="ml-2 block text-sm text-gray-700 dark:text-gray-300">
                Halen bu şirkette çalışıyorum
              </label>
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Açıklama
              </label>
              <textarea 
                v-model="formData.description"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="İş deneyiminiz hakkında kısa bir açıklama giriniz"
                rows="3"
              ></textarea>
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

<script setup lang="ts">
import { ref, onMounted, watch } from 'vue';
import { WorkExperience } from '../models/profile.model';

const props = defineProps<{
  modelValue: boolean;
  workExperience?: WorkExperience;
}>();

const emit = defineEmits(['update:modelValue', 'save']);

// Düzenleme modunda mı yoksa yeni ekleme modunda mı?
const isEditing = ref(false);

// Form verisi
const formData = ref({
  id: '',
  title: '',
  companyName: '',
  location: '',
  startDate: '',
  endDate: '',
  isCurrentlyWorking: false,
  description: ''
});

// İş deneyimi verisini forma yükle
onMounted(() => {
  loadFormData();
});

watch(() => props.workExperience, () => {
  loadFormData();
}, { deep: true });

const loadFormData = () => {
  if (props.workExperience) {
    isEditing.value = true;
    formData.value = {
      id: props.workExperience.id,
      title: props.workExperience.title,
      companyName: props.workExperience.companyName,
      location: props.workExperience.location,
      startDate: props.workExperience.startDate,
      endDate: props.workExperience.endDate || '',
      isCurrentlyWorking: props.workExperience.isCurrentlyWorking,
      description: props.workExperience.description
    };
  } else {
    isEditing.value = false;
    formData.value = {
      id: crypto.randomUUID(),
      title: '',
      companyName: '',
      location: '',
      startDate: '',
      endDate: '',
      isCurrentlyWorking: false,
      description: ''
    };
  }
};

// İş deneyimi bilgilerini kaydet
const saveWorkExperience = () => {
  const workExperienceData: WorkExperience = {
    id: formData.value.id,
    title: formData.value.title,
    companyName: formData.value.companyName,
    location: formData.value.location,
    startDate: formData.value.startDate,
    endDate: formData.value.isCurrentlyWorking ? null : formData.value.endDate,
    isCurrentlyWorking: formData.value.isCurrentlyWorking,
    description: formData.value.description
  };
  
  emit('save', workExperienceData);
  emit('update:modelValue', false);
};
</script>
