import api from "./orderApiClient";

export const createOrder = (
    data: any
) => {

    return api.post(
        "/api/Orders/checkout",
        data
    );
};