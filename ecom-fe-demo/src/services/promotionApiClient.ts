import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const promotionApiClient = axios.create({
    baseURL: import.meta.env.VITE_PROMOTION_API_URL || "http://localhost:5278",
});

attachAuthInterceptors(promotionApiClient);

export default promotionApiClient;
