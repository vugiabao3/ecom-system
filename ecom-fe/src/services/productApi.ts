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
    sellerId?: string;
    brandId?: string;
    brandName?: string;
    isDeleted?: boolean;
    status?: string;
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

export const getProductsBySeller = (sellerId: string) =>
    api.get<ProductItem[]>(`/api/Products/seller/${sellerId}`);

export const getBrandsBySeller = (sellerId: string) =>
    api.get<any[]>(`/api/Products/brand/seller/${sellerId}`);

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

export const createBrand = (data: { name: string }) =>
    api.post("/api/Products/brands", data);

export const updateProduct = (id: string, data: UpdateProductRequest) =>
    api.put(`/api/Products/${id}`, data);

export const deleteProduct = (id: string) =>
    api.delete(`/api/Products/${id}`);

export const restoreProduct = (id: string) =>
    api.put(`/api/Products/${id}/restore`);

export const patchProduct = (id: string, data: Partial<UpdateProductRequest>) =>
    api.patch(`/api/Products/${id}`, data);

export const getNewArrivals = (params?: GetProductsParams) =>
    api.get("/api/Products/new-arrivals", { params });

export const getBestSelling = (params?: GetProductsParams) =>
    api.get("/api/Products/best-selling", { params });

export const getRelatedProducts = (id: string) =>
    api.get(`/api/Products/${id}/related`);

export const getRecommendations = (id: string) =>
    api.get(`/api/Products/${id}/recommendations`);

export const patchProductStatus = (id: string, status: string) =>
    api.patch(`/api/Products/${id}/status`, { status });

export const duplicateProduct = (id: string) =>
    api.post(`/api/Products/${id}/duplicate`);

export const getProductImages = (id: string) =>
    api.get(`/api/Products/${id}/images`);

export const uploadProductImage = (id: string, file: File) => {
    const form = new FormData();
    form.append("file", file);
    return api.post(`/api/Products/${id}/images`, form, {
        headers: { "Content-Type": "multipart/form-data" },
    });
};

export const updateProductImage = (id: string, imageId: string, data: any) =>
    api.put(`/api/Products/${id}/images/${imageId}`, data);

export const deleteProductImage = (id: string, imageId: string) =>
    api.delete(`/api/Products/${id}/images/${imageId}`);

export const setPrimaryProductImage = (productId: string, imageId: string) =>
    api.patch(`/api/Products/${productId}/images/${imageId}/primary`);

export const reorderProductImages = (productId: string, imageIds: string[]) =>
    api.patch(`/api/Products/${productId}/images/reorder`, imageIds);

export const getProductVariants = (productId: string) =>
    api.get(`/api/Products/${productId}/variants`);

export const createProductVariant = (productId: string, data: any) =>
    api.post(`/api/Products/${productId}/variants`, data);

export const getProductVariant = (productId: string, variantId: string) =>
    api.get(`/api/Products/${productId}/variants/${variantId}`);