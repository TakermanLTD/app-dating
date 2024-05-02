<template>
    <router-link v-if="this.avatarUrl" :to="this.userId ? '/user-profile?id=' + this.userId : '#'">
        <img :src="this.avatarUrl" class="img" width="150" height="150" />
    </router-link>
</template>
<script>
export default {
    props: {
        userId: Number,
        userUrl: String
    },
    data() {
        return {
            avatarUrl: null
        }
    },
    async created() {
        if (this.userId) {
            let result = await fetch('Cdn/GetAvatar?userId=' + this.userId);
            this.avatarUrl = await result.text();
        }
    }
};
</script>