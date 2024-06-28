import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue';
import Contacts from '../views/Contacts.vue';
import Profile from '../views/Profile.vue';
import UserProfile from '../views/UserProfile.vue';
import Date from '../views/Date.vue';
import Dates from '../views/Dates.vue';
import Registration from '../views/Registration.vue';
import Login from '../views/Login.vue';
import ResetPasswordRequest from '../views/ResetPasswordRequest.vue';
import ResetPassword from '../views/ResetPassword.vue';
import Activate from '../views/Activate.vue';
import Orders from '../views/Orders.vue';
import Matches from '../views/Matches.vue';
import Gallery from "../views/Gallery.vue";
import Admin from "../views/Admin.vue";
import PrivacyPolicy from "../views/PrivacyPolicy.vue";
import TermsAndConditions from "../views/TermsAndConditions.vue";
import '@/helpers/auth-guard.js';
import { authGuard } from './auth-guard.js';

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Home },
        { path: '/home', component: Home },
        { path: '/contacts', component: Contacts },
        { path: '/profile', component: Profile, beforeEnter: authGuard },
        { path: '/user-profile', component: UserProfile, beforeEnter: authGuard },
        { path: '/user-gallery', component: Gallery, beforeEnter: authGuard },
        { path: '/date', component: Date },
        { path: '/dates', component: Dates },
        { path: '/orders', component: Orders, beforeEnter: authGuard },
        { path: '/matches', component: Matches, beforeEnter: authGuard },
        { path: '/register', component: Registration },
        { path: '/login', component: Login },
        { path: '/reset-password-request', component: ResetPasswordRequest },
        { path: '/reset-password', component: ResetPassword },
        { path: '/activate', component: Activate },
        { path: '/admin', component: Admin, beforeEnter: authGuard },
        { path: '/logout', component: Home },
        { path: '/privacy-policy', component: PrivacyPolicy },
        { path: '/terms-and-conditions', component: TermsAndConditions },
        { path: '/:pathMatch(.*)*', redirect: '/' }
    ]
});

export default router;