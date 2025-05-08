import api from "./Axios";
import { useUserStore } from "@/stores/user";

export async function fetchCurrentUser() {
  try {
    const response = await api.get(`${import.meta.env.VITE_API_URL}/api/user/current`);
    console.log("response", response);
    const userStore = useUserStore();
    userStore.setUser(response.data);
  } catch (error) {
    console.error("Kullanıcı bilgisi alınamadı:", error);
  }
}
