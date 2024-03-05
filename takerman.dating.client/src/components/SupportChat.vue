<template>
    <div id="fb-root"></div>
    <div id="fb-customer-chat" page_id="132952053246153" attribution="biz_inbox" class="fb-customerchat"></div>
</template>

<script lang="js">
export default {
    mounted() {
        window.fbAsyncInit = function () {
            FB.init({
                xfbml: true,
                version: 'v18.0'
            });
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = 'https://connect.facebook.net/en_GB/sdk/xfbml.customerchat.js';
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));
    }
}
</script>

<style scoped></style>