import axios from "axios";
import type { LoginRequest } from "../types/LoginRequest";
import type { LoginResponse } from "../types/LoginResponse";
import { RegisterRequest } from "../types/RegisterReqeust";
import { useRouter } from "vue-router";

class AuthService {
  router = useRouter();
  async login(request: LoginRequest): Promise<{ success: boolean; message: string }> {
    try {
      const response = await axios.post(`${import.meta.env.VITE_API_URL}/auth/login`, request);
      const loginResponse: LoginResponse = response.data.data;

      localStorage.setItem("accessToken", loginResponse.accessToken);
      localStorage.setItem("refreshToken", loginResponse.refreshToken);
      this.router.push({ name: "Jobs" });
      return { success: true, message: "Giriş başarılı" };
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("Error while logging in", error);
        if (error.response) {
          if (error.response.status === 401) {
            return { success: false, message: "UnAuthorized" };
          }
          return { success: false, message: "Kullanıcı adı veya şifre hatalı" };
        }
        return { success: false, message: "Sunucuya bağlanılırken hata oluştu" };
      }
      return { success: false, message: "Beklenmeyen bir hata oluştu" };
    }
  }

  async register(request: RegisterRequest): Promise<{ success: boolean; message: string }> {
    try {
      const res = await axios.post(`${import.meta.env.VITE_API_URL}/auth/sign-up`, request);

      const loginResponse: LoginResponse = res.data.data;

      localStorage.setItem("accessToken", loginResponse.accessToken);
      localStorage.setItem("refreshToken", loginResponse.refreshToken);

      return { success: true, message: "Giriş başarılı" };
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.error("Error while logging in", error);
        if (error.response) {
          if (error.response.status === 401) {
            return { success: false, message: "UnAuthorized" };
          }
          return { success: false, message: "Kullanıcı adı veya şifre hatalı" };
        }
        return { success: false, message: "Sunucuya bağlanılırken hata oluştu" };
      }
      return { success: false, message: "Beklenmeyen bir hata oluştu" };
    }
  }
}

export default new AuthService();
