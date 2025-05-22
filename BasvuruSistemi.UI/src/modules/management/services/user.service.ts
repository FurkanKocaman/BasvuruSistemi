import { PaginatedResponse } from "@/models/response/paginated-response.model";
import { UserSummariesModel } from "@/modules/management/models/user-summaries.model";
import api from "@/services/Axios";

class UserService {
  async getAllUsers(
    page: number,
    pageSize: number
  ): Promise<PaginatedResponse<UserSummariesModel>> {
    try {
      const res = await api.get<PaginatedResponse<UserSummariesModel>>(
        `${import.meta.env.VITE_API_URL}/api/users?page=${page}&pageSize=${pageSize}`
      );

      return res.data;
    } catch (err) {
      console.error(err);
      throw err;
    }
  }
}

export default new UserService();
