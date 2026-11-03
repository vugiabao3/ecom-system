import { useEffect, useState, useCallback } from "react";
import { useNavigate, Link } from "react-router-dom";
import {
    getMyOrders,
    getOrderSummary,
    getRecentOrders,
    searchOrders,
    getPendingOrders,
    getProcessingOrders,
    type PaginatedOrderResponse,
    type OrderSummaryDto,
} from "../services/orderApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";
import OrderStatusBadge from "../components/OrderStatusBadge";

const STATUS_TABS = [
    { key: undefined as string | undefined, label: "All" },
    { key: "PENDING", label: "Pending" },
    { key: "CONFIRMED", label: "Processing" },
    { key: "PREPARING", label: "Preparing" },
    { key: "READYFORSHIPMENT", label: "Ready to Ship" },
    { key: "SHIPPING", label: "Shipping" },
    { key: "DELIVERED", label: "Delivered" },
    { key: "CANCELLED", label: "Cancelled" },
];

const STATUS_COLORS: Record<string, string> = {
    PENDING: "#f59f00",
    CONFIRMED: "#0ca678",
    PREPARING: "#0ca678",
    READYFORSHIPMENT: "#0ca678",
    SHIPPING: "#2196f3",
    DELIVERED: "#2b8a3e",
    CANCELLED: "#e03131",
};

