<template>
  <ProfileLayout title="Profil Bilgilerim">
    <!-- Form bileşenleri -->
    <ProfileEditForm 
      v-model="showProfileEditForm" 
      :profile="profile"
      @profile-updated="updateProfile"
    />
    
    <EducationEditForm 
      v-model="showEducationForm" 
      :education="selectedEducation"
      @save="saveEducation"
    />
    
    <CertificateEditForm
      v-model="showCertificateForm"
      :certificate="selectedCertificate"
      @save="saveCertificate"
    />
    
    <WorkExperienceEditForm
      v-model="showWorkExperienceForm"
      :workExperience="selectedWorkExperience"
      @save="saveWorkExperience"
    />
    
    <SkillEditForm
      v-model="showSkillForm"
      :skill="selectedSkill"
      @save="saveSkill"
    />
    <div v-if="loading" class="p-8 text-center">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto"></div>
      <p class="mt-4 text-gray-600 dark:text-gray-400">Profil bilgileri yükleniyor...</p>
    </div>

    <div v-else-if="error" class="p-8 text-center">
      <div class="text-red-500 mb-2">
        <svg xmlns="http://www.w3.org/2000/svg" class="h-12 w-12 mx-auto" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
          <circle cx="12" cy="12" r="10"></circle>
          <line x1="12" y1="8" x2="12" y2="12"></line>
          <line x1="12" y1="16" x2="12.01" y2="16"></line>
        </svg>
      </div>
      <p class="text-gray-700 dark:text-gray-300">Profil bilgileri yüklenirken bir hata oluştu.</p>
      <button 
        @click="loadProfile" 
        class="mt-4 px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors"
      >
        Tekrar Dene
      </button>
    </div>

    <div v-else>
      <!-- Profil başlığı -->
      <ProfileHeader 
        :profile="profile" 
        @edit-profile="editProfile" 
        @edit-photo="editProfilePhoto" 
      />

      <!-- Eğitim bilgileri -->
      <ProfileSection 
        title="Eğitim Bilgileri" 
        :isEmpty="!profile.education || profile.education.length === 0"
        emptyMessage="Henüz eğitim bilgisi eklenmemiş"
        addButtonText="Eğitim Bilgisi Ekle"
        @edit="editEducation"
        @add="addEducation"
      >
        <template #icon>
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M22 10v6M2 10l10-5 10 5-10 5z"></path>
            <path d="M6 12v5c0 2 2 3 6 3s6-1 6-3v-5"></path>
          </svg>
        </template>
        
        <div v-for="edu in profile.education" :key="edu.id" class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg mb-3 last:mb-0">
          <div class="flex justify-between">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ edu.schoolName }}</h4>
              <p class="text-gray-600 dark:text-gray-300">{{ edu.degree }} - {{ edu.fieldOfStudy }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">
                {{ formatDate(edu.startDate) }} - {{ edu.isCurrentlyStudying ? 'Devam Ediyor' : formatDate(edu.endDate) }}
              </p>
              <p v-if="edu.description" class="mt-2 text-gray-700 dark:text-gray-300">{{ edu.description }}</p>
            </div>
            <div class="flex gap-2">
              <button 
                @click="editEducationItem(edu.id)" 
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button 
                @click="deleteEducationItem(edu.id)" 
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M3 6h18"></path>
                  <path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"></path>
                  <path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"></path>
                </svg>
              </button>
            </div>
          </div>
        </div>
      </ProfileSection>

      <!-- Sertifikalar -->
      <ProfileSection 
        title="Sertifikalar" 
        :isEmpty="!profile.certificates || profile.certificates.length === 0"
        emptyMessage="Henüz sertifika bilgisi eklenmemiş"
        addButtonText="Sertifika Ekle"
        @edit="editCertificates"
        @add="addCertificate"
      >
        <template #icon>
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="8" r="7"></circle>
            <polyline points="8.21 13.89 7 23 12 20 17 23 15.79 13.88"></polyline>
          </svg>
        </template>
        
        <div v-for="cert in profile.certificates" :key="cert.id" class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg mb-3 last:mb-0">
          <div class="flex justify-between">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ cert.name }}</h4>
              <p class="text-gray-600 dark:text-gray-300">{{ cert.issuingOrganization }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">
                Alınma Tarihi: {{ formatDate(cert.issueDate) }}
                <span v-if="cert.expirationDate">• Geçerlilik: {{ formatDate(cert.expirationDate) }}</span>
              </p>
              <p v-if="cert.credentialId" class="mt-1 text-sm text-gray-500 dark:text-gray-400">
                Kimlik No: {{ cert.credentialId }}
              </p>
            </div>
            <div class="flex gap-2">
              <button 
                @click="editCertificateItem(cert.id)" 
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button 
                @click="deleteCertificateItem(cert.id)" 
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M3 6h18"></path>
                  <path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"></path>
                  <path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"></path>
                </svg>
              </button>
            </div>
          </div>
        </div>
      </ProfileSection>

      <!-- İş Deneyimi -->
      <ProfileSection 
        title="İş Deneyimi" 
        :isEmpty="!profile.workExperience || profile.workExperience.length === 0"
        emptyMessage="Henüz iş deneyimi eklenmemiş"
        addButtonText="İş Deneyimi Ekle"
        @edit="editWorkExperience"
        @add="addWorkExperience"
      >
        <template #icon>
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <rect x="2" y="7" width="20" height="14" rx="2" ry="2"></rect>
            <path d="M16 21V5a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16"></path>
          </svg>
        </template>
        
        <div v-for="exp in profile.workExperience" :key="exp.id" class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg mb-3 last:mb-0">
          <div class="flex justify-between">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ exp.title }}</h4>
              <p class="text-gray-600 dark:text-gray-300">{{ exp.companyName }} - {{ exp.location }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">
                {{ formatDate(exp.startDate) }} - {{ exp.isCurrentlyWorking ? 'Devam Ediyor' : formatDate(exp.endDate) }}
              </p>
              <p v-if="exp.description" class="mt-2 text-gray-700 dark:text-gray-300">{{ exp.description }}</p>
            </div>
            <div class="flex gap-2">
              <button 
                @click="editWorkExperienceItem(exp.id)" 
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button 
                @click="deleteWorkExperienceItem(exp.id)" 
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M3 6h18"></path>
                  <path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"></path>
                  <path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"></path>
                </svg>
              </button>
            </div>
          </div>
        </div>
      </ProfileSection>

      <!-- Yetenekler -->
      <ProfileSection 
        title="Yetenekler" 
        :isEmpty="!profile.skills || profile.skills.length === 0"
        emptyMessage="Henüz yetenek eklenmemiş"
        addButtonText="Yetenek Ekle"
        @edit="editSkills"
        @add="addSkill"
      >
        <template #icon>
          <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z"></path>
          </svg>
        </template>
        
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div v-for="skill in profile.skills" :key="skill.id" class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg flex justify-between items-center">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ skill.name }}</h4>
              <p class="text-sm text-gray-500 dark:text-gray-400">{{ skill.level }}</p>
            </div>
            <div class="flex gap-2">
              <button 
                @click="editSkillItem(skill.id)" 
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button 
                @click="deleteSkillItem(skill.id)" 
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <path d="M3 6h18"></path>
                  <path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"></path>
                  <path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"></path>
                </svg>
              </button>
            </div>
          </div>
        </div>
      </ProfileSection>
    </div>
  </ProfileLayout>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import ProfileLayout from '../layouts/ProfileLayout.vue';
import ProfileHeader from '../components/ProfileHeader.vue';
import ProfileSection from '../components/ProfileSection.vue';
import ProfileEditForm from '../components/ProfileEditForm.vue';
import EducationEditForm from '../components/EducationEditForm.vue';
import CertificateEditForm from '../components/CertificateEditForm.vue';
import WorkExperienceEditForm from '../components/WorkExperienceEditForm.vue';
import SkillEditForm from '../components/SkillEditForm.vue';
import { ProfileService } from '../services/profile.service';
import { Profile, MaritalStatus, Education, Certificate, WorkExperience, Skill } from '../models/profile.model';

// Profil verisi, yükleme durumu ve hata durumu
const profile = ref<Profile>({
  id: '',
  firstName: '',
  lastName: '',
  tcKimlikNo: '',
  maritalStatus: MaritalStatus.Single,
  profilePicture: '',
  education: [],
  certificates: [],
  workExperience: [],
  skills: []
});
const loading = ref(true);
const error = ref(false);

// Sayfa yüklendiğinde profil verilerini getir
onMounted(() => {
  loadProfile();
});

// Profil verilerini yükle
const loadProfile = async () => {
  loading.value = true;
  error.value = false;
  
  try {
    profile.value = await ProfileService.getUserProfile();
  } catch (err) {
    console.error('Profil yüklenirken hata oluştu:', err);
    error.value = true;
  } finally {
    loading.value = false;
  }
};

// Tarih formatla
const formatDate = (dateString: string | null): string => {
  if (!dateString) return '';
  
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('tr-TR', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  }).format(date);
};

