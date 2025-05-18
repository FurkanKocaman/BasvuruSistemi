<template>
  <div class="fixed inset-0 bg-gradient-to-br from-blue-500/20 to-purple-600/30 backdrop-blur-sm flex items-center justify-center z-50" v-if="modelValue">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-xl w-full max-w-2xl max-h-[90vh] overflow-y-auto">
      <div class="p-6 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center">
        <h3 class="text-xl font-semibold text-gray-800 dark:text-white">
          Profil Bilgilerini Düzenle
        </h3>
        <button @click="$emit('update:modelValue', false)" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
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
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                TC Kimlik No
              </label>
              <input 
                type="text" 
                v-model="formData.tcKimlikNo"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="TC Kimlik Numaranız"
                maxlength="11"
              />
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Medeni Durum
              </label>
              <select 
                v-model="formData.maritalStatus"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
              >
                <option v-for="status in maritalStatusOptions" :key="status" :value="status">
                  {{ status }}
                </option>
              </select>
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
import { Profile, MaritalStatus } from '../models/profile.model';
import { ProfileService } from '../services/profile.service';

const props = defineProps<{
  modelValue: boolean;
  profile: Profile;
}>();

const emit = defineEmits(['update:modelValue', 'profileUpdated']);

// Form verisi
const formData = ref({
  firstName: '',
  lastName: '',
  tcKimlikNo: '',
  maritalStatus: MaritalStatus.Single
});

// Medeni durum seçenekleri
const maritalStatusOptions = Object.values(MaritalStatus);

// Profil verisini forma yükle
onMounted(() => {
  loadFormData();
});

watch(() => props.profile, () => {
  loadFormData();
}, { deep: true });

const loadFormData = () => {
  formData.value = {
    firstName: props.profile.firstName,
    lastName: props.profile.lastName,
    tcKimlikNo: props.profile.tcKimlikNo,
    maritalStatus: props.profile.maritalStatus
  };
};

// Profil bilgilerini kaydet
const saveProfile = async () => {
  try {
    // Profil verisini güncelle
    const updatedProfile = {
      ...props.profile,
      firstName: formData.value.firstName,
      lastName: formData.value.lastName,
      tcKimlikNo: formData.value.tcKimlikNo,
      maritalStatus: formData.value.maritalStatus
    };
    
    // Profil servisini çağır
    await ProfileService.updateProfile(updatedProfile);
    
    // Başarılı olduğunda modal'ı kapat ve güncelleme olayını tetikle
    emit('update:modelValue', false);
    emit('profileUpdated', updatedProfile);
  } catch (error) {
    console.error('Profil güncellenirken hata oluştu:', error);
    alert('Profil güncellenirken bir hata oluştu. Lütfen tekrar deneyin.');
  }
};
</script>
