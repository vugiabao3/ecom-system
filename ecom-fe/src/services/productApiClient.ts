import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const productApiClient = axios.create({
    baseURL: "http://localhost:5003",
});

attachAuthInterceptors(productApiClient);

export default productApiClient;
