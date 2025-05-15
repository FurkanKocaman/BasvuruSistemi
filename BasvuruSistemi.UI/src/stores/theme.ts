import { defineStore } from "pinia";
import { ref, computed, watch } from "vue";

type Theme = "light" | "dark" | "system";

export const useThemeStore = defineStore("theme", () => {
  const currentTheme = ref<Theme>("dark");

  const prefersDark = window.matchMedia("(prefers-color-scheme: dark)");
  const isSystemDark = computed(() => prefersDark.matches);

  const isDark = computed(() => {
    return currentTheme.value === "dark" || (currentTheme.value === "system" && isSystemDark.value);
  });

  function applyTheme() {
    const root = document.documentElement;
    root.classList.remove("dark");
    console.log(currentTheme.value);
    if (isDark.value) {
      root.classList.add("dark");
    }
  }

  function setTheme(theme: Theme) {
    currentTheme.value = theme;
    localStorage.setItem("theme", theme);
    applyTheme();
  }

  function initTheme() {
    const saved = localStorage.getItem("theme") as Theme | null;
    currentTheme.value = saved ?? "system";

    applyTheme();
  }

  prefersDark.addEventListener("change", applyTheme);

  watch(currentTheme, applyTheme);

  return {
    currentTheme,
    isDark,
    setTheme,
    initTheme,
  };
});
