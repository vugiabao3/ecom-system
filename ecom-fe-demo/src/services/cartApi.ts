import cartApiClient from "./cartApiClient";
import { isDemoMode } from "../demo/store";
import { demoCartApi } from "../demo/services";

export const addToCart = (data: any) => {
    if (isDemoMode()) return demoCartApi.addToCart(data) as any;
    return cartApiClient.post("/api/Cart", data);
};

export const getCart = () => {
    if (isDemoMode()) return demoCartApi.getCart() as any;
    return cartApiClient.get("/api/Cart");
};

export const updateCartItem = (itemId: string, data: any) => {
    if (isDemoMode()) return demoCartApi.updateCartItem(itemId, data) as any;
    return cartApiClient.put(`/api/Cart/items/${itemId}`, data);
};

export const updateCartItemQuantity = (itemId: string, quantity: number) => {
    if (isDemoMode()) return demoCartApi.updateCartItemQuantity(itemId, quantity) as any;
    return cartApiClient.patch(`/api/Cart/items/${itemId}/quantity`, { quantity });
};

export const removeCartItem = (itemId: string) => {
    if (isDemoMode()) return demoCartApi.removeCartItem(itemId) as any;
    return cartApiClient.delete(`/api/Cart/items/${itemId}`);
};

export const clearCart = () => {
    if (isDemoMode()) return demoCartApi.clearCart() as any;
    return cartApiClient.delete("/api/Cart/clear");
};

export const getSelectedCart = () => {
    if (isDemoMode()) return demoCartApi.getSelectedCart() as any;
    return cartApiClient.get<any[]>("/api/Cart/selected");
};
