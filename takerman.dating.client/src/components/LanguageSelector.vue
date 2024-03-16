<template>
    <select :value="tolgee.getLanguage()" v-on:change="changeLanguage">
        <option value="en">{{ $t('common.lang_en') }}</option>
        <option value="ru">{{ $t('common.lang_ru') }}</option>
        <option value="ro">{{ $t('common.lang_ro') }}</option>
        <option value="bg">{{ $t('common.lang_bg') }}</option>
    </select>
</template>

<script lang="js">
import { useTolgee } from '@tolgee/vue';

export default {
    data() {
        return {
            tolgee: useTolgee(['language'])
        }
    },
    methods: {
        changeLanguage(e) {
            this.tolgee.changeLanguage(e.target.value);
        }
    }
}
</script>