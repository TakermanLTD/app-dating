<template>
    <TolgeeProvider>
        <template v-slot:fallback>
            <div>Loading...</div>
        </template>
        <AppHeader />
        <main style="margin-top: 100px">
            <router-view></router-view>
        </main>
        <AppFooter />
    </TolgeeProvider>
</template>

<script lang="js">
import AppHeader from './components/AppHeader.vue';
import AppFooter from './components/AppFooter.vue';
import { TolgeeProvider } from '@tolgee/vue';

export default {
    components: {
        AppHeader,
        AppFooter,
        TolgeeProvider
    }
}
</script>

<style scoped></style>