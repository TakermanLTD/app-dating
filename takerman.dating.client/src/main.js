import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from "./helpers/router.js"
import App from './App.vue';
import './assets/css/style.css';

let pinia = createPinia();

createApp(App)
    .use(pinia)
    .use(router)
    .mount('#app00000');
