import { useState } from "react";

import { changePassword } from "../services/authApi";

import "../styles/auth.css";

export default function ChangePassword() {

    const [email, setEmail] = useState("");

    const [oldPassword, setOldPassword] =
        useState("");

    const [newPassword, setNewPassword] =
        useState("");

    const handleChange = async () => {

        try {

            const payload = {
                email,
                oldPassword,
                newPassword
            };

            console.log(payload);

            const res =
                await changePassword(payload);

            console.log(res);

            alert("Password changed");

        } catch (err: any) {

            console.log(err);

            console.log(err.response);

            console.log(err.response.data);

            alert("Failed");

        }
    };

    return (
        <div className="auth-container">

            <h2>Change Password</h2>

            <input
                placeholder="Email"
                onChange={(e) =>
                    setEmail(e.target.value)
                }
            />

            <input
                type="password"
                placeholder="Old Password"
                onChange={(e) =>
                    setOldPassword(e.target.value)
                }
            />

            <input
                type="password"
                placeholder="New Password"
                onChange={(e) =>
                    setNewPassword(e.target.value)
                }
            />

            <button onClick={handleChange}>
                Change Password
            </button>

        </div>
    );
}