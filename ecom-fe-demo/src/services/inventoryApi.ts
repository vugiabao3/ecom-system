import inventoryApiClient from "./inventoryApiClient";

export interface InventoryDto {
    productId: string;
    available: number;
    reserved: number;
}

export const getInventoryByProductId = (productId: string) => {
    return inventoryApiClient.get<InventoryDto>(`/api/Inventory/${productId}`);
};

export const addStock = (data: { productId: string; quantity: number }) => {
    return inventoryApiClient.post("/api/Inventory", data);
};

export const getInventoryList = (page = 1, pageSize = 20) => {
    return inventoryApiClient.get(`/api/inventory?page=${page}&pageSize=${pageSize}`);
};

export const getInventoryByVariantId = (variantId: string) => {
    return inventoryApiClient.get(`/api/inventory/variants/${variantId}`);
};

export const getInventorySummary = (productId: string) => {
    return inventoryApiClient.get(`/api/inventory/products/${productId}/summary`);
};

export const getProductVariantInventory = (productId: string) => {
    return inventoryApiClient.get(`/api/inventory/products/${productId}/variants`);
};