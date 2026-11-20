import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const productApiClient = axios.create({
    baseURL: import.meta.env.VITE_PRODUCT_API_URL || "http://localhost:5003",
});

attachAuthInterceptors(productApiClient);

export default productApiClient;
