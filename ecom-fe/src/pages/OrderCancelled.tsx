import { useEffect, useState, useCallback } from "react";
import { useNavigate, Link } from "react-router-dom";
import { getCancelledOrders, type PaginatedOrderResponse } from "../services/orderApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";
import OrderStatusBadge from "../components/OrderStatusBadge";

export default function OrderCancelled() {
    const navigate = useNavigate();
    const { user } = useAuth();

    const [page, setPage] = useState(1);
    const pageSize = 10;
    const [ordersData, setOrdersData] = useState<PaginatedOrderResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const loadOrders = useCallback(async () => {
        if (!user?.id) return;
        setLoading(true);
        setError(null);
        try {
            const res = await getCancelledOrders({ page, pageSize });
            setOrdersData(res.data);
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to load cancelled orders.");
        } finally {
            setLoading(false);
        }
    }, [user?.id, page, pageSize]);

    useEffect(() => {
        loadOrders();
    }, [loadOrders]);

    const formatDate = (dateStr: string) => {
        return new Date(dateStr).toLocaleDateString();
    };

    const renderOrderCard = (order: any) => {
        return (
            <div
                key={order.id}
                onClick={() => navigate(`/orders/me/${order.id}`)}
                style={{
                    background: "white",
                    borderRadius: "12px",
                    padding: "16px",
                    boxShadow: "0 2px 8px rgba(0,0,0,0.06)",
                    cursor: "pointer",
                    border: "1px solid #eee",
                    display: "flex",
                    flexDirection: "column",
                    gap: "12px",
                    transition: "all 0.2s",
                }}
                onMouseEnter={(e) => {
                    e.currentTarget.style.boxShadow = "0 4px 16px rgba(0,0,0,0.12)";
                    e.currentTarget.style.borderColor = "#ee4d2d";
                }}
                onMouseLeave={(e) => {
                    e.currentTarget.style.boxShadow = "0 2px 8px rgba(0,0,0,0.06)";
                    e.currentTarget.style.borderColor = "#eee";
                }}
            >
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start" }}>
                    <div style={{ display: "flex", alignItems: "baseline", gap: "8px" }}>
                        <span style={{ fontWeight: "700", color: "#333", fontSize: "16px" }}>
                            Order #{order.orderNumber}
                        </span>
                        <span style={{ fontSize: "12px", color: "#888" }}>{formatDate(order.createdAt)}</span>
                    </div>
                    <OrderStatusBadge status={order.status} />
                </div>

                <div style={{ display: "flex", gap: "16px", marginBottom: "4px" }}>
                    <div style={{ flex: 1, fontSize: "13px", color: "#666" }}>
                        <strong style={{ color: "#333" }}>Items:</strong> {order.itemCount}
                    </div>
                    <div style={{ flex: 1, fontSize: "13px", color: "#666" }}>
                        <strong style={{ color: "#333" }}>Payment:</strong> {order.paymentStatus}
                    </div>
                    <div style={{ flex: 1, fontSize: "13px", color: "#666" }}>
                        <strong style={{ color: "#333" }}>Method:</strong> {order.paymentMethod}
                    </div>
                </div>

                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                    <div style={{ fontSize: "13px", color: "#666", lineHeight: "1.4" }}>
                        {order.firstProductName && (
                            <div style={{ fontSize: "12px", color: "#888", marginBottom: "4px" }}>
                                {order.firstProductName}
                            </div>
                        )}
                        <div style={{ fontSize: "13px", color: "#333", fontWeight: "600" }}>
                            {order.address}
                        </div>
                    </div>
                    <div style={{ color: "#e03131", fontSize: "18px", fontWeight: "bold" }}>
                        {order.totalPrice.toLocaleString()} đ
                    </div>
                </div>

                <div style={{ marginTop: "8px", display: "flex", gap: "8px" }}>
                    <button
                        onClick={(e) => {
                            e.stopPropagation();
                            navigate(`/orders/me/${order.id}`);
                        }}
                        style={{
                            padding: "8px 16px",
                            background: "#ee4d2d",
                            color: "white",
                            border: "none",
                            borderRadius: "6px",
                            cursor: "pointer",
                            fontWeight: "600",
                            fontSize: "13px",
                        }}
                    >
                        View Details
                    </button>
                </div>
            </div>
        );
    };

    const items = ordersData?.items ?? [];
    const totalPages = ordersData?.totalPages ?? 1;

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <div style={{ fontSize: "18px", color: "#666" }}>Loading cancelled orders...</div>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div>
                <Navbar />
                <div style={{ maxWidth: "600px", margin: "40px auto", padding: "20px" }}>
                    <div style={{ background: "#ffe3e3", padding: "20px", borderRadius: "8px", color: "#e03131", textAlign: "center" }}>
                        <h2 style={{ marginTop: "0" }}>{error}</h2>
                    </div>
                    <button
                        onClick={() => loadOrders()}
                        style={{ marginTop: "16px", padding: "10px 24px", background: "#ee4d2d", color: "white", border: "none", borderRadius: "6px", cursor: "pointer", fontWeight: "600" }}
                    >
                        Retry
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "1200px", margin: "0 auto", padding: "20px" }}>
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                    <div>
                        <h1 style={{ fontSize: "24px", color: "#333", margin: "0 0 4px" }}>Cancelled Orders</h1>
                        <p style={{ color: "#666", margin: "0" }}>Orders that were cancelled.</p>
                    </div>
                    <Link
                        to="/orders"
                        style={{ padding: "10px 24px", background: "#ee4d2d", color: "white", borderRadius: "6px", textDecoration: "none", fontWeight: "600" }}
                    >
                        Back to My Orders
                    </Link>
                </div>

                {items.length === 0 ? (
                    <div style={{ textAlign: "center", padding: "80px 20px", background: "white", borderRadius: "12px" }}>
                        <div style={{ fontSize: "48px", marginBottom: "16px" }}>❌</div>
                        <h2 style={{ color: "#666", marginBottom: "8px" }}>No cancelled orders</h2>
                        <p style={{ color: "#999", marginBottom: "24px" }}>
                            You don't have any cancelled orders yet.
                        </p>
                        <Link
                            to="/"
                            style={{ padding: "12px 24px", background: "#ee4d2d", color: "white", borderRadius: "8px", textDecoration: "none", fontWeight: "bold" }}
                        >
                            Start Shopping
                        </Link>
                    </div>
                ) : (
                    <>
                        <div style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
                            {items.map((order) => renderOrderCard(order))}
                        </div>

                        {totalPages > 1 && (
                            <div style={{ display: "flex", justifyContent: "center", alignItems: "center", gap: "8px", marginTop: "30px" }}>
                                <button
                                    onClick={() => setPage((p) => Math.max(1, p - 1))}
                                    disabled={page === 1}
                                    style={{
                                        padding: "8px 16px",
                                        background: page === 1 ? "#f0f0f0" : "white",
                                        color: page === 1 ? "#999" : "#333",
                                        border: "1px solid #ddd",
                                        borderRadius: "6px",
                                        cursor: page === 1 ? "not-allowed" : "pointer",
                                    }}
                                >
                                    ← Prev
                                </button>
                                <span style={{ fontSize: "14px", color: "#666" }}>
                                    Page {page} of {totalPages}
                                </span>
                                <button
                                    onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
                                    disabled={page === totalPages}
                                    style={{
                                        padding: "8px 16px",
                                        background: page === totalPages ? "#f0f0f0" : "white",
                                        color: page === totalPages ? "#999" : "#333",
                                        border: "1px solid #ddd",
                                        borderRadius: "6px",
                                        cursor: page === totalPages ? "not-allowed" : "pointer",
                                    }}
                                >
                                    Next →
                                </button>
                            </div>
                        )}
                    </>
                )}
            </div>
        </div>
    );
}
