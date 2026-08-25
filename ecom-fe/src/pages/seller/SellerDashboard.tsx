import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import { getOrdersBySellerId, type OrderDto } from "../../services/orderApi";
import { getProductsBySeller, type ProductItem } from "../../services/productApi";
import { getInventoryByProductId } from "../../services/inventoryApi";
import { useAuth } from "../../context/AuthContext";
import OrderStatusBadge from "../../components/OrderStatusBadge";
import "../../styles/seller.css";

export default function SellerDashboard() {
    const { user } = useAuth();
    const [orders, setOrders] = useState<OrderDto[]>([]);
    const [products, setProducts] = useState<ProductItem[]>([]);
    const [lowStockProducts, setLowStockProducts] = useState<ProductItem[]>([]);
    const [loading, setLoading] = useState(true);

    const loadData = useCallback(async () => {
        if (!user?.id) return;
        setLoading(true);
        try {
            const [ordersRes, productsRes] = await Promise.all([
                getOrdersBySellerId(user.id),
                getProductsBySeller(user.id),
            ]);
            const ordersData = ordersRes.data || [];
            const productsList = productsRes.data || [];
            setOrders(ordersData);
            setProducts(productsList);

            const lowStock: ProductItem[] = [];
            for (const p of productsList) {
                try {
                    const invRes = await getInventoryByProductId(p.id);
                    const stock = (invRes.data?.available || 0) - (invRes.data?.reserved || 0);
                    if (stock < 5) {
                        lowStock.push(p);
                    }
                } catch {
                    // skip
                }
            }
            setLowStockProducts(lowStock);
        } catch {
            setOrders([]);
            setProducts([]);
        } finally {
            setLoading(false);
        }
    }, [user?.id]);

    useEffect(() => {
        loadData();
    }, [loadData]);

    const totalRevenue = orders.reduce((sum, o) => sum + o.totalPrice, 0);
    const today = new Date().toDateString();
    const todayRevenue = orders
        .filter((o) => new Date(o.id.substring(0, 8)).toDateString() === today)
        .reduce((sum, o) => sum + o.totalPrice, 0);

    const statusCounts = {
        PENDING: orders.filter((o) => o.status === "PENDING").length,
        CONFIRMED: orders.filter((o) => o.status === "CONFIRMED").length,
        SHIPPING: orders.filter((o) => o.status === "SHIPPING").length,
        DELIVERED: orders.filter((o) => o.status === "DELIVERED").length,
        RETURNED: orders.filter((o) => o.status === "RETURNED").length,
        CANCELLED: orders.filter((o) => o.status === "CANCELLED").length,
    };

    const recentOrders = [...orders].sort((a, b) => b.id.localeCompare(a.id)).slice(0, 5);

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
                        <h1>🏪 Seller Dashboard</h1>
                        <p style={{ color: "#666" }}>Welcome back! Here's your store overview.</p>
                    </div>
                    <Link to="/seller/products" className="portal-btn primary" style={{ padding: "10px 18px", fontSize: "14px" }}>
                        + Add Product
                    </Link>
                </div>

                <div className="portal-nav">
                    <Link to="/seller" className="portal-nav-item active">Dashboard</Link>
                    <Link to="/seller/products" className="portal-nav-item">Products</Link>
                    <Link to="/seller/orders" className="portal-nav-item">Orders</Link>
                    <Link to="/seller/promotions" className="portal-nav-item">Promotions</Link>
                    <Link to="/seller/revenue" className="portal-nav-item">Revenue</Link>
                </div>

                <div className="portal-stat-grid">
                    <div className="portal-stat-card" style={{ borderLeftColor: "#ee4d2d" }}>
                        <h3>Total Revenue</h3>
                        <div className="stat-value">{totalRevenue.toLocaleString()} đ</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#2b8a3e" }}>
                        <h3>Today's Revenue</h3>
                        <div className="stat-value">{todayRevenue.toLocaleString()} đ</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#f59f00" }}>
                        <h3>Total Orders</h3>
                        <div className="stat-value">{orders.length}</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#228be6" }}>
                        <h3>Products</h3>
                        <div className="stat-value">{products.length}</div>
                    </div>
                </div>

                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "24px", marginBottom: "24px" }}>
                    <div className="portal-card">
                        <h3 style={{ marginBottom: "16px", color: "#333" }}>Order Status</h3>
                        <div style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
                            {Object.entries(statusCounts).map(([status, count]) => (
                                <div key={status} style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                    <span style={{ fontSize: "14px", color: "#666" }}>{status}</span>
                                    <span style={{ fontWeight: "700", color: "#333" }}>{count}</span>
                                </div>
                            ))}
                        </div>
                    </div>

                    <div className="portal-card">
                        <h3 style={{ marginBottom: "16px", color: "#333" }}>Low Stock Alert</h3>
                        {lowStockProducts.length === 0 ? (
                            <p style={{ color: "#888", fontSize: "14px" }}>All products have sufficient stock.</p>
                        ) : (
                            <div style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
                                {lowStockProducts.slice(0, 5).map((p) => (
                                    <div key={p.id} style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                        <span style={{ fontSize: "14px", color: "#333" }}>{p.name}</span>
                                        <span style={{ fontSize: "13px", color: "#e03131", fontWeight: "600" }}>Low Stock</span>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>

                <div className="portal-card">
                    <h3 style={{ marginBottom: "16px", color: "#333" }}>Recent Orders</h3>
                    {recentOrders.length === 0 ? (
                        <p style={{ color: "#888", fontSize: "14px" }}>No orders yet.</p>
                    ) : (
                        <table className="portal-table">
                            <thead>
                                <tr>
                                    <th>Order ID</th>
                                    <th>Status</th>
                                    <th>Payment</th>
                                    <th>Total</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                {recentOrders.map((o) => (
                                    <tr key={o.id}>
                                        <td style={{ fontWeight: "600" }}>{o.id.substring(0, 8)}...</td>
                                        <td><OrderStatusBadge status={o.status} /></td>
                                        <td>{o.paymentStatus}</td>
                                        <td style={{ color: "#ee4d2d", fontWeight: "bold" }}>{o.totalPrice.toLocaleString()} đ</td>
                                        <td>
                                            <Link to={`/orders/${o.id}`} className="portal-btn info">View</Link>
                                        </td>
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
