import { Tenant } from "@/models/entities/tenant.model";
import api from "./Axios";

export async function fetchTenants(): Promise<Tenant[] | undefined> {
  try {
    const response = await api.get(`${import.meta.env.VITE_API_URL}/api/tenants`);
    console.log("response", response);

    return response.data;
  } catch (error) {
    console.error("Tenants bulunamadı:", error);
  }
}
