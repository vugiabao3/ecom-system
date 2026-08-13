import axios from "axios";

const orderApiClient = axios.create({
    baseURL: "http://localhost:5290"
});

orderApiClient.interceptors.request.use(config => {

    const token =
        localStorage.getItem("token");

    if (token) {

        config.headers.Authorization =
            `Bearer ${token}`;
    }

    return config;
});

export default orderApiClient;