import { Link, useLocation } from "react-router-dom";
import Navbar from "../components/Navbar";

export default function PaymentFailed() {
    const location = useLocation();
    const state = location.state;
    const error = state?.error;

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
                <div style={{ fontSize: "60px", marginBottom: "16px" }}>⚠️</div>
                <h1 style={{ color: "#c92a2a", fontSize: "26px", marginBottom: "12px" }}>
                    Payment Incomplete
                </h1>
                <p style={{ color: "#666", marginBottom: "20px" }}>
                    We were unable to process your payment.
                </p>

                {error && (
                    <div
                        style={{
                            background: "#ffe3e3",
                            color: "#e03131",
                            padding: "12px",
                            borderRadius: "6px",
                            marginBottom: "24px",
                            fontSize: "14px",
                        }}
                    >
                        {error}
                    </div>
                )}

                <div style={{ display: "flex", gap: "12px", justifyContent: "center" }}>
                    <Link
                        to="/cart"
                        style={{
                            padding: "12px 20px",
                            background: "#ee4d2d",
                            color: "white",
                            borderRadius: "8px",
                            textDecoration: "none",
                            fontWeight: "bold",
                        }}
                    >
                        Return to Cart
                    </Link>

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
                        Back to Home
                    </Link>
                </div>
            </div>
        </div>
    );
}