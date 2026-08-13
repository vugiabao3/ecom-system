import axios from "axios";

const inventoryApiClient = axios.create({
    baseURL: "http://localhost:5270"
});

inventoryApiClient.interceptors.request.use((config) => {

    const token = localStorage.getItem("token");

    if (token) {
        config.headers.Authorization =
            `Bearer ${token}`;
    }

    return config;
});

export default inventoryApiClient;