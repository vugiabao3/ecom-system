import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function ProtectedRoute({ children }: { children: React.ReactNode }) {
    const { isAuthenticated, isInitialized } = useAuth();
    const location = useLocation();

    if (!isInitialized) {
        return (
            <div style={{ minHeight: "60vh", display: "flex", alignItems: "center", justifyContent: "center", color: "#666" }}>
                <div style={{ textAlign: "center" }}>
                    <div style={{ fontSize: "18px", fontWeight: 600 }}>Loading your session…</div>
                </div>
            </div>
        );
    }

    if (!isAuthenticated) {
        return (
            <Navigate
                to="/login"
                replace
                state={{ from: `${location.pathname}${location.search}${location.hash}` }}
            />
        );
    }

    return <>{children}</>;
}
