<template>
    <div>{{ content ? content : 'Зареждане...' }}</div>
</template>
<script lang="js">
export default {
    props: {
        content: String
    }
}
</script>
<style scoped></style>