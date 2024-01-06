<template>
    <Header />

    <main style="margin-top: 100px">
        <router-view></router-view>
    </main>
    <!-- <Chat /> -->
    <Footer />
</template>

<script lang="js">
import Header from './components/Header.vue';
import Home from './components/Home.vue';
import Footer from './components/Footer.vue';
import Chat from './components/Chat.vue';
import { useAuthStore } from '@/stores';


export default {
    components: {
        Header,
        Home,
        Footer,
        Chat
    },
    mounted() {
        const authStore = useAuthStore();
    },
}
</script>

<style scoped></style>