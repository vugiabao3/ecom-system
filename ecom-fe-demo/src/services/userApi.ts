import userApiClient from "./userApiClient";

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

export const createUser = (data: CreateUserRequest) =>
    userApiClient.post("/users/create", data);

export const getUserById = (id: string) =>
    userApiClient.get(`/users/${id}`);

export const getUserProfile = (id: string) =>
    userApiClient.get(`/users/${id}/profile`);

export const getUserByEmail = (email: string) =>
    userApiClient.get("/users/by-email", {
        params: { email }
    });

export const updateUser = (id: string, data: UpdateUserRequest) =>
    userApiClient.put(`/users/${id}`, data);

export const getAllUsers = (page: number = 1, pageSize: number = 10) =>
    userApiClient.get("/users/getAllUser", {
        params: { page, pageSize }
    });

export const searchUsers = (keyword: string, page: number = 1, pageSize: number = 10) =>
    userApiClient.get("/users/search", {
        params: { keyword, page, pageSize }
    });

export const blockUser = (id: string) =>
    userApiClient.post(`/users/${id}/block`);

export const unblockUser = (id: string) =>
    userApiClient.post(`/users/${id}/unblock`);

export const softDeleteUser = (id: string) =>
    userApiClient.delete(`/users/${id}`);

export const restoreUser = (id: string) =>
    userApiClient.post(`/users/${id}/restore`);

export const getUserActivity = (id: string) =>
    userApiClient.get(`/users/${id}/activity`);

export const getUserAddresses = (id: string) =>
    userApiClient.get(`/users/${id}/addresses`);

export const addAddress = (id: string, data: CreateAddressRequest) =>
    userApiClient.post(`/users/${id}/addresses`, data);

export const getUserDevices = (id: string) =>
    userApiClient.get(`/users/${id}/devices`);

export const logoutAllDevices = (id: string) =>
    userApiClient.post(`/users/${id}/logout-all`);
