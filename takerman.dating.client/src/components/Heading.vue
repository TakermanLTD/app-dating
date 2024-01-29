<template>
    <div class="section-title" data-aos="fade-up">
        <hgroup>
            <h2 class="text-center">{{ heading }}</h2>
        </hgroup>
    </div>
</template>

<script lang="js">
export default {
    props: {
        heading: String
    }
}
</script>