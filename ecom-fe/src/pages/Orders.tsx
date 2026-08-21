import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import Navbar from "../components/Navbar";

export default function Orders() {
    const navigate = useNavigate();
    const [orderId, setOrderId] = useState("");
    const [error, setError] = useState<string | null>(null);

    const handleLookup = (e: React.FormEvent) => {
        e.preventDefault();
        if (!orderId.trim()) {
            setError("Please enter an order ID.");
            return;
        }
        setError(null);
        navigate(`/orders/${orderId.trim()}`);
    };

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "700px", margin: "40px auto", padding: "20px" }}>
                <div
                    style={{
                        background: "white",
                        borderRadius: "12px",
                        padding: "32px",
                        boxShadow: "0 2px 10px rgba(0,0,0,0.06)",
                    }}
                >
                    <h1 style={{ fontSize: "24px", color: "#333", marginBottom: "8px" }}>
                        📦 My Orders
                    </h1>
                    <p style={{ color: "#666", marginBottom: "24px" }}>
                        Enter your order ID to view order details, payment status, and shipment tracking.
                    </p>

                    {error && (
                        <div
                            style={{
                                padding: "10px",
                                background: "#ffe3e3",
                                color: "#e03131",
                                borderRadius: "6px",
                                marginBottom: "16px",
                                fontSize: "14px",
                            }}
                        >
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleLookup} style={{ display: "flex", gap: "10px" }}>
                        <input
                            type="text"
                            placeholder="Enter Order ID (e.g. 3fa85f64-5717-4562-b3fc-2c963f66afa6)"
                            value={orderId}
                            onChange={(e) => setOrderId(e.target.value)}
                            style={{
                                flex: 1,
                                padding: "10px 14px",
                                borderRadius: "8px",
                                border: "1px solid #ddd",
                                fontSize: "14px",
                            }}
                        />
                        <button
                            type="submit"
                            style={{
                                padding: "10px 20px",
                                background: "#ee4d2d",
                                color: "white",
                                border: "none",
                                borderRadius: "8px",
                                cursor: "pointer",
                                fontWeight: "600",
                            }}
                        >
                            Track Order
                        </button>
                    </form>

                    <div
                        style={{
                            marginTop: "24px",
                            padding: "16px",
                            background: "#f8f9fa",
                            borderRadius: "8px",
                            fontSize: "13px",
                            color: "#666",
                        }}
                    >
                        <strong>Note:</strong> The current backend OrderService only supports fetching a single
                        order by ID (<code>/api/Orders/&#123;id&#125;</code>). A full order history list endpoint is not
                        available in the backend, so you can look up individual orders by their ID here.
                    </div>

                    <div style={{ marginTop: "20px", textAlign: "center" }}>
                        <Link
                            to="/"
                            style={{
                                color: "#ee4d2d",
                                textDecoration: "none",
                                fontWeight: "600",
                                fontSize: "14px",
                            }}
                        >
                            ← Continue Shopping
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}