import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import QRPayment from "../components/QRPayment";
import { createPayment } from "../services/paymentApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";
import "../styles/payment.css";

export default function Payment() {
    const navigate = useNavigate();
    const location = useLocation();
    const { isAdmin, isSeller, isShipper } = useAuth();

    if (isAdmin || isSeller || isShipper) {
        navigate("/");
        return null;
    }

    const order = location.state;

    const paymentMethod = order?.paymentMethod || "QR";
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const orderId = order?.orderId || order?.id;
    const totalPrice = order?.totalPrice || 0;

    const handlePayment = async () => {
        if (!orderId) {
            alert("No order found to pay for.");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            const res = await createPayment({
                orderId,
                paymentMethod,
            });

            if (paymentMethod === "QR") {
                navigate("/qr-payment", {
                    state: {
                        orderId,
                        paymentId: res.data?.paymentId,
                        status: res.data?.status,
                        totalPrice,
                        paymentMethod,
                    },
                });
            } else {
                navigate("/orders", {
                    state: {
                        orderId,
                        paymentId: res.data?.paymentId,
                        status: res.data?.status,
                        totalPrice,
                        paymentMethod,
                    },
                });
            }
        } catch (err: any) {
            console.error("Payment error:", err);
            setError(err.response?.data?.message || "Payment processing failed.");
            navigate("/payment-failed", {
                state: {
                    orderId,
                    error: err.response?.data?.message,
                },
            });
        } finally {
            setLoading(false);
        }
    };

    if (!orderId) {
        return (
            <div>
                <Navbar />
                <div className="payment-page" style={{ textAlign: "center", padding: "60px 20px" }}>
                    <h2>No Order Found</h2>
                    <p style={{ color: "#666", margin: "12px 0 24px" }}>
                        Please proceed through checkout to create an order first.
                    </p>
                    <button
                        onClick={() => navigate("/cart")}
                        className="pay-btn"
                        style={{ maxWidth: "200px", margin: "0 auto" }}
                    >
                        View Cart
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div>
            <Navbar />
            <div className="payment-page">
                <h1>Payment Checkout</h1>

                {error && (
                    <div style={{ padding: "12px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px", marginBottom: "16px" }}>
                        {error}
                    </div>
                )}

                <div className="payment-order-card">
                    <h3>Order #{orderId.substring(0, 8)}...</h3>
                    <div style={{ display: "flex", justifyContent: "space-between", marginTop: "8px" }}>
                        <span>Total Payable:</span>
                        <strong style={{ color: "#ee4d2d", fontSize: "18px" }}>
                            {totalPrice.toLocaleString()} đ
                        </strong>
                    </div>
                    <div style={{ display: "flex", justifyContent: "space-between", marginTop: "8px", fontSize: "14px", color: "#666" }}>
                        <span>Payment Method:</span>
                        <span style={{ fontWeight: "600" }}>{paymentMethod === "QR" ? "QR Code Payment" : "Cash On Delivery"}</span>
                    </div>
                </div>

                {paymentMethod === "QR" && (
                    <QRPayment amount={totalPrice} />
                )}

                {paymentMethod === "COD" && (
                    <div style={{ padding: "16px", background: "#f8f9fa", borderRadius: "8px", marginTop: "16px", textAlign: "center", color: "#666" }}>
                        You will pay with cash upon delivery. No online payment required.
                    </div>
                )}

                <button
                    className="pay-btn"
                    onClick={handlePayment}
                    disabled={loading}
                    style={{ marginTop: "24px" }}
                >
                    {loading ? "Processing Payment..." : `Confirm & Pay ${totalPrice.toLocaleString()} đ`}
                </button>
            </div>
        </div>
    );
}