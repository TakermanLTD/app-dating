import { createRouter, createWebHistory } from "vue-router"
import Home from '../components/Home.vue';
import Contacts from '../components/Contacts.vue';
import Profile from '../components/Profile.vue';
import Shipping from '../components/Shipping.vue';
import Print from '../components/Print.vue';
import Registration from '../components/Registration.vue';
import Login from '../components/Login.vue';
import ResetPasswordRequest from '../components/ResetPasswordRequest.vue';
import ResetPassword from '../components/ResetPassword.vue';
import Activate from '../components/Activate.vue';
import Uploads from '../components/Uploads.vue';
import Orders from '../components/Orders.vue';
import PrivacyPolicy from '../components/PrivacyPolicy.vue';
import TermsAndConditions from '../components/TermsAndConditions.vue';
import { useAuthStore } from '@/stores';

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Home },
        { path: '/home', component: Home },
        { path: '/contacts', component: Contacts },
        { path: '/profile', component: Profile },
        { path: '/shipping', component: Shipping },
        { path: '/print', component: Print },
        { path: '/uploads', component: Uploads },
        { path: '/orders', component: Orders },
        { path: '/register', component: Registration },
        { path: '/login', component: Login },
        { path: '/reset-password-request', component: ResetPasswordRequest },
        { path: '/reset-password', component: ResetPassword },
        { path: '/activate', component: Activate },
        { path: '/logout', component: Home },
        { path: '/privacy-policy', component: PrivacyPolicy },
        { path: '/terms-and-conditions', component: TermsAndConditions }
    ]
});

router.beforeEach((to) => {
    const publicPages = [
        '/',
        '/home',
        '/contacts',
        '/register',
        '/login',
        '/privacy-policy',
        '/terms-and-conditions',
        '/reset-password',
        '/reset-password-request',
        '/activate'];

    const authRequired = !publicPages.includes(to.path);
    const auth = useAuthStore();

    if (authRequired && !auth.user) {
        auth.returnUrl = to.fullPath;
        return '/login';
    }
});

export default router;