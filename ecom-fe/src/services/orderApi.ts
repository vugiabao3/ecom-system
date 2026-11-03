import api from "./orderApiClient";

export interface CreateOrderRequest {
    address: string;
    phone: string;
    receiverName: string;
    couponCode?: string;
    paymentMethod: string;
    checkoutId?: string;
    idempotencyKey?: string;
}

export interface CreateOrderResponse {
    orderId: string;
    checkoutId?: string;
    subTotal: number;
    discount: number;
    shippingFee: number;
    totalPrice: number;
    paymentMethod: string;
    status: string;
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

// Preview
export interface PreviewRequest {
    address?: string;
    phone?: string;
    receiverName?: string;
    couponCode?: string;
    paymentMethod: string;
}

export interface PreviewItemDto {
    productId: string;
    sellerId: string;
    productName: string;
    sku?: string;
    variantName?: string;
    imageUrl?: string;
    price: number;
    quantity: number;
    subtotal: number;
    isValid: boolean;
    error?: string;
}

export interface PreviewResponse {
    checkoutId: string;
    items: PreviewItemDto[];
    subTotal: number;
    shippingFee: number;
    discount: number;
    totalPrice: number;
    isValid: boolean;
    errors: string[];
}

// Validate
export interface ValidateRequest {
    couponCode?: string;
}

export interface ItemValidationDto {
    productId: string;
    productName: string;
    quantity: number;
    isValid: boolean;
    error?: string;
}

export interface ValidateResponse {
    isValid: boolean;
    errors: string[];
    itemValidations: ItemValidationDto[];
}

// Calculate
export interface CalculateRequest {
    couponCode?: string;
}

export interface CalculateResponse {
    subTotal: number;
    shippingFee: number;
    discount: number;
    totalPrice: number;
}

// Checkout session
export interface CheckoutSessionItemDto {
    id: string;
    productId: string;
    sellerId: string;
    productName: string;
    sku?: string;
    variantName?: string;
    imageUrl?: string;
    price: number;
    quantity: number;
}

export interface CheckoutSessionDto {
    id: string;
    userId: string;
    status: string;
    items: CheckoutSessionItemDto[];
    subTotal: number;
    shippingFee: number;
    discount: number;
    totalPrice: number;
    couponCode?: string;
    address: string;
    phone: string;
    receiverName: string;
    paymentMethod: string;
    createdAt: string;
    expiresAt?: string;
    orderId?: string;
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

export const previewOrder = (data: PreviewRequest) => {
    return api.post<PreviewResponse>("/api/Orders/preview", data);
};

export const validateOrder = (data: ValidateRequest) => {
    return api.post<ValidateResponse>("/api/Orders/validate", data);
};

export const calculateOrder = (data: CalculateRequest) => {
    return api.post<CalculateResponse>("/api/Orders/calculate", data);
};

export const getCheckoutSession = (checkoutId: string) => {
    return api.get<CheckoutSessionDto>(`/api/Orders/checkout/${checkoutId}`);
};

// Confirm checkout
export interface ConfirmCheckoutRequest {
    checkoutId: string;
}

export interface OrderConfirmationResponse {
    checkoutId: string;
    orderId: string;
    status: string;
    message: string;
}

export const confirmCheckout = (data: ConfirmCheckoutRequest) => {
    return api.post<OrderConfirmationResponse>("/api/Orders/checkout/confirm", data);
};

// Order confirmation
export const getOrderConfirmation = (orderId: string) => {
    return api.get<OrderDetailDto>(`/api/Orders/${orderId}/confirmation`);
};

// My orders (paginated)
export interface MyOrdersRequest {
    page?: number;
    pageSize?: number;
    statusFilter?: string;
}

export interface OrderListItemDto {
    id: string;
    orderNumber: string;
    status: string;
    paymentStatus: string;
    paymentMethod: string;
    totalPrice: number;
    subTotal: number;
    shippingFee: number;
    discount: number;
    createdAt: string;
    receiverName: string;
    address: string;
    itemCount: number;
    firstProductImage?: string;
    firstProductName?: string;
}

export interface PaginatedOrderResponse {
    items: OrderListItemDto[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export const getMyOrders = (params: MyOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
            statusFilter: params.statusFilter ?? undefined,
        },
    });
};

// Order summary
export interface OrderSummaryDto {
    totalOrders: number;
    pendingOrders: number;
    confirmedOrders: number;
    preparingOrders: number;
    readyForShipmentOrders: number;
    shippingOrders: number;
    deliveredOrders: number;
    cancelledOrders: number;
    returnedOrders: number;
    totalSpent: number;
    averageOrderValue: number;
}

export const getOrderSummary = () => {
    return api.get<OrderSummaryDto>("/api/Orders/me/summary");
};

// Order detail (buyer)
export interface OrderDetailItemDto {
    id: string;
    productId: string;
    sellerId: string;
    productName: string;
    sku?: string;
    variantName?: string;
    imageUrl?: string;
    price: number;
    quantity: number;
    subtotal: number;
}

export interface OrderStatusHistoryDto {
    id: string;
    status: string;
    note?: string;
    createdAt: string;
}

export interface OrderDetailDto {
    id: string;
    status: string;
    paymentStatus: string;
    paymentMethod: string;
    subTotal: number;
    shippingFee: number;
    discount: number;
    totalPrice: number;
    address: string;
    phone: string;
    receiverName: string;
    createdAt: string;
    updatedAt?: string;
    items: OrderDetailItemDto[];
    statusHistory: OrderStatusHistoryDto[];
    shippingInfo?: {
        id: string;
        orderId: string;
        status: string;
        trackingCode?: string;
        shipperId?: string;
        receiverName?: string;
        phone?: string;
        address?: string;
        failureReason?: string;
        createdAt?: string;
        updatedAt?: string;
        deliveredAt?: string;
    };
}

export const getMyOrderDetail = (orderId: string) => {
    return api.get<OrderDetailDto>(`/api/Orders/me/${orderId}`);
};

// Recent orders
export interface RecentOrdersRequest {
    page?: number;
    pageSize?: number;
    limit?: number;
}

export const getRecentOrders = (params: RecentOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/recent", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
            limit: params.limit ?? 10,
        },
    });
};

