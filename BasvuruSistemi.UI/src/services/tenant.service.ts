import { Tenant } from "@/models/entities/tenant.model";
import api from "./Axios";
import { Result } from "@/models/entities/result.model";
import { LoginResponse } from "@/modules/auth/types/LoginResponse";

export async function fetchTenants(): Promise<Tenant[] | undefined> {
  try {
    const response = await api.get(`${import.meta.env.VITE_API_URL}/api/tenants`);

    return response.data;
  } catch (error) {
    console.error("Tenants bulunamadı:", error);
  }
}

export async function refreshJwtByTenant() {
  try {
    const res = await api.get<Result<LoginResponse>>(
      `${import.meta.env.VITE_API_URL}/tenants/refresh`
    );
    localStorage.setItem("accessToken", res.data.data.accessToken);
    localStorage.setItem("refreshToken", res.data.data.refreshToken);
  } catch (err) {
    console.error(err);
  }
}
