<template>
    <!-- ======= Hero Section ======= -->
    <section id="hero">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 pt-5 pt-lg-0 order-2 order-lg-1 d-flex flex-column justify-content-center"
                    data-aos="fade-up">
                    <heading heading="Срещни се с нови хора на живо или с видео връзка" />
                </div>
            </div>
            <dates />
        </div>
    </section><!-- End Hero -->
</template>

<script lang="js">
import dates from './Dates.vue';
import heading from '../components/Heading.vue';

export default {
    components: {
        dates,
        heading
    }
}
</script>

<style scoped></style>