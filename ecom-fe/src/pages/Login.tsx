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
    const [showPassword, setShowPassword] = useState(false);
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
        <div className="auth-page">
            <div className="auth-hero">
                <div className="auth-hero-content">
                    <div className="auth-logo">EcomSystem</div>
                    <h1 className="auth-hero-title">Welcome Back</h1>
                    <p className="auth-hero-subtitle">
                        Sign in to access your account, track orders, and shop the best deals on our marketplace.
                    </p>
                </div>
            </div>
            <div className="auth-form-section">
                <div className="auth-card">
                    <div className="auth-card-header">
                        <h2 className="auth-card-title">Sign In</h2>
                        <p className="auth-card-subtitle">Enter your credentials to continue</p>
                    </div>

                    {error && <div className="auth-alert auth-alert-error">{error}</div>}

                    <form onSubmit={handleLogin} className="auth-form">
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
                                    placeholder="Enter your password"
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

                        <button type="submit" disabled={loading} className="auth-button auth-button-primary">
                            {loading ? "Signing in..." : "Sign In"}
                        </button>
                    </form>

                    <div className="auth-divider">
                        <span>or</span>
                    </div>

                    <div className="auth-footer-links">
                        <Link to="/register" className="auth-link auth-link-accent">Create Account</Link>
                        <Link to="/forgot-password" className="auth-link">Forgot Password?</Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
