import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { getOrderTimeline, type OrderStatusHistoryDto } from "../services/orderApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";

const STATUS_COLORS: Record<string, string> = {
    PENDING: "#f59f00",
    CONFIRMED: "#0ca678",
    PREPARING: "#0ca678",
    READYFORSHIPMENT: "#0ca678",
    SHIPPING: "#2196f3",
    DELIVERED: "#2b8a3e",
    RETURNED: "#e03131",
    CANCELLED: "#e03131",
};

export default function OrderTimeline() {
    const { orderId } = useParams<{ orderId: string }>();
    const { user } = useAuth();

    if (!user?.id) {
        return null;
    }

    const [timeline, setTimeline] = useState<OrderStatusHistoryDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!orderId) return;
        setLoading(true);
        setError(null);
        getOrderTimeline(orderId)
            .then((res: any) => {
                setTimeline(res.data || []);
            })
            .catch((err: any) => {
                setError(err.response?.data?.message || "Failed to load order timeline.");
            })
            .finally(() => {
                setLoading(false);
            });
    }, [orderId]);

    const statusColor = (status: string) => {
        return STATUS_COLORS[status] || "#666";
    };

    const statusLabel = (status: string) => {
        const labels: Record<string, string> = {
            PENDING: "Pending Confirmation",
            CONFIRMED: "Confirmed",
            PREPARING: "Preparing",
            READYFORSHIPMENT: "Ready to Ship",
            SHIPPING: "Shipping",
            DELIVERED: "Delivered",
            RETURNED: "Returned",
            CANCELLED: "Cancelled",
        };
        return labels[status] || status;
    };

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Loading order timeline...</h2>
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
                    <Link to="/orders" style={{ marginTop: "20px", display: "inline-block", padding: "10px 24px", background: "#ee4d2d", color: "white", borderRadius: "6px", textDecoration: "none", fontWeight: "600" }}>
                        Back to My Orders
                    </Link>
                </div>
            </div>
        );
    }

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "900px", margin: "30px auto", padding: "20px" }}>
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "24px" }}>
                    <div>
                        <h1 style={{ fontSize: "24px", color: "#333", margin: "0 0 4px" }}>Order Timeline</h1>
                        <p style={{ color: "#666", margin: "0" }}>Order #{orderId}</p>
                    </div>
                    <Link
                        to="/orders"
                        style={{ padding: "10px 24px", background: "#ee4d2d", color: "white", borderRadius: "6px", textDecoration: "none", fontWeight: "600" }}
                    >
                        Back to My Orders
                    </Link>
                </div>

                <div style={{ background: "white", borderRadius: "12px", padding: "28px", boxShadow: "0 2px 10px rgba(0,0,0,0.06)" }}>
                    <div style={{ position: "relative", paddingLeft: "24px" }}>
                        <div style={{
                            position: "absolute",
                            left: "8px",
                            top: "0",
                            bottom: "0",
                            width: "2px",
                            background: "#eee",
                        }} />
                        {timeline.length === 0 && (
                            <div style={{ textAlign: "center", color: "#999", padding: "40px 0" }}>
                                <div style={{ fontSize: "32px", marginBottom: "12px" }}>📜</div>
                                <p>No status history available for this order.</p>
                            </div>
                        )}
                        {timeline.map((entry, index) => (
                            <div key={entry.id} style={{ display: "flex", alignItems: "flex-start", gap: "16px", marginBottom: "24px", position: "relative" }}>
                                <div style={{
                                    width: "14px",
                                    height: "14px",
                                    borderRadius: "50%",
                                    background: index === timeline.length - 1 ? "#ee4d2d" : "#e0e0e0",
                                    border: "2px solid white",
                                    boxShadow: "0 0 0 2px #eee",
                                    position: "relative",
                                    zIndex: 1,
                                    flexShrink: 0,
                                }} />
                                <div style={{ flex: 1 }}>
                                    <div style={{ display: "flex", justifyContent: "space-between", alignItems: "baseline", marginBottom: "4px" }}>
                                        <span style={{ fontSize: "15px", fontWeight: "700", color: "#333" }}>
                                            {statusLabel(entry.status)}
                                        </span>
                                        <span style={{ fontSize: "12px", color: "#999" }}>
                                            {new Date(entry.createdAt).toLocaleString()}
                                        </span>
                                    </div>
                                    {entry.note && (
                                        <p style={{ margin: "0 0 8px 0", fontSize: "13px", color: "#666" }}>{entry.note}</p>
                                    )}
                                    <span
                                        style={{
                                            fontSize: "11px",
                                            color: statusColor(entry.status),
                                            fontWeight: "600",
                                            background: `${statusColor(entry.status)}20`,
                                            padding: "2px 8px",
                                            borderRadius: "10px",
                                        }}
                                    >
                                        {entry.status}
                                    </span>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    );
}
