import inventoryApiClient from "./inventoryApiClient";
import { isDemoMode } from "../demo/store";
import { demoInventoryApi } from "../demo/services";

export interface InventoryDto {
    productId: string;
    available: number;
    reserved: number;
}

export const getInventoryByProductId = (productId: string) => {
    if (isDemoMode()) return demoInventoryApi.getInventoryByProductId(productId) as any;
    return inventoryApiClient.get<InventoryDto>(`/api/Inventory/${productId}`);
};

export const addStock = (data: { productId: string; quantity: number }) => {
    if (isDemoMode()) return demoInventoryApi.addStock(data) as any;
    return inventoryApiClient.post("/api/Inventory", data);
};

export const getInventoryList = (page = 1, pageSize = 20) => {
    if (isDemoMode()) return demoInventoryApi.getInventoryList(page, pageSize) as any;
    return inventoryApiClient.get(`/api/inventory?page=${page}&pageSize=${pageSize}`);
};

export const getInventoryByVariantId = (variantId: string) => {
    if (isDemoMode()) return demoInventoryApi.getInventoryByVariantId(variantId) as any;
    return inventoryApiClient.get(`/api/inventory/variants/${variantId}`);
};

export const getInventorySummary = (productId: string) => {
    if (isDemoMode()) return demoInventoryApi.getInventorySummary(productId) as any;
    return inventoryApiClient.get(`/api/inventory/products/${productId}/summary`);
};

export const getProductVariantInventory = (productId: string) => {
    if (isDemoMode()) return demoInventoryApi.getProductVariantInventory(productId) as any;
    return inventoryApiClient.get(`/api/inventory/products/${productId}/variants`);
};
