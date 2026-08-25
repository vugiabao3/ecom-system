import api from "./orderApiClient";

export interface CreateOrderRequest {
    address: string;
    phone: string;
    receiverName: string;
    couponCode?: string;
    paymentMethod: string;
}

export interface CreateOrderResponse {
    orderId: string;
    subTotal: number;
    discount: number;
    shippingFee: number;
    totalPrice: number;
    paymentMethod: string;
}

export interface OrderItemDto {
    productId: string;
    sellerId: string;
    quantity: number;
}

export interface OrderDto {
    id: string;
    totalPrice: number;
    status: string;
    paymentStatus: string;
    paymentMethod: string;
    shippingFee: number;
    subTotal: number;
    address: string;
    phone: string;
    receiverName: string;
    items: OrderItemDto[];
}

export interface UpdateOrderStatusRequest {
    status: string;
}

export const createOrder = (data: CreateOrderRequest) => {
    return api.post<CreateOrderResponse>("/api/Orders/checkout", data);
};

export const getOrderById = (id: string) => {
    return api.get<OrderDto>(`/api/Orders/${id}`);
};

export const getOrdersByUserId = (userId: string) => {
    return api.get<OrderDto[]>(`/api/Orders/user/${userId}`);
};

export const getOrdersBySellerId = (sellerId: string) => {
    return api.get<OrderDto[]>(`/api/Orders/seller/${sellerId}`);
};

export const updateOrderStatus = (id: string, status: string) => {
    return api.put(`/api/Orders/${id}/status`, { status });
};

export const cancelOrder = (id: string) => {
    return api.post(`/api/Orders/${id}/cancel`, {});
};