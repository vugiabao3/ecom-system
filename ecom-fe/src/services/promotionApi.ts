import promotionApiClient
from "./promotionApiClient";

export const applyCoupon = (
    code: string
) => {

    return promotionApiClient.get(
        `/promotions/apply?code=${code}`
    );
};