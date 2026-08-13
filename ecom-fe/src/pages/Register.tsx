import { useState } from "react";

import { register } from "../services/authApi";

import {
    useNavigate,
    Link
} from "react-router-dom";

import "../styles/auth.css";

export default function Register() {

    const navigate = useNavigate();

    const [email, setEmail] = useState("");

    const [password, setPassword] = useState("");

    const handleRegister = async () => {

        try {

            await register({
                email,
                password
            });

            alert("Register success");

            navigate("/login");

        } catch {

            alert("Register failed");

        }
    };

    return (
        <div className="auth-container">

            <h2>Register</h2>

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

            <button onClick={handleRegister}>
                Register
            </button>

            <div className="auth-links">

                <Link to="/login">
                    Login
                </Link>

            </div>

        </div>
    );
}