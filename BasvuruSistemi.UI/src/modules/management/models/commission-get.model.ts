import { EntityDto } from "@/models/entities/entity.model";
import { CommissionMemberGetModel } from "./commission-member-get.model";

export interface CommissionGetModel extends EntityDto {
  id: string;
  name: string;
  description?: string;
  commissionMembers: CommissionMemberGetModel[];
}
