import api from "./productApiClient";

export const getProducts = (params: any) => {

    return api.get("/api/Products", {
        params
    });

};

export const getProductById = (id: string) =>
    api.get(`/api/products/${id}`);

export const searchProducts = (
    keyword: string
) => {

    return api.get(
        `/api/Products/search`,
        {
            params: {
                keyword
            }
        }
    );

};