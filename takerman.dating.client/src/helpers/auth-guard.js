import { accountService } from '@/services';
import { router } from '@/helpers';

export const authGuard = function (to) {
    const account = accountService.accountValue;
    if (account) {
        return true;
    }

    router.push({ path: '/login', query: { returnUrl: to.fullPath } });
    return false;
}