<template>
  <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6 border border-gray-200 dark:border-gray-700">
    <h2 class="text-2xl font-bold text-gray-800 dark:text-white mb-6">İş Başvuru Formu</h2>
    
    <!-- Başvuru formu -->
    <form @submit.prevent="handleSubmit" class="space-y-6">
      <!-- Ad Soyad alanı -->
      <div>
        <label for="fullName" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
          Ad Soyad <span class="text-red-600">*</span>
        </label>
        <input 
          id="fullName" 
          v-model="form.fullName" 
          type="text" 
          required
          aria-required="true"
          class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          placeholder="Adınız ve soyadınız"
        />
        <p v-if="errors.fullName" class="mt-1 text-sm text-red-600">
          {{ errors.fullName }}
        </p>
      </div>
      
      <!-- Özgeçmiş yükleme alanı -->
      <div>
        <label for="resume" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
          Özgeçmiş (CV) <span class="text-red-600">*</span>
        </label>
        <div class="relative border-2 border-dashed border-gray-300 dark:border-gray-600 rounded-md p-6 bg-gray-50 dark:bg-gray-900 hover:bg-gray-100 dark:hover:bg-gray-800 transition-colors duration-200">
          <input 
            id="resume" 
            type="file" 
            ref="fileInput"
            @change="handleFileChange" 
            accept=".pdf,.doc,.docx"
            class="absolute inset-0 w-full h-full opacity-0 cursor-pointer"
            aria-required="true"
            required
          />
          <div class="text-center space-y-2">
            <svg xmlns="http://www.w3.org/2000/svg" class="mx-auto h-12 w-12 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" />
            </svg>
            <p class="text-sm text-gray-600 dark:text-gray-400">
              {{ form.resumeFileName || 'PDF, DOC veya DOCX dosyası sürükleyin veya yüklemek için tıklayın' }}
            </p>
            <p class="text-xs text-gray-500 dark:text-gray-500">Maksimum boyut: 5MB</p>
          </div>
        </div>
        <p v-if="errors.resume" class="mt-1 text-sm text-red-600">
          {{ errors.resume }}
        </p>
      </div>
      
      <!-- Departman seçimi -->
      <div>
        <label for="department" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
          Departman <span class="text-red-600">*</span>
        </label>
        <select 
          id="department" 
          v-model="form.department" 
          required
          aria-required="true"
          class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
        >
          <option value="" disabled>Departman seçin</option>
          <option v-for="dept in departments" :key="dept" :value="dept">
            {{ dept }}
          </option>
        </select>
        <p v-if="errors.department" class="mt-1 text-sm text-red-600">
          {{ errors.department }}
        </p>
      </div>
      
      <!-- Motivasyon mektubu (opsiyonel) -->
      <div>
        <label for="coverLetter" class="block text-sm font-medium text-gray-700 dark:text-gray-300 mb-1">
          Motivasyon Mektubu (Opsiyonel)
        </label>
        <textarea 
          id="coverLetter" 
          v-model="form.coverLetter" 
          rows="4"
          class="w-full rounded-md border-gray-300 dark:border-gray-600 shadow-sm focus:border-blue-500 focus:ring focus:ring-blue-500 focus:ring-opacity-50 bg-white dark:bg-gray-700 text-gray-900 dark:text-white"
          placeholder="Neden bu pozisyon için uygun olduğunuzu düşünüyorsunuz?"
        ></textarea>
      </div>
      
      <!-- Gönder ve İptal butonları -->
      <div class="flex flex-col sm:flex-row space-y-3 sm:space-y-0 sm:space-x-3">
        <button 
          type="submit" 
          :disabled="isSubmitting"
          class="w-full sm:w-auto flex-1 bg-blue-600 hover:bg-blue-700 text-white font-medium py-2 px-6 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-opacity-50 transition-colors duration-200 disabled:opacity-70 disabled:cursor-not-allowed"
        >
          <span v-if="isSubmitting" class="flex items-center justify-center">
            <svg class="animate-spin -ml-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
              <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
              <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
            Gönderiliyor...
          </span>
          <span v-else>Başvuruyu Gönder</span>
        </button>
        <button 
          type="button" 
          @click="goBack"
          class="w-full sm:w-auto flex-1 bg-gray-200 dark:bg-gray-600 hover:bg-gray-300 dark:hover:bg-gray-500 text-gray-800 dark:text-white font-medium py-2 px-6 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-500 focus:ring-opacity-50 transition-colors duration-200"
        >
          İptal
        </button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted, defineProps, defineEmits, defineComponent } from 'vue';
