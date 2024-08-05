import { createApp } from 'vue';
import { createRouter, createWebHistory } from "vue-router";
import Home from './views/Home.vue';
import Contacts from './views/Contacts.vue';
import Profile from './views/Profile.vue';
import UserProfile from './views/UserProfile.vue';
import DateDto from './views/Date.vue';
import Dates from './views/Dates.vue';
import Orders from './views/Orders.vue';
import Matches from './views/Matches.vue';
import Gallery from "./views/Gallery.vue";
import Admin from "./views/Admin.vue";
import PrivacyPolicy from "./views/PrivacyPolicy.vue";
import TermsAndConditions from "./views/TermsAndConditions.vue";
import { createI18n } from 'vue-i18n';
import App from './App.vue';
import mitt from 'mitt';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.min.css";
import "bootstrap";
import "easing";
import "./assets/lib/animate/animate.min.css";
import "./assets/lib/owlcarousel/assets/owl.carousel.min.css";
import "./assets/css/style.css";
import "./assets/js/main.js";
import moment from 'moment';
import authConfig from "./auth_config.json";
import en from './assets/languages/en.json';
import bg from './assets/languages/bg.json';
import ro from './assets/languages/ro.json';
import ru from './assets/languages/ru.json';
import cookies from './helpers/cookies';
import { createAuth0, createAuthGuard } from "@auth0/auth0-vue";

Date.prototype.toJSON = function () { return moment(this).format(); }
const emitter = mitt();
const i18n = createI18n({
	locale: cookies.get('language') || 'en',
	legacy: false,
	locale: cookies.get('language') || 'en',
	fallbackLocale: 'en',
	formatFallbackMessages: true,
	messages: {
		en: en,
		bg: bg,
		ro: ro,
		ru: ru
	}
});
let measurementId = 'G-B686JH31RT';
switch (window.location.host) {
	case 'uk.sreshti.net':
		measurementId = 'G-EWR2YD00DZ';
		break;
}

const app = createApp(App);
app.config.globalProperties.emitter = emitter;
app.config.productionTip = false;
app.use(i18n)
	.use(createRouter({
		history: createWebHistory(import.meta.env.BASE_URL),
		linkActiveClass: 'active',
		routes: [
			{ path: '/', component: Home },
			{ path: '/home', component: Home },
			{ path: '/contacts', component: Contacts },
			{ path: '/profile', component: Profile, beforeEnter: createAuthGuard(app) },
			{ path: '/user-profile', component: UserProfile, beforeEnter: createAuthGuard(app) },
			{ path: '/user-gallery', component: Gallery, beforeEnter: createAuthGuard(app) },
			{ path: '/date', component: DateDto },
			{ path: '/dates', component: Dates },
			{ path: '/orders', component: Orders, beforeEnter: createAuthGuard(app) },
			{ path: '/matches', component: Matches, beforeEnter: createAuthGuard(app) },
			{ path: '/admin', component: Admin, beforeEnter: createAuthGuard(app) },
			{ path: '/privacy-policy', component: PrivacyPolicy },
			{ path: '/terms-and-conditions', component: TermsAndConditions },
			{ path: '/:pathMatch(.*)*', redirect: '/' }
		]
	}))
	.use(createAuth0({
		domain: authConfig.domain,
		clientId: authConfig.clientId,
		authorizationParams: {
			redirect_uri: window.location.origin,
		}
	})).mount('#app');
