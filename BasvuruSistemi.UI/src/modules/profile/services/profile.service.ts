import api from "@/services/Axios";
import { Certificate, Education, Profile, Skill, WorkExperience } from "../models/profile.model";
import { EducationCreateModel } from "../models/education-create.model";
import { useToastStore } from "@/modules/toast/store/toast.store";
import { Result } from "@/models/entities/result.model";
import { CertificateCreateModel } from "../models/certificate-create.model";
import { ExperienceCreateModel } from "../models/experience-create.model";
import { SkillCreateModel } from "../models/skill-create.model";
import { UserUpdatemodel } from "../models/user-update.model";
import { AddressUpdateModel } from "../models/address-update.model";
import { Address } from "@/models/entities/address-model";

class ProfileService {
  toastStore = useToastStore();

  async getUserProfile(id?: string): Promise<Profile> {
    try {
      const res = id
        ? await api.get<Profile>(`${import.meta.env.VITE_API_URL}/api/users/profile?id=${id}`)
        : await api.get<Profile>(`${import.meta.env.VITE_API_URL}/api/users/profile`);
      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async updateUser(request: UserUpdatemodel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/${request.id}`,
        request
      );
      this.toastStore.addToast({
        message: res.data.data,
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async updateUserAvatar(file: File): Promise<string> {
    try {
      const formData = new FormData();
      formData.append("file", file);
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/avatar`,
        formData
      );
      this.toastStore.addToast({
        message: res.data.data,
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async createAddress(request: AddressUpdateModel): Promise<Address> {
    try {
      const res = await api.post<Result<Address>>(
        `${import.meta.env.VITE_API_URL}/users/address`,
        request
      );
      this.toastStore.addToast({
        message: "Adres oluşturuldu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async deleteAddress(id: string): Promise<string> {
    try {
      const res = await api.delete<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/address/${id}`
      );
      this.toastStore.addToast({
        message: "Adres silindi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async updateAddress(id: string, request: AddressUpdateModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/address/${id}`,
        request
      );
      this.toastStore.addToast({
        message: "Adres güncellendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async createSkill(request: SkillCreateModel): Promise<Skill> {
    try {
      const res = await api.post<Result<Skill>>(
        `${import.meta.env.VITE_API_URL}/users/skill`,
        request
      );
      this.toastStore.addToast({
        message: "Yetenek eklendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateSkill(request: SkillCreateModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/skill/${request.id}`,
        {
          skill: request,
        }
      );
      this.toastStore.addToast({
        message: "Yetenek güncellendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async deleteSkill(id: string): Promise<string> {
    try {
      const res = await api.delete(`${import.meta.env.VITE_API_URL}/users/skill/${id}`);

      this.toastStore.addToast({
        message: res.data.data,
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async createExperience(request: ExperienceCreateModel): Promise<WorkExperience> {
    try {
      const res = await api.post<Result<WorkExperience>>(
        `${import.meta.env.VITE_API_URL}/users/experience`,
        request
      );
      this.toastStore.addToast({
        message: "Deneyim eklendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateExperience(request: ExperienceCreateModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/experience/${request.id}`,
        {
          experience: request,
        }
      );
      this.toastStore.addToast({
        message: "Deneyim güncellendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async deleteExperience(id: string): Promise<string> {
    try {
      const res = await api.delete(`${import.meta.env.VITE_API_URL}/users/experience/${id}`);

      this.toastStore.addToast({
        message: res.data.data,
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async createCertificate(request: CertificateCreateModel): Promise<Certificate> {
    try {
      const res = await api.post<Result<Certificate>>(
        `${import.meta.env.VITE_API_URL}/users/certificates`,
        request
      );
      this.toastStore.addToast({
        message: "Sertifika eklendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateCertificate(request: CertificateCreateModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/certificates/${request.id}`,
        {
          certificate: request,
        }
      );
      this.toastStore.addToast({
        message: "Sertifika güncellendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async deleteCertificate(id: string): Promise<string> {
    try {
      const res = await api.delete(`${import.meta.env.VITE_API_URL}/users/certificates/${id}`);

      this.toastStore.addToast({
        message: res.data.data,
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  async createEducation(request: EducationCreateModel): Promise<Education> {
    try {
      const res = await api.post<Result<Education>>(
        `${import.meta.env.VITE_API_URL}/users/education`,
        request
      );
      console.log(res);
      this.toastStore.addToast({
        message: "Eğitim bilgisi oluşturuldu",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async updateEducation(request: EducationCreateModel): Promise<string> {
    try {
      const res = await api.put<Result<string>>(
        `${import.meta.env.VITE_API_URL}/users/education/${request.id}`,
        {
          education: request,
        }
      );
      this.toastStore.addToast({
        message: "Eğitim bilgisi güncellendi",
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });
      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
  async deleteEducation(id: string): Promise<string> {
    try {
      const res = await api.delete(`${import.meta.env.VITE_API_URL}/users/education/${id}`);

      this.toastStore.addToast({
        message: res.data.data,
        type: res.status == 200 ? "success" : "error",
        duration: 3000,
      });

      return res.data.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  // Profil bilgilerini güncelle
  static async updateProfile(profile: Profile): Promise<boolean> {
    // Şu an için sadece başarılı olduğunu varsayıyoruz
    // Gerçek uygulamada API'ye gönderilecek
    console.log("Profil güncellendi:", profile);
    return true;
  }
}

export default new ProfileService();
