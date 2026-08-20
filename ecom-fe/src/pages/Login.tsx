import { useState } from "react";

import { login } from "../services/authApi";
import { setAuthTokens } from "../utils/token";

import {
    useNavigate,
    Link
} from "react-router-dom";

import "../styles/auth.css";

export default function Login() {

    const navigate = useNavigate();

    const [email, setEmail] = useState("");

    const [password, setPassword] = useState("");

    const handleLogin = async () => {

        try {

            const res = await login({
                email,
                password
            });

            const accessToken = res.data?.accessToken;
            const refreshToken = res.data?.refreshToken;

            if (!accessToken) {
                alert("Login failed");
                return;
            }

            setAuthTokens(accessToken, refreshToken);

            navigate("/");

        } catch {

            alert("Login failed");

        }
    };

    return (
        <div className="auth-container">

            <h2>Login</h2>

            <input
                placeholder="Email"
                onChange={(e) =>
                    setEmail(e.target.value)
                }
            />

            <input
                type="password"
                placeholder="Password"
                onChange={(e) =>
                    setPassword(e.target.value)
                }
            />

            <button onClick={handleLogin}>
                Login
            </button>

            <div className="auth-links">

                <Link to="/register">
                    Register
                </Link>

                <Link to="/forgot-password">
                    Forgot Password?
                </Link>

            </div>

        </div>
    );
}