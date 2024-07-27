import { createApp } from 'vue';
import { StoryblokVue, apiPlugin } from '@storyblok/vue';
import { createI18n } from 'vue-i18n';
import { createPinia } from 'pinia';
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
import VueGtag from "vue-gtag";
import router from './helpers/router.js';

Date.prototype.toJSON = function () { return moment(this).format(); }
const emitter = mitt();
let pinia = createPinia();
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
app.use(pinia)
	.use(i18n)
	.use(router)
	.use(VueGtag, {
		config: { id: measurementId }
	})
	.use(StoryblokVue, {
		accessToken: 'saPzezRIW4jZNX0V5lfGmQtt',
		use: [apiPlugin],
		apiOptions: {
			region: "eu",
		}
	}).mount('#app');
