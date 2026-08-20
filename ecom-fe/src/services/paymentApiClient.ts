import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const paymentApiClient = axios.create({
    baseURL: "http://localhost:5289",
});

attachAuthInterceptors(paymentApiClient);

export default paymentApiClient;
