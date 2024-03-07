<template>
    <div>
        <router-link :to="'/userProfile?id=' + user.userId">
            <img :src="user.avatar == null ? 'defaultAvatar.png' : 'data:image/jpeg;base64,' + btoa(user.avatar)"
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