import { BehaviorSubject } from 'rxjs';
import { router } from '@/helpers';
import { fetchWrapper } from '@/helpers';
import { useAuthStore } from '@/stores';

const accountSubject = new BehaviorSubject(null);

export const accountService = {
    login,
    apiAuthenticate,
    logout,
    account: accountSubject.asObservable(),
    get accountValue() { return accountSubject.value; }
};

async function login() {
    const { authResponse } = await new Promise(FB.login);
    if (!authResponse) return;

    await apiAuthenticate(authResponse.accessToken);

    const returnUrl = router.currentRoute.value.query['returnUrl'] || '/';
    router.push(returnUrl);
}

async function apiAuthenticate(accessToken) {
    const response = await fetchWrapper.post(`/User/FacebookSignIn`, { AccessToken: accessToken });
    const account = response.data;
    accountSubject.next(account);
    startAuthenticateTimer();
    return account;
}

function logout() {
    // revoke app permissions to logout completely because FB.logout() doesn't remove FB cookie
    FB.api('/me/permissions', 'delete', null, () => FB.logout());
    stopAuthenticateTimer();
    accountSubject.next(null);
    router.push('/login');
}

let authenticateTimeout;

function startAuthenticateTimer() {
    const jwtToken = JSON.parse(atob(accountSubject.value.token.split('.')[1]));

    const expires = new Date(jwtToken.exp * 1000);
    const timeout = expires.getTime() - Date.now() - (60 * 1000);
    const { accessToken } = FB.getAuthResponse();
    authenticateTimeout = setTimeout(() => apiAuthenticate(accessToken), timeout);
}

function stopAuthenticateTimer() {
    clearTimeout(authenticateTimeout);
}