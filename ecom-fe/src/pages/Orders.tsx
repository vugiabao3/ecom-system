import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import Navbar from "../components/Navbar";
import OrderStatusBadge from "../components/OrderStatusBadge";
import { getOrdersByUserId, type OrderDto } from "../services/orderApi";
import { useAuth } from "../context/AuthContext";

const STATUS_TABS = [
    { key: "ALL", label: "All" },
    { key: "PENDING", label: "Pending" },
    { key: "CONFIRMED", label: "Confirmed" },
    { key: "SHIPPING", label: "Shipping" },
    { key: "DELIVERED", label: "Delivered" },
    { key: "RETURNED", label: "Returned" },
    { key: "CANCELLED", label: "Cancelled" },
];

export default function Orders() {
    const navigate = useNavigate();
    const { user } = useAuth();
    const [orders, setOrders] = useState<OrderDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [activeTab, setActiveTab] = useState("ALL");

    useEffect(() => {
        const loadOrders = async () => {
            if (!user?.id) return;
            setLoading(true);
            setError(null);
            try {
                const res = await getOrdersByUserId(user.id);
                setOrders(res.data || []);
            } catch (err: any) {
                setError(err.response?.data?.message || "Failed to load orders.");
                setOrders([]);
            } finally {
                setLoading(false);
            }
        };
        loadOrders();
    }, [user?.id]);

    const filteredOrders = activeTab === "ALL"
        ? orders
        : orders.filter((o) => o.status === activeTab);

    const formatDate = (id: string) => {
        return id.substring(0, 8);
    };

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "1000px", margin: "30px auto", padding: "20px" }}>
                <h1 style={{ fontSize: "24px", color: "#333", marginBottom: "20px" }}>
                    📦 My Orders
                </h1>

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

                <div className="tabs">
                    {STATUS_TABS.map((tab) => (
                        <button
                            key={tab.key}
                            className={`tab ${activeTab === tab.key ? "active" : ""}`}
                            onClick={() => setActiveTab(tab.key)}
                        >
                            {tab.label}
                        </button>
                    ))}
                </div>

                {loading ? (
                    <div style={{ textAlign: "center", padding: "60px 0", color: "#666" }}>
                        Loading orders...
                    </div>
                ) : filteredOrders.length === 0 ? (
                    <div
                        style={{
                            textAlign: "center",
                            padding: "60px 20px",
                            background: "white",
                            borderRadius: "12px",
                        }}
                    >
                        <h2 style={{ color: "#666", marginBottom: "8px" }}>No orders found</h2>
                        <p style={{ color: "#999", marginBottom: "24px" }}>
                            {activeTab === "ALL"
                                ? "You haven't placed any orders yet."
                                : `No orders with status "${activeTab}".`}
                        </p>
                        <Link
                            to="/"
                            style={{
                                padding: "12px 24px",
                                background: "#ee4d2d",
                                color: "white",
                                borderRadius: "8px",
                                textDecoration: "none",
                                fontWeight: "bold",
                            }}
                        >
                            Start Shopping
                        </Link>
                    </div>
                ) : (
                    <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                        {filteredOrders.map((order) => (
                            <div
                                key={order.id}
                                onClick={() => navigate(`/orders/${order.id}`)}
                                style={{
                                    background: "white",
                                    borderRadius: "12px",
                                    padding: "20px",
                                    boxShadow: "0 2px 10px rgba(0,0,0,0.06)",
                                    cursor: "pointer",
                                    transition: "all 0.2s",
                                }}
                                onMouseEnter={(e) => {
                                    e.currentTarget.style.boxShadow = "0 4px 16px rgba(0,0,0,0.1)";
                                }}
                                onMouseLeave={(e) => {
                                    e.currentTarget.style.boxShadow = "0 2px 10px rgba(0,0,0,0.06)";
                                }}
                            >
                                <div
                                    style={{
                                        display: "flex",
                                        justifyContent: "space-between",
                                        alignItems: "center",
                                        marginBottom: "12px",
                                        flexWrap: "wrap",
                                        gap: "8px",
                                    }}
                                >
                                    <div>
                                        <span style={{ fontWeight: "700", color: "#333", fontSize: "15px" }}>
                                            Order #{order.id.substring(0, 8)}...
                                        </span>
                                        <span style={{ marginLeft: "12px", fontSize: "13px", color: "#888" }}>
                                            {formatDate(order.id)}
                                        </span>
                                    </div>
                                    <OrderStatusBadge status={order.status} />
                                </div>

                                <div
                                    style={{
                                        display: "flex",
                                        justifyContent: "space-between",
                                        alignItems: "center",
                                        flexWrap: "wrap",
                                        gap: "12px",
                                    }}
                                >
                                    <div style={{ display: "flex", gap: "20px", fontSize: "14px", color: "#666" }}>
                                        <span>
                                            <strong>Subtotal:</strong> {order.subTotal.toLocaleString()} đ
                                        </span>
                                        <span>
                                            <strong>Shipping:</strong> {order.shippingFee.toLocaleString()} đ
                                        </span>
                                        <span>
                                            <strong>Payment:</strong> {order.paymentStatus}
                                        </span>
                                    </div>
                                    <div style={{ color: "#ee4d2d", fontSize: "18px", fontWeight: "bold" }}>
                                        {order.totalPrice.toLocaleString()} đ
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}
