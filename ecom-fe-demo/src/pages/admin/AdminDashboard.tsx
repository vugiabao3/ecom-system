import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import "../../styles/admin.css";

export default function AdminDashboard() {
    return (
        <div>
            <Navbar />
            <div className="admin-container">
                <div className="admin-header">
                    <div>
                        <h1 style={{ fontSize: "26px", color: "#222" }}>🛠️ Admin Management Portal</h1>
                        <p style={{ color: "#666", marginTop: "4px" }}>
                            Manage products, inventory, users, promotions, categories, and shipments.
                        </p>
                    </div>
                </div>

                <div className="admin-nav">
                    <Link to="/admin" className="admin-nav-item active">Dashboard</Link>
                    <Link to="/admin/products" className="admin-nav-item">Products & Stock</Link>
                    <Link to="/admin/categories" className="admin-nav-item">Categories</Link>
                    <Link to="/admin/users" className="admin-nav-item">Users & Roles</Link>
                    <Link to="/admin/promotions" className="admin-nav-item">Promotions</Link>
                    <Link to="/admin/shipping" className="admin-nav-item">Shipping & Orders</Link>
                </div>

                <div className="admin-grid-dashboard">
                    <div className="admin-stat-card">
                        <h3>Product Catalog</h3>
                        <div className="stat-value">Products</div>
                        <p style={{ color: "#666", fontSize: "14px", margin: "8px 0 16px" }}>
                            Create, update, delete products, and manage warehouse stock.
                        </p>
                        <Link to="/admin/products" className="admin-btn primary" style={{ textDecoration: "none", display: "inline-block" }}>
                            Manage Products →
                        </Link>
                    </div>

                    <div className="admin-stat-card" style={{ borderLeftColor: "#1971c2" }}>
                        <h3>Categories</h3>
                        <div className="stat-value">Categories</div>
                        <p style={{ color: "#666", fontSize: "14px", margin: "8px 0 16px" }}>
                            Organize store inventory by creating and editing product categories.
                        </p>
                        <Link to="/admin/categories" className="admin-btn primary" style={{ textDecoration: "none", display: "inline-block", background: "#1971c2" }}>
                            Manage Categories →
                        </Link>
                    </div>

                    <div className="admin-stat-card" style={{ borderLeftColor: "#2b8a3e" }}>
                        <h3>User Accounts</h3>
                        <div className="stat-value">Users</div>
                        <p style={{ color: "#666", fontSize: "14px", margin: "8px 0 16px" }}>
                            Search users, block/unblock, set roles (Admin/User), and monitor accounts.
                        </p>
                        <Link to="/admin/users" className="admin-btn primary" style={{ textDecoration: "none", display: "inline-block", background: "#2b8a3e" }}>
                            Manage Users →
                        </Link>
                    </div>

                    <div className="admin-stat-card" style={{ borderLeftColor: "#f59f00" }}>
                        <h3>Discounts & Coupons</h3>
                        <div className="stat-value">Promotions</div>
                        <p style={{ color: "#666", fontSize: "14px", margin: "8px 0 16px" }}>
                            Create discount codes, set percentage reductions and expiration dates.
                        </p>
                        <Link to="/admin/promotions" className="admin-btn primary" style={{ textDecoration: "none", display: "inline-block", background: "#f59f00" }}>
                            Manage Promotions →
                        </Link>
                    </div>

                    <div className="admin-stat-card" style={{ borderLeftColor: "#7950f2" }}>
                        <h3>Fulfillment</h3>
                        <div className="stat-value">Shipping</div>
                        <p style={{ color: "#666", fontSize: "14px", margin: "8px 0 16px" }}>
                            Track order shipments, trigger delivery dispatch, and complete delivery.
                        </p>
                        <Link to="/admin/shipping" className="admin-btn primary" style={{ textDecoration: "none", display: "inline-block", background: "#7950f2" }}>
                            Manage Shipping →
                        </Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
