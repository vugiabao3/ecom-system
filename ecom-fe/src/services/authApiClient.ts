import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const api = axios.create({
    baseURL: import.meta.env.VITE_AUTH_API_URL || "http://localhost:5001",
});

attachAuthInterceptors(api);

export default api;
