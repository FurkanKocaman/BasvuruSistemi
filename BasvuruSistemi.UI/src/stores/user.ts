// stores/user.store.ts
import type { CurrentUserModel } from "@/models/entities/current-user.model";
import { defineStore } from "pinia";
import { ref } from "vue";

export const useUserStore = defineStore("user", () => {
  const user = ref<CurrentUserModel | null>(null);

  function setUser(payload: CurrentUserModel) {
    console.log(payload);
    user.value = payload;
  }

  function clearUser() {
    user.value = null;
  }

  return {
    user,
    setUser,
    clearUser,
  };
});
