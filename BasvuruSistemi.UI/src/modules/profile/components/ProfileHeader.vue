<script setup lang="ts">
import { Profile } from "../models/profile.model";

defineProps<{
  profile: Profile;
}>();

const apiUrl = import.meta.env.VITE_API_PUBLIC_URL;

defineEmits(["editProfile", "editPhoto"]);
</script>

<template>
  <div class="p-6 border-b border-gray-200 dark:border-gray-700">
    <div class="flex flex-col md:flex-row items-start md:items-center gap-6">
      <!-- Profil resmi -->
      <div class="relative">
        <img
          :src="profile.avatarUrl ?? apiUrl + '/user.png'"
          alt="Profil Fotoğrafı"
          width="240"
          height="240"
          class="w-24 h-24 rounded-full object-cover border-4 border-white dark:border-gray-700 shadow-md"
        />
        <button
          @click="$emit('editPhoto')"
          class="absolute bottom-0 right-0 bg-blue-500 text-white p-1 rounded-full hover:bg-blue-600 transition-colors"
          title="Fotoğrafı Değiştir"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            class="h-4 w-4"
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
      </div>

      <!-- Kişisel bilgiler -->
      <div class="flex-1">
        <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
          <div>
            <h2 class="text-2xl font-bold text-gray-900 dark:text-white">
              {{ profile.firstName }} {{ profile.lastName }}
            </h2>
            <div class="mt-2 space-y-1">
              <p class="text-gray-600 dark:text-gray-300 flex items-center gap-2">
                <span class="font-medium">TC Kimlik No:</span> {{ profile.tckn }}
              </p>
              <p class="text-gray-600 dark:text-gray-300 flex items-center gap-2">
                <span class="font-medium">E-posta:</span> {{ profile.email }}
              </p>
              <p class="text-gray-600 dark:text-gray-300 flex items-center gap-2">
                <span class="font-medium">Telefon:</span> {{ profile.phone ?? "-" }}
              </p>
            </div>
          </div>

          <!-- Düzenleme butonu -->
          <button
            @click="$emit('editProfile')"
            class="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition-colors flex items-center gap-2 self-start"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-4 w-4"
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
            Profili Düzenle
          </button>
        </div>
      </div>
    </div>
  </div>
</template>
