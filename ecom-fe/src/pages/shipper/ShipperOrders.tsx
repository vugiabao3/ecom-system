import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import { getMyShipments, pickupShipment, deliverShipment, completeShipping, failShipment, confirmCashReceived, type ShipmentDto } from "../../services/shippingApi";
import { useAuth } from "../../context/AuthContext";
import "../../styles/shipper.css";

const STATUS_TABS = [
    { key: "ALL", label: "All" },
    { key: "ASSIGNED", label: "Assigned" },
    { key: "PickedUp", label: "Picked Up" },
    { key: "DELIVERING", label: "Delivering" },
    { key: "DELIVERED", label: "Completed" },
    { key: "FAILED", label: "Failed" },
];

export default function ShipperOrders() {
    const { user } = useAuth();
    const [shipments, setShipments] = useState<ShipmentDto[]>([]);
    const [loading, setLoading] = useState(true);
    const [actionLoading, setActionLoading] = useState(false);
    const [message, setMessage] = useState<string | null>(null);
    const [activeTab, setActiveTab] = useState("ALL");

    const loadData = useCallback(async () => {
        setLoading(true);
        setMessage(null);
        try {
            const res = await getMyShipments();
            const shipmentsData = res.data || [];
            setShipments(shipmentsData);
        } catch {
            setShipments([]);
        } finally {
            setLoading(false);
        }
    }, [user?.id]);

    useEffect(() => {
        loadData();
    }, [loadData]);

    const filteredShipments = activeTab === "ALL"
        ? shipments
        : shipments.filter((s) => s.status === activeTab);

    const handlePickup = async (shipmentId: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            await pickupShipment(shipmentId);
            setMessage("Shipment picked up!");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to pickup shipment.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleDeliver = async (shipmentId: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            await deliverShipment(shipmentId);
            setMessage("Delivery started!");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to start delivery.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleConfirmCash = async (orderId: string) => {
        if (!window.confirm("Confirm that you have received cash from the customer?")) return;
        setActionLoading(true);
        setMessage(null);
        try {
            await confirmCashReceived(orderId);
            setMessage("Cash confirmed! Payment marked as paid.");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to confirm cash.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleComplete = async (shipmentId: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            await completeShipping(shipmentId);
            setMessage("Delivery completed!");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to complete delivery.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleFail = async (shipmentId: string) => {
        if (!window.confirm("Mark this shipment as failed?")) return;
        setActionLoading(true);
        setMessage(null);
        try {
            await failShipment(shipmentId);
            setMessage("Shipment marked as failed.");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to mark as failed.");
        } finally {
            setActionLoading(false);
        }
    };

    const canPickup = (status: string) => status === "ASSIGNED";
    const canDeliver = (status: string) => status === "PickedUp";
    const canConfirmCash = (s: ShipmentDto) => s.paymentMethod === "COD" && s.paymentStatus === "Pending" && s.status === "DELIVERING";
    const canComplete = (status: string) => status === "DELIVERING";
    const canFail = (status: string) => ["ASSIGNED", "PickedUp", "DELIVERING"].includes(status);

    return (
        <div>
            <Navbar />
            <div className="portal-container">
                <div className="portal-header">
                    <div>
                        <h1>🚚 My Deliveries</h1>
                        <p style={{ color: "#666" }}>Your assigned shipments and delivery tasks.</p>
                    </div>
                </div>

                <div className="portal-nav">
                    <Link to="/shipper" className="portal-nav-item">Dashboard</Link>
                    <Link to="/shipper/orders" className="portal-nav-item active">My Deliveries</Link>
                    <Link to="/shipper/history" className="portal-nav-item">History</Link>
                </div>

                {message && (
                    <div style={{ padding: "12px", background: "#e6fcf5", color: "#0ca678", borderRadius: "6px", marginBottom: "16px" }}>
                        {message}
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

                <div className="portal-card">
                    {loading ? (
                        <p style={{ textAlign: "center", padding: "40px" }}>Loading shipments...</p>
                    ) : filteredShipments.length === 0 ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#888" }}>No shipments found.</p>
                    ) : (
                        <div style={{ overflowX: "auto" }}>
                            <table className="portal-table">
                                <thead>
                                    <tr>
                                        <th>Shipment ID</th>
                                        <th>Order ID</th>
                                        <th>Payment</th>
                                        <th>Status</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {filteredShipments.map((s) => (
                                        <tr key={s.id}>
                                            <td style={{ fontWeight: "600" }}>{s.id.substring(0, 8)}...</td>
                                            <td>
                                                <Link to={`/orders/${s.orderId}`} style={{ color: "#ee4d2d", textDecoration: "none" }}>
                                                    {s.orderId.substring(0, 8)}...
                                                </Link>
                                            </td>
                                            <td>
                                                {s.paymentMethod && (
                                                    <span style={{ fontSize: "12px", color: "#666" }}>
                                                        {s.paymentMethod} | {s.paymentStatus || "N/A"}
                                                    </span>
                                                )}
                                            </td>
                                            <td>
                                                <span className={`admin-badge ${s.status === "DELIVERED" ? "success" : s.status === "FAILED" ? "danger" : "warning"}`}>
                                                    {s.status}
                                                </span>
                                            </td>
                                            <td>
                                                {canPickup(s.status) && (
                                                    <button
                                                        className="portal-btn primary"
                                                        onClick={() => handlePickup(s.id)}
                                                        disabled={actionLoading}
                                                    >
                                                        Pick Up
                                                    </button>
                                                )}
                                                {canDeliver(s.status) && (
                                                    <button
                                                        className="portal-btn success"
                                                        onClick={() => handleDeliver(s.id)}
                                                        disabled={actionLoading}
                                                    >
                                                        Start Delivery
                                                    </button>
                                                )}
                                                {canConfirmCash(s) && (
                                                    <button
                                                        className="portal-btn warning"
                                                        onClick={() => handleConfirmCash(s.orderId)}
                                                        disabled={actionLoading}
                                                    >
                                                        Confirm Cash
                                                    </button>
                                                )}
                                                {canComplete(s.status) && (
                                                    <button
                                                        className="portal-btn success"
                                                        onClick={() => handleComplete(s.id)}
                                                        disabled={actionLoading}
                                                    >
                                                        Mark Delivered
                                                    </button>
                                                )}
                                                {canFail(s.status) && (
                                                    <button
                                                        className="portal-btn danger"
                                                        onClick={() => handleFail(s.id)}
                                                        disabled={actionLoading}
                                                    >
                                                        Mark Failed
                                                    </button>
                                                )}
                                            </td>
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
