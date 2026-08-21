import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { register } from "../services/authApi";
import "../styles/auth.css";

export default function Register() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        if (password !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            await register({
                email: email.trim(),
                password,
            });

            alert("Registration successful! You can now log in.");
            navigate("/login");
        } catch (err: any) {
            setError(err.response?.data?.message || err.response?.data || "Registration failed.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-container">
            <h2>Create Account</h2>

            {error && (
                <div style={{ padding: "10px", background: "#ffe3e3", color: "#e03131", borderRadius: "6px", marginBottom: "12px", fontSize: "14px" }}>
                    {error}
                </div>
            )}

            <form onSubmit={handleRegister}>
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

                <input
                    type="password"
                    placeholder="Confirm Password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                />

                <button type="submit" disabled={loading} style={{ background: "#ee4d2d", borderRadius: "6px", fontWeight: "bold" }}>
                    {loading ? "Registering..." : "Register"}
                </button>
            </form>

            <div className="auth-links" style={{ justifyContent: "center" }}>
                <span>Already have an account? <Link to="/login" style={{ color: "#ee4d2d" }}>Login</Link></span>
            </div>
        </div>
    );
}