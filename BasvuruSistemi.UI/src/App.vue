<script setup lang="ts">
import { onMounted } from "vue";
import { useThemeStore } from "./stores/theme";
import { fetchCurrentUser } from "./services/current-user.service";

const themeStore = useThemeStore();

onMounted(() => {
  themeStore.initTheme();
  fetchCurrentUser();
});
</script>

<template>
  <RouterView />
</template>

<style scoped></style>
