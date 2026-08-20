import type { AxiosInstance } from "axios";
import { clearAuth, getToken } from "../utils/token";

export function attachAuthInterceptors(api: AxiosInstance) {
    api.interceptors.request.use((config) => {
        const token = getToken();

        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    });

    api.interceptors.response.use(
        (response) => response,
        (error) => {
            if (error.response?.status === 401) {
                clearAuth();

                if (window.location.pathname !== "/login") {
                    window.location.assign("/login");
                }
            }

            return Promise.reject(error);
        }
    );
}
