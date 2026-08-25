import promotionApiClient from "./promotionApiClient";

export interface PromotionDto {
    id: string;
    code: string;
    discountPercent: number;
    isActive: boolean;
    startDate: string;
    endDate: string;
    quantity: number;
    sellerId?: string;
    brandId?: string;
}

export interface CreatePromotionRequest {
    code: string;
    discountPercent: number;
    startDate: string;
    endDate: string;
    quantity: number;
}

export interface UpdatePromotionRequest {
    id: string;
    code: string;
    discountPercent: number;
    startDate: string;
    endDate: string;
    quantity: number;
    isActive: boolean;
}

export const getAllPromotions = () => {
    return promotionApiClient.get<PromotionDto[]>("/api/Promotion/all");
};

export const getPromotionsBySeller = (sellerId: string) => {
    return promotionApiClient.get<PromotionDto[]>(`/api/Promotion/seller/${sellerId}`);
};

export const createPromotion = (data: CreatePromotionRequest) => {
    return promotionApiClient.post("/api/Promotion/create", data);
};

export const updatePromotion = (data: UpdatePromotionRequest) => {
    return promotionApiClient.put("/api/Promotion/update", data);
};

export const deletePromotion = (id: string) => {
    return promotionApiClient.delete(`/api/Promotion/delete/${id}`);
};