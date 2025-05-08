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
      // İş başvuru sistemi için yeni rotalar
      path: "/",
      component: () => import("@/modules/home/layouts/HomeLayout.vue"),
      children: [
        {
          path: "jobs",
          name: "Jobs",
          component: () => import("@/modules/home/pages/JobsListingPage.vue"),
          meta: { title: "İş İlanları" }
        },
        {
          path: "jobs/:id/apply",
          name: "JobApplication",
          component: () => import("@/modules/home/pages/JobApplicationPage.vue"),
          props: true,
          meta: { title: "İş Başvurusu" }
        },
        {
          path: "my-applications",
          name: "MyApplications",
          component: () => import("@/modules/home/pages/MyApplicationsPage.vue"),
          meta: { title: "Başvurularım" }
        }
      ]
    },
  ],
});

// Sayfa başlıklarını ayarlama
router.beforeEach((to, from, next) => {
  // Sayfa başlığını meta bilgisinden alıp ayarlama
  document.title = to.meta.title ? `${to.meta.title} | Başvuru Sistemi` : 'Başvuru Sistemi';
  next();
});

export default router;
