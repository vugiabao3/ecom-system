import { useState } from "react";
import { Link } from "react-router-dom";
import {
    getShippingStatusByOrderId,
    startDelivery,
    completeShipping,
    type ShipmentDto
} from "../../services/shippingApi";
import Navbar from "../../components/Navbar";
import "../../styles/admin.css";

export default function AdminShipping() {
    const [orderId, setOrderId] = useState("");
    const [shipment, setShipment] = useState<ShipmentDto | null>(null);
    const [loading, setLoading] = useState(false);
    const [actionLoading, setActionLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [message, setMessage] = useState<string | null>(null);

    const handleSearch = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!orderId.trim()) return;

        setLoading(true);
        setError(null);
        setMessage(null);
        setShipment(null);
        try {
            const res = await getShippingStatusByOrderId(orderId.trim());
            setShipment(res.data);
        } catch (err: any) {
            setError(err.response?.data?.message || err.response?.data || "No shipment found for this order ID.");
        } finally {
            setLoading(false);
        }
    };

    const handleStartDelivery = async () => {
        if (!shipment) return;
        setActionLoading(true);
        setError(null);
        try {
            await startDelivery(shipment.id);
            setMessage("🚚 Shipment status changed to DELIVERING!");
            // Refresh
            const res = await getShippingStatusByOrderId(shipment.orderId);
            setShipment(res.data);
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to update shipment status.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleCompleteDelivery = async () => {
        if (!shipment) return;
        setActionLoading(true);
        setError(null);
        try {
            await completeShipping(shipment.id);
            setMessage("📦 Shipment status changed to DELIVERED!");
            // Refresh
            const res = await getShippingStatusByOrderId(shipment.orderId);
            setShipment(res.data);
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to complete delivery.");
        } finally {
            setActionLoading(false);
        }
    };

    return (
        <div>
            <Navbar />
            <div className="admin-container">
                <div className="admin-header">
                    <div>
                        <h1 style={{ fontSize: "24px", color: "#222" }}>🚚 Shipping & Fulfillment</h1>
                        <p style={{ color: "#666" }}>Look up orders, track shipment state, and advance delivery workflows.</p>
                    </div>
                </div>

                <div className="admin-nav">
                    <Link to="/admin" className="admin-nav-item">Dashboard</Link>
                    <Link to="/admin/products" className="admin-nav-item">Products & Stock</Link>
                    <Link to="/admin/categories" className="admin-nav-item">Categories</Link>
                    <Link to="/admin/users" className="admin-nav-item">Users & Roles</Link>
                    <Link to="/admin/promotions" className="admin-nav-item">Promotions</Link>
                    <Link to="/admin/shipping" className="admin-nav-item active">Shipping & Orders</Link>
                </div>

                <div className="admin-card" style={{ maxWidth: "600px" }}>
                    <h3 style={{ marginBottom: "12px" }}>Find Shipment By Order ID</h3>
                    <form onSubmit={handleSearch} style={{ display: "flex", gap: "10px" }}>
                        <input
                            type="text"
                            placeholder="Enter Order GUID (e.g. e4d5c...)"
                            value={orderId}
                            onChange={(e) => setOrderId(e.target.value)}
                            required
                            style={{ flex: 1, padding: "8px 12px", borderRadius: "6px", border: "1px solid #ddd" }}
                        />
                        <button type="submit" className="admin-btn primary" disabled={loading}>
                            {loading ? "Searching..." : "Track"}
                        </button>
                    </form>
                </div>

                {message && (
                    <div style={{ padding: "12px", background: "#e6fcf5", color: "#0ca678", borderRadius: "6px", marginBottom: "16px" }}>
                        {message}
                    </div>
                )}

                {error && (
                    <div style={{ padding: "12px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px", marginBottom: "16px" }}>
                        {error}
                    </div>
                )}

                {shipment && (
                    <div className="admin-card">
                        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "16px" }}>
                            <h3>Shipment #{shipment.id}</h3>
                            <span
                                className={`admin-badge ${
                                    shipment.status === "DELIVERED"
                                        ? "success"
                                        : shipment.status === "DELIVERING"
                                        ? "warning"
                                        : "info"
                                }`}
                            >
                                {shipment.status}
                            </span>
                        </div>

                        <div style={{ background: "#f8f9fa", padding: "16px", borderRadius: "8px", marginBottom: "20px" }}>
                            <p style={{ margin: "6px 0" }}>
                                <strong>Order ID:</strong> {shipment.orderId}
                            </p>
                            <p style={{ margin: "6px 0" }}>
                                <strong>Current Status:</strong> {shipment.status}
                            </p>
                        </div>

                        <div style={{ display: "flex", gap: "12px" }}>
                            <button
                                className="admin-btn primary"
                                onClick={handleStartDelivery}
                                disabled={actionLoading || shipment.status === "DELIVERED"}
                            >
                                🚚 Start Delivery (DELIVERING)
                            </button>

                            <button
                                className="admin-btn success"
                                onClick={handleCompleteDelivery}
                                disabled={actionLoading || shipment.status === "DELIVERED"}
                            >
                                📦 Mark Complete (DELIVERED)
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
