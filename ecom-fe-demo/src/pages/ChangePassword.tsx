import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { changePassword } from "../services/authApi";
import Navbar from "../components/Navbar";
import "../styles/auth.css";

export default function ChangePassword() {
    const navigate = useNavigate();
    const [oldPassword, setOldPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [message, setMessage] = useState<string | null>(null);

    const handleChange = async (e: React.FormEvent) => {
        e.preventDefault();
        if (newPassword !== confirmPassword) {
            setError("New passwords do not match.");
            return;
        }

        setLoading(true);
        setError(null);
        setMessage(null);
        try {
            await changePassword({
                oldPassword,
                newPassword,
            });

            setMessage("Password updated successfully!");
            setTimeout(() => {
                navigate("/profile");
            }, 1500);
        } catch (err: any) {
            setError(err.response?.data?.message || err.response?.data || "Failed to change password. Please verify your current password.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <Navbar />
            <div className="auth-container" style={{ marginTop: "40px" }}>
                <h2>Change Password</h2>

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

                <form onSubmit={handleChange}>
                    <input
                        type="password"
                        placeholder="Current Password"
                        value={oldPassword}
                        onChange={(e) => setOldPassword(e.target.value)}
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
                        {loading ? "Updating..." : "Update Password"}
                    </button>
                </form>
            </div>
        </div>
    );
}