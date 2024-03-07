<template>
    <!-- ======= Header ======= -->
    <header id="header" class="fixed-top d-flex align-items-center">
        <div class="container d-flex align-items-center">

            <div class="logo me-auto">
                <hgroup>
                    <h1>
                        <router-link to="/"><img id="logo" src="/src/assets/img/favicon.png" alt="logo"
                                class="img-fluid"></router-link>
                        <router-link to="/">СРЕЩИ</router-link>
                    </h1>
                </hgroup>
            </div>

            <nav id="navbar" class="navbar order-last order-lg-0">
                <ul>
                    <li><router-link class="nav-link scrollto active" to="/">Начало</router-link></li>
                    <li><router-link class="nav-link scrollto active" to="/contacts">Контакти</router-link></li>
                    <li class="dropdown">
                        <router-link to="/profile"><span>Профил</span> <i class="bi bi-chevron-down"></i></router-link>
                        <ul>
                            <li v-if="this.user != null"><router-link class="nav-link scrollto"
                                    to="/profile">Детайли</router-link></li>
                            <li v-if="this.user != null"><router-link class="nav-link scrollto"
                                    to="/my-dates">Мои срещи</router-link></li>
                            <li v-if="this.user != null"><router-link class="nav-link scrollto"
                                    to="/matches">Съвпадения</router-link></li>
                            <li v-if="this.user == null"><router-link class="nav-link scrollto"
                                    to="/register">Регистрация</router-link></li>
                            <li v-if="this.user == null"><router-link class="nav-link scrollto"
                                    to="/login">Вход</router-link></li>
                            <li v-if="this.user != null"><a class="nav-link scrollto" href="#"
                                    @click="this.logout">Изход</a></li>
                        </ul>
                    </li>
                </ul>
                <i class="bi bi-list mobile-nav-toggle"></i>
            </nav><!-- .navbar -->

            <div class="header-social-links d-flex align-items-center">
                <a href="https://www.facebook.com/sreshti/" target="_blank" class="facebook"><i
                        class="bi bi-facebook"></i></a>
                <a href="https://www.instagram.com/sreshti/" target="_blank" class="instagram"><i
                        class="bi bi-instagram"></i></a>
                <a href="https://www.tiktok.com/@sreshti" target="_blank" class="tiktok"><i class="bi bi-tiktok"></i></a>
            </div>
        </div>
    </header><!-- End Header -->
</template>

<script lang="js">
import { storeToRefs } from 'pinia';
import { useAuthStore } from '@/stores';

export default {
    data() {
        return {
            user: null
        }
    },
    mounted() {
        const authStore = useAuthStore();
        let { user: authUser } = storeToRefs(authStore);
        this.user = authUser;
    },
    methods: {
        logout() {
            const authStore = useAuthStore();
            let { user: authUser } = storeToRefs(authStore);
            authStore.logout();
            authUser = null;
            this.user = null;
        }
    },
}
</script>

<style scoped>
img#logo {
    margin-right: 10px;
}
</style>