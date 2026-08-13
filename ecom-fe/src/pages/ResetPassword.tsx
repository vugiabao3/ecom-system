import { useState } from "react";

import { resetPassword }
from "../services/authApi";

import {
    useNavigate
} from "react-router-dom";

import "../styles/auth.css";

export default function ResetPassword() {

    const navigate = useNavigate();

    const [token, setToken] =
        useState("");

    const [newPassword, setNewPassword] =
        useState("");

    const handleReset = async () => {

        try {

            const payload = {
                token,
                newPassword
            };

            console.log(payload);

            const res =
                await resetPassword(payload);

            console.log(res);

            alert("Password reset success");

            // 🔥 CHUYỂN VỀ LOGIN
            navigate("/login");

        } catch (err: any) {

            console.log(err);

            console.log(err.response);

            console.log(err.response.data);

            alert("Reset failed");

        }
    };

    return (
        <div className="auth-container">

            <h2>Reset Password</h2>

            <input
                placeholder="Reset Token"
                onChange={(e) =>
                    setToken(e.target.value)
                }
            />

            <input
                type="password"
                placeholder="New Password"
                onChange={(e) =>
                    setNewPassword(e.target.value)
                }
            />

            <button onClick={handleReset}>
                Reset Password
            </button>

        </div>
    );
}