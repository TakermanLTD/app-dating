<template>
    <!-- ======= Footer ======= -->
    <footer id="footer">
        <div class="footer-top">
            <div class="container">
                <div class="row">

                    <div class="col-lg-3 col-md-6">
                        <div class="footer-info">
                            <h3>Срещи</h3>
                            <p>
                                бул. Източен 80 <br>
                                Пловдив 4018, България<br><br>
                                <strong>Телефон:</strong> <a href="tel:+359897887191">+359 897887191</a><br>
                                <strong>Имейл:</strong> <a href="mailto:contact@sreshti.net">contact@sreshti.net</a><br>
                            </p>
                            <div class="social-links mt-3">
                                <a href="https://www.facebook.com/sreshti/" target="_blank" class="facebook"><i class="bx bxl-facebook"></i></a>
                                <a href="https://www.instagram.com/sreshti/" target="_blank" class="instagram"><i class="bx bxl-instagram"></i></a>
                                <a href="https://www.tiktok.com/@sreshti" target="_blank" class="tiktok"><i class="bx bxl-tiktok"></i></a>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-2 col-md-6 footer-links">
                        <h4>General</h4>
                        <ul>
                            <li><i class="bx bx-chevron-right"></i> <router-link to="/">Начало</router-link></li>
                            <li><i class="bx bx-chevron-right"></i> <router-link to="/profile">Профил</router-link></li>
                        </ul>
                    </div>

                    <div class="col-lg-3 col-md-6 footer-links">
                        <!-- <h4>Our Services</h4>
                        <ul>
                            <li><i class="bx bx-chevron-right"></i> <router-link to="/dating-3d">3D Dating</router-link></li>
                            <li><i class="bx bx-chevron-right"></i> <router-link to="/dating-stamps">Stamped Items</router-link></li>
                            <li><i class="bx bx-chevron-right"></i> <router-link to="/dating-embrodery">Embrodery</router-link></li>
                            <li><i class="bx bx-chevron-right"></i> <router-link to="/dating-paper">Paper Dating</router-link></li>
                            <li><i class="bx bx-chevron-right"></i> <router-link to="/shop">Shop</router-link></li>
                        </ul> -->
                    </div>

                    <newsletter />
                </div>
            </div>
        </div>

        <div class="container">
            <div class="copyright">
                &copy; Авторски права <strong><span><a href="https://takerman.net" target="_blank">Такерман</a></span></strong>. Всички права запазени
            </div>
            <!-- <div class="credits">
                Designed by <a href="https://bootstrapmade.com/">BootstrapMade</a>
            </div> -->
        </div>
    </footer><!-- End Footer -->
</template>

<script lang="js">
import newsletter from './Newsletter.vue';

export default {
    components: { newsletter }
}
</script>

<style scoped></style>