import { useState, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { confirmPayment, failPayment } from "../services/paymentApi";
import Navbar from "../components/Navbar";

export default function QRPaymentMock() {
    const navigate = useNavigate();
    const location = useLocation();
    const order = location.state;

    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [paid, setPaid] = useState(false);

    const orderId = order?.orderId || order?.id;
    const paymentId = order?.paymentId;
    const totalPrice = order?.totalPrice || 0;

    useEffect(() => {
        if (!orderId) {
            navigate("/");
        }
    }, [orderId, navigate]);

    const handleConfirm = async () => {
        if (!paymentId) return;
        setLoading(true);
        setError(null);
        setMessage(null);
        try {
            await confirmPayment(paymentId);
            setPaid(true);
            setMessage("Payment confirmed successfully!");
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to confirm payment.");
        } finally {
            setLoading(false);
        }
    };

    const handleFail = async () => {
        if (!paymentId) return;
        setLoading(true);
        setError(null);
        setMessage(null);
        try {
            await failPayment(paymentId);
            setError("Payment marked as failed.");
            setTimeout(() => navigate("/payment-failed"), 1500);
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to mark payment as failed.");
        } finally {
            setLoading(false);
        }
    };

    if (!orderId) return null;

    return (
        <div>
            <Navbar />
            <div
                style={{
                    maxWidth: "500px",
                    margin: "40px auto",
                    padding: "32px",
                    background: "white",
                    borderRadius: "12px",
                    boxShadow: "0 4px 16px rgba(0,0,0,0.08)",
                    textAlign: "center",
                }}
            >
                <div style={{ fontSize: "48px", marginBottom: "16px" }}>🏦</div>
                <h2 style={{ color: "#222", marginBottom: "8px" }}>Bank Payment Portal</h2>
                <p style={{ color: "#666", marginBottom: "24px" }}>
                    Complete your payment for this order
                </p>

                <div
                    style={{
                        background: "#f8f9fa",
                        padding: "20px",
                        borderRadius: "8px",
                        marginBottom: "24px",
                        textAlign: "left",
                    }}
                >
                    <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "8px" }}>
                        <span style={{ color: "#666" }}>Order ID:</span>
                        <span style={{ fontWeight: "600" }}>{orderId.substring(0, 8)}...</span>
                    </div>
                    {paymentId && (
                        <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "8px" }}>
                            <span style={{ color: "#666" }}>Payment ID:</span>
                            <span style={{ fontWeight: "600" }}>{paymentId.substring(0, 8)}...</span>
                        </div>
                    )}
                    <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "8px" }}>
                        <span style={{ color: "#666" }}>Merchant:</span>
                        <span style={{ fontWeight: "600" }}>EcomSystem Store</span>
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between" }}>
                        <span style={{ color: "#666" }}>Amount:</span>
                        <span style={{ color: "#ee4d2d", fontSize: "18px", fontWeight: "bold" }}>
                            {totalPrice.toLocaleString()} đ
                        </span>
                    </div>
                </div>

                {message && (
                    <div
                        style={{
                            padding: "12px",
                            background: "#e6fcf5",
                            color: "#0ca678",
                            borderRadius: "6px",
                            marginBottom: "16px",
                        }}
                    >
                        {message}
                    </div>
                )}

                {error && (
                    <div
                        style={{
                            padding: "12px",
                            background: "#ffe3e3",
                            color: "#e03131",
                            borderRadius: "6px",
                            marginBottom: "16px",
                        }}
                    >
                        {error}
                    </div>
                )}

                {!paid ? (
                    <div style={{ display: "flex", gap: "12px", justifyContent: "center" }}>
                        <button
                            onClick={handleConfirm}
                            disabled={loading}
                            style={{
                                padding: "14px 28px",
                                background: "#ee4d2d",
                                color: "white",
                                border: "none",
                                borderRadius: "8px",
                                fontSize: "16px",
                                fontWeight: "bold",
                                cursor: loading ? "not-allowed" : "pointer",
                            }}
                        >
                            {loading ? "Processing..." : "THANH TOÁN"}
                        </button>
                        <button
                            onClick={handleFail}
                            disabled={loading}
                            style={{
                                padding: "14px 28px",
                                background: "#f0f0f0",
                                color: "#666",
                                border: "1px solid #ddd",
                                borderRadius: "8px",
                                fontSize: "16px",
                                fontWeight: "600",
                                cursor: loading ? "not-allowed" : "pointer",
                            }}
                        >
                            Cancel Payment
                        </button>
                    </div>
                ) : (
                    <button
                        onClick={() => navigate(`/orders/${orderId}`)}
                        style={{
                            padding: "14px 28px",
                            background: "#2b8a3e",
                            color: "white",
                            border: "none",
                            borderRadius: "8px",
                            fontSize: "16px",
                            fontWeight: "bold",
                            cursor: "pointer",
                        }}
                    >
                        View Order Details
                    </button>
                )}
            </div>
        </div>
    );
}
