import { Link, useLocation } from "react-router-dom";
import Navbar from "../components/Navbar";

export default function PaymentSuccess() {
    const location = useLocation();
    const state = location.state;
    const orderId = state?.orderId;
    const paymentId = state?.paymentId;

    return (
        <div>
            <Navbar />
            <div
                style={{
                    maxWidth: "550px",
                    margin: "50px auto",
                    padding: "40px 30px",
                    background: "white",
                    borderRadius: "12px",
                    textAlign: "center",
                    boxShadow: "0 4px 16px rgba(0,0,0,0.08)",
                }}
            >
                <div style={{ fontSize: "60px", marginBottom: "16px" }}>🎉</div>
                <h1 style={{ color: "#2b8a3e", fontSize: "26px", marginBottom: "12px" }}>
                    Payment Successful!
                </h1>
                <p style={{ color: "#666", marginBottom: "24px" }}>
                    Thank you for your order. Your payment has been confirmed.
                </p>

                {orderId && (
                    <div
                        style={{
                            background: "#f8f9fa",
                            padding: "16px",
                            borderRadius: "8px",
                            textAlign: "left",
                            marginBottom: "24px",
                            fontSize: "14px",
                        }}
                    >
                        <p style={{ margin: "4px 0" }}>
                            <strong>Order ID:</strong> {orderId}
                        </p>
                        {paymentId && (
                            <p style={{ margin: "4px 0" }}>
                                <strong>Payment ID:</strong> {paymentId}
                            </p>
                        )}
                    </div>
                )}

                <div style={{ display: "flex", gap: "12px", justifyContent: "center" }}>
                    {orderId && (
                        <Link
                            to={`/orders/${orderId}`}
                            style={{
                                padding: "12px 20px",
                                background: "#ee4d2d",
                                color: "white",
                                borderRadius: "8px",
                                textDecoration: "none",
                                fontWeight: "bold",
                            }}
                        >
                            View Order Details
                        </Link>
                    )}

                    <Link
                        to="/"
                        style={{
                            padding: "12px 20px",
                            background: "#f0f0f0",
                            color: "#333",
                            borderRadius: "8px",
                            textDecoration: "none",
                            fontWeight: "600",
                        }}
                    >
                        Continue Shopping
                    </Link>
                </div>
            </div>
        </div>
    );
}