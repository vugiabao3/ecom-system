import { Navigate } from "react-router-dom";
import { getToken } from "../utils/token";

export default function ProtectedRoute({ children }: { children: React.ReactNode }) {
    const token = getToken();

    if (!token) {
        return <Navigate to="/login" replace />;
    }

    return <>{children}</>;
}