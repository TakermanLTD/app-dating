<template>
    <div class="container">
        <heading heading="Профил" />
        <br />
        <div v-if="this.loading">Loading...</div>
        <div style="margin-bottom: 50px;" class="text-center" v-if="this.user != null">
            <Avatar :userId="this.user.id" />
            <p>
                <span v-if="this.user?.firstName && this.user.lastName">{{ this.user?.firstName }} {{ this.user?.lastName }} <br /></span>
                <a v-if="this.user?.email" :href="'mailto:' + this.user.email">{{ this.user?.email }}<br /></a>
                <span v-if="this.user?.country && this.user.city">{{ this.user?.country }}{{ this.user?.country ? ', ' + this.user?.city :  this.user?.city }}<br /></span>
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
import { fetchWrapper } from '@/helpers';
import heading from '../components/Heading.vue';
import Avatar from '../components/Avatar.vue';

export default {
    data() {
        return {
            loading: false,
            user: null
        };
    },
    components: {
        heading, Avatar
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