import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { resetPassword } from "../services/authApi";
import "../styles/auth.css";

export default function ResetPassword() {
    const navigate = useNavigate();
    const [token, setToken] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleReset = async (e: React.FormEvent) => {
        e.preventDefault();
        if (newPassword !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            await resetPassword({
                token: token.trim(),
                newPassword,
            });

            alert("Password has been reset successfully! Please sign in with your new password.");
            navigate("/login");
        } catch (err: any) {
            setError(err.response?.data?.message || err.response?.data || "Failed to reset password. Invalid or expired token.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-container">
            <h2>Reset Password</h2>

            {error && (
                <div style={{ padding: "10px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px", marginBottom: "12px", fontSize: "14px" }}>
                    {error}
                </div>
            )}

            <form onSubmit={handleReset}>
                <input
                    placeholder="Reset Token"
                    value={token}
                    onChange={(e) => setToken(e.target.value)}
                    required
                />

                <input
                    type="password"
                    placeholder="New Password"
                    value={newPassword}
                    onChange={(e) => setNewPassword(e.target.value)}
                    required
                />

                <input
                    type="password"
                    placeholder="Confirm New Password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                />

                <button type="submit" disabled={loading} style={{ background: "#ee4d2d", borderRadius: "6px", fontWeight: "bold" }}>
                    {loading ? "Resetting..." : "Reset Password"}
                </button>
            </form>

            <div className="auth-links" style={{ justifyContent: "center" }}>
                <Link to="/login" style={{ color: "#666" }}>Back to Login</Link>
            </div>
        </div>
    );
}