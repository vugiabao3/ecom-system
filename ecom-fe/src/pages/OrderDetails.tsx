import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getOrderById, type OrderDto } from "../services/orderApi";
import { getShippingStatusByOrderId, type ShipmentDto } from "../services/shippingApi";
import Navbar from "../components/Navbar";
import OrderStatusBadge from "../components/OrderStatusBadge";

export default function OrderDetails() {
    const { id } = useParams<{ id: string }>();
    const [order, setOrder] = useState<OrderDto | null>(null);
    const [shipment, setShipment] = useState<ShipmentDto | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!id) return;
        setLoading(true);
        setError(null);

        getOrderById(id)
            .then((res) => {
                setOrder(res.data);
                // Attempt to fetch shipment status if available
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

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "800px", margin: "30px auto", padding: "20px" }}>
                <div
                    style={{
                        background: "white",
                        borderRadius: "12px",
                        padding: "28px",
                        boxShadow: "0 2px 10px rgba(0,0,0,0.06)",
                    }}
                >
                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "20px", borderBottom: "1px solid #eee", paddingBottom: "16px" }}>
                        <div>
                            <h1 style={{ fontSize: "22px", color: "#333", margin: "0 0 6px" }}>
                                Order Details
                            </h1>
                            <span style={{ fontSize: "14px", color: "#888" }}>ID: {order.id}</span>
                        </div>
                        <div>
                            <OrderStatusBadge status={order.status || "PENDING"} />
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
                                <strong>Status:</strong> {order.status}
                            </p>
                            <p style={{ margin: "6px 0", fontSize: "14px" }}>
                                <strong>Total:</strong>{" "}
                                <span style={{ color: "#ee4d2d", fontSize: "18px", fontWeight: "bold" }}>
                                    {order.totalPrice?.toLocaleString()} đ
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
                                <strong>Status:</strong> {shipment.status}
                            </p>
                        </div>
                    )}

                    <div style={{ marginTop: "28px", textAlign: "right" }}>
                        <Link
                            to="/"
                            style={{
                                padding: "10px 20px",
                                background: "#ee4d2d",
                                color: "white",
                                borderRadius: "6px",
                                textDecoration: "none",
                                fontWeight: "bold",
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
