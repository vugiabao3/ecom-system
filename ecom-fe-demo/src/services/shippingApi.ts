import shippingApiClient from "./shippingApiClient";
import { isDemoMode } from "../demo/store";
import { demoShippingApi } from "../demo/services";

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
    if (isDemoMode()) return demoShippingApi.getShippingStatusByOrderId(orderId) as any;
    return shippingApiClient.get<ShipmentDto>(`/api/Shipping/by-order/${orderId}`);
};

export const getMyShipments = () => {
    if (isDemoMode()) return demoShippingApi.getMyShipments() as any;
    return shippingApiClient.get<ShipmentDto[]>("/api/Shipping/my-shipments");
};

export const startDelivery = (shipmentId: string) => {
    if (isDemoMode()) return demoShippingApi.startDelivery(shipmentId) as any;
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/start-delivery`);
};

export const completeShipping = (shipmentId: string) => {
    if (isDemoMode()) return demoShippingApi.completeShipping(shipmentId) as any;
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/complete`);
};

export const assignShipment = (shipmentId: string, shipperId: string) => {
    if (isDemoMode()) return demoShippingApi.assignShipment(shipmentId, shipperId) as any;
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/assign`, { shipperId });
};

export const pickupShipment = (shipmentId: string) => {
    if (isDemoMode()) return demoShippingApi.pickupShipment(shipmentId) as any;
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/pickup`);
};

export const deliverShipment = (shipmentId: string) => {
    if (isDemoMode()) return demoShippingApi.deliverShipment(shipmentId) as any;
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/deliver`);
};

export const failShipment = (shipmentId: string) => {
    if (isDemoMode()) return demoShippingApi.failShipment(shipmentId) as any;
    return shippingApiClient.post(`/api/Shipping/${shipmentId}/fail`, {});
};

export const confirmCashReceived = (orderId: string) => {
    if (isDemoMode()) return demoShippingApi.confirmCashReceived(orderId) as any;
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
    if (isDemoMode()) return demoShippingApi.createShipment(data) as any;
    return shippingApiClient.post("/api/Shipping", data);
};
