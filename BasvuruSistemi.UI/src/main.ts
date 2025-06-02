import "./assets/main.css";
import { createApp } from "vue";
import { createPinia } from "pinia";
import PrimeVue from "primevue/config";
import App from "./App.vue";
import router from "./router";
import { useUserStore } from "./stores/user";
import { fetchCurrentUser } from "./services/current-user.service";

const app = createApp(App);

app.use(PrimeVue, {
  locale: {
    firstDayOfWeek: 1,
    dayNames: ["Pazar", "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi"],
    dayNamesShort: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"],
    dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
    monthNames: [
      "Ocak",
      "Şubat",
      "Mart",
      "Nisan",
      "Mayıs",
      "Haziran",
      "Temmuz",
      "Ağustos",
      "Eylül",
      "Ekim",
      "Kasım",
      "Aralık",
    ],
    monthNamesShort: [
      "Oca",
      "Şub",
      "Mar",
      "Nis",
      "May",
      "Haz",
      "Tem",
      "Ağu",
      "Eyl",
      "Eki",
      "Kas",
      "Ara",
    ],
    today: "Bugün",
    clear: "Temizle",
    dateFormat: "dd.mm.yy",
    weekHeader: "Hf",
  },
});
app.use(router);
app.use(createPinia());

const userStore = useUserStore();

router.beforeEach(async (to, from, next) => {
  await fetchCurrentUser();
  if (!userStore.user && to.meta.requiresAuth) {
    next("/auth/login");
  } else {
    next();
  }
});

app.mount("#app");