import { useRouter } from 'vue-router';
import { jobData } from '../data/jobs';
import type { Job } from '../data/jobs';
import { applicationData } from '../data/applications';

// Router
const router = useRouter();

// Props
const props = defineProps<{
  jobId?: number
}>();

// Emits
const emit = defineEmits<{
  (e: 'submit', formData: Record<string, unknown>): void
}>();

// TypeScript için Vue bileşeni tanımlama
defineComponent({
  name: 'ApplicationForm',
  props: {
    jobId: Number
  },
  emits: ['submit']
});

// İş verisi
const job = ref<Job | null>(null);

// Departman listesi
const departments = computed(() => {
  const deptSet = new Set(jobData.map(job => job.department));
  return Array.from(deptSet);
});

// Form durumları
const form = reactive({
  fullName: '',
  resumeFileName: '',
  resumeFile: null as File | null,
  department: '',
  coverLetter: '',
});

// Validation hataları
const errors = reactive({
  fullName: '',
  resume: '',
  department: '',
});

// Form gönderim durumu
const isSubmitting = ref(false);
const fileInput = ref<HTMLInputElement | null>(null);

// Geri dönme fonksiyonu
const goBack = () => {
  router.back();
};

// Dosya değişikliği işleyicisi
const handleFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement;
  if (target.files && target.files.length > 0) {
    const file = target.files[0];
    
    // Dosya tipi kontrolü
    const allowedTypes = ['application/pdf', 'application/msword', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'];
    if (!allowedTypes.includes(file.type)) {
      errors.resume = 'Yalnızca PDF, DOC veya DOCX dosyaları yükleyebilirsiniz.';
      return;
    }
    
    // Dosya boyutu kontrolü (5MB)
    if (file.size > 5 * 1024 * 1024) {
      errors.resume = 'Dosya boyutu 5MB\'dan küçük olmalıdır.';
      return;
    }
    
    form.resumeFile = file;
    form.resumeFileName = file.name;
    errors.resume = '';
  }
};

// Form doğrulama
const validateForm = (): boolean => {
  let isValid = true;
  
  // Ad Soyad doğrulama
  if (!form.fullName.trim()) {
    errors.fullName = 'Ad Soyad alanı zorunludur.';
    isValid = false;
  } else if (form.fullName.trim().length < 3) {
    errors.fullName = 'Ad Soyad en az 3 karakter olmalıdır.';
    isValid = false;
  } else {
    errors.fullName = '';
  }
  
  // Özgeçmiş doğrulama
  if (!form.resumeFile) {
    errors.resume = 'Özgeçmiş dosyası zorunludur.';
    isValid = false;
  } else {
    errors.resume = '';
  }
  
  // Departman doğrulama
  if (!form.department) {
    errors.department = 'Departman seçimi zorunludur.';
    isValid = false;
  } else {
    errors.department = '';
  }
  
  return isValid;
};

// Form gönderimi
const handleSubmit = () => {
  if (!validateForm()) {
    return;
  }
  
  isSubmitting.value = true;
  
  // Gerçek bir API olmadığı için simülasyon yapıyoruz
  setTimeout(() => {
    // Yeni başvuru nesnesi
    const newApplication = {
      id: applicationData.length + 1,
      jobId: job.value?.id || 0,
      jobTitle: job.value?.title || '',
      applicantName: form.fullName,
      submissionDate: new Date().toISOString().split('T')[0],
      status: 'Pending' as const,
      resumeFilename: form.resumeFileName,
      department: form.department
    };
    
    // Dummy veriyi güncelleme (gerçek uygulamada API çağrısı olacak)
    applicationData.push(newApplication);
    
    // Form verilerini emit etme
    emit('submit', newApplication);
    
    // Kullanıcıyı başvurular sayfasına yönlendirme
    router.push({ name: 'MyApplications' });
    
    isSubmitting.value = false;
  }, 1500); // 1.5 saniyelik simülasyon gecikmesi
};

// Component yüklendiğinde
onMounted(() => {
  // İlgili iş ilanını bulma
  if (props.jobId) {
    job.value = jobData.find(j => j.id === props.jobId) || null;
    
    // Eğer iş bulunduysa, departmanı otomatik doldur
    if (job.value) {
      form.department = job.value.department;
    }
  }
});
</script>