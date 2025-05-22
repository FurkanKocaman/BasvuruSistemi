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
