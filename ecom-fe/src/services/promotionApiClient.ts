import axios from "axios";

const token = localStorage.getItem("token");

const promotionApiClient = axios.create({
    baseURL: "http://localhost:5278",
    headers: {
        Authorization: `Bearer ${token}`
    }
});

export default promotionApiClient;