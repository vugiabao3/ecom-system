import api from "./paymentApiClient";

export interface CreatePaymentRequest {
    orderId: string;
    paymentMethod: string;
}

export interface CreatePaymentResponse {
    paymentId: string;
    status: string;
}

export const createPayment = (data: CreatePaymentRequest) => {
    return api.post<CreatePaymentResponse>("/api/Payments", data);
};

export const confirmPayment = (paymentId: string) => {
    return api.post(`/api/Payments/${paymentId}/confirm`);
};

export const failPayment = (paymentId: string) => {
    return api.post(`/api/Payments/${paymentId}/fail`, {});
};