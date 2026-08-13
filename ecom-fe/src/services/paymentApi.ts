import api from "./paymentApiClient";

export const createPayment = (
    data: any
) => {

    return api.post(
        "/api/Payments",
        data
    );
};