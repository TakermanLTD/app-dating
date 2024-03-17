<template>
    <TolgeeProvider>
        <template v-slot:fallback>
            <div>{{ $t('common.loading') }}...</div>
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