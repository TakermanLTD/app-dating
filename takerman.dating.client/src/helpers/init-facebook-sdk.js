import { accountService } from '@/services';

// const facebookAppId = process.env.VUE_APP_FACEBOOK_APP_ID;

export const initFacebookSdk = function() {
    return new Promise(resolve => {
        window.fbAsyncInit = function () {
            FB.init({
                appId: '1595400384616188',
                cookie: true,
                xfbml: true,
                version: 'v8.0'
            });

            FB.getLoginStatus(({ authResponse }) => {
                if (authResponse) {
                    accountService.apiAuthenticate(authResponse.accessToken).then(resolve);
                } else {
                    resolve();
                }
            });
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) { return; }
            js = d.createElement(s); js.id = id;
            js.src = "https://connect.facebook.net/en_US/sdk.js";
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));    
    });
}