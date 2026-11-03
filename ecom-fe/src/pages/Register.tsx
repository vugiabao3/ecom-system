import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { register } from "../services/authApi";
import "../styles/auth.css";

export default function Register() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!email.trim() || !password.trim() || !confirmPassword.trim()) {
            setError("Please fill in all fields.");
            return;
        }
        if (password !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }
        if (password.length < 6) {
            setError("Password must be at least 6 characters long.");
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
        <div className="auth-page">
            <div className="auth-hero">
                <div className="auth-hero-content">
                    <div className="auth-logo">EcomSystem</div>
                    <h1 className="auth-hero-title">Join Us Today</h1>
                    <p className="auth-hero-subtitle">
                        Create your account to start shopping, selling, and enjoying the best deals on our marketplace.
                    </p>
                </div>
            </div>
            <div className="auth-form-section">
                <div className="auth-card">
                    <div className="auth-card-header">
                        <h2 className="auth-card-title">Create Account</h2>
                        <p className="auth-card-subtitle">Get started with your free account</p>
                    </div>

                    {error && <div className="auth-alert auth-alert-error">{error}</div>}

                    <form onSubmit={handleRegister} className="auth-form">
                        <div className="auth-form-group">
                            <label className="auth-label">Email Address</label>
                            <input
                                type="email"
                                className="auth-input"
                                placeholder="Enter your email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                            />
                        </div>

                        <div className="auth-form-group">
                            <label className="auth-label">Password</label>
                            <div className="password-toggle">
                                <input
                                    type={showPassword ? "text" : "password"}
                                    className="auth-input"
                                    placeholder="Create a password (min 6 chars)"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                    style={{ paddingRight: "80px" }}
                                />
                                <button
                                    type="button"
                                    className="password-toggle-btn"
                                    onClick={() => setShowPassword(!showPassword)}
                                >
                                    {showPassword ? "Hide" : "Show"}
                                </button>
                            </div>
                        </div>

                        <div className="auth-form-group">
                            <label className="auth-label">Confirm Password</label>
                            <input
                                type={showPassword ? "text" : "password"}
                                className="auth-input"
                                placeholder="Confirm your password"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                                required
                            />
                        </div>

                        <button type="submit" disabled={loading} className="auth-button auth-button-primary">
                            {loading ? "Creating Account..." : "Create Account"}
                        </button>
                    </form>

                    <div className="auth-divider">
                        <span>or</span>
                    </div>

                    <div className="auth-footer-links" style={{ justifyContent: "center" }}>
                        <span style={{ color: "#666", fontSize: "14px" }}>
                            Already have an account?{" "}
                            <Link to="/login" className="auth-link auth-link-accent">
                                Sign In
                            </Link>
                        </span>
                    </div>
                </div>
            </div>
        </div>
    );
}
