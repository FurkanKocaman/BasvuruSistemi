// stores/toast.store.ts
import { ref } from "vue";
import { defineStore } from "pinia";

export type ToastType = "success" | "error" | "info" | "warning";

export interface Toast {
  id: number;
  message: string;
  type: ToastType;
  duration?: number;
}

export const useToastStore = defineStore("toast", () => {
  const toasts = ref<Toast[]>([]);

  const addToast = (toast: Omit<Toast, "id">) => {
    const id = Date.now();
    const newToast: Toast = { id, ...toast };
    toasts.value.push(newToast);
    console.log(newToast);
    setTimeout(() => removeToast(id), toast.duration ?? 3000);
  };

  const removeToast = (id: number) => {
    toasts.value = toasts.value.filter((t) => t.id !== id);
  };

  return {
    toasts,
    addToast,
    removeToast,
  };
});
