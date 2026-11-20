import api from "./paymentApiClient";
import { isDemoMode } from "../demo/store";
import { demoPaymentApi } from "../demo/services";

export interface CreatePaymentRequest {
    orderId: string;
    paymentMethod: string;
}

export interface CreatePaymentResponse {
    paymentId: string;
    status: string;
}

export const createPayment = (data: CreatePaymentRequest) => {
    if (isDemoMode()) return demoPaymentApi.createPayment(data) as any;
    return api.post<CreatePaymentResponse>("/api/Payments", data);
};

export const confirmPayment = (paymentId: string) => {
    if (isDemoMode()) return demoPaymentApi.confirmPayment(paymentId) as any;
    return api.post(`/api/Payments/${paymentId}/confirm`);
};

export const failPayment = (paymentId: string) => {
    if (isDemoMode()) return demoPaymentApi.failPayment(paymentId) as any;
    return api.post(`/api/Payments/${paymentId}/fail`, {});
};
