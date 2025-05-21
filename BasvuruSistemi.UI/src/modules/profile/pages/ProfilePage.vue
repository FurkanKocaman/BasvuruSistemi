<script setup lang="ts">
import { ref, onMounted } from "vue";
import ProfileLayout from "../layouts/ProfileLayout.vue";
import ProfileHeader from "../components/ProfileHeader.vue";
import ProfileSection from "../components/ProfileSection.vue";
import ProfileEditForm from "../components/ProfileEditForm.vue";
import EducationEditForm from "../components/EducationEditForm.vue";
import CertificateEditForm from "../components/CertificateEditForm.vue";
import WorkExperienceEditForm from "../components/WorkExperienceEditForm.vue";
import SkillEditForm from "../components/SkillEditForm.vue";
import ConfirmModal from "@/components/ConfirmModal.vue";
import {
  Profile,
  MaritalStatus,
  Education,
  Certificate,
  WorkExperience,
  Skill,
} from "../models/profile.model";
import { MapPin } from "lucide-vue-next";
import profileService from "../services/profile.service";
import { getSkillLevelOptionByValue } from "@/models/constants/skill-level";
import { Address } from "@/models/entities/address-model";
import AddressEditForm from "../components/AddressEditForm.vue";

// Profil verisi, yükleme durumu ve hata durumu
const profile = ref<Profile>({
  id: "",
  firstName: "",
  lastName: "",
  profileStatus: 0,
  addresses: [],
  maritalStatus: MaritalStatus.Single,
  educations: [],
  certificates: [],
  experiences: [],
  skills: [],
});
const loading = ref(true);
const error = ref(false);
const confirmModal = ref();

const confirModalTitle = ref("");
const confirModalDescription = ref("");
const previewUrl = ref("");

const fileInput = ref(null);
const avatarImageFile = ref();

onMounted(() => {
  loadProfile();
});

const loadProfile = async () => {
  loading.value = true;
  error.value = false;

  profile.value = await profileService.getUserProfile();
  if (profile.value) {
    console.log(profile.value);
    loading.value = false;
  }
};

const formatDate = (dateString: string | null): string => {
  if (!dateString) return "";

  const date = new Date(dateString);
  return new Intl.DateTimeFormat("tr-TR", {
    year: "numeric",
    month: "long",
    day: "numeric",
  }).format(date);
};

// Form gösterme durumları
const showProfileEditForm = ref(false);
const showAddressEditForm = ref(false);
const showEducationForm = ref(false);
const showCertificateForm = ref(false);
const showWorkExperienceForm = ref(false);
const showSkillForm = ref(false);
const selectedAddress = ref<Address | undefined>(undefined);
const selectedEducation = ref<Education | undefined>(undefined);
const selectedCertificate = ref<Certificate | undefined>(undefined);
const selectedWorkExperience = ref<WorkExperience | undefined>(undefined);
const selectedSkill = ref<Skill | undefined>(undefined);

// Profil düzenleme işlevleri
const editProfile = () => {
  showProfileEditForm.value = true;
};

const editProfilePhoto = () => {
  fileInput.value?.click();
};

async function onFileSelected(event) {
  const file = event.target.files[0];
  if (file) {
    const res = await profileService.updateUserAvatar(file);
    if (res) {
      console.log(res);
      previewUrl.value = URL.createObjectURL(file);
      profile.value.avatarUrl = previewUrl.value;
    }
    avatarImageFile.value = file;
  }
}

const updateProfile = (updatedProfile: Profile) => {
  profile.value = updatedProfile;
};

// Address bölümü işlevleri start
const addAddress = () => {
  selectedAddress.value = undefined;
  showAddressEditForm.value = true;
};

const editAdressItem = (address: Address) => {
  selectedAddress.value = address;
  showAddressEditForm.value = true;
};

const deleteAddressItem = async (id: string) => {
  confirModalTitle.value = "Adresi silmek istediğinize emin misiniz?";
  confirModalDescription.value = "Bu işlem geri alınamaz.";
  const result = await confirmModal.value.open();
  if (result) {
    const res = await profileService.deleteAddress(id);
    if (res) {
      profile.value.addresses = profile.value.addresses.filter((p) => p.id != id);
    }
  }
};

