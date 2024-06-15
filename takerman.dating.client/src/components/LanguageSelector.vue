<template>
	<div class="btn-group mx-2">
		<button
			type="button"
			class="btn btn-sm btn-light dropdown-toggle"
			data-toggle="dropdown">
			{{ $t("common.lang_" + selectedLanguage) }}
		</button>
		<div class="dropdown-menu dropdown-menu-right">
			<button
				@click="changeLanguage(language)"
				v-for="(language, languageKey) in languages"
				:key="languageKey"
				class="dropdown-item"
				type="button" >
				{{ $t("common.lang_" + language) }}
			</button>
		</div>
	</div>
</template>

<script setup>
import cookies from '../helpers/cookies.js';
import { useI18n } from 'vue-i18n';
const { locale } = useI18n();

let selectedLanguage = cookies.get('language') || "en";
let languages = ["en", "bg", "ru", "ro"];

function changeLanguage(language) {
    selectedLanguage = language;
    locale.value = selectedLanguage;
    cookies.set('language', language);
}
</script>
