<template>
    <Header></Header>
    <main style="margin-top: 100px">
        <router-view></router-view>
    </main>
    <!-- <Chat /> -->
    <Footer />
</template>

<script lang="js">
import { useAuthStore } from '@/stores';
import Header from './components/Header.vue';
import Footer from './components/Footer.vue';

export default {
    components: {
        Header,
        Footer
    },
    mounted() {
        const authStore = useAuthStore();
    },
}
</script>

<style scoped></style>