import {
    useNavigate,
    Link
} from "react-router-dom";
import CartIcon
from "./CartIcon";
export default function Navbar() {

    const navigate = useNavigate();

    const handleLogout = () => {

        localStorage.removeItem("token");

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