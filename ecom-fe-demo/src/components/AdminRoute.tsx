import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function AdminRoute({ children }: { children: React.ReactNode }) {
    const { isAuthenticated, isAdmin, isInitialized } = useAuth();
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

    if (!isAdmin) {
        return <Navigate to="/" replace />;
    }

    return <>{children}</>;
}
