import { useState } from "react";
import { Link } from "react-router-dom";
import { forgotPassword } from "../services/authApi";
import "../styles/auth.css";

export default function ForgotPassword() {
    const [email, setEmail] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!email.trim()) {
            setError("Please enter your email.");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            await forgotPassword({ email: email.trim() });
            setSuccess(true);
        } catch (err: any) {
            setError(err.response?.data?.message || err.response?.data || "Failed to send reset email.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-page">
            <div className="auth-hero">
                <div className="auth-hero-content">
                    <div className="auth-logo">EcomSystem</div>
                    <h1 className="auth-hero-title">Reset Password</h1>
                    <p className="auth-hero-subtitle">
                        Enter your email address and we'll send you a secure reset token.
                    </p>
                </div>
            </div>
            <div className="auth-form-section">
                <div className="auth-card">
                    <div className="auth-card-header">
                        <h2 className="auth-card-title">Forgot Password</h2>
                        <p className="auth-card-subtitle">We'll send you a reset token</p>
                    </div>

                    {error && <div className="auth-alert auth-alert-error">{error}</div>}
                    {success && (
                        <div className="auth-alert auth-alert-success">
                            If the email exists, a password reset token has been sent.
                        </div>
                    )}

                    {!success ? (
                        <form onSubmit={handleSubmit} className="auth-form">
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

                            <button type="submit" disabled={loading} className="auth-button auth-button-primary">
                                {loading ? "Sending..." : "Send Reset Token"}
                            </button>
                        </form>
                    ) : (
                        <Link to="/reset-password" className="auth-button auth-button-primary" style={{ textDecoration: "none", display: "block", textAlign: "center" }}>
                            Enter Reset Token
                        </Link>
                    )}

                    <div className="auth-divider">
                        <span>or</span>
                    </div>

                    <div className="auth-footer-links">
                        <Link to="/login" className="auth-link">Back to Login</Link>
                        <Link to="/register" className="auth-link auth-link-accent">Create Account</Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
