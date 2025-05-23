export type ClaimType =
  | "jobposting.create"
  | "jobposting.edit"
  | "jobposting.delete"
  | "jobposting.view"
  | "application.create"
  | "application.view"
  | "application.review"
  | "application.approve"
  | "user.manage"
  | "role.manage"
  | "tenant.manage"
  | "unit.manage"
  | "template.manage"
  | "formfield.manage"
  | "document.download";

export const ClaimTypes = {
  CreateJobPosting: "jobposting.create",
  EditJobPosting: "jobposting.edit",
  DeleteJobPosting: "jobposting.delete",
  ViewJobPosting: "jobposting.view",

  CreateApplication: "application.create",
  ViewApplications: "application.view",
  ReviewApplications: "application.review",
  ApproveApplications: "application.approve",

  ManageUsers: "user.manage",
  ManageRoles: "role.manage",
  ManageTenants: "tenant.manage",
  ManageUnits: "unit.manage",

  ManageTemplates: "template.manage",
  ManageFormFields: "formfield.manage",

  DownloadDocuments: "document.download",
} as const;

export const claimOptions = [
  { value: "jobposting.create", label: "İlan Oluşturma" },
  { value: "jobposting.edit", label: "İlan Düzenleme" },
  { value: "jobposting.delete", label: "İlan Silme" },
  { value: "jobposting.view", label: "İlanları Görüntüleme" },

  { value: "application.create", label: "Başvuru Oluşturma" },
  { value: "application.view", label: "Başvuruları Görüntüleme" },
  { value: "application.review", label: "Başvuruları İnceleme" },
  { value: "application.approve", label: "Başvuruları Onaylama" },

  { value: "user.manage", label: "Kullanıcı Yönetimi" },
  { value: "role.manage", label: "Rol Yönetimi" },
  { value: "tenant.manage", label: "Tenant Yönetimi" },
  { value: "unit.manage", label: "Birim Yönetimi" },

  { value: "template.manage", label: "Şablon Yönetimi" },
  { value: "formfield.manage", label: "Form Alanı Yönetimi" },

  { value: "document.download", label: "Belge İndirme" },
];
