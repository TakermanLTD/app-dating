import { createApp } from 'vue';
import router from "./helpers/router.js"
import App from './App.vue';
import './assets/css/style.css';
import { createPinia } from 'pinia';

let pinia = createPinia();

createApp(App)
    .use(pinia)
    .use(router)
    .mount('#app');
