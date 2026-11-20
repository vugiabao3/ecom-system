import React from "react";
import ReactDOM from "react-dom/client";

import App from "./App";
import { seedDemoIfEmpty } from "./demo/seed";
import { isDemoMode } from "./demo/store";

import "./index.css";
import "./styles/product.css";
console.info("[EcomSystem] Demo Mode:", isDemoMode());
seedDemoIfEmpty();
ReactDOM.createRoot(
    document.getElementById("root")!
).render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);