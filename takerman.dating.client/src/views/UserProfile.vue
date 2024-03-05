<template>
    <div class="container">
        <heading heading="Профил" />
        <br />
        <div v-if="this.loading">Loading...</div>
        <div v-if="this.user != null">
            <img :src="user.pictures == null ? 'defaultAvatar.png' : 'data:image/jpeg;base64,' + btoa(choice.pictures[0])" class="img" width="150" height="150" /><br />
            <span>{{ this.user.firstName }} {{ this.user.lastName }} <br /></span>
            <span v-if="this.user.email">Email: {{ this.user.email }} <br /></span>
            <span v-if="this.user.city">City: {{ this.user.city }} <br /></span>
            <span v-if="this.user.country">Country: {{ this.user.country }} <br /></span>
            <span v-if="this.user.phone">Phone: {{ this.user.phone }} <br /></span>
            <span v-if="this.user.facebook">Facebook: {{ this.user.facebook }} <br /></span>
            <span v-if="this.user.instagram">Instagram: {{ this.user.instagram }} <br /></span>
            <span v-if="this.user.gender">Gender: {{ this.user.gender }} <br /></span>
            <span v-if="this.user.ethnicity">Ethnicity: {{ this.user.ethnicity }} <br /></span>
            <button class="btn btn-info">Chat</button>
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