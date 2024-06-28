import { router } from '@/helpers';
import { useAuthStore } from '@/stores';

export const authGuard = function (to) {
    const authStore = useAuthStore();
    if (authStore.user) {
        return true;
    }

    router.push({ path: '/login', query: { returnUrl: to.fullPath } });
    return false;
}