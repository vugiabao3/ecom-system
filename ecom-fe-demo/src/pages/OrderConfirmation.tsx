import { useEffect, useState } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import { confirmCheckout } from "../services/orderApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";

export default function OrderConfirmation() {
    const navigate = useNavigate();
    const { checkoutId } = useParams<{ checkoutId: string }>();
    const { isAdmin, isSeller, isShipper } = useAuth();

    if (isAdmin || isSeller || isShipper) {
        navigate("/");
        return null;
    }

    const [loading, setLoading] = useState(false);
    const [confirming, setConfirming] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [confirmed, setConfirmed] = useState(false);
    const [confirmation, setConfirmation] = useState<any>(null);

    useEffect(() => {
        if (!checkoutId) {
            setError("Checkout session not found.");
            return;
        }
        setLoading(true);
        setError(null);
        confirmCheckout({ checkoutId })
            .then((res) => {
                setConfirmation(res.data);
                setConfirmed(true);
            })
            .catch((err: any) => {
                setError(err.response?.data?.message || "Failed to confirm checkout.");
            })
            .finally(() => {
                setLoading(false);
            });
    }, [checkoutId]);

    const handleConfirm = () => {
        if (!checkoutId) return;
        setConfirming(true);
        setError(null);
        confirmCheckout({ checkoutId })
            .then((res) => {
                setConfirmation(res.data);
                setConfirmed(true);
            })
            .catch((err: any) => {
                setError(err.response?.data?.message || "Failed to confirm checkout.");
            })
            .finally(() => {
                setConfirming(false);
            });
    };

    const handlePay = () => {
        if (!confirmation?.orderId) {
            setError("No order ID available for payment.");
            return;
        }
        navigate(`/payment?orderId=${confirmation.orderId}`);
    };

    if (loading && !confirmed) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Confirming checkout...</h2>
                </div>
            </div>
        );
    }

    if (error && !confirmed) {
        return (
            <div>
                <Navbar />
                <div style={{ maxWidth: "600px", margin: "40px auto", padding: "20px" }}>
                    <div style={{ background: "#ffe3e3", padding: "20px", borderRadius: "8px", color: "#e03131", textAlign: "center" }}>
                        <h2 style={{ marginTop: "0" }}>Confirmation Failed</h2>
                        <p>{error}</p>
                    </div>
                    <Link to="/checkout" style={{ marginTop: "20px", display: "inline-block", padding: "10px 24px", background: "#ee4d2d", color: "white", borderRadius: "6px", textDecoration: "none", fontWeight: "600" }}>
                        Back to Checkout
                    </Link>
                </div>
            </div>
        );
    }

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "700px", margin: "30px auto", padding: "20px" }}>
                <div style={{ background: "white", borderRadius: "12px", padding: "32px", boxShadow: "0 2px 10px rgba(0,0,0,0.06)" }}>
                    <div style={{ display: "flex", alignItems: "center", gap: "12px", marginBottom: "24px" }}>
                        <div style={{ width: "32px", height: "32px", borderRadius: "50%", background: "#ee4d2d", display: "flex", alignItems: "center", justifyContent: "center", color: "white", fontWeight: "bold" }}>
                            1
                        </div>
                        <div style={{ width: "32px", height: "32px", borderRadius: "50%", background: confirmed ? "#2b8a3e" : "#ddd", display: "flex", alignItems: "center", justifyContent: "center", color: "white", fontWeight: "bold" }}>
                            2
                        </div>
                        <div style={{ width: "32px", height: "32px", borderRadius: "50%", background: "#ddd", display: "flex", alignItems: "center", justifyContent: "center", color: "#999", fontWeight: "bold" }}>
                            3
                        </div>
                    </div>

                    <h1 style={{ fontSize: "24px", color: "#333", marginBottom: "8px" }}>Confirm Order</h1>
                    <p style={{ color: "#666", marginBottom: "24px" }}>Review your order details before proceeding to payment.</p>

                    {!confirmed && (
                        <div style={{ textAlign: "center", padding: "40px 0" }}>
                            <p style={{ color: "#666" }}>Validating checkout session and stock availability...</p>
                            <button
                                onClick={handleConfirm}
                                disabled={confirming}
                                style={{
                                    marginTop: "16px",
                                    padding: "10px 24px",
                                    background: "#ee4d2d",
                                    color: "white",
                                    border: "none",
                                    borderRadius: "6px",
                                    cursor: confirming ? "not-allowed" : "pointer",
                                    fontWeight: "600",
                                }}
                            >
                                {confirming ? "Confirming..." : "Confirm Order"}
                            </button>
                        </div>
                    )}

                    {confirmed && confirmation && (
                        <div style={{ background: "#e6fcf5", border: "1px solid #0ca678", borderRadius: "8px", padding: "20px", marginBottom: "24px" }}>
                            <h2 style={{ marginTop: "0", color: "#0ca678", fontSize: "18px" }}>✓ Checkout Confirmed</h2>
                            <p style={{ margin: "8px 0", color: "#333" }}>{confirmation.message}</p>
                            <p style={{ margin: "8px 0", color: "#666", fontSize: "14px" }}>
                                Order ID: {confirmation.orderId}<br />
                                Status: {confirmation.status}
                            </p>
                        </div>
                    )}

                    {confirmed && confirmation && (
                        <div style={{ display: "flex", gap: "12px" }}>
                            <button
                                onClick={handlePay}
                                style={{
                                    flex: 1,
                                    padding: "12px 24px",
                                    background: "#ee4d2d",
                                    color: "white",
                                    border: "none",
                                    borderRadius: "6px",
                                    cursor: "pointer",
                                    fontWeight: "600",
                                    fontSize: "16px",
                                }}
                            >
                                Proceed to Payment
                            </button>
                            <Link
                                to="/orders"
                                style={{
                                    flex: 1,
                                    padding: "12px 24px",
                                    background: "#f0f0f0",
                                    color: "#333",
                                    borderRadius: "6px",
                                    textDecoration: "none",
                                    fontWeight: "600",
                                    textAlign: "center",
                                    display: "flex",
                                    alignItems: "center",
                                    justifyContent: "center",
                                }}
                            >
                                View My Orders
                            </Link>
                        </div>
                    )}

                    {error && confirmed && (
                        <div style={{ marginTop: "16px", padding: "12px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px" }}>
                            {error}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
