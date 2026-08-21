import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const cartApiClient = axios.create({
    baseURL: import.meta.env.VITE_CART_API_URL || "http://localhost:5002",
});

attachAuthInterceptors(cartApiClient);

export default cartApiClient;
