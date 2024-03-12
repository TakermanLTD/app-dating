<template>
    <div>
        <router-link :to="'/user-profile?id=' + user.userId">
            <img :src="user.avatar == null"
                class="img" width="150" height="150" />
        </router-link>
        <br />
        <span>{{ user.name }}</span>
    </div>
</template>

<script lang="js">
export default {
    props: {
        user: null
    }
}
</script>

<style scoped></style>