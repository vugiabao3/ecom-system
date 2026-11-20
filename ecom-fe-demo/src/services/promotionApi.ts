import promotionApiClient from "./promotionApiClient";
import { isDemoMode } from "../demo/store";
import { demoPromotionApi } from "../demo/services";

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
    if (isDemoMode()) return demoPromotionApi.getAllPromotions() as any;
    return promotionApiClient.get<PromotionDto[]>("/api/Promotion/all");
};

export const getPromotionsBySeller = (sellerId: string) => {
    if (isDemoMode()) return demoPromotionApi.getPromotionsBySeller() as any;
    return promotionApiClient.get<PromotionDto[]>(`/api/Promotion/seller/${sellerId}`);
};

export const createPromotion = (data: CreatePromotionRequest) => {
    if (isDemoMode()) return demoPromotionApi.createPromotion(data) as any;
    return promotionApiClient.post("/api/Promotion/create", data);
};

export const updatePromotion = (data: UpdatePromotionRequest) => {
    if (isDemoMode()) return demoPromotionApi.updatePromotion(data) as any;
    return promotionApiClient.put("/api/Promotion/update", data);
};

export const deletePromotion = (id: string) => {
    if (isDemoMode()) return demoPromotionApi.deletePromotion(id) as any;
    return promotionApiClient.delete(`/api/Promotion/delete/${id}`);
};
