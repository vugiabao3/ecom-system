import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import { getOrdersBySellerId, type OrderDto } from "../../services/orderApi";
import { getProductsBySeller, type ProductItem } from "../../services/productApi";
import { useAuth } from "../../context/AuthContext";
import "../../styles/seller.css";

export default function SellerRevenue() {
    const { user } = useAuth();
    const [orders, setOrders] = useState<OrderDto[]>([]);
    const [products, setProducts] = useState<ProductItem[]>([]);
    const [loading, setLoading] = useState(true);

    const loadData = useCallback(async () => {
        if (!user?.id) return;
        setLoading(true);
        try {
            const [ordersRes, productsRes] = await Promise.all([
                getOrdersBySellerId(user.id),
                getProductsBySeller(user.id),
            ]);
            setOrders(ordersRes.data || []);
            setProducts(productsRes.data || []);
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
    const totalOrders = orders.length;
    const totalSold = orders.reduce((sum, o) => sum + o.items.reduce((s, i) => s + i.quantity, 0), 0);
    const totalReturned = orders
        .filter((o) => o.status === "RETURNED")
        .reduce((sum, o) => sum + o.items.reduce((s, i) => s + i.quantity, 0), 0);

    const productSales: Record<string, { name: string; revenue: number; sold: number }> = {};
    for (const o of orders) {
        for (const item of o.items) {
            const product = products.find((p) => p.id === item.productId);
            const name = product?.name || item.productId.substring(0, 8);
            if (!productSales[name]) {
                productSales[name] = { name, revenue: 0, sold: 0 };
            }
            productSales[name].revenue += (o.totalPrice / o.items.length) * item.quantity;
            productSales[name].sold += item.quantity;
        }
    }

    const topProducts = Object.values(productSales)
        .sort((a, b) => b.revenue - a.revenue)
        .slice(0, 10);

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Loading revenue data...</h2>
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
                        <h1>📊 Revenue Analytics</h1>
                        <p style={{ color: "#666" }}>Track your sales performance and product metrics.</p>
                    </div>
                </div>

                <div className="portal-nav">
                    <Link to="/seller" className="portal-nav-item">Dashboard</Link>
                    <Link to="/seller/products" className="portal-nav-item">Products</Link>
                    <Link to="/seller/orders" className="portal-nav-item">Orders</Link>
                    <Link to="/seller/promotions" className="portal-nav-item">Promotions</Link>
                    <Link to="/seller/revenue" className="portal-nav-item active">Revenue</Link>
                </div>

                <div className="portal-stat-grid">
                    <div className="portal-stat-card" style={{ borderLeftColor: "#ee4d2d" }}>
                        <h3>Total Revenue</h3>
                        <div className="stat-value">{totalRevenue.toLocaleString()} đ</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#2b8a3e" }}>
                        <h3>Total Orders</h3>
                        <div className="stat-value">{totalOrders}</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#228be6" }}>
                        <h3>Total Sold</h3>
                        <div className="stat-value">{totalSold} items</div>
                    </div>
                    <div className="portal-stat-card" style={{ borderLeftColor: "#e03131" }}>
                        <h3>Returned</h3>
                        <div className="stat-value">{totalReturned} items</div>
                    </div>
                </div>

                <div className="portal-card">
                    <h3 style={{ marginBottom: "16px", color: "#333" }}>Sales by Product</h3>
                    {topProducts.length === 0 ? (
                        <p style={{ color: "#888", fontSize: "14px" }}>No sales data yet.</p>
                    ) : (
                        <table className="portal-table">
                            <thead>
                                <tr>
                                    <th>Product</th>
                                    <th>Revenue</th>
                                    <th>Sold Qty</th>
                                </tr>
                            </thead>
                            <tbody>
                                {topProducts.map((p) => (
                                    <tr key={p.name}>
                                        <td style={{ fontWeight: "600" }}>{p.name}</td>
                                        <td style={{ color: "#ee4d2d", fontWeight: "bold" }}>{p.revenue.toLocaleString()} đ</td>
                                        <td>{p.sold}</td>
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
