import axios from "axios";

const cartApiClient = axios.create({
    baseURL: "http://localhost:5002"
});

cartApiClient.interceptors.request.use((config) => {

    const token = localStorage.getItem("token");
console.log("TOKEN =", token);
    if (token) {
        config.headers.Authorization =
            `Bearer ${token}`;
    }

    return config;
});

export default cartApiClient;