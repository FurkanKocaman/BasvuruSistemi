<script setup lang="ts">
import { useThemeStore } from "@/stores/theme";
import { computed, onMounted, ref, type Ref } from "vue";
import type { LoginRequest } from "../types/LoginRequest";
import AuthService from "../services/AuthService";
import { useRouter } from "vue-router";

const themeStore = useThemeStore();
const router = useRouter();

const request: Ref<LoginRequest> = ref({
  usernameOrEmail: "",
  password: "",
});

const currentTheme = computed(() => themeStore.currentTheme);

onMounted(() => {});

type Theme = "light" | "dark" | "system";
const setTheme = (theme: Theme) => {
  themeStore.setTheme(theme);
};

async function login() {
  const res = await AuthService.login(request.value);
  if (res) {
    router.push({ name: "Jobs" });
  }
}
</script>
<template>
  <main class="w-[100dvw] h-[100dvh] flex">
    <div
      class="w-full md:w-[50dvw] xl:w-[50dvw] bg-neutral-100 dark:bg-gray-900 h-full flex flex-col justify-center items-center"
    >
      <div class="flex-1 flex items-center justify-center h-full min-w-[50%]">
        <form class="px-10 w-full" @submit.prevent="login()">
          <div
            class="w-full py-5 mb-5 text-3xl text-neutral-700 dark:text-neutral-200 font-semibold text-left select-none"
          >
            Giriş Yap
            <p class="text-sm font-normal mt-3 dark:text-gray-500">
              E-posta ve şifrenizle giriş yapın!
            </p>
          </div>
          <div class="mb-5">
            <label
              for="emailOrUsername"
              class="block mb-2 text-sm font-medium text-neutral-600 dark:text-neutral-300 select-none"
              >E-posta</label
            >
            <input
              type="text"
              id="emailOrUsername"
              name="emailOrUsername"
              v-model="request.usernameOrEmail"
              placeholder="user@gmail.com"
              class="border font-normal border-neutral-300 dark:border-gray-700 focus:border-indigo-500 focus:shadow-xl text-neutral-700 text-sm rounded-md outline-none bg-transparent dark:text-gray-200 block w-full p-3"
              required
            />
          </div>
          <div class="mb-5">
            <label
              for="password"
              class="block mb-2 text-sm font-medium text-neutral-600 dark:text-neutral-300 select-none"
              >Şifre</label
            >
            <input
              type="password"
              id="password"
              name="password"
              v-model="request.password"
              placeholder="******"
              class="border font-normal border-neutral-300 dark:border-gray-700 focus:border-indigo-500 focus:shadow-xl text-neutral-700 text-sm rounded-md outline-none bg-transparent dark:text-gray-200 block w-full p-3"
              required
            />
          </div>

          <div class="flex justify-between mb-5">
            <div class="flex items-center cursor-pointer">
              <div class="flex items-center h-5">
                <input
                  id="remember"
                  type="checkbox"
                  value=""
                  name="remember"
                  class="w-4 h-4 border accent-blue-600 rounded-sm dark:fill-gray-800 cursor-pointer"
                />
              </div>

              <label
                for="remember"
                class="ms-2 text-sm font-medium text-gray-900 dark:text-gray-300 select-none cursor-pointer"
                >Oturumu açık tut</label
              >
            </div>
            <div class="flex items-center">
              <label
                class="text-sm font-medium text-gray-900 dark:text-gray-400 dark:hover:text-gray-50 cursor-pointer select-none"
                >Şifremi unuttum?</label
              >
            </div>
          </div>
          <div class="flex items-center justify-center">
            <button
              type="submit"
              class="text-white bg-indigo-500 hover:bg-indigo-600 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm w-full px-5 py-2.5 text-center dark:bg-indigo-600 dark:hover:bg-indigo-700 select-none cursor-pointer"
            >
              Giriş Yap
            </button>
          </div>
          <div class="my-3">
            <router-link
              to="/auth/register"
              class="text-sm font-medium text-gray-900 dark:text-gray-300 hover:border-b border-neutral-400 cursor-pointer select-none"
              >Hesabınız yok mu?</router-link
            >
          </div>
          <div class="flex justify-center items-center my-8">
            <div class="flex-1 border-b border-neutral-400 dark:border-neutral-600 mx-3"></div>
            <div class="text-sm font-semibold text-neutral-600 dark:text-neutral-400 select-none">
              Yada
            </div>
            <div class="flex-1 border-b border-neutral-400 dark:border-neutral-600 mx-3"></div>
          </div>
          <div class="mt-3 flex justify-between mx-10">
            <button
              type="button"
              class="flex-1 flex justify-center items-center px-2 border py-2 hover:bg-neutral-200/40 dark:hover:bg-neutral-700/30 dark:border-neutral-500 rounded-sm ml-2"
            >
              <svg
                class="size-6"
                viewBox="-0.5 0 48 48"
                version="1.1"
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <g id="Icons" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                  <g id="Color-" transform="translate(-401.000000, -860.000000)">
                    <g id="Google" transform="translate(401.000000, 860.000000)">
                      <path
                        d="M9.82727273,24 C9.82727273,22.4757333 10.0804318,21.0144 10.5322727,19.6437333 L2.62345455,13.6042667 C1.08206818,16.7338667 0.213636364,20.2602667 0.213636364,24 C0.213636364,27.7365333 1.081,31.2608 2.62025,34.3882667 L10.5247955,28.3370667 C10.0772273,26.9728 9.82727273,25.5168 9.82727273,24"
                        id="Fill-1"
                        fill="#FBBC05"
                      ></path>
                      <path
                        d="M23.7136364,10.1333333 C27.025,10.1333333 30.0159091,11.3066667 32.3659091,13.2266667 L39.2022727,6.4 C35.0363636,2.77333333 29.6954545,0.533333333 23.7136364,0.533333333 C14.4268636,0.533333333 6.44540909,5.84426667 2.62345455,13.6042667 L10.5322727,19.6437333 C12.3545909,14.112 17.5491591,10.1333333 23.7136364,10.1333333"
                        id="Fill-2"
                        fill="#EB4335"
                      ></path>
                      <path
                        d="M23.7136364,37.8666667 C17.5491591,37.8666667 12.3545909,33.888 10.5322727,28.3562667 L2.62345455,34.3946667 C6.44540909,42.1557333 14.4268636,47.4666667 23.7136364,47.4666667 C29.4455,47.4666667 34.9177955,45.4314667 39.0249545,41.6181333 L31.5177727,35.8144 C29.3995682,37.1488 26.7323182,37.8666667 23.7136364,37.8666667"
                        id="Fill-3"
                        fill="#34A853"
                      ></path>
                      <path
                        d="M46.1454545,24 C46.1454545,22.6133333 45.9318182,21.12 45.6113636,19.7333333 L23.7136364,19.7333333 L23.7136364,28.8 L36.3181818,28.8 C35.6879545,31.8912 33.9724545,34.2677333 31.5177727,35.8144 L39.0249545,41.6181333 C43.3393409,37.6138667 46.1454545,31.6490667 46.1454545,24"
                        id="Fill-4"
                        fill="#4285F4"
                      ></path>
                    </g>
                  </g>
                </g>
              </svg>
              <span
                class="ml-3 font-semibold text-base text-neutral-600 dark:text-neutral-300 select-none"
                >Google</span
              >
            </button>
            <button
              type="button"
              class="flex-1 flex justify-center items-center px-2 border py-2 hover:bg-neutral-200/40 dark:hover:bg-neutral-700/30 dark:border-neutral-500 rounded-sm ml-2"
            >
              <svg
                class="size-6"
                viewBox="0 0 20 20"
                version="1.1"
                xmlns="http://www.w3.org/2000/svg"
                xmlns:xlink="http://www.w3.org/1999/xlink"
              >
                <g id="Page-1" stroke="none" stroke-width="1" fill="none" fill-rule="evenodd">
                  <g
                    id="Dribbble-Light-Preview"
                    transform="translate(-140.000000, -7559.000000)"
                    fill="#000000"
                  >
                    <g id="icons" transform="translate(56.000000, 160.000000)">
                      <path
                        d="M94,7399 C99.523,7399 104,7403.59 104,7409.253 C104,7413.782 101.138,7417.624 97.167,7418.981 C96.66,7419.082 96.48,7418.762 96.48,7418.489 C96.48,7418.151 96.492,7417.047 96.492,7415.675 C96.492,7414.719 96.172,7414.095 95.813,7413.777 C98.04,7413.523 100.38,7412.656 100.38,7408.718 C100.38,7407.598 99.992,7406.684 99.35,7405.966 C99.454,7405.707 99.797,7404.664 99.252,7403.252 C99.252,7403.252 98.414,7402.977 96.505,7404.303 C95.706,7404.076 94.85,7403.962 94,7403.958 C93.15,7403.962 92.295,7404.076 91.497,7404.303 C89.586,7402.977 88.746,7403.252 88.746,7403.252 C88.203,7404.664 88.546,7405.707 88.649,7405.966 C88.01,7406.684 87.619,7407.598 87.619,7408.718 C87.619,7412.646 89.954,7413.526 92.175,7413.785 C91.889,7414.041 91.63,7414.493 91.54,7415.156 C90.97,7415.418 89.522,7415.871 88.63,7414.304 C88.63,7414.304 88.101,7413.319 87.097,7413.247 C87.097,7413.247 86.122,7413.234 87.029,7413.87 C87.029,7413.87 87.684,7414.185 88.139,7415.37 C88.139,7415.37 88.726,7417.2 91.508,7416.58 C91.513,7417.437 91.522,7418.245 91.522,7418.489 C91.522,7418.76 91.338,7419.077 90.839,7418.982 C86.865,7417.627 84,7413.783 84,7409.253 C84,7403.59 88.478,7399 94,7399"
                      ></path>
                    </g>
                  </g>
                </g>
              </svg>
              <span
                class="ml-3 font-semibold text-base text-neutral-600 dark:text-neutral-300 select-none"
                >Github</span
              >
            </button>
          </div>
        </form>
      </div>
    </div>
    <div class="flex-1 dark:bg-gray-900/50 flex flex-col justify-end items-end">
      <div
        class="mb-5 mr-5 bg-indigo-600 rounded-full w-12 h-12 flex items-center justify-center cursor-pointer"
      >
        <svg
          v-if="currentTheme == 'light' || currentTheme == 'system'"
          class="size-6"
          viewBox="0 0 24 24"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
          v-on:click="setTheme('dark')"
        >
          <path
            d="M21.0672 11.8568L20.4253 11.469L21.0672 11.8568ZM12.1432 2.93276L11.7553 2.29085V2.29085L12.1432 2.93276ZM21.25 12C21.25 17.1086 17.1086 21.25 12 21.25V22.75C17.9371 22.75 22.75 17.9371 22.75 12H21.25ZM12 21.25C6.89137 21.25 2.75 17.1086 2.75 12H1.25C1.25 17.9371 6.06294 22.75 12 22.75V21.25ZM2.75 12C2.75 6.89137 6.89137 2.75 12 2.75V1.25C6.06294 1.25 1.25 6.06294 1.25 12H2.75ZM15.5 14.25C12.3244 14.25 9.75 11.6756 9.75 8.5H8.25C8.25 12.5041 11.4959 15.75 15.5 15.75V14.25ZM20.4253 11.469C19.4172 13.1373 17.5882 14.25 15.5 14.25V15.75C18.1349 15.75 20.4407 14.3439 21.7092 12.2447L20.4253 11.469ZM9.75 8.5C9.75 6.41182 10.8627 4.5828 12.531 3.57467L11.7553 2.29085C9.65609 3.5593 8.25 5.86509 8.25 8.5H9.75ZM12 2.75C11.9115 2.75 11.8077 2.71008 11.7324 2.63168C11.6686 2.56527 11.6538 2.50244 11.6503 2.47703C11.6461 2.44587 11.6482 2.35557 11.7553 2.29085L12.531 3.57467C13.0342 3.27065 13.196 2.71398 13.1368 2.27627C13.0754 1.82126 12.7166 1.25 12 1.25V2.75ZM21.7092 12.2447C21.6444 12.3518 21.5541 12.3539 21.523 12.3497C21.4976 12.3462 21.4347 12.3314 21.3683 12.2676C21.2899 12.1923 21.25 12.0885 21.25 12H22.75C22.75 11.2834 22.1787 10.9246 21.7237 10.8632C21.286 10.804 20.7293 10.9658 20.4253 11.469L21.7092 12.2447Z"
            class="fill-gray-50"
          />
        </svg>
        <svg
          v-if="currentTheme == 'dark'"
          class="size-7"
          viewBox="0 -0.5 25 25"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
          v-on:click="setTheme('light')"
        >
          <path
            fill-rule="evenodd"
            clip-rule="evenodd"
            d="M15.125 12C15.125 13.4497 13.9497 14.625 12.5 14.625C11.0503 14.625 9.875 13.4497 9.875 12C9.875 10.5503 11.0503 9.375 12.5 9.375C13.9497 9.375 15.125 10.5503 15.125 12Z"
            class="stroke-gray-50"
            stroke-width="1.5"
            stroke-linecap="round"
            stroke-linejoin="round"
          />
          <path
            d="M19.5 12.75C19.9142 12.75 20.25 12.4142 20.25 12C20.25 11.5858 19.9142 11.25 19.5 11.25V12.75ZM17.75 11.25C17.3358 11.25 17 11.5858 17 12C17 12.4142 17.3358 12.75 17.75 12.75V11.25ZM7.25 12.75C7.66421 12.75 8 12.4142 8 12C8 11.5858 7.66421 11.25 7.25 11.25V12.75ZM5.5 11.25C5.08579 11.25 4.75 11.5858 4.75 12C4.75 12.4142 5.08579 12.75 5.5 12.75V11.25ZM13.25 5C13.25 4.58579 12.9142 4.25 12.5 4.25C12.0858 4.25 11.75 4.58579 11.75 5H13.25ZM11.75 6.75C11.75 7.16421 12.0858 7.5 12.5 7.5C12.9142 7.5 13.25 7.16421 13.25 6.75H11.75ZM13.25 17.25C13.25 16.8358 12.9142 16.5 12.5 16.5C12.0858 16.5 11.75 16.8358 11.75 17.25H13.25ZM11.75 19C11.75 19.4142 12.0858 19.75 12.5 19.75C12.9142 19.75 13.25 19.4142 13.25 19H11.75ZM17.9803 7.58033C18.2732 7.28744 18.2732 6.81256 17.9803 6.51967C17.6874 6.22678 17.2126 6.22678 16.9197 6.51967L17.9803 7.58033ZM15.6817 7.75767C15.3888 8.05056 15.3888 8.52544 15.6817 8.81833C15.9746 9.11122 16.4494 9.11122 16.7423 8.81833L15.6817 7.75767ZM9.31833 16.2423C9.61122 15.9494 9.61122 15.4746 9.31833 15.1817C9.02544 14.8888 8.55056 14.8888 8.25767 15.1817L9.31833 16.2423ZM7.01967 16.4197C6.72678 16.7126 6.72678 17.1874 7.01967 17.4803C7.31256 17.7732 7.78744 17.7732 8.08033 17.4803L7.01967 16.4197ZM8.08033 6.51967C7.78744 6.22678 7.31256 6.22678 7.01967 6.51967C6.72678 6.81256 6.72678 7.28744 7.01967 7.58033L8.08033 6.51967ZM8.25767 8.81833C8.55056 9.11122 9.02544 9.11122 9.31833 8.81833C9.61122 8.52544 9.61122 8.05056 9.31833 7.75767L8.25767 8.81833ZM16.7433 15.1827C16.4504 14.8898 15.9756 14.8898 15.6827 15.1827C15.3898 15.4756 15.3898 15.9504 15.6827 16.2433L16.7433 15.1827ZM16.9197 17.4803C17.2126 17.7732 17.6874 17.7732 17.9803 17.4803C18.2732 17.1874 18.2732 16.7126 17.9803 16.4197L16.9197 17.4803ZM19.5 11.25H17.75V12.75H19.5V11.25ZM7.25 11.25H5.5V12.75H7.25V11.25ZM11.75 5V6.75H13.25V5H11.75ZM11.75 17.25V19H13.25V17.25H11.75ZM16.9197 6.51967L15.6817 7.75767L16.7423 8.81833L17.9803 7.58033L16.9197 6.51967ZM8.25767 15.1817L7.01967 16.4197L8.08033 17.4803L9.31833 16.2423L8.25767 15.1817ZM7.01967 7.58033L8.25767 8.81833L9.31833 7.75767L8.08033 6.51967L7.01967 7.58033ZM15.6827 16.2433L16.9197 17.4803L17.9803 16.4197L16.7433 15.1827L15.6827 16.2433Z"
            class="fill-gray-50"
          />
        </svg>
      </div>
    </div>
  </main>
</template>
