import type { AxiosInstance } from "axios";
import { clearAuth, getToken } from "../utils/token";
import { isDemoMode } from "../demo/store";

export function attachAuthInterceptors(api: AxiosInstance) {
    api.interceptors.request.use((config) => {
        if (isDemoMode()) {
            return config;
        }

        const token = getToken();

        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    });

    api.interceptors.response.use(
        (response) => response,
        (error) => {
            // Demo authentication never depends on real backend responses. A failing
            // or unreachable microservice must never clear the demo session or bounce
            // the user back to the login page.
            if (isDemoMode()) {
                return Promise.reject(error);
            }

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
