import { accountService } from '@/services';

export const jwtInterceptor = function () {
    const account = accountService.accountValue;
    window.fetch = function (url, options) {
        const headers = {
            ...options?.headers
        };

        if (account?.token) {
            headers.Authorization=`Bearer ${account.token}`;
        }

        const finalOptions = {
            ...options,
            headers
        };

        return fetch(url, finalOptions);
    };
}