import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const promotionApiClient = axios.create({
    baseURL: "http://localhost:5278",
});

attachAuthInterceptors(promotionApiClient);

export default promotionApiClient;
