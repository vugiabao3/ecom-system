import axios from "axios";
import { attachAuthInterceptors } from "./httpInterceptors";

const userApiClient = axios.create({
    baseURL: import.meta.env.VITE_USER_API_URL || "http://localhost:5004",
});

attachAuthInterceptors(userApiClient);

export default userApiClient;
