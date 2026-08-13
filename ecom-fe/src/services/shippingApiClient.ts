import axios from "axios";

const token = localStorage.getItem("token");

const shippingApiClient = axios.create({
    baseURL: "http://localhost:5243",
    headers: {
        Authorization: `Bearer ${token}`
    }
});

export default shippingApiClient;