import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import { getOrdersBySellerId, updateOrderStatus, type OrderDto } from "../../services/orderApi";
import { getShippingStatusByOrderId, createShipment, assignShipment, type ShipmentDto } from "../../services/shippingApi";
import OrderStatusBadge from "../../components/OrderStatusBadge";
import { useAuth } from "../../context/AuthContext";
import "../../styles/seller.css";

const STATUS_TABS = [
    { key: "ALL", label: "All" },
    { key: "PENDING", label: "Pending" },
    { key: "CONFIRMED", label: "Confirmed" },
    { key: "PREPARING", label: "Preparing" },
    { key: "READY_FOR_SHIPMENT", label: "Ready to Ship" },
    { key: "SHIPPING", label: "Shipping" },
    { key: "DELIVERED", label: "Delivered" },
    { key: "RETURNED", label: "Returned" },
    { key: "CANCELLED", label: "Cancelled" },
];

export default function SellerOrders() {
    const { user } = useAuth();
    const [orders, setOrders] = useState<OrderDto[]>([]);
    const [shipments, setShipments] = useState<Record<string, ShipmentDto>>({});
    const [loading, setLoading] = useState(true);
    const [actionLoading, setActionLoading] = useState(false);
    const [message, setMessage] = useState<string | null>(null);
    const [activeTab, setActiveTab] = useState("ALL");

    const loadData = useCallback(async () => {
        if (!user?.id) return;
        setLoading(true);
        setMessage(null);
        try {
            const res = await getOrdersBySellerId(user.id);
            const ordersData = res.data || [];
            setOrders(ordersData);

            const shipmentMap: Record<string, ShipmentDto> = {};
            for (const o of ordersData) {
                try {
                    const sRes = await getShippingStatusByOrderId(o.id);
                    if (sRes.data) {
                        shipmentMap[o.id] = sRes.data;
                    }
                } catch {
                    // skip
                }
            }
            setShipments(shipmentMap);
        } catch {
            setOrders([]);
        } finally {
            setLoading(false);
        }
    }, [user?.id]);

    useEffect(() => {
        loadData();
    }, [loadData]);

    const filteredOrders = activeTab === "ALL"
        ? orders
        : orders.filter((o) => o.status === activeTab);

    const handleConfirm = async (orderId: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            await updateOrderStatus(orderId, "CONFIRMED");
            setMessage("Order confirmed!");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to confirm order.");
        } finally {
            setActionLoading(false);
        }
    };

    const handlePrepare = async (orderId: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            await updateOrderStatus(orderId, "PREPARING");
            setMessage("Order is being prepared!");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to prepare order.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleReady = async (orderId: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            await updateOrderStatus(orderId, "READY_FOR_SHIPMENT");
            setMessage("Order is ready for shipment!");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to update order.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleAssign = async (orderId: string, shipperId: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            const shipment = shipments[orderId];
            if (!shipment) throw new Error("No shipment found");
            await assignShipment(shipment.id, shipperId);
            setMessage("Shipper assigned!");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to assign shipper.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleCreateShipment = async (orderId: string, address: string, phone: string, receiverName: string) => {
        setActionLoading(true);
        setMessage(null);
        try {
            await createShipment({
                orderId,
                address,
                phone,
                receiverName,
            });
            setMessage("Shipment created! Now assign a shipper.");
            loadData();
        } catch (err: any) {
            setMessage(err.response?.data?.message || "Failed to create shipment.");
        } finally {
            setActionLoading(false);
        }
    };

    const canConfirm = (status: string) => status === "PENDING";
    const canPrepare = (status: string) => status === "CONFIRMED";
    const canReady = (status: string) => status === "PREPARING";

    return (
        <div>
            <Navbar />
            <div className="portal-container">
                <div className="portal-header">
                    <div>
                        <h1>📋 My Orders</h1>
                        <p style={{ color: "#666" }}>Orders containing your products.</p>
                    </div>
                </div>

                <div className="portal-nav">
                    <Link to="/seller" className="portal-nav-item">Dashboard</Link>
                    <Link to="/seller/products" className="portal-nav-item">Products</Link>
                    <Link to="/seller/orders" className="portal-nav-item active">Orders</Link>
                    <Link to="/seller/promotions" className="portal-nav-item">Promotions</Link>
                    <Link to="/seller/revenue" className="portal-nav-item">Revenue</Link>
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
                        <p style={{ textAlign: "center", padding: "40px" }}>Loading orders...</p>
                    ) : filteredOrders.length === 0 ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#888" }}>No orders found.</p>
                    ) : (
                        <div style={{ overflowX: "auto" }}>
                            <table className="portal-table">
                                <thead>
                                    <tr>
                                        <th>Order ID</th>
                                        <th>Status</th>
                                        <th>Payment</th>
                                        <th>Total</th>
                                        <th>Shipping</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {filteredOrders.map((o) => {
                                        const shipment = shipments[o.id];
                                        return (
                                            <tr key={o.id}>
                                                <td style={{ fontWeight: "600" }}>
                                                    <Link to={`/orders/${o.id}`} style={{ color: "#ee4d2d", textDecoration: "none" }}>
                                                        {o.id.substring(0, 8)}...
                                                    </Link>
                                                </td>
                                                <td><OrderStatusBadge status={o.status} /></td>
                                                <td>{o.paymentStatus}</td>
                                                <td style={{ color: "#ee4d2d", fontWeight: "bold" }}>{o.totalPrice.toLocaleString()} đ</td>
                                                <td>{shipment ? shipment.status : "N/A"}</td>
                                                <td>
                                                    {canConfirm(o.status) && (
                                                        <button
                                                            className="portal-btn success"
                                                            onClick={() => handleConfirm(o.id)}
                                                            disabled={actionLoading}
                                                        >
                                                            Confirm
                                                        </button>
                                                    )}
                                                    {canPrepare(o.status) && (
                                                        <button
                                                            className="portal-btn primary"
                                                            onClick={() => handlePrepare(o.id)}
                                                            disabled={actionLoading}
                                                        >
                                                            Prepare
                                                        </button>
                                                    )}
                                                    {canReady(o.status) && (
                                                        <button
                                                            className="portal-btn info"
                                                            onClick={() => handleReady(o.id)}
                                                            disabled={actionLoading}
                                                        >
                                                            Ready to Ship
                                                        </button>
                                                    )}
                                                    {o.status === "READY_FOR_SHIPMENT" && !shipment && (
                                                        <button
                                                            className="portal-btn primary"
                                                            onClick={() => handleCreateShipment(o.id, o.address, o.phone, o.receiverName)}
                                                            disabled={actionLoading}
                                                        >
                                                            Create Shipment
                                                        </button>
                                                    )}
                                                    {shipment && o.status === "READY_FOR_SHIPMENT" && (
                                                        <button
                                                            className="portal-btn info"
                                                            onClick={() => {
                                                                const shipperId = prompt("Enter Shipper ID:");
                                                                if (shipperId) handleAssign(o.id, shipperId);
                                                            }}
                                                            disabled={actionLoading}
                                                        >
                                                            Assign Shipper
                                                        </button>
                                                    )}
                                                </td>
                                            </tr>
                                        );
                                    })}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
