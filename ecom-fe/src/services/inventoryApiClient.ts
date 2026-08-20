import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const inventoryApiClient = axios.create({
    baseURL: "http://localhost:5270",
});

attachAuthInterceptors(inventoryApiClient);

export default inventoryApiClient;
