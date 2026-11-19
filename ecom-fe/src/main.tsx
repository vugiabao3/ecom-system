import React from "react";
import ReactDOM from "react-dom/client";

import App from "./App";
import { seedDemoIfEmpty } from "./demo/seed";

import "./index.css";
import "./styles/product.css";
seedDemoIfEmpty();
ReactDOM.createRoot(
    document.getElementById("root")!
).render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);