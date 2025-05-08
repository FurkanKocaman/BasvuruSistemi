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
      path: "/management",
      name: "management",
      component: () => import("@/modules/management/layouts/ManagementLayout.vue"),
      redirect: "/management/job-postings",
      children: [
        {
          path: "job-postings",
          name: "job-posting",
          redirect: "",
          children: [
            {
              path: "",
              name: "job-posting-list",
              component: () => import("@/modules/management/pages/JobPostingPage.vue"),
            },
            {
              path: "create",
              name: "job-posting-create",
              component: () => import("@/modules/management/pages/JobpostingCreatePage.vue"),
            },
          ],
        },
      ],
    },
  ],
});

// Sayfa başlıklarını ayarlama
router.beforeEach((to, from, next) => {
  // Sayfa başlığını meta bilgisinden alıp ayarlama
  document.title = to.meta.title ? `${to.meta.title} | Başvuru Sistemi` : "Başvuru Sistemi";
  next();
});

export default router;
