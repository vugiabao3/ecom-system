import { useNavigate, Link } from "react-router-dom";
import CartIcon from "./CartIcon";
import { useAuth } from "../context/AuthContext";
import "../styles/navbar.css";

export default function Navbar() {
    const navigate = useNavigate();
    const { user, isAuthenticated, isAdmin, logout } = useAuth();

    const handleLogout = async () => {
        await logout();
        navigate("/login");
    };

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

                        <Link to="/orders" className="navbar-link">
                            📦 Orders
                        </Link>

                        {isAdmin && (
                            <Link to="/admin" className="navbar-admin-badge">
                                ⚙️ Admin Portal
                            </Link>
                        )}

                        <CartIcon />

                        <div className="navbar-user-section">
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