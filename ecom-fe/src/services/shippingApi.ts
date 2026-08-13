import shippingApiClient
from "./shippingApiClient";

export const calculateShippingFee = (
    address: string
) => {

    return shippingApiClient.post(
        "/api/shipping/calculate",
        {
            address
        }
    );
};

export const getShippingStatus = (
    orderId: string
) => {

    return shippingApiClient.get(
        `/api/shipping/${orderId}`
    );
};