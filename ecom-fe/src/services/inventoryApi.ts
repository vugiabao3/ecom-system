import inventoryApiClient
from "./inventoryApiClient";

export const getInventoryByProductId =
    (productId: string) => {

        return inventoryApiClient.get(
            `/api/inventory/${productId}`
        );
    };