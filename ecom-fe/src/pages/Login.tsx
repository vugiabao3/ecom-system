import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { login as apiLogin } from "../services/authApi";
import { useAuth } from "../context/AuthContext";
import "../styles/auth.css";

export default function Login() {
    const navigate = useNavigate();
    const { login } = useAuth();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleLogin = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!email.trim() || !password.trim()) {
            setError("Please enter both email and password.");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            const res = await apiLogin({
                email: email.trim(),
                password,
            });

            const accessToken = res.data?.accessToken;
            const refreshToken = res.data?.refreshToken;

            if (!accessToken) {
                setError("Login response missing token.");
                return;
            }

            login(accessToken, refreshToken);
            navigate("/");
        } catch (err: any) {
            setError(err.response?.data?.message || err.response?.data || "Invalid email or password.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-container">
            <h2>Sign In</h2>

            {error && (
                <div style={{ padding: "10px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px", marginBottom: "12px", fontSize: "14px" }}>
                    {error}
                </div>
            )}

            <form onSubmit={handleLogin}>
                <input
                    type="email"
                    placeholder="Email Address"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />

                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />

                <button type="submit" disabled={loading} style={{ background: "#ee4d2d", borderRadius: "6px", fontWeight: "bold" }}>
                    {loading ? "Signing in..." : "Login"}
                </button>
            </form>

            <div className="auth-links">
                <Link to="/register" style={{ color: "#ee4d2d" }}>
                    Create account
                </Link>

                <Link to="/forgot-password" style={{ color: "#666" }}>
                    Forgot Password?
                </Link>
            </div>
        </div>
    );
}