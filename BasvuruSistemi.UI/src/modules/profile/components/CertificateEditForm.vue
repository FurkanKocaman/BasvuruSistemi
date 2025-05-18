<template>
  <div class="fixed inset-0 bg-gradient-to-br from-blue-500/20 to-purple-600/30 backdrop-blur-sm flex items-center justify-center z-50" v-if="modelValue">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-xl w-full max-w-2xl max-h-[90vh] overflow-y-auto">
      <div class="p-6 border-b border-gray-200 dark:border-gray-700 flex justify-between items-center">
        <h3 class="text-xl font-semibold text-gray-800 dark:text-white">
          {{ isEditing ? 'Sertifika Düzenle' : 'Yeni Sertifika Ekle' }}
        </h3>
        <button @click="$emit('update:modelValue', false)" class="text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-200">
          <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
      
      <div class="p-6">
        <form @submit.prevent="saveCertificate">
          <div class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Sertifika Adı
              </label>
              <input 
                type="text" 
                v-model="formData.name"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Sertifika adını giriniz"
                required
              />
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                Veren Kuruluş
              </label>
              <input 
                type="text" 
                v-model="formData.issuingOrganization"
                class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                placeholder="Sertifikayı veren kuruluşun adını giriniz"
                required
              />
            </div>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Alınma Tarihi
                </label>
                <input 
                  type="date" 
                  v-model="formData.issueDate"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  required
                />
              </div>
              
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Geçerlilik Tarihi
                </label>
                <input 
                  type="date" 
                  v-model="formData.expirationDate"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                />
              </div>
            </div>
            
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Kimlik No
                </label>
                <input 
                  type="text" 
                  v-model="formData.credentialId"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder="Sertifika kimlik numarası"
                />
              </div>
              
              <div>
                <label class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
                  Doğrulama URL'si
                </label>
                <input 
                  type="url" 
                  v-model="formData.credentialUrl"
                  class="w-full px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-md focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:text-white"
                  placeholder="https://example.com/verify"
                />
              </div>
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
import { Certificate } from '../models/profile.model';

const props = defineProps<{
  modelValue: boolean;
  certificate?: Certificate;
}>();

const emit = defineEmits(['update:modelValue', 'save']);

// Düzenleme modunda mı yoksa yeni ekleme modunda mı?
const isEditing = ref(false);

// Form verisi
const formData = ref({
  id: '',
  name: '',
  issuingOrganization: '',
  issueDate: '',
  expirationDate: '',
  credentialId: '',
  credentialUrl: ''
});

// Sertifika verisini forma yükle
onMounted(() => {
  loadFormData();
});

watch(() => props.certificate, () => {
  loadFormData();
}, { deep: true });

const loadFormData = () => {
  if (props.certificate) {
    isEditing.value = true;
    formData.value = {
      id: props.certificate.id,
      name: props.certificate.name,
      issuingOrganization: props.certificate.issuingOrganization,
      issueDate: props.certificate.issueDate,
      expirationDate: props.certificate.expirationDate || '',
      credentialId: props.certificate.credentialId,
      credentialUrl: props.certificate.credentialUrl
    };
  } else {
    isEditing.value = false;
    formData.value = {
      id: crypto.randomUUID(),
      name: '',
      issuingOrganization: '',
      issueDate: '',
      expirationDate: '',
      credentialId: '',
      credentialUrl: ''
    };
  }
};

// Sertifika bilgilerini kaydet
const saveCertificate = () => {
  const certificateData: Certificate = {
    id: formData.value.id,
    name: formData.value.name,
    issuingOrganization: formData.value.issuingOrganization,
    issueDate: formData.value.issueDate,
    expirationDate: formData.value.expirationDate || null,
    credentialId: formData.value.credentialId,
    credentialUrl: formData.value.credentialUrl
  };
  
  emit('save', certificateData);
  emit('update:modelValue', false);
};
</script>
