import api from "./orderApiClient";

export interface CreateOrderRequest {
    address: string;
    phone: string;
    receiverName: string;
    couponCode?: string;
}

export interface CreateOrderResponse {
    orderId: string;
    subTotal: number;
    discount: number;
    totalPrice: number;
}

export interface OrderDto {
    id: string;
    totalPrice: number;
    status: string;
    address: string;
    phone: string;
    receiverName: string;
}

export const createOrder = (data: CreateOrderRequest) => {
    return api.post<CreateOrderResponse>("/api/Orders/checkout", data);
};

export const getOrderById = (id: string) => {
    return api.get<OrderDto>(`/api/Orders/${id}`);
};