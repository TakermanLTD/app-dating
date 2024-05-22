import { createApp } from 'vue';
import { createPinia } from 'pinia';
import router from "./helpers/router.js"
import App from './App.vue';
import mitt from 'mitt';
import { Tolgee, DevTools, VueTolgee, FormatSimple, BackendFetch } from '@tolgee/vue';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap";
import "easing";
import "/src/assets/lib/animate/animate.min.css";
import "/src/assets/lib/owlcarousel/assets/owl.carousel.min.css";
import "/src/assets/css/style.css";
import "/src/assets/js/main.js";
import moment from 'moment';

Date.prototype.toJSON = function () { return moment(this).format(); }

const tolgee = Tolgee()
    .use(DevTools())
    .use(FormatSimple())
    // .use(BackendFetch({ prefix: 'https://cdn.tolg.ee/1c54d56393a899fd6effe5dea892e867' }))
    .init({
        language: 'en',
        apiUrl: import.meta.env.VUE_APP_TOLGEE_API_URL,
        apiKey: import.meta.env.VUE_APP_TOLGEE_API_KEY,
    });
const emitter = mitt();
let pinia = createPinia();

const app = createApp(App);
app.config.globalProperties.emitter = emitter;
app.use(VueTolgee, { tolgee })
    .use(pinia)
    .use(router)
    .mount('#app');
