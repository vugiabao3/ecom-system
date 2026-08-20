import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const shippingApiClient = axios.create({
    baseURL: "http://localhost:5243",
});

attachAuthInterceptors(shippingApiClient);

export default shippingApiClient;
