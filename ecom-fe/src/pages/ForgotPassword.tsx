import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { forgotPassword } from "../services/authApi";
import "../styles/auth.css";

export default function ForgotPassword() {
    const navigate = useNavigate();
    const [email, setEmail] = useState("");
    const [loading, setLoading] = useState(false);
    const [message, setMessage] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);

    const handleForgot = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);
        setMessage(null);
        try {
            await forgotPassword({ email: email.trim() });
            setMessage("Password reset token sent to your email. Please check your inbox.");
            setTimeout(() => {
                navigate("/reset-password");
            }, 2000);
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to request password reset.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-container">
            <h2>Forgot Password</h2>

            {error && (
                <div style={{ padding: "10px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px", marginBottom: "12px", fontSize: "14px" }}>
                    {error}
                </div>
            )}

            {message && (
                <div style={{ padding: "10px", background: "#e6fcf5", color: "#0ca678", borderRadius: "6px", marginBottom: "12px", fontSize: "14px" }}>
                    {message}
                </div>
            )}

            <form onSubmit={handleForgot}>
                <input
                    type="email"
                    placeholder="Enter your registered email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />

                <button type="submit" disabled={loading} style={{ background: "#ee4d2d", borderRadius: "6px", fontWeight: "bold" }}>
                    {loading ? "Sending..." : "Send Reset Code"}
                </button>
            </form>

            <div className="auth-links" style={{ justifyContent: "space-between" }}>
                <Link to="/login" style={{ color: "#666" }}>Back to Login</Link>
                <Link to="/reset-password" style={{ color: "#ee4d2d" }}>Have a token? Reset</Link>
            </div>
        </div>
    );
}