import cartApiClient
from "./cartApiClient";

export const addToCart = (data: any) => {
    return cartApiClient.post("/api/Cart", data);
};

export const getCart = () => {
    return cartApiClient.get("/api/Cart");
};

export const updateCartItem = (itemId: string, data: any) => {
    return cartApiClient.put(`/api/Cart/items/${itemId}`, data);
};

export const updateCartItemQuantity = (itemId: string, quantity: number) => {
    return cartApiClient.patch(`/api/Cart/items/${itemId}/quantity`, { quantity });
};

export const removeCartItem = (itemId: string) => {
    return cartApiClient.delete(`/api/Cart/items/${itemId}`);
};

export const clearCart = () => {
    return cartApiClient.delete("/api/Cart/clear");
};

export const getSelectedCart = () => {
    return cartApiClient.get<any[]>("/api/Cart/selected");
};