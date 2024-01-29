<template>
    <section id="breadcrumbs" class="breadcrumbs">
        <div class="container">
            <ol>
                <li v-for="path in paths">
                    <router-link :to="path.name">{{ path.title }}</router-link>
                </li>
            </ol>
        </div>
    </section>
</template>
<script lang="js">
export default {
    props: {
        paths: []
    }
}
</script>
<style scoped>
</style>