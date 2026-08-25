import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import { getMyShipments, type ShipmentDto } from "../../services/shippingApi";
import "../../styles/shipper.css";

export default function ShipperDashboard() {
    const [shipments, setShipments] = useState<ShipmentDto[]>([]);
    const [loading, setLoading] = useState(true);

    const loadShipments = useCallback(async () => {
        setLoading(true);
        try {
            const res = await getMyShipments();
            setShipments(res.data || []);
        } catch {
            setShipments([]);
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        loadShipments();
    }, [loadShipments]);

    const assignedCount = shipments.filter((s) => s.status === "ASSIGNED").length;
    const deliveringCount = shipments.filter((s) => s.status === "DELIVERING" || s.status === "PickedUp").length;
    const completedCount = shipments.filter((s) => s.status === "DELIVERED").length;
    const failedCount = shipments.filter((s) => s.status === "FAILED").length;

    const recentShipments = [...shipments].sort((a, b) => b.id.localeCompare(a.id)).slice(0, 5);

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Loading dashboard...</h2>
                </div>
            </div>
        );
    }

    return (
        <div>
            <Navbar />
            <div className="portal-container">
                <div className="portal-header">
                    <div>
                        <h1>🚚 Shipper Dashboard</h1>
                        <p style={{ color: "#666" }}>Welcome! Manage your deliveries here.</p>
                    </div>
                </div>

                <div className="portal-nav">
                    <Link to="/shipper" className="portal-nav-item active">Dashboard</Link>
                    <Link to="/shipper/orders" className="portal-nav-item">My Deliveries</Link>
                    <Link to="/shipper/history" className="portal-nav-item">History</Link>
                </div>

                <div className="portal-stat-grid">
                    <div className="portal-stat-card" style={{ borderLeftColor: "#228be6" }}>
                        <h3>Assigned</h3>
                        <div className="stat-value">{assignedCount}</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#f59f00" }}>
                        <h3>Delivering</h3>
                        <div className="stat-value">{deliveringCount}</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#2b8a3e" }}>
                        <h3>Completed</h3>
                        <div className="stat-value">{completedCount}</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#e03131" }}>
                        <h3>Failed</h3>
                        <div className="stat-value">{failedCount}</div>
                    </div>
                </div>

                <div className="portal-card">
                    <h3 style={{ marginBottom: "16px", color: "#333" }}>Recent Deliveries</h3>
                    {recentShipments.length === 0 ? (
                        <p style={{ color: "#888", fontSize: "14px" }}>No deliveries yet.</p>
                    ) : (
                        <table className="portal-table">
                            <thead>
                                <tr>
                                    <th>Shipment ID</th>
                                    <th>Order ID</th>
                                    <th>Status</th>
                                    <th>Updated</th>
                                </tr>
                            </thead>
                            <tbody>
                                {recentShipments.map((s) => (
                                    <tr key={s.id}>
                                        <td style={{ fontWeight: "600" }}>{s.id.substring(0, 8)}...</td>
                                        <td>{s.orderId.substring(0, 8)}...</td>
                                        <td><span className={`admin-badge ${s.status === "DELIVERED" ? "success" : s.status === "FAILED" ? "danger" : "warning"}`}>{s.status}</span></td>
                                        <td>{s.updatedAt ? new Date(s.updatedAt).toLocaleString() : "N/A"}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    )}
                </div>
            </div>
        </div>
    );
}