// Search orders
export interface SearchOrdersRequest {
    searchTerm: string;
    page?: number;
    pageSize?: number;
}

export const searchOrders = (params: SearchOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/search", {
        params: {
            searchTerm: params.searchTerm,
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Filter orders by status
export interface FilterOrdersByStatusRequest {
    statusFilter: string;
    page?: number;
    pageSize?: number;
}

export const filterOrdersByStatus = (params: FilterOrdersByStatusRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/filter", {
        params: {
            statusFilter: params.statusFilter,
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Pending orders
export interface PendingOrdersRequest {
    page?: number;
    pageSize?: number;
}

export const getPendingOrders = (params: PendingOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/pending", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Processing orders
export interface ProcessingOrdersRequest {
    page?: number;
    pageSize?: number;
}

export const getProcessingOrders = (params: ProcessingOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/processing", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Shipping orders
export interface ShippingOrdersRequest {
    page?: number;
    pageSize?: number;
}

export const getShippingOrders = (params: ShippingOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/shipping", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Completed (delivered) orders
export interface CompletedOrdersRequest {
    page?: number;
    pageSize?: number;
}

export const getCompletedOrders = (params: CompletedOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/completed", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Cancelled orders
export interface CancelledOrdersRequest {
    page?: number;
    pageSize?: number;
}

export const getCancelledOrders = (params: CancelledOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/cancelled", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Returned orders
export interface ReturnedOrdersRequest {
    page?: number;
    pageSize?: number;
}

export const getReturnedOrders = (params: ReturnedOrdersRequest) => {
    return api.get<PaginatedOrderResponse>("/api/Orders/me/returned", {
        params: {
            page: params.page ?? 1,
            pageSize: params.pageSize ?? 10,
        },
    });
};

// Order timeline (status history)
export const getOrderTimeline = (orderId: string) => {
    return api.get<OrderStatusHistoryDto[]>(`/api/Orders/me/${orderId}/timeline`);
};
