import api from "./authApiClient";

export const login = (data: any) =>
    api.post("/auth/login", data);

export const register = (data: any) =>
    api.post("/auth/register", data);

export const forgotPassword = (data: any) =>
    api.post("/auth/forgot-password", data);

export const resetPassword = (data: any) =>
    api.post("/auth/reset-password", data);

export const changePassword = (data: any) =>
    api.post("/auth/change-password", data);

export const logout = (data: { refreshToken: string }) =>
    api.post("/auth/logout", data);

export const refresh = (data: any) =>
    api.post("/auth/refresh", data);