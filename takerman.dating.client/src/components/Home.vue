<template>
    <!-- ======= Hero Section ======= -->
    <section id="hero">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 pt-5 pt-lg-0 order-2 order-lg-1 d-flex flex-column justify-content-center"
                    data-aos="fade-up">
                    <div>
                        <hgroup>
                            <h1>Срещни се с нови хора на живо или с видео връзка</h1>
                        </hgroup>
                    </div>
                </div>
            </div>
            <Dates />
        </div>
    </section><!-- End Hero -->
</template>

<script lang="js">
import Dates from './Dates.vue';
export default {
    components: {
        Dates
    }
}
</script>

<style scoped></style>