// Form gösterme durumları
const showProfileEditForm = ref(false);
const showEducationForm = ref(false);
const showCertificateForm = ref(false);
const showWorkExperienceForm = ref(false);
const showSkillForm = ref(false);
const selectedEducation = ref<Education | undefined>(undefined);
const selectedCertificate = ref<Certificate | undefined>(undefined);
const selectedWorkExperience = ref<WorkExperience | undefined>(undefined);
const selectedSkill = ref<Skill | undefined>(undefined);

// Profil düzenleme işlevleri
const editProfile = () => {
  showProfileEditForm.value = true;
};

const editProfilePhoto = () => {
  alert('Profil fotoğrafı düzenleme özelliği yakında eklenecek');
};

// Profil güncelleme
const updateProfile = (updatedProfile: Profile) => {
  profile.value = updatedProfile;
};

// Eğitim bölümü işlevleri
const editEducation = () => {
  // Tüm eğitim bilgilerini düzenleme
  alert('Tüm eğitim bilgilerini düzenleme özelliği yakında eklenecek');
};

const addEducation = () => {
  selectedEducation.value = undefined;
  showEducationForm.value = true;
};

const editEducationItem = (id: string) => {
  const education = profile.value.education.find(edu => edu.id === id);
  if (education) {
    selectedEducation.value = education;
    showEducationForm.value = true;
  }
};

