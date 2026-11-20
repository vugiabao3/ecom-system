import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getOrderById, cancelOrder, type OrderDto } from "../services/orderApi";
import { getShippingStatusByOrderId, type ShipmentDto } from "../services/shippingApi";
import Navbar from "../components/Navbar";
import OrderStatusBadge from "../components/OrderStatusBadge";

export default function OrderDetails() {
    const { id } = useParams<{ id: string }>();
    const [order, setOrder] = useState<OrderDto | null>(null);
    const [shipment, setShipment] = useState<ShipmentDto | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [actionLoading, setActionLoading] = useState(false);
    const [actionMessage, setActionMessage] = useState<string | null>(null);

    useEffect(() => {
        if (!id) return;
        setLoading(true);
        setError(null);

        getOrderById(id)
            .then((res) => {
                setOrder(res.data);
                return getShippingStatusByOrderId(id)
                    .then((sRes) => setShipment(sRes.data))
                    .catch(() => setShipment(null));
            })
            .catch((err) => {
                console.error(err);
                setError("Could not load order details.");
            })
            .finally(() => {
                setLoading(false);
            });
    }, [id]);

    const handleCancel = async () => {
        if (!id || !window.confirm("Are you sure you want to cancel this order?")) return;
        setActionLoading(true);
        setActionMessage(null);
        try {
            await cancelOrder(id);
            setActionMessage("Order cancelled successfully.");
            setOrder((prev) => prev ? { ...prev, status: "CANCELLED" } : prev);
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to cancel order.");
        } finally {
            setActionLoading(false);
        }
    };

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
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Order Not Found</h2>
                    <p style={{ color: "#888", margin: "10px 0 20px" }}>{error || "We could not find the requested order."}</p>
                    <Link
                        to="/"
                        style={{
                            padding: "10px 20px",
                            background: "#ee4d2d",
                            color: "white",
                            borderRadius: "6px",
                            textDecoration: "none",
                        }}
                    >
                        Back to Home
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
                <div
                    style={{
                        background: "white",
                        borderRadius: "12px",
                        padding: "28px",
                        boxShadow: "0 2px 10px rgba(0,0,0,0.06)",
                    }}
                >
                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "20px", borderBottom: "1px solid #eee", paddingBottom: "16px", flexWrap: "wrap", gap: "12px" }}>
                        <div>
                            <h1 style={{ fontSize: "22px", color: "#333", margin: "0 0 6px" }}>
                                Order Details
                            </h1>
                            <span style={{ fontSize: "14px", color: "#888" }}>ID: {order.id}</span>
                        </div>
                        <div>
                            <OrderStatusBadge status={order.status} />
                        </div>
                    </div>

                    <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px", margin: "20px 0" }}>
                        <div style={{ background: "#f8f9fa", padding: "16px", borderRadius: "8px" }}>
                            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>
                                📍 Delivery Details
                            </h3>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Receiver:</strong> {order.receiverName}
                            </p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Phone:</strong> {order.phone}
                            </p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Address:</strong> {order.address}
                            </p>
                        </div>

                        <div style={{ background: "#f8f9fa", padding: "16px", borderRadius: "8px" }}>
                            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>
                                💳 Payment Summary
                            </h3>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Method:</strong> {order.paymentMethod || "N/A"}
                            </p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Payment Status:</strong>{" "}
                                <span style={{
                                    color: order.paymentStatus === "PAID" ? "#2b8a3e" : "#f59f00",
                                    fontWeight: "600"
                                }}>
                                    {order.paymentStatus}
                                </span>
                            </p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Subtotal:</strong> {order.subTotal.toLocaleString()} đ
                            </p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Shipping Fee:</strong> {order.shippingFee.toLocaleString()} đ
                            </p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Total:</strong>{" "}
                                <span style={{ color: "#ee4d2d", fontSize: "18px", fontWeight: "bold" }}>
                                    {order.totalPrice.toLocaleString()} đ
                                </span>
                            </p>
                        </div>
                    </div>

                    {shipment && (
                        <div style={{ marginTop: "20px", background: "#e7f5ff", padding: "16px", borderRadius: "8px", border: "1px solid #a5d8ff" }}>
                            <h3 style={{ fontSize: "16px", color: "#1864ab", marginBottom: "8px" }}>
                                🚚 Shipment Status
                            </h3>
                            <p style={{ margin: "4px 0", fontSize: "14px" }}>
                                <strong>Shipment ID:</strong> {shipment.id}
                            </p>
                            <p style={{ margin: "4px 0", fontSize: "14px" }}>
                                <strong>Tracking Code:</strong> {shipment.id.substring(0, 12)}-E{shipment.id.substring(12, 16)}
                            </p>
                            <p style={{ margin: "4px 0", fontSize: "14px" }}>
                                <strong>Status:</strong> {shipment.status}
                            </p>
                            {shipment.shipperId && (
                                <p style={{ margin: "4px 0", fontSize: "14px" }}>
                                    <strong>Shipper ID:</strong> {shipment.shipperId}
                                </p>
                            )}
                        </div>
                    )}

                    <div style={{ marginTop: "20px", background: "#fff9f0", padding: "16px", borderRadius: "8px", border: "1px solid #ffec99" }}>
                        <h3 style={{ fontSize: "16px", color: "#e67700", marginBottom: "12px" }}>
                            📋 Order Timeline
                        </h3>
                        <div style={{ display: "flex", alignItems: "center", gap: "12px", marginBottom: "8px" }}>
                            <div style={{ width: "10px", height: "10px", borderRadius: "50%", background: "#f59f00" }}></div>
                            <span style={{ fontSize: "14px" }}>Order Created</span>
                        </div>
                        <div style={{ display: "flex", alignItems: "center", gap: "12px", marginBottom: "8px" }}>
                            <div style={{ width: "10px", height: "10px", borderRadius: "50%", background: order.status !== "PENDING" ? "#228be6" : "#ddd" }}></div>
                            <span style={{ fontSize: "14px", color: order.status !== "PENDING" ? "#333" : "#999" }}>Order Confirmed</span>
                        </div>
                        <div style={{ display: "flex", alignItems: "center", gap: "12px", marginBottom: "8px" }}>
                            <div style={{ width: "10px", height: "10px", borderRadius: "50%", background: ["SHIPPING", "DELIVERED"].includes(order.status) ? "#228be6" : "#ddd" }}></div>
                            <span style={{ fontSize: "14px", color: ["SHIPPING", "DELIVERED"].includes(order.status) ? "#333" : "#999" }}>Shipping</span>
                        </div>
                        <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                            <div style={{ width: "10px", height: "10px", borderRadius: "50%", background: order.status === "DELIVERED" ? "#2b8a3e" : "#ddd" }}></div>
                            <span style={{ fontSize: "14px", color: order.status === "DELIVERED" ? "#333" : "#999" }}>Delivered</span>
                        </div>
                    </div>

                    {actionMessage && (
                        <div style={{ marginTop: "16px", padding: "12px", background: "#e6fcf5", color: "#0ca678", borderRadius: "6px" }}>
                            {actionMessage}
                        </div>
                    )}

                    {error && (
                        <div style={{ marginTop: "16px", padding: "12px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px" }}>
                            {error}
                        </div>
                    )}

                    <div style={{ marginTop: "20px", display: "flex", gap: "12px", flexWrap: "wrap" }}>
                        {canCancel && (
                            <button
                                onClick={handleCancel}
                                disabled={actionLoading}
                                style={{
                                    padding: "10px 20px",
                                    background: "#e03131",
                                    color: "white",
                                    border: "none",
                                    borderRadius: "6px",
                                    cursor: actionLoading ? "not-allowed" : "pointer",
                                    fontWeight: "600",
                                }}
                            >
                                {actionLoading ? "Processing..." : "Cancel Order"}
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
