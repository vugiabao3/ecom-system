import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function SellerRoute({ children }: { children: React.ReactNode }) {
    const { isAuthenticated, isSeller, isInitialized } = useAuth();
    const location = useLocation();

    if (!isInitialized) {
        return <div style={{ padding: "80px", textAlign: "center", color: "#666" }}>Loading your session…</div>;
    }

    if (!isAuthenticated) {
        return (
            <Navigate
                to="/login"
                replace
                state={{ from: `${location.pathname}${location.search}` }}
            />
        );
    }

    if (!isSeller) {
        return <Navigate to="/" replace />;
    }

    return <>{children}</>;
}
