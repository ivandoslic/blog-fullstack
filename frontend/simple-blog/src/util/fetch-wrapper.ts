import { useAuthStore } from '@/stores/auth.store';
import type { RequestOptions } from '@/types/BlogTypes';

export const fetchWrapper = {
    get: request('GET'),
    post: request('POST'),
    put: request('PUT'),
    delete: request('DELETE')
};

function request(method: string) {
    return (url: string, body?: any) => {
        const requestOptions: RequestOptions = {
            method,
            headers: authHeader(url)
        };
        if (body) {
            requestOptions.headers['Content-Type'] = 'application/json';
            requestOptions.body = JSON.stringify(body);
        }
        return fetch(url, requestOptions).then(handleResponse);
    }
}

function authHeader(url: string): Record<string, string> {
    // return auth header with jwt if user is logged in and request is to the api url
    const { user } = useAuthStore();
    const isLoggedIn = !!user?.token;
    const isApiUrl = url.startsWith(import.meta.env.VITE_BASE_API_URL as string);

    if (isLoggedIn && isApiUrl) {
        return { Authorization: `Bearer ${user.token}` };
    } else {
        return {};
    }
}

async function handleResponse(response: Response): Promise<any> {
    const text = await response.text(); // Text from response body

    if (!response.ok) {
        const { user, logout } = useAuthStore();
        if ([401, 403].includes(response.status) && user) {
            // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
            logout();
        }

        const error = {
            statusCode: response.status,
            statusText: response.statusText,
            message: text
        };

        return Promise.reject(error);
    }

    const data = text ? JSON.parse(text) : null; // It'll throw an error when text is only text as you can't parse if like an object

    return data;
}
