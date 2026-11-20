import userApiClient from "./userApiClient";
import { isDemoMode } from "../demo/store";
import { demoUserApi } from "../demo/services";

export interface CreateUserRequest {
    id: string;
    email: string;
    password?: string;
    fullName: string;
}

export interface UpdateUserRequest {
    fullName?: string;
    passwordHash?: string;
    phone?: string;
    avatar?: string;
    currentAddress?: string;
    currentLocation?: string;
}

export interface UserAddressDto {
    id?: string;
    userId?: string;
    fullName: string;
    phone: string;
    addressLine: string;
    city: string;
    country: string;
    postalCode: string;
}

export interface CreateAddressRequest {
    fullName: string;
    phone: string;
    addressLine: string;
    city: string;
    country: string;
    postalCode: string;
}

export const createUser = (data: CreateUserRequest) => {
    if (isDemoMode()) return demoUserApi.createUser(data) as any;
    return userApiClient.post("/users/create", data);
};

export const getUserById = (id: string) => {
    if (isDemoMode()) return demoUserApi.getUserById(id) as any;
    return userApiClient.get(`/users/${id}`);
};

export const getUserProfile = (id: string) => {
    if (isDemoMode()) return demoUserApi.getUserProfile(id) as any;
    return userApiClient.get(`/users/${id}/profile`);
};

export const getUserByEmail = (email: string) => {
    if (isDemoMode()) return demoUserApi.getUserByEmail(email) as any;
    return userApiClient.get("/users/by-email", {
        params: { email }
    });
};

export const updateUser = (id: string, data: UpdateUserRequest) => {
    if (isDemoMode()) return demoUserApi.updateUser(id, data) as any;
    return userApiClient.put(`/users/${id}`, data);
};

export const getAllUsers = (page: number = 1, pageSize: number = 10) => {
    if (isDemoMode()) return demoUserApi.getAllUsers(page, pageSize) as any;
    return userApiClient.get("/users/getAllUser", {
        params: { page, pageSize }
    });
};

export const searchUsers = (keyword: string, page: number = 1, pageSize: number = 10) => {
    if (isDemoMode()) return demoUserApi.searchUsers(keyword, page, pageSize) as any;
    return userApiClient.get("/users/search", {
        params: { keyword, page, pageSize }
    });
};

export const blockUser = (id: string) => {
    if (isDemoMode()) return demoUserApi.blockUser(id) as any;
    return userApiClient.post(`/users/${id}/block`);
};

export const unblockUser = (id: string) => {
    if (isDemoMode()) return demoUserApi.unblockUser(id) as any;
    return userApiClient.post(`/users/${id}/unblock`);
};

export const softDeleteUser = (id: string) => {
    if (isDemoMode()) return demoUserApi.softDeleteUser(id) as any;
    return userApiClient.delete(`/users/${id}`);
};

export const restoreUser = (id: string) => {
    if (isDemoMode()) return demoUserApi.restoreUser(id) as any;
    return userApiClient.post(`/users/${id}/restore`);
};

export const getUserActivity = (id: string) => {
    if (isDemoMode()) return demoUserApi.getUserActivity(id) as any;
    return userApiClient.get(`/users/${id}/activity`);
};

export const getUserAddresses = (id: string) => {
    if (isDemoMode()) return demoUserApi.getUserAddresses(id) as any;
    return userApiClient.get(`/users/${id}/addresses`);
};

export const addAddress = (id: string, data: CreateAddressRequest) => {
    if (isDemoMode()) return demoUserApi.addAddress(id, data) as any;
    return userApiClient.post(`/users/${id}/addresses`, data);
};

export const getUserDevices = (id: string) => {
    if (isDemoMode()) return demoUserApi.getUserDevices(id) as any;
    return userApiClient.get(`/users/${id}/devices`);
};

export const logoutAllDevices = (id: string) => {
    if (isDemoMode()) return demoUserApi.logoutAllDevices(id) as any;
    return userApiClient.post(`/users/${id}/logout-all`);
};
