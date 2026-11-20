import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function SellerRoute({ children }: { children: React.ReactNode }) {
    const { isAuthenticated, isSeller } = useAuth();

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />;
    }

    if (!isSeller) {
        return <Navigate to="/" replace />;
    }

    return <>{children}</>;
}
