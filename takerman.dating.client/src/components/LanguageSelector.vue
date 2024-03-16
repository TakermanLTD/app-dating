<template>
    <div class="btn-group mx-2">
        <button type="button" class="btn btn-sm btn-light dropdown-toggle" data-toggle="dropdown">{{ this.selectedLanguage.toUpperCase() }}</button>
        <div class="dropdown-menu dropdown-menu-right">
            <button @click="changeLanguage(language)" v-for="(language, languageKey) in this.languages" :key="languageKey" class="dropdown-item" type="button">
                {{ $t('common.lang_' + language) }}
            </button>
        </div>
    </div>
</template>

<script lang="js">
import cookies from '../helpers/cookies';
import { useTolgee } from '@tolgee/vue';

export default {
    data() {
        return {
            selectedLanguage: "en",
            languages: ["en", "ru", "ro", "bg"],
            tolgee: useTolgee(['language'])
        }
    },
    mounted() {
        var languageCookie = cookies.get('language');
        if (languageCookie) {
            this.selectedLanguage = languageCookie;
        }
    },
    methods: {
        changeLanguage(language) {
            this.tolgee.changeLanguage(language);
            this.selectedLanguage = language;
            cookies.set('language', language);
        }
    }
}
</script>