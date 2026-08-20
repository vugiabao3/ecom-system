import promotionApiClient
from "./promotionApiClient";

export const applyCoupon = (
    couponCode: string,
    totalAmount: number
) => {

    return promotionApiClient.post(
        "/api/Promotion/apply",
        {
            couponCode,
            totalAmount
        }
    );
};