import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const orderApiClient = axios.create({
    baseURL: "http://localhost:5290",
});

attachAuthInterceptors(orderApiClient);

export default orderApiClient;
