<template>
    <div class="container">
        <heading heading="Профил" />
        <br />
        <div v-if="this.loading">Loading...</div>
        <div style="margin-bottom: 50px;" class="text-center" v-if="this.user != null && this.isAuthenticated">
            <Avatar :userId="this.user.id" />
            <p>
                <span v-if="this.user?.firstName && this.user.lastName">{{ this.user?.firstName }} {{ this.user?.lastName }} <br /></span>
                <a v-if="this.user?.email" :href="'mailto:' + this.user.email">{{ this.user?.email }}<br /></a>
                <span v-if="this.user?.country && this.user.city">{{ this.user?.country }}{{ this.user?.country ? ', ' + this.user?.city : this.user?.city }}<br /></span>
                <a v-if="this.user?.phone" :href="'tel:' + this.user?.phone">{{ this.user?.phone }} <br /></a>
                <span v-if="this.user?.facebook || this.user?.instagram">
                    <span style="margin: 5px;" v-if="this.user?.facebook"><a :href="this.user?.facebook" target="_blank"><i class="bi bi-facebook"></i></a></span>
                    <span style="margin: 5px;" v-if="this.user?.instagram"><a :href="this.user?.instagram" target="_blank"><i class="bi bi-instagram"></i></a></span>
                    <br />
                </span>
                <span v-if="this.user?.gender">{{ this.user.gender == 1 ? 'Мъж' : 'Жена' }} <br /></span>
            </p>
        </div>
    </div>
</template>

<script lang="js">
import heading from '../components/Heading.vue';
import Avatar from '../components/Avatar.vue';
import { useAuth0 } from '@auth0/auth0-vue';

export default {
    data() {
        return {
            loading: false,
            user: null,
            isAuthenticated: false
        };
    },
    components: {
        heading, Avatar
    },
    async mounted() {
        const { user, isAuthenticated } = useAuth0();
        this.user = user;
        this.isAuthenticated = isAuthenticated;
        /*
        this.loading = true;
        let queryString = window.location.search;
        let urlParams = new URLSearchParams(queryString);

        if (urlParams.has('id')) {
            this.user = await fetch('User/Get?id=' + urlParams.get('id'));
        }

        this.loading = false;
        */
    }
}
</script>

<style scoped></style>