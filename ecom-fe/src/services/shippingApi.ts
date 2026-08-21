import shippingApiClient from "./shippingApiClient";

export interface ShipmentDto {
    id: string;
    orderId: string;
    status: string;
    createdAt?: string;
    updatedAt?: string;
}

export const getShippingStatusByOrderId = (orderId: string) => {
    return shippingApiClient.get<ShipmentDto>(`/api/Shipping/by-order/${orderId}`);
};

export const startDelivery = (shipmentId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/start-delivery`);
};

export const completeShipping = (shipmentId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/complete`);
};