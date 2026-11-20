import api from "./authApiClient";
import { isDemoMode } from "../demo/store";
import { demoAuthApi } from "../demo/services";

export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
}

export interface ChangePasswordRequest {
    oldPassword: string;
    newPassword: string;
    email?: string;
}

export interface ForgotPasswordRequest {
    email: string;
}

export interface ResetPasswordRequest {
    token: string;
    newPassword: string;
}

export interface GoogleLoginRequest {
    idToken: string;
}

export interface AuthResponse {
    accessToken: string;
    refreshToken: string;
}

export const login = (data: LoginRequest) =>
    api.post<AuthResponse>("/auth/login", data);

export const register = (data: RegisterRequest) => {
    if (isDemoMode()) return demoAuthApi.register(data) as any;
    return api.post("/auth/register", data);
};

export const forgotPassword = (data: ForgotPasswordRequest) => {
    if (isDemoMode()) return demoAuthApi.forgotPassword(data) as any;
    return api.post("/auth/forgot-password", data);
};

export const resetPassword = (data: ResetPasswordRequest) => {
    if (isDemoMode()) return demoAuthApi.resetPassword(data) as any;
    return api.post("/auth/reset-password", data);
};

export const changePassword = (data: ChangePasswordRequest) => {
    if (isDemoMode()) return demoAuthApi.changePassword(data) as any;
    return api.post("/auth/change-password", data);
};

export const logout = (data: { refreshToken: string }) => {
    if (isDemoMode()) return demoAuthApi.logout() as any;
    return api.post("/auth/logout", data);
};

export const refresh = (data: { refreshToken: string }) =>
    api.post<AuthResponse>("/auth/refresh", data);

export const googleLogin = (data: GoogleLoginRequest) =>
    api.post<AuthResponse>("/auth/oauth/google", data);

export const verifyEmail = (data: { token: string }) => {
    if (isDemoMode()) return demoAuthApi.verifyEmail() as any;
    return api.post("/auth/verify-email", data);
};

export const resendVerification = (data: { email: string }) => {
    if (isDemoMode()) return demoAuthApi.resendVerification() as any;
    return api.post("/auth/resend-verification", data);
};

export const logoutAll = () => {
    if (isDemoMode()) return demoAuthApi.logoutAll() as any;
    return api.post("/auth/logout-all");
};

export const adminSetRole = (userId: string, role: string) => {
    if (isDemoMode()) return demoAuthApi.adminSetRole(userId, role) as any;
    return api.post("/api/admin/set-role", null, {
        params: { userId, role }
    });
};

export const adminSetActive = (userId: string, status: string) => {
    if (isDemoMode()) return demoAuthApi.adminSetActive(userId, status) as any;
    return api.post("/api/admin/set-active", null, {
        params: { userId, Status: status }
    });
};
