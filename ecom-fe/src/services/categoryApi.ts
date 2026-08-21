import productApiClient from "./productApiClient";

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

export const getCategories = () =>
    productApiClient.get<CategoryDto[]>("/api/categories");

export const getCategoryById = (id: number) =>
    productApiClient.get<CategoryDto>(`/api/categories/${id}`);

export const createCategory = (data: { name: string }) =>
    productApiClient.post("/api/categories", data);

export const updateCategory = (id: number, data: { name: string }) =>
    productApiClient.put(`/api/categories/${id}`, data);

export const deleteCategory = (id: number) =>
    productApiClient.delete(`/api/categories/${id}`);
