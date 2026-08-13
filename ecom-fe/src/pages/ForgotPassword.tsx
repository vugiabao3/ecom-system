import { useState } from "react";

import { forgotPassword }
from "../services/authApi";

import { useNavigate }
from "react-router-dom";

import "../styles/auth.css";

export default function ForgotPassword() {

    const [email, setEmail] =
        useState("");

    const navigate = useNavigate();

    const handleForgot = async () => {

        try {

            await forgotPassword({ email });

            alert("Reset token sent");

            // 🔥 CHUYỂN QUA RESET PASSWORD
            navigate("/reset-password");

        } catch {

            alert("Failed");

        }
    };

    return (
        <div className="auth-container">

            <h2>Forgot Password</h2>

            <input
                placeholder="Email"
                onChange={(e) =>
                    setEmail(e.target.value)
                }
            />

            <button onClick={handleForgot}>
                Send Reset Email
            </button>

        </div>
    );
}