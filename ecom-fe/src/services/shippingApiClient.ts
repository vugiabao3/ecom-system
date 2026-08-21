import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const shippingApiClient = axios.create({
    baseURL: import.meta.env.VITE_SHIPPING_API_URL || "http://localhost:5243",
});

attachAuthInterceptors(shippingApiClient);

export default shippingApiClient;
