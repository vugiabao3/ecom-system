import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const cartApiClient = axios.create({
    baseURL: "http://localhost:5002",
});

attachAuthInterceptors(cartApiClient);

export default cartApiClient;
