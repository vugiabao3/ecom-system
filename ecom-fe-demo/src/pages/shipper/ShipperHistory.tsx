import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import { getMyShipments, type ShipmentDto } from "../../services/shippingApi";
import "../../styles/shipper.css";

export default function ShipperHistory() {
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

    const completedShipments = shipments
        .filter((s) => s.status === "DELIVERED" || s.status === "FAILED")
        .sort((a, b) => b.id.localeCompare(a.id));

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Loading history...</h2>
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
                        <h1>📜 Delivery History</h1>
                        <p style={{ color: "#666" }}>Your past deliveries and their outcomes.</p>
                    </div>
                </div>

                <div className="portal-nav">
                    <Link to="/shipper" className="portal-nav-item">Dashboard</Link>
                    <Link to="/shipper/orders" className="portal-nav-item">My Deliveries</Link>
                    <Link to="/shipper/history" className="portal-nav-item active">History</Link>
                </div>

                <div className="portal-card">
                    {completedShipments.length === 0 ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#888" }}>No delivery history yet.</p>
                    ) : (
                        <div style={{ overflowX: "auto" }}>
                            <table className="portal-table">
                                <thead>
                                    <tr>
                                        <th>Shipment ID</th>
                                        <th>Order ID</th>
                                        <th>Status</th>
                                        <th>Completed At</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {completedShipments.map((s) => (
                                        <tr key={s.id}>
                                            <td style={{ fontWeight: "600" }}>{s.id.substring(0, 8)}...</td>
                                            <td>
                                                <Link to={`/orders/${s.orderId}`} style={{ color: "#ee4d2d", textDecoration: "none" }}>
                                                    {s.orderId.substring(0, 8)}...
                                                </Link>
                                            </td>
                                            <td>
                                                <span className={`admin-badge ${s.status === "DELIVERED" ? "success" : "danger"}`}>
                                                    {s.status}
                                                </span>
                                            </td>
                                            <td>{s.updatedAt ? new Date(s.updatedAt).toLocaleString() : "N/A"}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
