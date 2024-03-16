<template>
    <header>
        <div class="container-fluid">
            <div class="row align-items-center bg-light py-3 px-xl-5 d-none d-lg-flex">
                <div class="col-lg-4">
                    <a href="" class="text-decoration-none">
                        <span class="h1 text-uppercase text-primary bg-dark px-2">{{ $t('header.titleFirst') }}</span>
                        <span class="h1 text-uppercase text-dark bg-primary px-2 ml-n1">{{ $t('header.titleSecond') }}</span>
                    </a>
                </div>
                <div class="col-lg-4 col-6 text-left">
                    <div class="header-social-links d-flex align-items-center">
                        <a href="https://facebook.com/sreshti/" target="_blank" class="facebook"><i class="bi bi-facebook"></i></a>
                        <a href="https://instagram.com/sreshti/" target="_blank" class="instagram"><i class="bi bi-instagram"></i></a>
                        <a href="https://tiktok.com/@sreshti" target="_blank" class="tiktok"><i class="bi bi-tiktok"></i></a>
                    </div>
                </div>
                <div class="col-lg-4 col-6 text-right">
                    <p class="m-0">{{ $t('header.customerSupportTitle') }}</p>
                    <h5 class="m-0">{{ $t('header.customerSupportNumber') }}</h5>
                </div>
                <LanguageSelector />
            </div>
        </div>

        <div class="container-fluid bg-dark mb-30">
            <div class="row px-xl-5">
                <div class="col-lg-12">
                    <nav class="navbar navbar-expand-lg bg-dark navbar-dark py-3 py-lg-0 px-0">
                        <a href="" class="text-decoration-none d-block d-lg-none">
                            <span class="h1 text-uppercase text-dark bg-light px-2">{{ $t('header.titleFirst') }}</span>
                            <span class="h1 text-uppercase text-light bg-primary px-2 ml-n1">{{ $t('header.titleSecond') }}</span>
                        </a>
                        <button type="button" class="navbar-toggler" data-toggle="collapse" data-target="#navbarCollapse">
                            <span class="navbar-toggler-icon"></span>
                        </button>
                        <div class="collapse navbar-collapse justify-content-between" id="navbarCollapse">
                            <div class="navbar-nav mr-auto py-0">
                                <router-link class="nav-item nav-link active" to="/">{{ $t('nav.home') }}</router-link>
                                <router-link class="nav-item nav-link active" to="/dates">{{ $t('nav.dates') }}</router-link>
                                <router-link class="nav-item nav-link active" to="/contacts">{{ $t('nav.contacts') }}</router-link>
                                <div class="nav-item dropdown">
                                    <router-link to="/profile" class="nav-link dropdown-toggle" data-toggle="dropdown">{{ $t('nav.profile') }} <i class="fa fa-angle-down mt-1"></i></router-link>
                                    <div class="dropdown-menu bg-primary rounded-0 border-0 m-0">
                                        <router-link class="dropdown-item" to="/profile">{{ $t('nav.myProfile') }}</router-link>
                                        <router-link class="dropdown-item" to="/my-dates">{{ $t('nav.myDates') }}</router-link>
                                        <router-link class="dropdown-item" to="/matches">{{ $t('nav.matches') }}</router-link>
                                        <router-link class="dropdown-item" to="/register">{{ $t('nav.register') }}</router-link>
                                        <router-link class="dropdown-item" to="/login">{{ $t('nav.login') }}</router-link>
                                        <a class="dropdown-item" href="#" @click="this.logout">{{ $t('nav.logout') }}</a>
                                    </div>
                                </div>
                            </div>
                            <div class="navbar-nav ml-auto py-0 d-none d-lg-block">
                                <a href="" class="btn px-0">
                                    <i class="fas fa-heart text-primary"></i>
                                    <span class="badge text-secondary border border-secondary rounded-circle" style="padding-bottom: 2px;">{{ this.savedSpotslength }}</span>
                                </a>
                            </div>
                        </div>
                    </nav>
                </div>
            </div>
        </div>
    </header>
</template>

<script lang="js">
import { storeToRefs } from 'pinia';
import { useAuthStore } from '@/stores';
import LanguageSelector from './LanguageSelector.vue';

export default {
    data() {
        return {
            user: null,
            savedSpotslength: 0
        }
    },
    components: {
        LanguageSelector
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
            // eslint-disable-next-line no-unused-vars
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