<script setup lang="ts">
import { onMounted } from "vue";
import { useThemeStore } from "./stores/theme";
import { fetchCurrentUser } from "./services/current-user.service";
import ToastContainer from "./modules/toast/components/ToastContainer.vue";

const themeStore = useThemeStore();

onMounted(() => {
  themeStore.initTheme();
  fetchCurrentUser();
});
</script>

<template>
  <ToastContainer />
  <RouterView />
</template>

<style scoped></style>
