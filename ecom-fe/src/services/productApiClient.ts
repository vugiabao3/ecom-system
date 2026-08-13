import axios from "axios";

const productApiClient = axios.create({

    baseURL: "http://localhost:5003",

});

productApiClient.interceptors.request.use(
    (config) => {

        const token =
            localStorage.getItem("token");

        if (token) {

            config.headers.Authorization =
                `Bearer ${token}`;
        }

        return config;
    }
);

export default productApiClient;