export default function MyOrders() {
    const navigate = useNavigate();
    const { user } = useAuth();

    const [page, setPage] = useState(1);
    const pageSize = 10;
    const [statusFilter, setStatusFilter] = useState<string | undefined>(undefined);
    const [searchTerm, setSearchTerm] = useState("");

    const [ordersData, setOrdersData] = useState<PaginatedOrderResponse | null>(null);
    const [summary, setSummary] = useState<OrderSummaryDto | null>(null);
    const [recentOrders, setRecentOrders] = useState<OrderSummaryDto[] | null>(null);
    const [pendingOrders, setPendingOrders] = useState<PaginatedOrderResponse | null>(null);
    const [processingOrders, setProcessingOrders] = useState<PaginatedOrderResponse | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const loadOrders = useCallback(async () => {
        if (!user?.id) return;
        setLoading(true);
        setError(null);
        try {
            const [ordersRes, summaryRes] = await Promise.all([
                getMyOrders({ page, pageSize, statusFilter }),
                getOrderSummary(),
            ]);
            setOrdersData(ordersRes.data);
            setSummary(summaryRes.data);
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to load orders.");
        } finally {
            setLoading(false);
        }
    }, [user?.id, page, pageSize, statusFilter]);

    const loadRecent = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getRecentOrders({ page: 1, pageSize: 5, limit: 5 });
            setRecentOrders(res.data.items as any);
        } catch {
            // Recent orders is optional; ignore failure
        }
    }, [user?.id]);

    const loadPending = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getPendingOrders({ page: 1, pageSize: 5 });
            setPendingOrders(res.data);
        } catch {
            setPendingOrders(null);
        }
    }, [user?.id]);

    const loadProcessing = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getProcessingOrders({ page: 1, pageSize: 5 });
            setProcessingOrders(res.data);
        } catch {
            setProcessingOrders(null);
        }
    }, [user?.id]);

    useEffect(() => {
        loadOrders();
        loadRecent();
        loadPending();
        loadProcessing();
    }, [loadOrders, loadRecent, loadPending, loadProcessing]);

    const handleSearch = async (term: string) => {
        if (!user?.id) return;
        setLoading(true);
        setError(null);
        try {
            const res = await searchOrders({ searchTerm: term, page: 1, pageSize });
            setOrdersData(res.data);
            setPage(1);
        } catch (err: any) {
            setError(err.response?.data?.message || "Search failed.");
        } finally {
            setLoading(false);
        }
    };

    const handleFilter = async (status: string | undefined) => {
        setStatusFilter(status);
        setPage(1);
        setSearchTerm("");
        await loadOrders();
    };

    const filteredTabs = STATUS_TABS.map((tab) => ({
        ...tab,
        count: summary
            ? tab.key === null
                ? summary.totalOrders
                : tab.key === "PENDING"
                  ? summary.pendingOrders
                  : tab.key === "CONFIRMED"
                    ? summary.confirmedOrders
                    : tab.key === "PREPARING"
                      ? summary.preparingOrders
                      : tab.key === "READYFORSHIPMENT"
                        ? summary.readyForShipmentOrders
                        : tab.key === "SHIPPING"
                          ? summary.shippingOrders
                          : tab.key === "DELIVERED"
                            ? summary.deliveredOrders
                            : tab.key === "CANCELLED"
                              ? summary.cancelledOrders
                              : 0
            : 0,
    }));

    const formatDate = (dateStr: string) => {
        return new Date(dateStr).toLocaleDateString();
    };

    const statusColor = (status: string) => {
        return STATUS_COLORS[status] || "#666";
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
                    transition: "all 0.2s",
                    border: "1px solid #eee",
                    display: "flex",
                    flexDirection: "column",
                    gap: "12px",
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
                    <div style={{ color: "#ee4d2d", fontSize: "18px", fontWeight: "bold" }}>
                        {order.totalPrice.toLocaleString()} đ
                    </div>
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
                    <div style={{ fontSize: "18px", color: "#666" }}>Loading your orders...</div>
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
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px", flexWrap: "wrap", gap: "16px" }}>
                    <div>
                        <h1 style={{ fontSize: "24px", color: "#333", margin: "0 0 4px" }}>My Orders</h1>
                        <p style={{ color: "#666", margin: "0" }}>Track and manage your EcomSystem orders.</p>
                    </div>
                    <Link
                        to="/"
                        style={{ padding: "10px 24px", background: "#ee4d2d", color: "white", borderRadius: "6px", textDecoration: "none", fontWeight: "600" }}
                    >
                        Start Shopping
                    </Link>
                </div>

                {summary && (
                    <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))", gap: "12px", marginBottom: "24px" }}>
                        <div style={{ background: "white", padding: "16px", borderRadius: "10px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee" }}>
                            <div style={{ fontSize: "11px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "6px" }}>Total Orders</div>
                            <div style={{ fontSize: "22px", fontWeight: "800", color: "#333" }}>{summary.totalOrders}</div>
                        </div>
                        <div style={{ background: "white", padding: "16px", borderRadius: "10px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee" }}>
                            <div style={{ fontSize: "11px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "6px" }}>Total Spent</div>
                            <div style={{ fontSize: "22px", fontWeight: "800", color: "#ee4d2d" }}>{summary.totalSpent.toLocaleString()} đ</div>
                        </div>
                        <div style={{ background: "white", padding: "16px", borderRadius: "10px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee" }}>
                            <div style={{ fontSize: "11px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "6px" }}>Pending</div>
                            <div style={{ fontSize: "22px", fontWeight: "800", color: "#f59f00" }}>{summary.pendingOrders}</div>
                        </div>
                        <div style={{ background: "white", padding: "16px", borderRadius: "10px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee" }}>
                            <div style={{ fontSize: "11px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "6px" }}>Processing</div>
                            <div style={{ fontSize: "22px", fontWeight: "800", color: "#0ca678" }}>{summary.confirmedOrders}</div>
                        </div>
                        <div style={{ background: "white", padding: "16px", borderRadius: "10px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee" }}>
                            <div style={{ fontSize: "11px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "6px" }}>Delivered</div>
                            <div style={{ fontSize: "22px", fontWeight: "800", color: "#2b8a3e" }}>{summary.deliveredOrders}</div>
                        </div>
                    </div>
                )}

                <div style={{ background: "white", borderRadius: "12px", padding: "20px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee", marginBottom: "24px" }}>
                    <div style={{ display: "flex", gap: "12px", marginBottom: "16px" }}>
                        <input
                            type="text"
                            placeholder="Search orders by order number..."
                            value={searchTerm}
                            onChange={(e) => setSearchTerm(e.target.value)}
                            onKeyDown={(e) => e.key === "Enter" && handleSearch(searchTerm)}
                            style={{
                                flex: 1,
                                padding: "10px 16px",
                                border: "1px solid #ddd",
                                borderRadius: "8px",
                                fontSize: "14px",
                                outline: "none",
                            }}
                        />
                        <button
                            onClick={() => handleSearch(searchTerm)}
                            style={{
                                padding: "10px 24px",
                                background: "#ee4d2d",
                                color: "white",
                                border: "none",
                                borderRadius: "8px",
                                cursor: "pointer",
                                fontWeight: "600",
                                fontSize: "14px",
                            }}
                        >
                            Search
                        </button>
                    </div>
                </div>

                <div style={{ marginBottom: "20px" }}>
                    <div style={{ fontSize: "13px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "10px" }}>Filter by Status</div>
                    <div style={{ display: "flex", gap: "8px", overflowX: "auto", paddingBottom: "8px" }}>
                        {filteredTabs.map((tab) => (
                            <button
                                key={tab.key}
                                onClick={() => handleFilter(tab.key)}
                                style={{
                                    padding: "8px 16px",
                                    background: statusFilter === tab.key ? "#ee4d2d" : "white",
                                    color: statusFilter === tab.key ? "white" : "#666",
                                    border: `1px solid ${statusFilter === tab.key ? "#ee4d2d" : "#ddd"}`,
                                    borderRadius: "20px",
                                    cursor: "pointer",
                                    fontSize: "13px",
                                    fontWeight: "600",
                                    whiteSpace: "nowrap",
                                }}
                            >
                                {tab.label}
                                {tab.count > 0 && (
                                    <span
                                        style={{
                                            marginLeft: "6px",
                                            fontSize: "10px",
                                            background: statusFilter === tab.key ? "rgba(255,255,255,0.2)" : "#eee",
                                            color: statusFilter === tab.key ? "white" : "#999",
                                            padding: "2px 6px",
                                            borderRadius: "10px",
                                        }}
                                    >
                                        {tab.count}
                                    </span>
                                )}
                            </button>
                        ))}
                    </div>
                </div>

                {summary && (
                    <div style={{ display: "flex", gap: "20px", marginBottom: "24px", overflowX: "auto" }}>
                        <div style={{ minWidth: "200px", background: "white", borderRadius: "10px", padding: "16px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee", flexShrink: 0 }}>
                            <div style={{ fontSize: "12px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "12px" }}>Recent Orders</div>
                            {recentOrders && recentOrders.length > 0 ? (
                                <div style={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                                    {recentOrders.slice(0, 3).map((o: any) => (
                                        <div key={o.id} style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "8px 0", borderBottom: "1px solid #f5f5f5" }}>
                                            <div style={{ fontSize: "13px", color: "#333", fontWeight: "600" }}>{o.orderNumber}</div>
                                            <span style={{ fontSize: "11px", color: statusColor(o.status), fontWeight: "600", background: `${statusColor(o.status)}20`, padding: "2px 8px", borderRadius: "10px" }}>
                                                {o.status}
                                            </span>
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <div style={{ textAlign: "center", color: "#999", fontSize: "13px", padding: "16px 0" }}>No recent orders</div>
                            )}
                        </div>

                        <div style={{ minWidth: "200px", background: "white", borderRadius: "10px", padding: "16px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee", flexShrink: 0 }}>
                            <div style={{ fontSize: "12px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "12px" }}>Pending Confirmation</div>
                            {pendingOrders && pendingOrders.items.length > 0 ? (
                                <div style={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                                    {pendingOrders.items.slice(0, 3).map((o: any) => (
                                        <div key={o.id} style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "8px 0", borderBottom: "1px solid #f5f5f5" }}>
                                            <div style={{ fontSize: "13px", color: "#333", fontWeight: "600" }}>{o.orderNumber}</div>
                                            <span style={{ fontSize: "11px", color: "#f59f00", fontWeight: "600", background: "#f59f0020", padding: "2px 8px", borderRadius: "10px" }}>
                                                Pending
                                            </span>
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <div style={{ textAlign: "center", color: "#999", fontSize: "13px", padding: "16px 0" }}>No pending orders</div>
                            )}
                        </div>

                        <div style={{ minWidth: "200px", background: "white", borderRadius: "10px", padding: "16px", boxShadow: "0 2px 8px rgba(0,0,0,0.06)", border: "1px solid #eee", flexShrink: 0 }}>
                            <div style={{ fontSize: "12px", color: "#999", textTransform: "uppercase", fontWeight: "700", marginBottom: "12px" }}>Processing</div>
                            {processingOrders && processingOrders.items.length > 0 ? (
                                <div style={{ display: "flex", flexDirection: "column", gap: "8px" }}>
                                    {processingOrders.items.slice(0, 3).map((o: any) => (
                                        <div key={o.id} style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "8px 0", borderBottom: "1px solid #f5f5f5" }}>
                                            <div style={{ fontSize: "13px", color: "#333", fontWeight: "600" }}>{o.orderNumber}</div>
                                            <span style={{ fontSize: "11px", color: "#0ca678", fontWeight: "600", background: "#0ca67820", padding: "2px 8px", borderRadius: "10px" }}>
                                                Processing
                                            </span>
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <div style={{ textAlign: "center", color: "#999", fontSize: "13px", padding: "16px 0" }}>No processing orders</div>
                            )}
                        </div>
                    </div>
                )}

                {items.length === 0 ? (
                    <div style={{ textAlign: "center", padding: "60px 20px", background: "white", borderRadius: "12px" }}>
                        <div style={{ fontSize: "32px", marginBottom: "12px" }}>📭</div>
                        <h2 style={{ color: "#666", marginBottom: "8px" }}>No orders found</h2>
                        <p style={{ color: "#999", marginBottom: "24px" }}>
                            {searchTerm ? `No orders matching "${searchTerm}".` : statusFilter ? `No orders with status "${statusFilter}".` : "You haven't placed any orders yet."}
                        </p>
                        <Link
                            to="/"
                            style={{ padding: "12px 24px", background: "#ee4d2d", color: "white", borderRadius: "8px", textDecoration: "none", fontWeight: "bold" }}
                        >
                            Start Shopping
                        </Link>
                    </div>
                ) : (
                    <div style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
                        {items.map((order) => renderOrderCard(order))}
                    </div>
                )}

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
            </div>
        </div>
    );
}
