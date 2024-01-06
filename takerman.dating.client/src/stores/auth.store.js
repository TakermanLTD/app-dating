import { defineStore } from 'pinia';
import { fetchWrapper, router } from '@/helpers';

const baseUrl = `User`;

export const useAuthStore = defineStore({
    id: 'auth',
    state: () => ({
        user: JSON.parse(localStorage.getItem('user')),
        returnUrl: null
    }),
    actions: {
        async login(email, password) {
            const user = await fetchWrapper.post(`${baseUrl}/Authenticate`, { email, password });
            this.user = user;
            localStorage.setItem('user', JSON.stringify(user));
            router.push('/uploads');
            return user;
        },
        async changePassword(userId, password) {
            await fetchWrapper.post(`${baseUrl}/ChangePassword`, { userId, password });
        },
        async activate(code) {
            return await fetchWrapper.get(`${baseUrl}/Activate?code=${Number.parseInt(code)}`);
        },
        async resetPassword(email) {
            const user = await fetchWrapper.post(`${baseUrl}/ResetPasswordRequest`, { email });
            this.user = user;
            return user;
        },
        logout() {
            this.user = null;
            localStorage.removeItem('user');
            router.push('/');
        }
    }
});
