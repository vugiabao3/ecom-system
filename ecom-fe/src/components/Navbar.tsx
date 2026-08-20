import {
    useNavigate,
    Link
} from "react-router-dom";
import CartIcon
from "./CartIcon";
import { logout } from "../services/authApi";
import { clearAuth, getRefreshToken } from "../utils/token";
export default function Navbar() {

    const navigate = useNavigate();

    const handleLogout = async () => {
        const refreshToken = getRefreshToken();

        try {
            if (refreshToken) {
                await logout({ refreshToken });
            }
        } catch {
            // still clear local session
        }

        clearAuth();
        navigate("/login");
    };

    return (

        <div className="navbar">

            <h2>EcomSystem</h2>

            <div className="nav-links">

                <Link to="/">
                    Home
                </Link>

                <Link to="/change-password">
                    Change Password
                </Link>

                <button onClick={handleLogout}>
                    Logout
                </button>
                <CartIcon />

            </div>

        </div>
    );
}