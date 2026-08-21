import api from "./productApiClient";

export interface ProductItem {
    id: string;
    name: string;
    price: number;
    categoryName?: string;
    categoryId?: number;
    imageUrl?: string;
    rating?: number;
    description?: string;
}

export interface GetProductsParams {
    Page?: number;
    PageSize?: number;
    CategoryId?: number;
    MinPrice?: number;
    MaxPrice?: number;
    SortBy?: string;
}

export interface SearchProductsParams {
    keyword: string;
    categoryId?: number;
    minPrice?: number;
    maxPrice?: number;
    page?: number;
    pageSize?: number;
    sortBy?: string;
}

export interface CreateProductRequest {
    name: string;
    price: number;
    categoryId: number;
    imageUrl?: string;
}

export interface UpdateProductRequest {
    name: string;
    price: number;
    categoryId: number;
}

export const getProducts = (params?: GetProductsParams) => {
    return api.get("/api/Products", {
        params,
    });
};

export const getProductById = (id: string) =>
    api.get(`/api/Products/${id}`);

export const getProductsByIds = (ids: string[]) =>
    api.post("/api/Products/batch", ids);

export const searchProducts = (paramsOrKeyword: string | SearchProductsParams) => {
    const params =
        typeof paramsOrKeyword === "string"
            ? { keyword: paramsOrKeyword }
            : paramsOrKeyword;

    return api.get("/api/Products/search", {
        params,
    });
};

export const createProduct = (data: CreateProductRequest) =>
    api.post("/api/Products", data);

export const updateProduct = (id: string, data: UpdateProductRequest) =>
    api.put(`/api/Products/${id}`, data);

export const deleteProduct = (id: string) =>
    api.delete(`/api/Products/${id}`);

export const restoreProduct = (id: string) =>
    api.put(`/api/Products/${id}/restore`);