<script setup lang="ts">
import { onMounted } from "vue";
import { useThemeStore } from "./stores/theme";
import ToastContainer from "./modules/toast/components/ToastContainer.vue";
import { fetchCurrentUser } from "./services/current-user.service";

const themeStore = useThemeStore();

onMounted(async () => {
  themeStore.initTheme();
  await fetchCurrentUser();
});
</script>

<template>
  <ToastContainer />
  <RouterView />
</template>

<style scoped></style>
