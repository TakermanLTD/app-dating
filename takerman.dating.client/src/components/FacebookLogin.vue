<template>
    <div>
        <button class="btn btn-info" type="button" @click="loginWithFacebook">Login with Facebook</button>
    </div>
</template>

<script lang="js">
export default {
    methods: {
        loginWithFacebook() {
            FB.login(response => {
                if (response.authResponse) {
                    const accessToken = response.authResponse.accessToken;
                    this.validateFacebookToken(accessToken);
                } else {
                    console.error('User cancelled login or did not fully authorize.');
                }
            }, { scope: 'email' });
        },
        async validateFacebookToken(accessToken) {
            try {
                const response = await fetch('User/validate-facebook-token', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ accessToken })
                });

                if (response.ok) {
                    const data = await response.json();
                    console.log('Token is valid:', data);
                    // Save the token (e.g., in local storage or Vuex)
                    localStorage.setItem('authToken', accessToken);
                    // Redirect to a protected route or homepage
                    this.$router.push({ name: 'Home' });
                } else {
                    console.error('Token validation failed');
                }
            } catch (error) {
                console.error('Error validating token:', error);
            }
        }
    }
}
</script>