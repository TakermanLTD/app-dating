<template>
    <AppHeader style="margin-bottom: 100px" />
    <router-view></router-view>
    <AppFooter />
</template>

<script lang="js">
import AppHeader from './components/AppHeader.vue';
import AppFooter from './components/AppFooter.vue';
import { ref } from 'vue';
import { accountService } from '@/services';

export default {
    components: {
        AppHeader,
        AppFooter
    },
    mounted() {
        const account = ref(null);
        accountService.account.subscribe(x => account.value = x);

        return {
            account,
            logout: accountService.logout
        }
    },
    metaInfo: {
        title: 'common.title',
        titleTemplate: '%s | ' + 'common.host',
        meta: [
            { charset: 'utf-8' },
            { name: 'viewport', content: 'width=device-width, initial-scale=1' },
            { name: 'description', content: 'common.description' },
            { name: 'keywords', content: 'common.keywords' }
        ]
    }
}
</script>

<style scoped></style>