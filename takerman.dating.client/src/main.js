import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from "./helpers/router.js"
import App from './App.vue';
import { Tolgee, DevTools, VueTolgee, FormatSimple, BackendFetch } from '@tolgee/vue';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import './assets/css/style.css';


const tolgee = Tolgee()
    .use(DevTools())
    .use(FormatSimple())
    .use(BackendFetch({ prefix: 'https://cdn.tolg.ee/1c54d56393a899fd6effe5dea892e867' }))
    .init({
        language: 'en',
        apiUrl: import.meta.env.VUE_APP_TOLGEE_API_URL,
        apiKey: import.meta.env.VUE_APP_TOLGEE_API_KEY,
    });

let pinia = createPinia();

createApp(App)
    .use(VueTolgee, { tolgee })
    .use(pinia)
    .use(router)
    .mount('#app');
