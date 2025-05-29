import Cookies from "js-cookie";
import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      redirect: "/jobs", // Ana sayfa artık iş ilanları sayfasına yönlendiriyor
    },
    {
      path: "/auth",
      name: "auth",
      children: [
        {
          path: "login",
          name: "login",
          component: () => import("@/modules/auth/pages/LoginPage.vue"),
        },
        {
          path: "register",
          name: "register",
          component: () => import("@/modules/auth/pages/RegisterPage.vue"),
        },
      ],
    },
    {
      path: "/",
      component: () => import("@/modules/home/layouts/HomeLayout.vue"),
      children: [
        {
          path: "jobs",
          name: "Jobs",
          component: () => import("@/modules/home/pages/JobsListingPage.vue"),
          meta: { title: "İlanlar" },
        },
        {
          path: "jobs/:id/apply",
          name: "JobApplication",
          component: () => import("@/modules/home/pages/JobApplicationPage.vue"),
          props: true,
          meta: { title: "İlan Başvuru" },
        },
        {
          path: "posting-groups/:id/apply",
          name: "PostingGroupApplication",
          component: () => import("@/modules/home/pages/PostingGroupApplicationPage.vue"),
          props: true,
          meta: { title: "İlan Başvuru" },
        },
        {
          path: "my-applications",
          name: "MyApplications",
          component: () => import("@/modules/home/pages/MyApplicationsPage.vue"),
          meta: { title: "Başvurularım" },
        },
      ],
    },
    {
      path: "/tenants",
      name: "tenants",
      component: () => import("@/modules/management/pages/TenantPage.vue"),
      meta: { title: "Kurumlar", requiresAuth: true },
    },
    {
      path: "/profile",
      name: "profile",
      component: () => import("@/modules/profile/pages/ProfilePage.vue"),
      meta: { title: "Profil", requiresAuth: true },
    },
    {
      path: "/management",
      name: "management",
      component: () => import("@/modules/management/layouts/ManagementLayout.vue"),
      redirect: "/management/job-postings",
      meta: { title: "Yönetim", requiresAuth: true, requireTenant: true },
      children: [
        {
          path: "job-postings",
          name: "job-posting",
          redirect: "",
          meta: { title: "İlanlar", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "job-posting-list",
              component: () => import("@/modules/management/pages/job-posting/JobPostingPage.vue"),
            },
            {
              path: "create",
              name: "job-posting-create",
              component: () =>
                import("@/modules/management/pages/job-posting/JobpostingCreatePage.vue"),
              meta: { title: "İlan Oluştur", requiresAuth: true, requireTenant: true },
            },
            {
              path: "update/:id",
              name: "job-posting-update",
              component: () =>
                import("@/modules/management/pages/job-posting/JobpostingCreatePage.vue"),
              meta: { title: "İlan Güncelle", requiresAuth: true, requireTenant: true },
            },
          ],
        },
        {
          path: "job-postings-group",
          name: "job-posting-group",
          redirect: "create",
          meta: { title: "İlan Grubu", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "create",
              name: "job-posting-group-create",
              component: () =>
                import("@/modules/management/pages/job-posting/JobpostingGroupCreatePage.vue"),
              meta: { title: "İlan Grubu Oluştur", requiresAuth: true, requireTenant: true },
            },
            {
              path: "update/:id",
              name: "job-posting-group-update",
              component: () =>
                import("@/modules/management/pages/job-posting/JobpostingGroupCreatePage.vue"),
              meta: { title: "ilan Grubu Güncelle", requiresAuth: true, requireTenant: true },
            },
          ],
        },
        {
          path: "form-templates",
          name: "form-templates",
          redirect: "",
          meta: { title: "Şablonlar", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "form-templates-list",
              component: () =>
                import("@/modules/management/pages/form-templates/FormTemplatesPage.vue"),
            },
            {
              path: "create",
              name: "form-templates-create",
              component: () =>
                import("@/modules/management/pages/form-templates/FormTemplateCreatePage.vue"),
              meta: { title: "Şablon Oluştur", requiresAuth: true, requireTenant: true },
            },
            {
              path: "update/:id",
              name: "form-templates-update",
              component: () =>
                import("@/modules/management/pages/form-templates/FormTemplateCreatePage.vue"),
              meta: { title: "Şablon Güncelle", requiresAuth: true, requireTenant: true },
            },
          ],
        },

        {
          path: "applications",
          name: "applications",
          redirect: { name: "applications-list" },
          meta: { title: "Başvurular", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "applications-list",
              component: () => import("@/modules/management/pages/ApplicationsPage.vue"),
            },
            {
              path: "detail/:id",
              name: "application-detail",
              component: () => import("@/modules/management/pages/ApplicationDetailPage.vue"),
              meta: { title: "Başvuru Detayı", requiresAuth: true, requireTenant: true },
            },
          ],
        },
        {
          path: "pending-evaluations",
          name: "pending-evaluations",
          redirect: { name: "pending-evaluations-list" },
          meta: {
            title: "Değerlendirme Bekleyen Başvurular",
            requiresAuth: true,
            requireTenant: true,
          },
          children: [
            {
              path: "",
              name: "pending-evaluations-list",
              component: () => import("@/modules/management/pages/PendingEvaluationsPage.vue"),
            },
            {
              path: "evaluate/:id",
              name: "pending-evaluations-evaluate",
              component: () =>
                import(
                  "@/modules/management/pages/application-evaluations/ApplicationEvaluatePage.vue"
                ),
            },
          ],
        },
        {
          path: "units",
          name: "units",
          redirect: { name: "units-tree-list" },
          meta: { title: "Birimler", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "units-tree-list",
              component: () => import("@/modules/management/pages/UnitsPage.vue"),
            },
          ],
        },
        {
          path: "users",
          name: "users",
          redirect: { name: "users-list" },
          meta: { title: "Kullanıcılar", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "users-list",
              component: () => import("@/modules/management/pages/UsersPage.vue"),
            },
          ],
        },
        {
          path: "commissions",
          name: "commissions",
          redirect: { name: "commissions-list" },
          meta: { title: "Komisyonlar", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "commissions-list",
              component: () => import("@/modules/management/pages/CommissionsPage.vue"),
            },
            {
              path: "create",
              name: "commission-create",
              component: () => import("@/modules/management/pages/CreateCommissionPage.vue"),
            },
            {
              path: "update/:id",
              name: "commission-update",
              component: () => import("@/modules/management/pages/CreateCommissionPage.vue"),
            },
          ],
        },
        {
          path: "evaluation-stages",
          name: "evaluation-stages",
          redirect: { name: "evaluation-stages-list" },
          meta: { title: "Değerlendirme Adımları", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "evaluation-stages-list",
              component: () =>
                import("@/modules/management/pages/evaluations/EvaluationStagesPage.vue"),
            },
            {
              path: "create",
              name: "evaluations-stage-create",
              component: () =>
                import("@/modules/management/pages/evaluations/EvaluationStageCreatePage.vue"),
            },
            {
              path: "update/:id",
              name: "evaluations-stage-update",
              component: () =>
                import("@/modules/management/pages/evaluations/EvaluationStageCreatePage.vue"),
            },
            {
              path: "create-form/:id",
              name: "evaluations-form-create",
              component: () =>
                import("@/modules/management/pages/evaluations/EvaluationFormCreatePage.vue"),
            },
            {
              path: "update-form/:id",
              name: "evaluations-form-update",
              component: () =>
                import("@/modules/management/pages/evaluations/EvaluationFormCreatePage.vue"),
            },
          ],
        },

        {
          path: "commissions",
          name: "commissions",
          redirect: { name: "commissions-list" },
          meta: { title: "Komisyonlar", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "commissions-list",
              component: () => import("@/modules/management/pages/CommissionsPage.vue"),
            },
            {
              path: "create",
              name: "commission-create",
              component: () => import("@/modules/management/pages/CreateCommissionPage.vue"),
            },
            {
              path: "update/:id",
              name: "commission-update",
              component: () => import("@/modules/management/pages/CreateCommissionPage.vue"),
            },
          ],
        },
        {
          path: "roles",
          name: "roles",
          redirect: { name: "roles-list" },
          meta: { title: "Roller", requiresAuth: true, requireTenant: true },
          children: [
            {
              path: "",
              name: "roles-list",
              component: () => import("@/modules/management/pages/RolesPage.vue"),
            },
          ],
        },
      ],
    },
    {
      path: "/invitation",
      name: "role-invitation",
      component: () => import("@/modules/management/pages/RoleInviteVerifyPage.vue"),
    },
  ],
});

// Archive module routes
router.addRoute({
  path: "/archive",
  component: () => import("@/modules/archive/layouts/ArchiveLayout.vue"),
  children: [
    {
      path: "",
      name: "archive",
      component: () => import("@/modules/archive/pages/ArchivePage.vue"),
      meta: { title: "Arşiv", requiresAuth: true, requireTenant: true },
    },
    {
      path: ":year",
      name: "archive-year",
      component: () => import("@/modules/archive/pages/ArchiveYearPage.vue"),
      props: true,
      meta: { title: "Yıllık Arşiv", requiresAuth: true, requireTenant: true },
    },
  ],
});

router.beforeEach((to, from, next) => {
  const accessToken = localStorage.getItem("accessToken");

  document.title = to.meta.title ? `${to.meta.title} | Başvuru Sistemi` : "Başvuru Sistemi";

  if (to.matched.some((record) => record.meta.requiresAuth) && !accessToken) {
    next({ name: "login", query: { returnUrl: to.fullPath } });
  } else if (to.matched.some((record) => record.meta.requireTenant) && !Cookies.get("tenantId")) {
    next({ name: "tenants", query: { returnUrl: to.fullPath } });
  } else {
    next();
  }
});

export default router;
