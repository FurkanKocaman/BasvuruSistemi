<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { TokenInformation } from "../models/token-information.model";
import roleService from "../services/role.service";
import { CircleX } from "lucide-vue-next";

const route = useRoute();
const router = useRouter();

const token = route.query.token as string | undefined;

const tokenInformation = ref<TokenInformation>({
  inviterName: "",
  roleName: "",
});

const errorMessages = ref<string[]>([]);

onMounted(() => {
  if (token) {
    console.log(token);
    getTokenInformation();
  }
});

const getTokenInformation = async () => {
  errorMessages.value = [];
  if (token) {
    try {
      const res = await roleService.getTokenInformation(token);
      tokenInformation.value = res.data;
    } catch (err) {
      errorMessages.value = err.response.data.errorMessages;
    }
  }
};

const reviewInvitation = async (value: boolean) => {
  if (value) {
    if (token) {
      const res = await roleService.verifyInvitation(token);
      if (res) {
        router.push("/management");
      }
    }
  } else {
    router.push("/");
  }
};
</script>

<template>
  <main class="flex justify-center h-[100dvh] items-center w-full">
    <div
      v-if="errorMessages.length == 0"
      class="border rounded-md px-3 py-2 bg-gray-900/40 border-gray-200 dark:border-gray-700 flex flex-col"
    >
      <span class="text-xl font-semibold text-gray-800 dark:text-gray-200 text-center"
        >Rol Daveti</span
      >
      <div class="my-3">
        <span class="text-gray-800 dark:text-gray-300"
          ><span class="font-semibold text-gray-950 dark:text-gray-50">{{
            tokenInformation.inviterName
          }}</span>
          <span
            v-if="tokenInformation.unitName"
            class="font-semibold text-gray-950 dark:text-gray-50 ml-1"
            >{{ tokenInformation.unitName }} biriminde</span
          >
          size
          <span class="font-semibold text-gray-950 dark:text-gray-50">{{
            tokenInformation.roleName
          }}</span>
          rolünü atamak istiyor</span
        >
      </div>
      <div class="flex justify-end my-3">
        <button
          class="px-2 py-1 rounded-md bg-red-500 text-gray-50 cursor-pointer"
          @click.stop="reviewInvitation(false)"
        >
          Reddet
        </button>
        <button
          class="px-2 py-1 rounded-md bg-green-600 text-gray-50 ml-3 cursor-pointer"
          @click.stop="reviewInvitation(true)"
        >
          Kabul Et
        </button>
      </div>
    </div>
    <div
      v-else
      class="border rounded-md px-3 py-2 bg-gray-900/40 border-gray-200 dark:border-gray-700 flex flex-col"
    >
      <div v-for="message in errorMessages" :key="message" class="my-3">
        <span class="border border-red-500 text-red-600 px-3 py-1.5 rounded-lg flex">
          <CircleX class="mr-2" /> {{ message }}</span
        >
      </div>
    </div>
  </main>
</template>
