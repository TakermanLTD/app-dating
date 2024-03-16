<template>
    <footer>
        <div class="container-fluid bg-dark text-secondary mt-5 pt-5">
            <div class="row px-xl-5 pt-5">
                <div class="col-lg-4 col-md-12 mb-5 pr-3 pr-xl-5">
                    <h5 class="text-secondary text-uppercase mb-4">{{ $t('footer.contact.title') }}</h5>
                    <p class="mb-4">{{ $t('footer.contact.intro') }}</p>
                    <p class="mb-2"><i class="fa fa-map-marker-alt text-primary mr-3"></i>{{ $t('footer.contact.street') }}</p>
                    <p class="mb-2"><i class="fa fa-envelope text-primary mr-3"></i>{{ $t('footer.contact.email') }}</p>
                    <p class="mb-0"><i class="fa fa-phone-alt text-primary mr-3"></i>{{ $t('footer.contact.phone') }}</p>
                </div>
                <div class="col-lg-8 col-md-12">
                    <div class="row">
                        <div class="col-md-4 mb-5">
                            <h5 class="text-secondary text-uppercase mb-4">{{ $t('footer.menu.title') }}</h5>
                            <div class="d-flex flex-column justify-content-start">
                                <router-link class="text-secondary mb-2" to="/"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.home') }}</router-link>
                                <router-link class="text-secondary mb-2" to="/about"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.about') }}</router-link>
                                <router-link class="text-secondary" to="/contacts"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.contacts') }}</router-link>
                            </div>
                        </div>
                        <div class="col-md-4 mb-5">
                            <h5 class="text-secondary text-uppercase mb-4">{{ $t('footer.menu.account') }}</h5>
                            <div class="d-flex flex-column justify-content-start">
                                <router-link class="text-secondary mb-2" to="/profile"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.profile') }}</router-link>
                                <router-link class="text-secondary mb-2" to="/register"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.register') }}</router-link>
                                <router-link class="text-secondary mb-2" to="/login"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.login') }}</router-link>
                                <router-link class="text-secondary mb-2" to="/matches"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.matches') }}</router-link>
                                <router-link class="text-secondary mb-2" to="/wishlist"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.wishlist') }}</router-link>
                                <router-link class="text-secondary" to="/orders"><i class="fa fa-angle-right mr-2"></i>{{ $t('footer.menu.orders') }}</router-link>
                            </div>
                        </div>
                        <div class="col-md-4 mb-5">
                            <Newsletter />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row border-top mx-xl-5 py-4" style="border-color: rgba(256, 256, 256, .1) !important;">
                <div class="col-md-6 px-xl-0">
                    <p class="mb-md-0 text-center text-md-left text-secondary">
                        &copy; <a class="text-primary" href="#">{{ $t('common.domain') }}</a>. {{ $t('footer.allRightsReserved') }}
                    </p>
                </div>
                <div class="col-md-6 px-xl-0 text-center text-md-right">
                    <img class="img-fluid" src="/src/assets/img/payments.png" alt="">
                </div>
            </div>
        </div>
    </footer>
</template>

<script lang="js">
import Newsletter from './Newsletter.vue';

export default {
    components: { Newsletter }
}
</script>

<style scoped></style>