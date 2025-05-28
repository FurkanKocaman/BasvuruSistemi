<script setup lang="ts">
import { ClaimTypes } from "@/models/constants/claim-type";
import { useThemeStore } from "@/stores/theme";
import { useUserStore } from "@/stores/user";
import { useClaimChecker } from "@/utilities/useClaimChecker";
import { computed, onMounted, ref } from "vue";

const { hasClaim } = useClaimChecker();

const themeStore = useThemeStore();
const userStore = useUserStore();
const user = ref();
const currentTheme = computed(() => themeStore.currentTheme);

const isPostingMenuOpen = ref(false);
const isEvaluationMenuOpen = ref(false);

const apiUrl = import.meta.env.VITE_API_PUBLIC_URL;

type Theme = "light" | "dark" | "system";

onMounted(() => {
  user.value = userStore.user;
});

const setTheme = (theme: Theme) => {
  themeStore.setTheme(theme);
};

function togglePostingMenu() {
  isPostingMenuOpen.value = !isPostingMenuOpen.value;
  isEvaluationMenuOpen.value = false;
}

function toggleEvaluationMenu() {
  isPostingMenuOpen.value = false;
  isEvaluationMenuOpen.value = !isEvaluationMenuOpen.value;
}
</script>

<template>
  <header
    class="border-b dark:border-gray-800 border-gray-200 dark:bg-gray-900 bg-gray-100 fixed w-full z-50"
  >
    <nav class="mx-auto flex items-center justify-between p-4 lg:px-8" aria-label="Global">
      <div class="flex lg:flex-1">
        <router-link to="/jobs" class="-m-1.5 p-1.5">
          <span class="dark:text-gray-50 text-gray-800 font-semibold">Başvuru Sistemi</span>
        </router-link>
      </div>
      <div class="flex lg:hidden">
        <button
          type="button"
          class="-m-2.5 inline-flex items-center justify-center rounded-md p-2.5 text-gray-700 dark:text-gray-50"
        >
          <span class="sr-only">Open main menu</span>
          <svg
            class="size-6"
            fill="none"
            viewBox="0 0 24 24"
            stroke-width="1.5"
            stroke="currentColor"
            aria-hidden="true"
            data-slot="icon"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5"
            />
          </svg>
        </button>
      </div>
      <div class="hidden lg:flex lg:gap-x-12">
        <div class="relative">
          <button
            type="button"
            class="flex items-center gap-x-1 text-sm/6 font-semibold text-gray-700 dark:text-gray-50 cursor-pointer"
            @click.prevent="togglePostingMenu()"
          >
            İlanlar
            <svg
              class="size-5 flex-none text-gray-400"
              viewBox="0 0 20 20"
              fill="currentColor"
              aria-hidden="true"
              data-slot="icon"
            >
              <path
                fill-rule="evenodd"
                d="M5.22 8.22a.75.75 0 0 1 1.06 0L10 11.94l3.72-3.72a.75.75 0 1 1 1.06 1.06l-4.25 4.25a.75.75 0 0 1-1.06 0L5.22 9.28a.75.75 0 0 1 0-1.06Z"
                clip-rule="evenodd"
              />
            </svg>
          </button>
          <div
            v-if="isPostingMenuOpen"
            class="absolute top-full -left-8 z-10 mt-3 w-screen max-w-md overflow-hidden rounded-xl bg-white dark:bg-gray-800 shadow-lg ring-1 ring-gray-900/5 border dark:border-gray-700 border-gray-200"
          >
            <div class="px-2 py-2" v-if="hasClaim(ClaimTypes.ViewJobPosting)">
              <div
                class="group relative flex items-center gap-x-6 rounded-lg p-2 text-sm/6 hover:bg-gray-100 dark:hover:bg-gray-700/20"
              >
                <div class="flex-auto">
                  <router-link
                    to="/management/job-postings"
                    class="block font-semibold text-gray-700 dark:text-gray-200"
                    @click.prevent="togglePostingMenu()"
                  >
                    İlan Listesi
                    <span class="absolute inset-0"></span>
                  </router-link>
                  <p class="mt-1 text-gray-600 dark:text-gray-500">Mevcut ilanları görüntüleyin</p>
                </div>
              </div>
            </div>
            <div class="px-2 py-2" v-if="hasClaim(ClaimTypes.CreateJobPosting)">
              <div
                class="group relative flex items-center gap-x-6 rounded-lg p-2 text-sm/6 hover:bg-gray-100 dark:hover:bg-gray-700/20"
              >
                <div class="flex-auto">
                  <router-link
                    to="/management/job-postings/create"
                    class="block font-semibold text-gray-700 dark:text-gray-200"
                    @click.prevent="togglePostingMenu()"
                  >
                    İlan Oluştur
                    <span class="absolute inset-0"></span>
                  </router-link>
                  <p class="mt-1 text-gray-600 dark:text-gray-500">Yeni bir ilan oluşturun</p>
                </div>
              </div>
            </div>
            <div class="px-2 py-2" v-if="hasClaim(ClaimTypes.CreateJobPosting)">
              <div
                class="group relative flex items-center gap-x-6 rounded-lg p-2 text-sm/6 hover:bg-gray-100 dark:hover:bg-gray-700/20"
              >
                <div class="flex-auto">
                  <router-link
                    to="/management/job-postings-group/create"
                    class="block font-semibold text-gray-700 dark:text-gray-200"
                    @click.prevent="togglePostingMenu()"
                  >
                    İlan Grubu Oluştur
                    <span class="absolute inset-0"></span>
                  </router-link>
                  <p class="mt-1 text-gray-600 dark:text-gray-500">
                    İçinde birden çok benzer türde ilanın bulunduğu grup oluşturun
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <router-link
          v-if="hasClaim(ClaimTypes.ManageTemplates)"
          to="/management/form-templates"
          class="text-sm/6 font-semibold text-gray-700 dark:text-gray-50"
          >Form Şablonları</router-link
        >
        <router-link
          v-if="hasClaim(ClaimTypes.ViewApplications)"
          to="/management/applications"
          class="text-sm/6 font-semibold text-gray-700 dark:text-gray-50"
          >Başvurular</router-link
        >
        <router-link
          v-if="hasClaim(ClaimTypes.ViewApplications)"
          to="/management/pending-evaluations"
          class="text-sm/6 font-semibold text-gray-700 dark:text-gray-50"
          >Değerlendirme Bekleyen Başvurular</router-link
        >
        <div class="relative" v-if="hasClaim(ClaimTypes.ManageUsers)">
          <button
            type="button"
            class="flex items-center gap-x-1 text-sm/6 font-semibold text-gray-700 dark:text-gray-50 cursor-pointer"
            @click.prevent="toggleEvaluationMenu()"
          >
            Değerlendirme
            <svg
              class="size-5 flex-none text-gray-400"
              viewBox="0 0 20 20"
              fill="currentColor"
              aria-hidden="true"
              data-slot="icon"
            >
              <path
                fill-rule="evenodd"
                d="M5.22 8.22a.75.75 0 0 1 1.06 0L10 11.94l3.72-3.72a.75.75 0 1 1 1.06 1.06l-4.25 4.25a.75.75 0 0 1-1.06 0L5.22 9.28a.75.75 0 0 1 0-1.06Z"
                clip-rule="evenodd"
              />
            </svg>
          </button>
          <div
            v-if="isEvaluationMenuOpen"
            class="absolute top-full -left-8 z-10 mt-3 w-screen max-w-md overflow-hidden rounded-xl bg-white dark:bg-gray-800 shadow-lg ring-1 ring-gray-900/5 border dark:border-gray-700 border-gray-200"
          >
            <div class="px-2 py-2" v-if="hasClaim(ClaimTypes.ViewJobPosting)">
              <div
                class="group relative flex items-center gap-x-6 rounded-lg p-2 text-sm/6 hover:bg-gray-100 dark:hover:bg-gray-700/20"
              >
                <div class="flex-auto">
                  <router-link
                    v-if="hasClaim(ClaimTypes.ManageUsers)"
                    to="/management/commissions"
                    class="block font-semibold text-gray-700 dark:text-gray-200"
                    @click.prevent="toggleEvaluationMenu()"
                  >
                    Komisyonlar
                    <span class="absolute inset-0"></span>
                  </router-link>
                  <p class="mt-1 text-gray-600 dark:text-gray-500">
                    Değerlendirme komisyonlarını görüntüleyin
                  </p>
                </div>
              </div>
            </div>
            <div class="px-2 py-2" v-if="hasClaim(ClaimTypes.CreateJobPosting)">
              <div
                class="group relative flex items-center gap-x-6 rounded-lg p-2 text-sm/6 hover:bg-gray-100 dark:hover:bg-gray-700/20"
              >
                <div class="flex-auto">
                  <router-link
                    v-if="hasClaim(ClaimTypes.ManageUsers)"
                    to="/management/evaluation-stages"
                    class="block font-semibold text-gray-700 dark:text-gray-200"
                    @click.prevent="toggleEvaluationMenu()"
                  >
                    Değerlendirme Adımları
                    <span class="absolute inset-0"></span>
                  </router-link>
                  <p class="mt-1 text-gray-600 dark:text-gray-500">
                    İlanlarda kullanacağınız değerlendirme adımlarını görüntüleyin
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <router-link
          v-if="hasClaim(ClaimTypes.ManageUnits)"
          to="/management/units"
          class="text-sm/6 font-semibold text-gray-700 dark:text-gray-50"
          >Birimler</router-link
        >
        <router-link
          v-if="hasClaim(ClaimTypes.ManageUsers)"
          to="/management/users"
          class="text-sm/6 font-semibold text-gray-700 dark:text-gray-50"
          >Kullanıcılar</router-link
        >

        <router-link
          v-if="hasClaim(ClaimTypes.ManageRoles)"
          to="/management/roles"
          class="text-sm/6 font-semibold text-gray-700 dark:text-gray-50"
          >Roller</router-link
        >
        <router-link to="/tenants" class="text-sm/6 font-semibold text-gray-700 dark:text-gray-50"
          >Kurumlar</router-link
        >
      </div>
      <div class="hidden lg:flex lg:flex-1 lg:justify-end">
        <div
          v-if="currentTheme == 'dark'"
          v-on:click="setTheme('light')"
          class="border border-gray-600 rounded-full w-10 h-10 flex items-center justify-center cursor-pointer"
        >
          <svg class="size-6" viewBox="0 -0.5 25 25" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path
              fill-rule="evenodd"
              clip-rule="evenodd"
              d="M15.125 12C15.125 13.4497 13.9497 14.625 12.5 14.625C11.0503 14.625 9.875 13.4497 9.875 12C9.875 10.5503 11.0503 9.375 12.5 9.375C13.9497 9.375 15.125 10.5503 15.125 12Z"
              class="stroke-gray-400"
              stroke-width="1.5"
              stroke-linecap="round"
              stroke-linejoin="round"
            />
            <path
              d="M19.5 12.75C19.9142 12.75 20.25 12.4142 20.25 12C20.25 11.5858 19.9142 11.25 19.5 11.25V12.75ZM17.75 11.25C17.3358 11.25 17 11.5858 17 12C17 12.4142 17.3358 12.75 17.75 12.75V11.25ZM7.25 12.75C7.66421 12.75 8 12.4142 8 12C8 11.5858 7.66421 11.25 7.25 11.25V12.75ZM5.5 11.25C5.08579 11.25 4.75 11.5858 4.75 12C4.75 12.4142 5.08579 12.75 5.5 12.75V11.25ZM13.25 5C13.25 4.58579 12.9142 4.25 12.5 4.25C12.0858 4.25 11.75 4.58579 11.75 5H13.25ZM11.75 6.75C11.75 7.16421 12.0858 7.5 12.5 7.5C12.9142 7.5 13.25 7.16421 13.25 6.75H11.75ZM13.25 17.25C13.25 16.8358 12.9142 16.5 12.5 16.5C12.0858 16.5 11.75 16.8358 11.75 17.25H13.25ZM11.75 19C11.75 19.4142 12.0858 19.75 12.5 19.75C12.9142 19.75 13.25 19.4142 13.25 19H11.75ZM17.9803 7.58033C18.2732 7.28744 18.2732 6.81256 17.9803 6.51967C17.6874 6.22678 17.2126 6.22678 16.9197 6.51967L17.9803 7.58033ZM15.6817 7.75767C15.3888 8.05056 15.3888 8.52544 15.6817 8.81833C15.9746 9.11122 16.4494 9.11122 16.7423 8.81833L15.6817 7.75767ZM9.31833 16.2423C9.61122 15.9494 9.61122 15.4746 9.31833 15.1817C9.02544 14.8888 8.55056 14.8888 8.25767 15.1817L9.31833 16.2423ZM7.01967 16.4197C6.72678 16.7126 6.72678 17.1874 7.01967 17.4803C7.31256 17.7732 7.78744 17.7732 8.08033 17.4803L7.01967 16.4197ZM8.08033 6.51967C7.78744 6.22678 7.31256 6.22678 7.01967 6.51967C6.72678 6.81256 6.72678 7.28744 7.01967 7.58033L8.08033 6.51967ZM8.25767 8.81833C8.55056 9.11122 9.02544 9.11122 9.31833 8.81833C9.61122 8.52544 9.61122 8.05056 9.31833 7.75767L8.25767 8.81833ZM16.7433 15.1827C16.4504 14.8898 15.9756 14.8898 15.6827 15.1827C15.3898 15.4756 15.3898 15.9504 15.6827 16.2433L16.7433 15.1827ZM16.9197 17.4803C17.2126 17.7732 17.6874 17.7732 17.9803 17.4803C18.2732 17.1874 18.2732 16.7126 17.9803 16.4197L16.9197 17.4803ZM19.5 11.25H17.75V12.75H19.5V11.25ZM7.25 11.25H5.5V12.75H7.25V11.25ZM11.75 5V6.75H13.25V5H11.75ZM11.75 17.25V19H13.25V17.25H11.75ZM16.9197 6.51967L15.6817 7.75767L16.7423 8.81833L17.9803 7.58033L16.9197 6.51967ZM8.25767 15.1817L7.01967 16.4197L8.08033 17.4803L9.31833 16.2423L8.25767 15.1817ZM7.01967 7.58033L8.25767 8.81833L9.31833 7.75767L8.08033 6.51967L7.01967 7.58033ZM15.6827 16.2433L16.9197 17.4803L17.9803 16.4197L16.7433 15.1827L15.6827 16.2433Z"
              class="fill-gray-400"
            />
          </svg>
        </div>
        <div
          v-if="currentTheme == 'light' || currentTheme == 'system'"
          class="border border-gray-400 rounded-full w-10 h-10 flex items-center justify-center cursor-pointer"
          v-on:click="setTheme('dark')"
        >
          <svg class="size-5" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path
              d="M21.0672 11.8568L20.4253 11.469L21.0672 11.8568ZM12.1432 2.93276L11.7553 2.29085V2.29085L12.1432 2.93276ZM21.25 12C21.25 17.1086 17.1086 21.25 12 21.25V22.75C17.9371 22.75 22.75 17.9371 22.75 12H21.25ZM12 21.25C6.89137 21.25 2.75 17.1086 2.75 12H1.25C1.25 17.9371 6.06294 22.75 12 22.75V21.25ZM2.75 12C2.75 6.89137 6.89137 2.75 12 2.75V1.25C6.06294 1.25 1.25 6.06294 1.25 12H2.75ZM15.5 14.25C12.3244 14.25 9.75 11.6756 9.75 8.5H8.25C8.25 12.5041 11.4959 15.75 15.5 15.75V14.25ZM20.4253 11.469C19.4172 13.1373 17.5882 14.25 15.5 14.25V15.75C18.1349 15.75 20.4407 14.3439 21.7092 12.2447L20.4253 11.469ZM9.75 8.5C9.75 6.41182 10.8627 4.5828 12.531 3.57467L11.7553 2.29085C9.65609 3.5593 8.25 5.86509 8.25 8.5H9.75ZM12 2.75C11.9115 2.75 11.8077 2.71008 11.7324 2.63168C11.6686 2.56527 11.6538 2.50244 11.6503 2.47703C11.6461 2.44587 11.6482 2.35557 11.7553 2.29085L12.531 3.57467C13.0342 3.27065 13.196 2.71398 13.1368 2.27627C13.0754 1.82126 12.7166 1.25 12 1.25V2.75ZM21.7092 12.2447C21.6444 12.3518 21.5541 12.3539 21.523 12.3497C21.4976 12.3462 21.4347 12.3314 21.3683 12.2676C21.2899 12.1923 21.25 12.0885 21.25 12H22.75C22.75 11.2834 22.1787 10.9246 21.7237 10.8632C21.286 10.804 20.7293 10.9658 20.4253 11.469L21.7092 12.2447Z"
              class="fill-gray-500"
            />
          </svg>
        </div>
        <div
          class="border border-gray-400 dark:border-gray-600 rounded-full w-10 h-10 flex items-center justify-center cursor-pointer mx-3"
        >
          <svg class="size-5" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <g clip-path="url(#clip0_429_11023)">
              <path
                d="M6 19V10C6 6.68629 8.68629 4 12 4V4C15.3137 4 18 6.68629 18 10V19M6 19H18M6 19H4M18 19H20"
                class="dark:stroke-gray-400 stroke-gray-500"
                stroke-width="1.5"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
              <path
                d="M11 22L13 22"
                class="dark:stroke-gray-400 stroke-gray-500"
                stroke-width="1.5"
                stroke-linecap="round"
                stroke-linejoin="round"
              />
              <circle
                cx="12"
                cy="3"
                r="1"
                class="dark:stroke-gray-400 stroke-gray-500"
                stroke-width="1.5"
              />
            </g>
          </svg>
        </div>
        <div class="flex items-center cursor-pointer select-none">
          <img
            class="size-10 rounded-full object-cover mx-2"
            :src="userStore.user?.avatarUrl ?? apiUrl + '/user.png'"
            alt=""
          />
          <span class="dark:text-gray-200 text-gray-700"> {{ userStore.user?.fullName }} </span>
        </div>
      </div>
    </nav>
  </header>
</template>
<style scoped></style>
