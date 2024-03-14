<template>
    <AppHeader />
    <main style="margin-top: 100px">
        <router-view></router-view>
    </main>
    <SupportChat />
    <AppFooter />
</template>

<script lang="js">
import AppHeader from './components/Header.vue';
import AppFooter from './components/Footer.vue';
import SupportChat from './components/SupportChat.vue';

export default {
    components: {
        AppHeader,
        AppFooter,
        SupportChat
    }
}
</script>

<style scoped></style>