import { useNavigate, Link } from "react-router-dom";
import CartIcon from "./CartIcon";
import { useAuth } from "../context/AuthContext";
import "../styles/navbar.css";

export default function Navbar() {
    const navigate = useNavigate();
    const { user, isAuthenticated, isAdmin, isSeller, isShipper, logout } = useAuth();

    const handleLogout = async () => {
        await logout();
        navigate("/login");
    };

    const getRoleBadge = () => {
        if (isAdmin) return { text: "ADMIN", color: "#7950f2" };
        if (isSeller) return { text: "SELLER", color: "#2b8a3e" };
        if (isShipper) return { text: "SHIPPER", color: "#228be6" };
        return null;
    };

    const roleBadge = getRoleBadge();

    return (
        <header className="navbar-container">
            <Link to="/" className="navbar-brand">
                🛍️ <span>EcomSystem</span>
            </Link>

            <nav className="navbar-links">
                <Link to="/" className="navbar-link">
                    Products
                </Link>

                {isAuthenticated ? (
                    <>
                        <Link to="/profile" className="navbar-link">
                            👤 Profile
                        </Link>

                        {!isAdmin && (
                            <Link to="/orders" className="navbar-link">
                                📦 Orders
                            </Link>
                        )}

                        {isSeller && (
                            <Link to="/seller" className="navbar-admin-badge" style={{ background: "#2b8a3e" }}>
                                🏪 Seller Portal
                            </Link>
                        )}

                        {isShipper && (
                            <Link to="/shipper" className="navbar-admin-badge" style={{ background: "#228be6" }}>
                                🚚 Shipper Portal
                            </Link>
                        )}

                        {isAdmin && (
                            <Link to="/admin" className="navbar-admin-badge">
                                ⚙️ Admin Portal
                            </Link>
                        )}

                        {!isAdmin && !isSeller && !isShipper && (
                            <CartIcon />
                        )}

                        <div className="navbar-user-section">
                            {roleBadge && (
                                <span
                                    style={{
                                        background: roleBadge.color,
                                        color: "white",
                                        padding: "2px 8px",
                                        borderRadius: "4px",
                                        fontSize: "11px",
                                        fontWeight: "700",
                                    }}
                                >
                                    {roleBadge.text}
                                </span>
                            )}
                            <span className="navbar-user-email">
                                {user?.email}
                            </span>
                            <button
                                onClick={handleLogout}
                                className="navbar-btn outline"
                            >
                                Logout
                            </button>
                        </div>
                    </>
                ) : (
                    <div className="navbar-user-section">
                        <Link to="/login" className="navbar-btn outline">
                            Login
                        </Link>
                        <Link to="/register" className="navbar-btn primary">
                            Register
                        </Link>
                    </div>
                )}
            </nav>
        </header>
    );
}