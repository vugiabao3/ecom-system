import axios from "axios";

const paymentApiClient = axios.create({
    baseURL: "http://localhost:5289" // sửa theo PaymentService của bạn
});

paymentApiClient.interceptors.request.use(config => {

    const token =
        localStorage.getItem("token");

    if (token) {

        config.headers.Authorization =
            `Bearer ${token}`;
    }

    return config;
});

export default paymentApiClient;