const saveAddress = (addressData: Address) => {
  const index = profile.value.addresses.findIndex((edu) => edu.id === addressData.id);

  if (index !== -1) {
    profile.value.addresses[index] = addressData;
  } else {
    profile.value.addresses.push(addressData);
  }
};

// Address bölümü işlevleri end
// Education bölümü işlevleri start
const addEducation = () => {
  selectedEducation.value = undefined;
  showEducationForm.value = true;
};

const editEducationItem = (id: string) => {
  const education = profile.value.educations.find((edu) => edu.id === id);
  if (education) {
    selectedEducation.value = education;
    showEducationForm.value = true;
  }
};

const deleteEducationItem = async (id: string) => {
  confirModalTitle.value = "Eğitimi silmek istediğinize emin misiniz?";
  confirModalDescription.value = "Bu işlem geri alınamaz.";
  const result = await confirmModal.value.open();
  if (result) {
    const res = await profileService.deleteEducation(id);
    if (res) {
      profile.value.educations = profile.value.educations.filter((p) => p.id != id);
    }
  }
};

const saveEducation = (educationData: Education) => {
  const index = profile.value.educations.findIndex((edu) => edu.id === educationData.id);

  if (index !== -1) {
    profile.value.educations[index] = educationData;
  } else {
    profile.value.educations.push(educationData);
  }
};

// Education bölümü işlevleri end
// Sertifika bölümü işlevleri start

const addCertificate = () => {
  selectedCertificate.value = undefined;
  showCertificateForm.value = true;
};

const editCertificateItem = (id: string) => {
  const certificate = profile.value.certificates.find((cert) => cert.id === id);
  if (certificate) {
    selectedCertificate.value = certificate;
    showCertificateForm.value = true;
  }
};

const deleteCertificateItem = async (id: string) => {
  confirModalTitle.value = "Kayıtlı sertifikayı silmek istediğinize emin misiniz?";
  confirModalDescription.value = "Bu işlem geri alınamaz.";
  const result = await confirmModal.value.open();
  if (result) {
    const res = await profileService.deleteCertificate(id);
    if (res) {
      profile.value.certificates = profile.value.certificates.filter((p) => p.id != id);
    }
  }
};

// Sertifika bilgisini kaydet
const saveCertificate = (certificateData: Certificate) => {
  const index = profile.value.certificates.findIndex((cert) => cert.id === certificateData.id);

  if (index !== -1) {
    profile.value.certificates[index] = certificateData;
  } else {
    profile.value.certificates.push(certificateData);
  }
};
// Sertifika bölümü işlevleri end
// İş deneyimi bölümü start

const addWorkExperience = () => {
  selectedWorkExperience.value = undefined;
  showWorkExperienceForm.value = true;
};

const editWorkExperienceItem = (id: string) => {
  const workExperience = profile.value.experiences.find((exp) => exp.id === id);
  if (workExperience) {
    selectedWorkExperience.value = workExperience;
    showWorkExperienceForm.value = true;
  }
};

const deleteWorkExperienceItem = async (id: string) => {
  confirModalTitle.value = "Kayıtlı deneyimi silmek istediğinize emin misiniz?";
  confirModalDescription.value = "Bu işlem geri alınamaz.";
  const result = await confirmModal.value.open();
  if (result) {
    const res = await profileService.deleteExperience(id);
    if (res) {
      profile.value.experiences = profile.value.experiences.filter((p) => p.id != id);
    }
  }
};

const saveWorkExperience = (workExperienceData: WorkExperience) => {
  const index = profile.value.experiences.findIndex((exp) => exp.id === workExperienceData.id);

  if (index !== -1) {
    profile.value.experiences[index] = workExperienceData;
  } else {
    profile.value.experiences.push(workExperienceData);
  }
};
// İş deneyimi bölümü end
// Yetenekler bölümü start
const addSkill = () => {
  selectedSkill.value = undefined;
  showSkillForm.value = true;
};

const editSkillItem = (id: string) => {
  const skill = profile.value.skills.find((s) => s.id === id);
  if (skill) {
    selectedSkill.value = skill;
    showSkillForm.value = true;
  }
};

