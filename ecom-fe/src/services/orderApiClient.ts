import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const orderApiClient = axios.create({
    baseURL: import.meta.env.VITE_ORDER_API_URL || "http://localhost:5290",
});

attachAuthInterceptors(orderApiClient);

export default orderApiClient;
