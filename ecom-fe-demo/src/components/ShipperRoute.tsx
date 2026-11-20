import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function ShipperRoute({ children }: { children: React.ReactNode }) {
    const { isAuthenticated, isShipper } = useAuth();

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    if (!isShipper) {
        return <Navigate to="/" replace />;
    }

    return <>{children}</>;
}
