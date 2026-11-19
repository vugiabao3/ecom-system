import productApiClient from "./productApiClient";
import { isDemoMode } from "../demo/store";

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
    if (isDemoMode()) {
        const cats = [
            { id: 1, name: "Điện tử" }, { id: 2, name: "Thời trang" }, { id: 3, name: "Gia dụng" },
            { id: 4, name: "Làm đẹp" }, { id: 5, name: "Phụ kiện" },
        ];
        return Promise.resolve({ data: cats });
    }
    return productApiClient.get<CategoryDto[]>("/api/categories");
};

export const getCategoryById = (id: number) => {
    if (isDemoMode()) {
        const cats: any[] = [
            { id: 1, name: "Điện tử" }, { id: 2, name: "Thời trang" }, { id: 3, name: "Gia dụng" },
            { id: 4, name: "Làm đẹp" }, { id: 5, name: "Phụ kiện" },
        ];
        return Promise.resolve({ data: cats.find(c => c.id === id) });
    }
    return productApiClient.get<CategoryDto>(`/api/categories/${id}`);
};

export const createCategory = (data: { name: string }) =>
    productApiClient.post("/api/categories", data);

export const updateCategory = (id: number, data: { name: string }) =>
    productApiClient.put(`/api/categories/${id}`, data);

export const deleteCategory = (id: number) =>
    productApiClient.delete(`/api/categories/${id}`);
