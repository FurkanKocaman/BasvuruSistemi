import api from "@/services/Axios";

class OrganizationService {
  async getOrganizationsByTenant(): Promise<{ name: string; unitId: string }[] | undefined> {
    try {
      const res = await api.get(`${import.meta.env.VITE_API_URL}/api/units`);

      return res.data;
    } catch (ex) {
      console.error(ex);
    }
  }
}

export default new OrganizationService();
