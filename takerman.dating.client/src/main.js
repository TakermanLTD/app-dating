import { createApp } from 'vue';
import { createI18n } from 'vue-i18n';
import { createPinia } from 'pinia';
import router from "./helpers/router.js";
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
import en from './assets/languages/en.json';
import bg from './assets/languages/bg.json';
import ro from './assets/languages/ro.json';
import ru from './assets/languages/ru.json';
import cookies from './helpers/cookies';

Date.prototype.toJSON = function () { return moment(this).format(); }

const emitter = mitt();
let pinia = createPinia();
const i18n = createI18n({
	locale: 'en',
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

const app = createApp(App);
app.config.globalProperties.emitter = emitter;
app.use(pinia)
	.use(i18n)
	.use(router)
	.mount('#app');
