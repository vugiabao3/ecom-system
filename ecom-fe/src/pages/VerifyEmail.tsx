import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { verifyEmail } from "../services/authApi";
import "../styles/auth.css";

export default function VerifyEmail() {
    const navigate = useNavigate();
    const [token, setToken] = useState("");
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState(false);

    const handleVerify = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!token.trim()) {
            setError("Please enter verification token.");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            await verifyEmail({ token: token.trim() });
            setSuccess(true);
        } catch (err: any) {
            setError(err.response?.data?.message || err.response?.data || "Verification failed.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="auth-page">
            <div className="auth-hero">
                <div className="auth-hero-content">
                    <div className="auth-logo">EcomSystem</div>
                    <h1 className="auth-hero-title">Secure Your Account</h1>
                    <p className="auth-hero-subtitle">
                        Verify your email to unlock all features of the marketplace.
                    </p>
                </div>
            </div>
            <div className="auth-form-section">
                <div className="auth-card">
                    <div className="auth-card-header">
                        <h2 className="auth-card-title">Verify Email</h2>
                        <p className="auth-card-subtitle">Enter the verification token sent to your email</p>
                    </div>

                    {error && <div className="auth-alert auth-alert-error">{error}</div>}
                    {success && (
                        <div className="auth-alert auth-alert-success">
                            Email verified successfully! You can now sign in.
                        </div>
                    )}

                    {!success ? (
                        <form onSubmit={handleVerify} className="auth-form">
                            <div className="auth-form-group">
                                <label className="auth-label">Verification Token</label>
                                <input
                                    type="text"
                                    className="auth-input"
                                    placeholder="Enter your verification token"
                                    value={token}
                                    onChange={(e) => setToken(e.target.value)}
                                    required
                                />
                            </div>

                            <button type="submit" disabled={loading} className="auth-button auth-button-primary">
                                {loading ? "Verifying..." : "Verify Email"}
                            </button>
                        </form>
                    ) : (
                        <button onClick={() => navigate("/login")} className="auth-button auth-button-primary">
                            Go to Login
                        </button>
                    )}

                    <div className="auth-divider">
                        <span>or</span>
                    </div>

                    <div className="auth-footer-links">
                        <Link to="/login" className="auth-link">Back to Login</Link>
                        <Link to="/resend-verification" className="auth-link auth-link-accent">Resend Verification</Link>
                    </div>
                </div>
            </div>
        </div>
    );
}
