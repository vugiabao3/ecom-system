import shippingApiClient from "./shippingApiClient";

export interface ShipmentDto {
    id: string;
    orderId: string;
    status: string;
    shipperId?: string;
    receiverName?: string;
    phone?: string;
    address?: string;
    trackingCode?: string;
    createdAt?: string;
    updatedAt?: string;
    deliveredAt?: string;
    failureReason?: string;
    paymentMethod?: string;
    paymentStatus?: string;
}

export const getShippingStatusByOrderId = (orderId: string) => {
    return shippingApiClient.get<ShipmentDto>(`/api/Shipping/by-order/${orderId}`);
};

export const getMyShipments = () => {
    return shippingApiClient.get<ShipmentDto[]>("/api/Shipping/my-shipments");
};

export const startDelivery = (shipmentId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/start-delivery`);
};

export const completeShipping = (shipmentId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/complete`);
};

export const assignShipment = (shipmentId: string, shipperId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/assign`, { shipperId });
};

export const pickupShipment = (shipmentId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/pickup`);
};

export const deliverShipment = (shipmentId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/deliver`);
};

export const failShipment = (shipmentId: string) => {
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/fail`, {});
};

export const confirmCashReceived = (orderId: string) => {
    return shippingApiClient.post(`/api/Payments/${orderId}/confirm-cod`);
};

export interface CreateShipmentRequest {
    orderId: string;
    address: string;
    phone: string;
    receiverName: string;
    shipperId?: string;
}

export const createShipment = (data: CreateShipmentRequest) => {
    return shippingApiClient.post("/api/Shipping", data);
};
