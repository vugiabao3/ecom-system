import api from "./authApiClient";

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

export const register = (data: RegisterRequest) =>
    api.post("/auth/register", data);

export const forgotPassword = (data: ForgotPasswordRequest) =>
    api.post("/auth/forgot-password", data);

export const resetPassword = (data: ResetPasswordRequest) =>
    api.post("/auth/reset-password", data);

export const changePassword = (data: ChangePasswordRequest) =>
    api.post("/auth/change-password", data);

export const logout = (data: { refreshToken: string }) =>
    api.post("/auth/logout", data);

export const refresh = (data: { refreshToken: string }) =>
    api.post<AuthResponse>("/auth/refresh", data);

export const googleLogin = (data: GoogleLoginRequest) =>
    api.post<AuthResponse>("/auth/oauth/google", data);

export const adminSetRole = (userId: string, role: string) =>
    api.post("/api/admin/set-role", null, {
        params: { userId, role }
    });

export const adminSetActive = (userId: string, status: string) =>
    api.post("/api/admin/set-active", null, {
        params: { userId, Status: status }
    });