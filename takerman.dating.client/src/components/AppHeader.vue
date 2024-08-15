<template>
    <header>
        <div class="container-fluid">
            <div class="row align-items-center bg-light py-3 px-xl-5 d-none d-lg-flex">
                <div class="col-lg-6">
                    <router-link to="/" class="text-decoration-none">
                        <span class="h1 text-uppercase text-primary bg-dark px-2">{{ $t('header.titleFirst') }}</span>
                        <span class="h1 text-uppercase text-dark bg-primary px-2 ml-n1">{{ $t('header.titleSecond') }}</span>
                    </router-link>
                </div>
                <div class="col-lg-6 col-8 text-right">
                    <!-- <p class="m-0">{{ $t('header.customerSupportTitle') }}</p>
                    <h5 class="m-0">{{ $t('header.customerSupportNumber') }}</h5> -->
                </div>
            </div>
        </div>

        <div id="headerNavigation" class="container-fluid bg-dark mb-30">
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
                                <router-link class="nav-item nav-link" to="/">{{ $t('nav.home') }}</router-link>
                                <router-link v-if="this.user != null" class="nav-item nav-link" to="/orders">{{ $t('nav.myDates') }}</router-link>
                                <router-link v-if="this.user != null" class="nav-item nav-link" to="/matches">{{ $t('nav.matches') }}</router-link>
                                <router-link v-if="this.user != null && this.user.isAdmin" class="nav-item nav-link" to="/admin">{{ $t('nav.admin') }}</router-link>
                                <div class="nav-item dropdown">
                                    <a href="#" class="nav-link dropdown-toggle" data-toggle="dropdown">{{ $t('nav.account') }} <i class="fa fa-angle-down mt-1"></i></a>
                                    <div class="dropdown-menu bg-primary rounded-0 border-0 m-0">
                                        <router-link v-if="this.user != null" class="dropdown-item" to="/profile">{{ $t('nav.profile') }}</router-link>
                                        <router-link v-if="this.user == null" class="dropdown-item" to="/register">{{ $t('nav.register') }}</router-link>
                                        <router-link v-if="this.user != null" class="dropdown-item" to="/user-gallery">{{ $t('nav.photos') }}</router-link>
                                        <router-link v-if="this.user == null" class="dropdown-item" to="/login">{{ $t('nav.login') }}</router-link>
                                        <router-link v-if="this.user != null" class="dropdown-item" to="/logout" @click="this.logout">{{ $t('nav.logout') }}</router-link>
                                    </div>
                                </div>
                            </div>
                            <div class="navbar-nav ml-auto py-0 d-none d-lg-block">
                                <router-link id="watchlist" to="/orders" class="btn px-0">
                                    <i class="fas fa-heart text-primary"></i>
                                    <span class="badge text-secondary border border-secondary rounded-circle" style="padding-bottom: 2px;">{{ this.savedSpotslength }}</span>
                                </router-link>
                                <LanguageSelector />
                                <!-- <div class="header-social-links d-flex align-items-center">
                                    <a href="https://facebook.com/sreshti/" target="_blank" class="facebook"><i class="fa fa-facebook"></i></a>
                                    <a href="https://instagram.com/sreshti/" target="_blank" class="instagram"><i class="fa fa-instagram"></i></a>
                                    <a href="https://tiktok.com/@sreshti" target="_blank" class="tiktok"><i class="fa fa-tiktok"></i></a>
                                </div> -->
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
import { fetchWrapper } from '@/helpers';
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
    async mounted() {
        const authStore = useAuthStore();
        let { user: authUser } = storeToRefs(authStore);
        this.user = authUser;
        if (this.user) {
            let spots = await fetchWrapper.get('Dates/GetSavedSpots?userId=' + authStore.user?.id);
            this.savedSpotslength = spots.length;
        }
        this.emitter.on('addToSpotCount', (evt) => {
            this.savedSpotslength += evt.eventContent;
        })
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