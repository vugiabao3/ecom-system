import productApiClient from "./productApiClient";
import { isDemoMode } from "../demo/store";
import { demoCategoryApi } from "../demo/services";

export interface CategoryDto {
    id: number;
    name: string;
    products?: Array<{
        id: string;
        name: string;
        price: number;
        categoryId: number;
    }>;
}

export const getCategories = () => {
    if (isDemoMode()) return demoCategoryApi.getAll() as any;
    return productApiClient.get<CategoryDto[]>("/api/categories");
};

export const getCategoryById = (id: number) => {
    if (isDemoMode()) return demoCategoryApi.getById(id) as any;
    return productApiClient.get<CategoryDto>(`/api/categories/${id}`);
};

export const createCategory = (data: { name: string }) =>
    productApiClient.post("/api/categories", data);

export const updateCategory = (id: number, data: { name: string }) =>
    productApiClient.put(`/api/categories/${id}`, data);

export const deleteCategory = (id: number) =>
    productApiClient.delete(`/api/categories/${id}`);
