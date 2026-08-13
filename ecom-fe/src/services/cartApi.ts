import cartApiClient
from "./cartApiClient";

// ADD TO CART
export const addToCart = (data: any) => {
    return cartApiClient.post("/api/Cart", data);
};

// GET CART
export const getCart = () => {
    return cartApiClient.get("/api/Cart");
};

// REMOVE ITEM
export const removeCartItem = (productId: string) => {
    return cartApiClient.delete(`/api/Cart`, {
        data: { productId }
    });
};

// CLEAR CART
export const clearCart = () => {
    return cartApiClient.delete("/api/Cart/clear");
};