import { defineStore } from "pinia";
import { fetchWrapper, router } from '@/util';

const baseUrl = `${import.meta.env.VITE_BASE_API_URL}/auth`;

export const useAuthStore = defineStore({
    id: 'auth',
    state: () => ({
        user: JSON.parse(localStorage.getItem('user') || 'null'),
        returnUrl: null as string | null
    }),
    actions: {
        async login(username: string, password: string) {
            try {
                const user = await fetchWrapper.post(`${baseUrl}/login`, { username, password });
                this.user = user;
        
                localStorage.setItem('user', JSON.stringify(user));
        
                router.push(this.returnUrl || '/');
            } catch (err) {
                throw err;
            }
        },
        async register(username: string, email: string, password: string) {
            const user = await fetchWrapper.post(`${baseUrl}/register`, { username, email, password });

            this.user = user;

            localStorage.setItem('user', JSON.stringify(user));

            router.push(this.returnUrl || '/');
        },
        logout() {
            this.user = null;
            localStorage.removeItem('user');
            router.push('/login');
        }
    }
})