const deleteSkillItem = async (id: string) => {
  confirModalTitle.value = "Kayıtlı yeteneği silmek istediğinize emin misiniz?";
  confirModalDescription.value = "Bu işlem geri alınamaz.";
  const result = await confirmModal.value.open();
  if (result) {
    const res = await profileService.deleteSkill(id);
    if (res) {
      profile.value.skills = profile.value.skills.filter((p) => p.id != id);
    }
  }
};

const saveSkill = (skillData: Skill) => {
  const index = profile.value.skills.findIndex((s) => s.id === skillData.id);

  if (index !== -1) {
    profile.value.skills[index] = skillData;
  } else {
    profile.value.skills.push(skillData);
  }
};
// Yetenekler bölümü start
</script>

<template>
  <ProfileLayout title="Profil Bilgilerim">
    <input type="file" accept="image/*" ref="fileInput" class="hidden" @change="onFileSelected" />
    <ConfirmModal
      ref="confirmModal"
      :title="confirModalTitle"
      :description="confirModalDescription"
    />
    <!-- Form bileşenleri -->
    <AddressEditForm v-model="showAddressEditForm" :address="selectedAddress" @save="saveAddress" />

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

    <SkillEditForm v-model="showSkillForm" :skill="selectedSkill" @save="saveSkill" />
    <div v-if="loading" class="p-8 text-center">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500 mx-auto"></div>
      <p class="mt-4 text-gray-600 dark:text-gray-400">Profil bilgileri yükleniyor...</p>
    </div>

    <div v-else-if="error" class="p-8 text-center">
      <div class="text-red-500 mb-2">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          class="h-12 w-12 mx-auto"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        >
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
      <!-- Adres-->
      <ProfileHeader
        :profile="profile"
        @edit-profile="editProfile"
        @edit-photo="editProfilePhoto"
      />

      <!-- Adres bilgileri -->
      <ProfileSection
        title="Adres Bilgileri"
        :isEmpty="!profile.addresses || profile.addresses.length === 0"
        emptyMessage="Henüz adres bilgisi eklenmemiş"
        addButtonText="Adres Bilgisi Ekle"
        @edit="addAddress"
        @add="addAddress"
      >
        <template #icon>
          <MapPin />
        </template>

        <div
          v-for="address in profile.addresses"
          :key="address.id"
          class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg mb-3 last:mb-0"
        >
          <div class="flex justify-between">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">
                {{ address.name }}
              </h4>
              <p class="text-gray-600 dark:text-gray-300">
                {{ address.city }} - {{ address.district }}
              </p>
              <p class="text-gray-600 dark:text-gray-300">
                {{ address.fullAddress }} - {{ address.country }} -({{ address.postalCode }})
              </p>
            </div>
            <div class="flex gap-2">
              <button
                @click="editAdressItem(address)"
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button
                @click="deleteAddressItem(address.id!)"
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <path d="M3 6h18"></path>
                  <path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"></path>
                  <path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"></path>
                </svg>
              </button>
            </div>
          </div>
        </div>
      </ProfileSection>
      <!-- Eğitim bilgileri -->
      <ProfileSection
        title="Eğitim Bilgileri"
        :isEmpty="!profile.educations || profile.educations.length === 0"
        emptyMessage="Henüz eğitim bilgisi eklenmemiş"
        addButtonText="Eğitim Bilgisi Ekle"
        @edit="addEducation"
        @add="addEducation"
      >
        <template #icon>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <path d="M22 10v6M2 10l10-5 10 5-10 5z"></path>
            <path d="M6 12v5c0 2 2 3 6 3s6-1 6-3v-5"></path>
          </svg>
        </template>

        <div
          v-for="edu in profile.educations"
          :key="edu.id"
          class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg mb-3 last:mb-0"
        >
          <div class="flex justify-between">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ edu.institution }}</h4>
              <p class="text-gray-600 dark:text-gray-300">
                {{ edu.degree }} - {{ edu.department }}
              </p>
              <p class="text-sm text-gray-500 dark:text-gray-400">
                {{ formatDate(edu.startDate) }} -
                {{ !edu.graduationDate ? "Devam Ediyor" : formatDate(edu.graduationDate) }}
              </p>
              <p v-if="edu.description" class="mt-2 text-gray-700 dark:text-gray-300">
                {{ edu.description }}
              </p>
            </div>
            <div class="flex gap-2">
              <button
                @click="editEducationItem(edu.id)"
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button
                @click="deleteEducationItem(edu.id)"
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
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
        @edit="addCertificate"
        @add="addCertificate"
      >
        <template #icon>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <circle cx="12" cy="8" r="7"></circle>
            <polyline points="8.21 13.89 7 23 12 20 17 23 15.79 13.88"></polyline>
          </svg>
        </template>

        <div
          v-for="cert in profile.certificates"
          :key="cert.id"
          class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg mb-3 last:mb-0"
        >
          <div class="flex justify-between">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ cert.title }}</h4>
              <p class="text-gray-600 dark:text-gray-300">{{ cert.issuer }}</p>
              <p class="text-sm text-gray-500 dark:text-gray-400">
                Alınma Tarihi: {{ formatDate(cert.dateReceived) }}
                <span v-if="cert.expiryDate">• Geçerlilik: {{ formatDate(cert.expiryDate) }}</span>
              </p>
            </div>
            <div class="flex gap-2">
              <button
                @click="editCertificateItem(cert.id)"
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button
                @click="deleteCertificateItem(cert.id)"
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
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
        :isEmpty="!profile.experiences || profile.experiences.length === 0"
        emptyMessage="Henüz iş deneyimi eklenmemiş"
        addButtonText="İş Deneyimi Ekle"
        @edit="addWorkExperience"
        @add="addWorkExperience"
      >
        <template #icon>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <rect x="2" y="7" width="20" height="14" rx="2" ry="2"></rect>
            <path d="M16 21V5a2 2 0 0 0-2-2h-4a2 2 0 0 0-2 2v16"></path>
          </svg>
        </template>

        <div
          v-for="exp in profile.experiences"
          :key="exp.id"
          class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg mb-3 last:mb-0"
        >
          <div class="flex justify-between">
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ exp.position }}</h4>
              <p class="text-gray-600 dark:text-gray-300">
                {{ exp.companyName }} - {{ exp.location }}
              </p>
              <p class="text-sm text-gray-500 dark:text-gray-400">
                {{ formatDate(exp.startDate) }} -
                {{ !exp.endDate ? "Devam Ediyor" : formatDate(exp.endDate) }}
              </p>
              <p v-if="exp.description" class="mt-2 text-gray-700 dark:text-gray-300">
                {{ exp.description }}
              </p>
            </div>
            <div class="flex gap-2">
              <button
                @click="editWorkExperienceItem(exp.id)"
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button
                @click="deleteWorkExperienceItem(exp.id)"
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
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
        @edit="addSkill"
        @add="addSkill"
      >
        <template #icon>
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-5 w-5"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            stroke-width="2"
            stroke-linecap="round"
            stroke-linejoin="round"
          >
            <path
              d="M14.7 6.3a1 1 0 0 0 0 1.4l1.6 1.6a1 1 0 0 0 1.4 0l3.77-3.77a6 6 0 0 1-7.94 7.94l-6.91 6.91a2.12 2.12 0 0 1-3-3l6.91-6.91a6 6 0 0 1 7.94-7.94l-3.76 3.76z"
            ></path>
          </svg>
        </template>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div
            v-for="skill in profile.skills"
            :key="skill.id"
            class="p-4 bg-gray-50 dark:bg-gray-700 rounded-lg flex justify-between items-center"
          >
            <div>
              <h4 class="font-semibold text-gray-800 dark:text-white">{{ skill.name }}</h4>
              <p class="text-sm text-gray-500 dark:text-gray-400">
                {{ getSkillLevelOptionByValue(skill.level)?.label }}
              </p>
            </div>
            <div class="flex gap-2">
              <button
                @click="editSkillItem(skill.id)"
                class="text-blue-500 hover:text-blue-700 dark:text-blue-400 dark:hover:text-blue-300"
                title="Düzenle"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
                  <path d="M12 20h9"></path>
                  <path d="M16.5 3.5a2.121 2.121 0 0 1 3 3L7 19l-4 1 1-4L16.5 3.5z"></path>
                </svg>
              </button>
              <button
                @click="deleteSkillItem(skill.id)"
                class="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300"
                title="Sil"
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  class="h-5 w-5"
                  viewBox="0 0 24 24"
                  fill="none"
                  stroke="currentColor"
                  stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                >
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
