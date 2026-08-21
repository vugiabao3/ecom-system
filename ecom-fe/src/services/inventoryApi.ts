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

export const reserveStock = (data: { productId: string; quantity: number }) => {
    return inventoryApiClient.post("/api/Inventory/reserve", data);
};