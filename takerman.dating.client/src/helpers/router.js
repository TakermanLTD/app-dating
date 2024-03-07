import { createRouter, createWebHistory } from "vue-router";
import Home from '../views/Home.vue';
import Contacts from '../views/Contacts.vue';
import Profile from '../views/Profile.vue';
import UserProfile from '../views/UserProfile.vue';
import Date from '../views/Date.vue';
import Registration from '../views/Registration.vue';
import Login from '../views/Login.vue';
import ResetPasswordRequest from '../views/ResetPasswordRequest.vue';
import ResetPassword from '../views/ResetPassword.vue';
import Activate from '../views/Activate.vue';
import MyDates from '../views/MyDates.vue';
import Matches from '../views/Matches.vue';
import { useAuthStore } from '@/stores';

export const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    linkActiveClass: 'active',
    routes: [
        { path: '/', component: Home },
        { path: '/home', component: Home },
        { path: '/contacts', component: Contacts },
        { path: '/profile', component: Profile },
        { path: '/userProfile', component: UserProfile },
        { path: '/date', component: Date },
        { path: '/my-dates', component: MyDates },
        { path: '/matches', component: Matches },
        { path: '/register', component: Registration },
        { path: '/login', component: Login },
        { path: '/reset-password-request', component: ResetPasswordRequest },
        { path: '/reset-password', component: ResetPassword },
        { path: '/activate', component: Activate },
        { path: '/logout', component: Home }
    ]
});

router.beforeEach((to) => {
    const publicPages = [
        '/',
        '/home',
        '/date',
        '/contacts',
        '/register',
        '/login',
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