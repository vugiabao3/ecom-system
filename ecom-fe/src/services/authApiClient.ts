import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const api = axios.create({
    baseURL: "http://localhost:5001",
});

attachAuthInterceptors(api);

export default api;
