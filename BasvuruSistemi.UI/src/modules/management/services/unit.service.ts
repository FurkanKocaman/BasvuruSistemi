import api from "@/services/Axios";
import { Unit } from "../models/unit-node.model";
import { UnitCreateModel } from "../models/unit-create.model";
import { Result } from "@/models/entities/result.model";
import { useToastStore } from "@/modules/toast/store/toast.store";

class UnitService {
  toastStore = useToastStore();
  async createUnit(request: UnitCreateModel): Promise<Result<string>> {
    try {
      const res = await api.post<Result<string>>(
        `${import.meta.env.VITE_API_URL}/api/units`,
        request
      );

      if (res.status == 200) {
        this.toastStore.addToast({
          message: res.data.data,
          type: "success",
          duration: 3000,
        });
      } else {
        this.toastStore.addToast({
          message: res.data.data,
          type: "error",
          duration: 3000,
        });
      }

      return res.data;
    } catch (ex) {
      console.error(ex);
      throw ex;
    }
  }

  async getUnit(id: string): Promise<Unit | undefined> {
    try {
      const res = await api.get(`${import.meta.env.VITE_API_URL}/api/units/${id}`);

      return res.data;
    } catch (ex) {
      console.error(ex);
    }
  }

  async getUnitsByTenant(): Promise<Unit[] | undefined> {
    try {
      const res = await api.get(`${import.meta.env.VITE_API_URL}/api/units`);

      return res.data;
    } catch (ex) {
      console.error(ex);
    }
  }
  async updateUnit(id: string, request: UnitCreateModel): Promise<Result<string>> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/api/units/update/${id}`,
        request
      );

      this.toastStore.addToast({
        message: res.data.data,
        type: res.status === 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data;
    } catch (ex) {
      console.error(ex);
      throw ex;
    }
  }

  async deleteUnit(id: string): Promise<Result<string>> {
    try {
      const res = await api.delete<Result<string>>(
        `${import.meta.env.VITE_API_URL}/api/units/delete/${id}`
      );

      this.toastStore.addToast({
        message: res.data.data,
        type: res.status === 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data;
    } catch (ex) {
      console.error(ex);
      throw ex;
    }
  }
}

export default new UnitService();
