import axios from "axios";
import type { LoginRequest } from "../types/LoginRequest";
import type { LoginResponse } from "../types/LoginResponse";

class AuthService {
  async login(request: LoginRequest): Promise<{ success: boolean; message: string }> {
    try {
      const response = await axios.post(`${import.meta.env.VITE_API_URL}/auth/login`, request);
      console.log(response);
      const loginResponse: LoginResponse = response.data.data;

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
