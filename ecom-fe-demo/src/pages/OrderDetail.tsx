import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getMyOrderDetail, type OrderDetailDto } from "../services/orderApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";
import OrderStatusBadge from "../components/OrderStatusBadge";

export default function OrderDetail() {
    const { orderId } = useParams<{ orderId: string }>();
    const { isAdmin, isSeller, isShipper } = useAuth();

    if (isAdmin || isSeller || isShipper) {
        return null;
    }

    const [order, setOrder] = useState<OrderDetailDto | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!orderId) return;
        setLoading(true);
        setError(null);
        getMyOrderDetail(orderId)
            .then((res: any) => {
                setOrder(res.data);
            })
            .catch((err: any) => {
                setError(err.response?.data?.message || "Failed to load order details.");
            })
            .finally(() => {
                setLoading(false);
            });
    }, [orderId]);

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Loading order details...</h2>
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
                        <p>{error || "We could not find the requested order."}</p>
                    </div>
                    <Link to="/orders" style={{ marginTop: "20px", display: "inline-block", padding: "10px 24px", background: "#ee4d2d", color: "white", borderRadius: "6px", textDecoration: "none", fontWeight: "600" }}>
                        Back to My Orders
                    </Link>
                </div>
            </div>
        );
    }

    const canCancel = order.status === "PENDING" || order.status === "CONFIRMED";

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "900px", margin: "30px auto", padding: "20px" }}>
                <div style={{ background: "white", borderRadius: "12px", padding: "28px", boxShadow: "0 2px 10px rgba(0,0,0,0.06)" }}>
                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "20px", borderBottom: "1px solid #eee", paddingBottom: "16px", flexWrap: "wrap", gap: "12px" }}>
                        <div>
                            <h1 style={{ fontSize: "22px", color: "#333", margin: "0 0 6px" }}>Order Details</h1>
                            <span style={{ fontSize: "14px", color: "#888" }}>ID: {order.id}</span>
                        </div>
                        <div>
                            <OrderStatusBadge status={order.status} />
                        </div>
                    </div>

                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px", margin: "20px 0" }}>
                        <div style={{ background: "#f8f9fa", padding: "16px", borderRadius: "8px" }}>
                            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>📍 Delivery Details</h3>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Receiver:</strong> {order.receiverName}</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Phone:</strong> {order.phone}</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Address:</strong> {order.address}</p>
                        </div>

                        <div style={{ background: "#f8f9fa", padding: "16px", borderRadius: "8px" }}>
                            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>💳 Payment Summary</h3>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Method:</strong> {order.paymentMethod}</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Payment Status:</strong> <span style={{ color: order.paymentStatus === "PAID" ? "#2b8a3e" : "#f59f00", fontWeight: "600" }}>{order.paymentStatus}</span></p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Subtotal:</strong> {order.subTotal.toLocaleString()} đ</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Shipping Fee:</strong> {order.shippingFee.toLocaleString()} đ</p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}><strong>Discount:</strong> -{order.discount.toLocaleString()} đ</p>
                            <p style={{ margin: "6px 0", fontSize: "14px", color: "#ee4d2d", fontWeight: "bold" }}><strong>Total:</strong> {order.totalPrice.toLocaleString()} đ</p>
                        </div>
                    </div>

                    <div style={{ marginBottom: "24px" }}>
                        <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>📋 Order Items</h3>
                        {order.items.map((item) => (
                            <div key={item.id} style={{ display: "flex", gap: "12px", padding: "12px 0", borderBottom: "1px solid #eee" }}>
                                {item.imageUrl && (
                                    <img src={item.imageUrl} alt={item.productName} style={{ width: "60px", height: "60px", borderRadius: "6px", objectFit: "cover" }} />
                                )}
                                <div style={{ flex: 1 }}>
                                    <p style={{ margin: "0 0 4px 0", fontSize: "14px", fontWeight: "600", color: "#333" }}>{item.productName}</p>
                                    <p style={{ margin: "0", fontSize: "13px", color: "#666" }}>{item.variantName || ""}</p>
                                    <p style={{ margin: "4px 0 0 0", fontSize: "14px", color: "#333" }}>Qty: {item.quantity} × {item.price.toLocaleString()} đ</p>
                                </div>
                                <div style={{ fontSize: "14px", fontWeight: "600", color: "#333" }}>{(item.price * item.quantity).toLocaleString()} đ</div>
                            </div>
                        ))}
                    </div>

                    {order.statusHistory && order.statusHistory.length > 0 && (
                        <div style={{ marginBottom: "24px" }}>
                            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>📜 Order Timeline</h3>
                            {order.statusHistory.map((h) => (
                                <div key={h.id} style={{ display: "flex", alignItems: "center", gap: "12px", marginBottom: "8px" }}>
                                    <div style={{ width: "10px", height: "10px", borderRadius: "50%", background: "#ee4d2d" }}></div>
                                    <span style={{ fontSize: "14px", color: "#333" }}>{h.status}</span>
                                    <span style={{ fontSize: "12px", color: "#999", marginLeft: "8px" }}>{h.note}</span>
                                    <span style={{ fontSize: "12px", color: "#999", marginLeft: "auto" }}>{new Date(h.createdAt).toLocaleString()}</span>
                                </div>
                            ))}
                        </div>
                    )}

                    <div style={{ display: "flex", gap: "12px", flexWrap: "wrap" }}>
                        {canCancel && (
                            <button
                                onClick={() => {
                                    if (window.confirm("Are you sure you want to cancel this order?")) {
                                        // Cancel order - would call cancel API
                                        alert("Cancellation request submitted.");
                                    }
                                }}
                                style={{
                                    padding: "10px 20px",
                                    background: "#e03131",
                                    color: "white",
                                    border: "none",
                                    borderRadius: "6px",
                                    cursor: "pointer",
                                    fontWeight: "600",
                                }}
                            >
                                Cancel Order
                            </button>
                        )}
                        <Link
                            to="/orders"
                            style={{
                                padding: "10px 20px",
                                background: "#f0f0f0",
                                color: "#333",
                                borderRadius: "6px",
                                textDecoration: "none",
                                fontWeight: "600",
                            }}
                        >
                            ← Back to Orders
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
