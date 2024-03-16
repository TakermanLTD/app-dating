<template>
    <AppHeader />
    <main style="margin-top: 100px">
        <router-view></router-view>
    </main>
    <AppFooter />
</template>

<script lang="js">
import AppHeader from './components/AppHeader.vue';
import AppFooter from './components/AppFooter.vue';

export default {
    components: {
        AppHeader,
        AppFooter
    }
}
</script>

<style scoped></style>