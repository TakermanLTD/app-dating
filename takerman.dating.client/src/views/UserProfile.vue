<template>
    <div class="container">
        <heading heading="Профил" />
        <br />
        <div v-if="this.loading">Loading...</div>
        <div style="margin-bottom: 50px;" class="text-center" v-if="this.user != null">
            <img :src="user.pictures == null ? 'defaultAvatar.png' : 'data:image/jpeg;base64,' + btoa(choice.pictures[0])" class="img" width="150" height="150" /><br />
            <span v-if="this.user.firstName && this.user.lastName"><strong>Име:</strong> {{ this.user.firstName }} {{ this.user.lastName }} <br /></span>
            <span v-if="this.user.email"><strong>Имейл:</strong> {{ this.user.email }} <br /></span>
            <span v-if="this.user.country"><strong>Държава:</strong> {{ this.user.country }} <br /></span>
            <span v-if="this.user.city"><strong>Град:</strong> {{ this.user.city }} <br /></span>
            <span v-if="this.user.phone"><strong>Телефон:</strong> {{ this.user.phone }} <br /></span>
            <span v-if="this.user.facebook"><strong>Facebook:</strong> {{ this.user.facebook }} <br /></span>
            <span v-if="this.user.instagram"><strong>Instagram:</strong> {{ this.user.instagram }} <br /></span>
            <span v-if="this.user.gender"><strong>Пол:</strong> {{ this.user.gender == 1 ? 'Мъж' : 'Жена' }} <br /></span>
            <span v-if="this.user.ethnicity"><strong>Етнос:</strong> {{ this.user.ethnicity }} <br /></span>
        </div>
    </div>
</template>

<script lang="js">
import { fetchWrapper } from '@/helpers';
import heading from '../components/Heading.vue';

export default {
    data() {
        return {
            loading: false,
            user: null
        };
    },
    components: {
        heading
    },
    async created() {
        this.loading = true;
        let queryString = window.location.search;
        let urlParams = new URLSearchParams(queryString);

        if (urlParams.has('id')) {
            this.user = await fetchWrapper.get('User/Get?id=' + urlParams.get('id'));
        }

        this.loading = false;
    }
}
</script>

<style scoped></style>