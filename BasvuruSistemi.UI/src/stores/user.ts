// stores/user.store.ts
import type { CurrentUserModel } from "@/models/entities/current-user.model";
import { fetchCurrentUser } from "@/services/current-user.service";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useUserStore = defineStore("user", () => {
  const user = ref<CurrentUserModel | null>(null);

  function setUser(payload: CurrentUserModel) {
    user.value = payload;
  }

  async function getUser() {
    fetchCurrentUser();
  }

  function clearUser() {
    user.value = null;
  }

  return {
    user,
    setUser,
    getUser,
    clearUser,
  };
});
