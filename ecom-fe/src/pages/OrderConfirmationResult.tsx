import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getOrderConfirmation, type OrderDetailDto } from "../services/orderApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";
import OrderStatusBadge from "../components/OrderStatusBadge";

export default function OrderConfirmationResult() {
    const { id } = useParams<{ id: string }>();
    const { isAdmin, isSeller, isShipper } = useAuth();

    if (isAdmin || isSeller || isShipper) {
        return null;
    }

    const [order, setOrder] = useState<OrderDetailDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!id) return;
        setLoading(true);
        setError(null);
        getOrderConfirmation(id)
            .then((res) => {
                setOrder(res.data);
            })
            .catch((err: any) => {
                setError(err.response?.data?.message || "Failed to load order confirmation.");
            })
            .finally(() => {
                setLoading(false);
            });
    }, [id]);

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Loading order confirmation...</h2>
                </div>
            </div>
        );
    }

    if (error || !order) {
        return (
            <div>
                <Navbar />
                <div style={{ maxWidth: "600px", margin: "40px auto", padding: "20px" }}>
                    <div style={{ background: "#ffe3e3", padding: "20px", borderRadius: "8px", color: "#e03131", textAlign: "center" }}>
                        <h2 style={{ marginTop: "0" }}>Order Not Found</h2>
                        <p>{error || "We could not find the requested order confirmation."}</p>
                    </div>
                    <Link to="/" style={{ marginTop: "20px", display: "inline-block", padding: "10px 24px", background: "#ee4d2d", color: "white", borderRadius: "6px", textDecoration: "none", fontWeight: "600" }}>
                        Back to Home
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
                        <div style={{ width: "32px", height: "32px", borderRadius: "50%", background: "#2b8a3e", display: "flex", alignItems: "center", justifyContent: "center", color: "white", fontWeight: "bold" }}>
                            1
                        </div>
                        <div style={{ width: "32px", height: "32px", borderRadius: "50%", background: "#2b8a3e", display: "flex", alignItems: "center", justifyContent: "center", color: "white", fontWeight: "bold" }}>
                            2
                        </div>
                        <div style={{ width: "32px", height: "32px", borderRadius: "50%", background: "#2b8a3e", display: "flex", alignItems: "center", justifyContent: "center", color: "white", fontWeight: "bold" }}>
                            3
                        </div>
                    </div>

                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "8px" }}>
                        <h1 style={{ fontSize: "24px", color: "#333", margin: "0" }}>Order Confirmation</h1>
                        <OrderStatusBadge status={order.status} />
                    </div>
                    <p style={{ color: "#666", marginBottom: "24px" }}>Thank you for your purchase! Your order has been confirmed.</p>

                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px", marginBottom: "24px" }}>
                        <div style={{ background: "#f8f9fa", padding: "20px", borderRadius: "8px" }}>
                            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>📦 Order Summary</h3>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Order ID:</strong> {order.id}</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Order Date:</strong> {new Date(order.createdAt).toLocaleDateString()}</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Payment Method:</strong> {order.paymentMethod}</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Payment Status:</strong> {order.paymentStatus}</p>
                        </div>
                        <div style={{ background: "#f8f9fa", padding: "20px", borderRadius: "8px" }}>
                            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>💰 Price Breakdown</h3>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Subtotal:</strong> {order.subTotal.toLocaleString()} đ</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Shipping Fee:</strong> {order.shippingFee.toLocaleString()} đ</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Discount:</strong> -{order.discount.toLocaleString()} đ</p>
                            <p style={{ margin: "6px 0", fontSize: "16px", fontWeight: "bold", color: "#ee4d2d" }}><strong>Total:</strong> {order.totalPrice.toLocaleString()} đ</p>
                        </div>
                    </div>

                    <div style={{ marginBottom: "24px" }}>
                        <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>🚚 Delivery Details</h3>
                        <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Receiver:</strong> {order.receiverName}</p>
                        <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Phone:</strong> {order.phone}</p>
                        <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Address:</strong> {order.address}</p>
                    </div>

                    <div style={{ marginBottom: "24px" }}>
                        <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>📋 Items</h3>
                        {order.items.map((item) => (
                            <div key={item.id} style={{ display: "flex", gap: "12px", padding: "12px 0", borderBottom: "1px solid #eee" }}>
                                {item.imageUrl && (
                                    <img src={item.imageUrl} alt={item.productName} style={{ width: "60px", height: "60px", borderRadius: "6px", objectFit: "cover" }} />
                                )}
                                <div style={{ flex: 1 }}>
                                    <p style={{ margin: "0 0 4px 0", fontSize: "14px", fontWeight: "600", color: "#333" }}>{item.productName}</p>
                                    <p style={{ margin: "0", fontSize: "13px", color: "#666" }}>{item.variantName || ""} x {item.quantity}</p>
                                    <p style={{ margin: "4px 0 0 0", fontSize: "14px", fontWeight: "600", color: "#333" }}>{(item.price * item.quantity).toLocaleString()} đ</p>
                                </div>
                            </div>
                        ))}
                    </div>

                    <div style={{ display: "flex", gap: "12px" }}>
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
                            }}
                        >
                            View My Orders
                        </Link>
                        <Link
                            to="/"
                            style={{
                                flex: 1,
                                padding: "12px 24px",
                                background: "#ee4d2d",
                                color: "white",
                                borderRadius: "6px",
                                textDecoration: "none",
                                fontWeight: "600",
                                textAlign: "center",
                            }}
                        >
                            Continue Shopping
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
