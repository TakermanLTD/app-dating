<template>
    <div class="container-fluid">
        <div class="row px-xl-5">
            <div class="col-12">
                <nav class="breadcrumb bg-light mb-30">
                    <router-link :to="path.name" v-for="(path, pathKey) in paths" :key="pathKey" class="breadcrumb-item text-dark">{{ path.title }}</router-link>
                </nav>
            </div>
        </div>
    </div>
</template>
<script lang="js">
export default {
    props: {
        paths: Array
    }
}
</script>
<style scoped></style>