export const initFacebookSdk = function () {
    return new Promise((resolve, reject) => {
        window.fbAsyncInit = function () {
            FB.init({
                appId: 1595400384616188,
                autoLogAppEvents: true,
                xfbml: true,
                version: 'v10.0',
            });
            resolve(FB);
        };

        (function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) {
                return;
            }
            js = d.createElement(s);
            js.id = id;
            js.src = 'https://connect.facebook.net/en_US/sdk.js';
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk'));
    });
}