const deleteEducationItem = (id: string) => {
  if (confirm('Bu eğitim bilgisini silmek istediğinize emin misiniz?')) {
    profile.value.education = profile.value.education.filter(edu => edu.id !== id);
  }
};

// Eğitim bilgisini kaydet
const saveEducation = (educationData: Education) => {
  // Eğer düzenleme modundaysa, mevcut eğitim bilgisini güncelle
  const index = profile.value.education.findIndex(edu => edu.id === educationData.id);
  
  if (index !== -1) {
    profile.value.education[index] = educationData;
  } else {
    // Yeni eğitim bilgisi ekleme
    profile.value.education.push(educationData);
  }
};

// Sertifika bölümü işlevleri
const editCertificates = () => {
  alert('Tüm sertifikaları düzenleme özelliği yakında eklenecek');
};

const addCertificate = () => {
  selectedCertificate.value = undefined;
  showCertificateForm.value = true;
};

const editCertificateItem = (id: string) => {
  const certificate = profile.value.certificates.find(cert => cert.id === id);
  if (certificate) {
    selectedCertificate.value = certificate;
    showCertificateForm.value = true;
  }
};

const deleteCertificateItem = (id: string) => {
  if (confirm('Bu sertifikayı silmek istediğinize emin misiniz?')) {
    profile.value.certificates = profile.value.certificates.filter(cert => cert.id !== id);
  }
};

// Sertifika bilgisini kaydet
const saveCertificate = (certificateData: Certificate) => {
  // Eğer düzenleme modundaysa, mevcut sertifikayı güncelle
  const index = profile.value.certificates.findIndex(cert => cert.id === certificateData.id);
  
  if (index !== -1) {
    profile.value.certificates[index] = certificateData;
  } else {
    // Yeni sertifika ekleme
    profile.value.certificates.push(certificateData);
  }
};

// İş deneyimi bölümü işlevleri
const editWorkExperience = () => {
  alert('Tüm iş deneyimlerini düzenleme özelliği yakında eklenecek');
};

const addWorkExperience = () => {
  selectedWorkExperience.value = undefined;
  showWorkExperienceForm.value = true;
};

const editWorkExperienceItem = (id: string) => {
  const workExperience = profile.value.workExperience.find(exp => exp.id === id);
  if (workExperience) {
    selectedWorkExperience.value = workExperience;
    showWorkExperienceForm.value = true;
  }
};

const deleteWorkExperienceItem = (id: string) => {
  if (confirm('Bu iş deneyimini silmek istediğinize emin misiniz?')) {
    profile.value.workExperience = profile.value.workExperience.filter(exp => exp.id !== id);
  }
};

// İş deneyimi bilgisini kaydet
const saveWorkExperience = (workExperienceData: WorkExperience) => {
  // Eğer düzenleme modundaysa, mevcut iş deneyimini güncelle
  const index = profile.value.workExperience.findIndex(exp => exp.id === workExperienceData.id);
  
  if (index !== -1) {
    profile.value.workExperience[index] = workExperienceData;
  } else {
    // Yeni iş deneyimi ekleme
    profile.value.workExperience.push(workExperienceData);
  }
};

// Yetenekler bölümü işlevleri
const editSkills = () => {
  alert('Tüm yetenekleri düzenleme özelliği yakında eklenecek');
};

const addSkill = () => {
  selectedSkill.value = undefined;
  showSkillForm.value = true;
};

const editSkillItem = (id: string) => {
  const skill = profile.value.skills.find(s => s.id === id);
  if (skill) {
    selectedSkill.value = skill;
    showSkillForm.value = true;
  }
};

const deleteSkillItem = (id: string) => {
  if (confirm('Bu yeteneği silmek istediğinize emin misiniz?')) {
    profile.value.skills = profile.value.skills.filter(s => s.id !== id);
  }
};

// Yetenek bilgisini kaydet
const saveSkill = (skillData: Skill) => {
  // Eğer düzenleme modundaysa, mevcut yeteneği güncelle
  const index = profile.value.skills.findIndex(s => s.id === skillData.id);
  
  if (index !== -1) {
    profile.value.skills[index] = skillData;
  } else {
    // Yeni yetenek ekleme
    profile.value.skills.push(skillData);
  }
};
</script>
