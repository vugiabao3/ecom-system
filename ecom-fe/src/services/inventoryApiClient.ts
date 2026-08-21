import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const inventoryApiClient = axios.create({
    baseURL: import.meta.env.VITE_INVENTORY_API_URL || "http://localhost:5270",
});

attachAuthInterceptors(inventoryApiClient);

export default inventoryApiClient;
