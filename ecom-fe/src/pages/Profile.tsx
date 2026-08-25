import { useEffect, useState, useCallback } from "react";
import { useAuth } from "../context/AuthContext";
import {
    getUserById,
    updateUser,
    getUserAddresses,
    addAddress,
    getUserActivity,
    getUserDevices,
    logoutAllDevices,
    type UserAddressDto,
    type CreateAddressRequest
} from "../services/userApi";
import Navbar from "../components/Navbar";
import { Link } from "react-router-dom";
import "../styles/profile.css";

export default function Profile() {
    const { user, logout, isShipper } = useAuth();
    const [activeTab, setActiveTab] = useState<"profile" | "addresses" | "activity" | "devices">("profile");

    const [currentAddress, setCurrentAddress] = useState("");
    const [currentLocation, setCurrentLocation] = useState("");
    const [savingLocation, setSavingLocation] = useState(false);
    const [locationMessage, setLocationMessage] = useState<string | null>(null);

    // Profile state
    const [fullName, setFullName] = useState("");
    const [loadingProfile, setLoadingProfile] = useState(false);
    const [profileMessage, setProfileMessage] = useState<string | null>(null);

    // Addresses state
    const [addresses, setAddresses] = useState<UserAddressDto[]>([]);
    const [newAddress, setNewAddress] = useState<CreateAddressRequest>({
        fullName: "",
        phone: "",
        addressLine: "",
        city: "",
        country: "Vietnam",
        postalCode: "70000",
    });
    const [addingAddress, setAddingAddress] = useState(false);

    // Activity state
    const [activities, setActivities] = useState<any[]>([]);

    // Devices state
    const [devices, setDevices] = useState<any[]>([]);

    const loadUserData = useCallback(async () => {
        if (!user?.id) return;
        setLoadingProfile(true);
        try {
            const res = await getUserById(user.id);
            if (res.data) {
                setFullName(res.data.fullName || "");
            }
        } catch {
            // Ignore error
        } finally {
            setLoadingProfile(false);
        }
    }, [user?.id]);

    const loadAddresses = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getUserAddresses(user.id);
            setAddresses(res.data?.addresses || []);
        } catch {
            setAddresses([]);
        }
    }, [user?.id]);

    const loadActivity = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getUserActivity(user.id);
            setActivities(res.data || []);
        } catch {
            setActivities([]);
        }
    }, [user?.id]);

    const loadDevices = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getUserDevices(user.id);
            setDevices(res.data || []);
        } catch {
            setDevices([]);
        }
    }, [user?.id]);

    const loadLocationData = useCallback(async () => {
        if (!user?.id || !isShipper) return;
        try {
            const res = await getUserById(user.id);
            if (res.data) {
                setCurrentAddress(res.data.currentAddress || "");
                setCurrentLocation(res.data.currentLocation || "");
            }
        } catch {
            // Ignore
        }
    }, [user?.id, isShipper]);

    useEffect(() => {
        loadUserData();
        if (isShipper) {
            loadLocationData();
        }
    }, [loadUserData, loadLocationData, isShipper]);

    useEffect(() => {
        if (activeTab === "addresses") loadAddresses();
        if (activeTab === "activity") loadActivity();
        if (activeTab === "devices") loadDevices();
    }, [activeTab, loadAddresses, loadActivity, loadDevices]);

    const handleUpdateProfile = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!user?.id) return;
        setProfileMessage(null);
        try {
            await updateUser(user.id, { fullName });
            setProfileMessage("Profile updated successfully!");
        } catch {
            setProfileMessage("Failed to update profile.");
        }
    };

    const handleUpdateLocation = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!user?.id) return;
        setLocationMessage(null);
        setSavingLocation(true);
        try {
            await updateUser(user.id, {
                currentAddress,
                currentLocation,
            });
            setLocationMessage("Delivery location updated successfully!");
        } catch {
            setLocationMessage("Failed to update location.");
        } finally {
            setSavingLocation(false);
        }
    };

    const handleAddAddress = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!user?.id) return;
        setAddingAddress(true);
        try {
            await addAddress(user.id, newAddress);
            alert("Address added successfully!");
            setNewAddress({
                fullName: "",
                phone: "",
                addressLine: "",
                city: "",
                country: "Vietnam",
                postalCode: "70000",
            });
            await loadAddresses();
        } catch {
            alert("Failed to add address.");
        } finally {
            setAddingAddress(false);
        }
    };

    const handleLogoutAll = async () => {
        if (!user?.id) return;
        if (!window.confirm("Are you sure you want to log out from all devices?")) return;
        try {
            await logoutAllDevices(user.id);
            alert("Logged out from all devices.");
            await logout();
        } catch {
            alert("Failed to logout all devices.");
        }
    };

    return (
        <div>
            <Navbar />
            <div className="profile-page">
                <div className="profile-header">
                    <div style={{ display: "flex", alignItems: "center", gap: "16px" }}>
                        <div className="profile-avatar-circle">
                            {(fullName || user?.email || "U").charAt(0).toUpperCase()}
                        </div>
                        <div>
                            <h2 style={{ margin: "0 0 4px", fontSize: "20px" }}>{fullName || "User Profile"}</h2>
                            <p style={{ margin: 0, color: "#666", fontSize: "14px" }}>{user?.email}</p>
                            <span
                                style={{
                                    display: "inline-block",
                                    marginTop: "4px",
                                    fontSize: "12px",
                                    background: "#e7f5ff",
                                    color: "#1971c2",
                                    padding: "2px 8px",
                                    borderRadius: "4px",
                                    fontWeight: "600",
                                }}
                            >
                                Role: {user?.role || "User"}
                            </span>
                        </div>
                    </div>

                    <Link
                        to="/change-password"
                        style={{
                            padding: "8px 16px",
                            border: "1px solid #ddd",
                            borderRadius: "6px",
                            color: "#444",
                            textDecoration: "none",
                            fontSize: "14px",
                            fontWeight: "600",
                        }}
                    >
                        🔒 Change Password
                    </Link>
                </div>

                <div className="profile-tabs">
                    <button
                        className={`profile-tab-btn ${activeTab === "profile" ? "active" : ""}`}
                        onClick={() => setActiveTab("profile")}
                    >
                        Account Info
                    </button>
                    <button
                        className={`profile-tab-btn ${activeTab === "addresses" ? "active" : ""}`}
                        onClick={() => setActiveTab("addresses")}
                    >
                        Saved Addresses
                    </button>
                    <button
                        className={`profile-tab-btn ${activeTab === "devices" ? "active" : ""}`}
                        onClick={() => setActiveTab("devices")}
                    >
                        Login Devices
                    </button>
                    <button
                        className={`profile-tab-btn ${activeTab === "activity" ? "active" : ""}`}
                        onClick={() => setActiveTab("activity")}
                    >
                        Recent Activity
                    </button>
                </div>

                {/* TAB 1: PROFILE INFO */}
                {activeTab === "profile" && (
                    <div className="profile-card">
                        <h3 style={{ marginBottom: "16px", color: "#333" }}>Personal Details</h3>

                        {profileMessage && (
                            <div
                                style={{
                                    padding: "10px",
                                    background: "#e6fcf5",
                                    color: "#0ca678",
                                    borderRadius: "6px",
                                    marginBottom: "16px",
                                    fontSize: "14px",
                                }}
                            >
                                {profileMessage}
                            </div>
                        )}

                        <form onSubmit={handleUpdateProfile}>
                            <div className="profile-form-group">
                                <label>Email Address</label>
                                <input type="email" value={user?.email || ""} disabled style={{ background: "#f8f9fa" }} />
                            </div>

                            <div className="profile-form-group">
                                <label>Full Name</label>
                                <input
                                    type="text"
                                    value={fullName}
                                    onChange={(e) => setFullName(e.target.value)}
                                    placeholder="Enter your full name"
                                    required
                                />
                            </div>

                            <button type="submit" className="profile-btn" disabled={loadingProfile}>
                                {loadingProfile ? "Saving..." : "Save Changes"}
                            </button>
                        </form>

                        {isShipper && (
                            <hr style={{ border: "none", borderTop: "1px solid #eee", margin: "24px 0" }} />
                        )}

                        {isShipper && (
                            <div>
                                <h3 style={{ marginBottom: "16px", color: "#333" }}>🚚 Delivery Location</h3>
                                {locationMessage && (
                                    <div
                                        style={{
                                            padding: "10px",
                                            background: "#e6fcf5",
                                            color: "#0ca678",
                                            borderRadius: "6px",
                                            marginBottom: "16px",
                                            fontSize: "14px",
                                        }}
                                    >
                                        {locationMessage}
                                    </div>
                                )}
                                <form onSubmit={handleUpdateLocation}>
                                    <div className="profile-form-group">
                                        <label>Current Address</label>
                                        <input
                                            type="text"
                                            value={currentAddress}
                                            onChange={(e) => setCurrentAddress(e.target.value)}
                                            placeholder="Enter your current delivery address"
                                        />
                                    </div>
                                    <div className="profile-form-group">
                                        <label>Current Location (City/Area)</label>
                                        <input
                                            type="text"
                                            value={currentLocation}
                                            onChange={(e) => setCurrentLocation(e.target.value)}
                                            placeholder="e.g. District 1, Ho Chi Minh City"
                                        />
                                    </div>
                                    <button type="submit" className="profile-btn" disabled={savingLocation}>
                                        {savingLocation ? "Saving..." : "Update Delivery Location"}
                                    </button>
                                </form>
                            </div>
                        )}
                    </div>
                )}

                {/* TAB 2: ADDRESSES */}
                {activeTab === "addresses" && (
                    <div className="profile-card">
                        <h3 style={{ marginBottom: "16px", color: "#333" }}>Shipping Addresses</h3>

                        {addresses.length === 0 ? (
                            <p style={{ color: "#888", marginBottom: "20px" }}>No saved addresses yet.</p>
                        ) : (
                            <div style={{ marginBottom: "28px" }}>
                                {addresses.map((a, idx) => (
                                    <div key={a.id || idx} className="address-item-card">
                                        <h4 style={{ margin: "0 0 4px" }}>
                                            {a.fullName} <span style={{ color: "#888", fontWeight: "normal" }}>({a.phone})</span>
                                        </h4>
                                        <p style={{ margin: 0, color: "#555", fontSize: "14px" }}>
                                            {a.addressLine}, {a.city}, {a.country} {a.postalCode}
                                        </p>
                                    </div>
                                ))}
                            </div>
                        )}

                        <hr style={{ border: "none", borderTop: "1px solid #eee", margin: "20px 0" }} />

                        <h4 style={{ marginBottom: "12px", color: "#444" }}>+ Add New Address</h4>
                        <form onSubmit={handleAddAddress}>
                            <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "12px" }}>
                                <div className="profile-form-group">
                                    <label>Recipient Name</label>
                                    <input
                                        value={newAddress.fullName}
                                        onChange={(e) => setNewAddress({ ...newAddress, fullName: e.target.value })}
                                        placeholder="Full Name"
                                        required
                                    />
                                </div>
                                <div className="profile-form-group">
                                    <label>Phone Number</label>
                                    <input
                                        value={newAddress.phone}
                                        onChange={(e) => setNewAddress({ ...newAddress, phone: e.target.value })}
                                        placeholder="Phone"
                                        required
                                    />
                                </div>
                            </div>

                            <div className="profile-form-group">
                                <label>Street Address</label>
                                <input
                                    value={newAddress.addressLine}
                                    onChange={(e) => setNewAddress({ ...newAddress, addressLine: e.target.value })}
                                    placeholder="Street, Building, Unit"
                                    required
                                />
                            </div>

                            <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr 1fr", gap: "12px" }}>
                                <div className="profile-form-group">
                                    <label>City</label>
                                    <input
                                        value={newAddress.city}
                                        onChange={(e) => setNewAddress({ ...newAddress, city: e.target.value })}
                                        placeholder="City"
                                        required
                                    />
                                </div>
                                <div className="profile-form-group">
                                    <label>Country</label>
                                    <input
                                        value={newAddress.country}
                                        onChange={(e) => setNewAddress({ ...newAddress, country: e.target.value })}
                                        placeholder="Country"
                                        required
                                    />
                                </div>
                                <div className="profile-form-group">
                                    <label>Postal Code</label>
                                    <input
                                        value={newAddress.postalCode}
                                        onChange={(e) => setNewAddress({ ...newAddress, postalCode: e.target.value })}
                                        placeholder="Postal Code"
                                        required
                                    />
                                </div>
                            </div>

                            <button type="submit" className="profile-btn" disabled={addingAddress}>
                                {addingAddress ? "Adding..." : "Add Address"}
                            </button>
                        </form>
                    </div>
                )}

                {/* TAB 3: DEVICES */}
                {activeTab === "devices" && (
                    <div className="profile-card">
                        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: "16px" }}>
                            <h3 style={{ margin: 0, color: "#333" }}>Registered Devices & Sessions</h3>
                            <button
                                onClick={handleLogoutAll}
                                style={{
                                    padding: "8px 14px",
                                    background: "#e03131",
                                    color: "white",
                                    border: "none",
                                    borderRadius: "6px",
                                    fontWeight: "600",
                                    fontSize: "13px",
                                    cursor: "pointer",
                                }}
                            >
                                🚪 Logout All Devices
                            </button>
                        </div>

                        {devices.length === 0 ? (
                            <p style={{ color: "#888" }}>No specific devices registered.</p>
                        ) : (
                            <div>
                                {devices.map((d, idx) => (
                                    <div key={d.id || idx} className="activity-item">
                                        <div>
                                            <strong>{d.deviceName || "Web Browser"}</strong>
                                            <div style={{ fontSize: "12px", color: "#888" }}>IP: {d.ipAddress || "Unknown"}</div>
                                        </div>
                                        <span style={{ fontSize: "12px", color: "#666" }}>
                                            {d.lastActive ? new Date(d.lastActive).toLocaleString() : "Active"}
                                        </span>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                )}

                {/* TAB 4: ACTIVITY */}
                {activeTab === "activity" && (
                    <div className="profile-card">
                        <h3 style={{ marginBottom: "16px", color: "#333" }}>Account Activity Log</h3>
                        {activities.length === 0 ? (
                            <p style={{ color: "#888" }}>No recent activity records available.</p>
                        ) : (
                            <div>
                                {activities.map((act, idx) => (
                                    <div key={act.id || idx} className="activity-item">
                                        <div>
                                            <p style={{ margin: "0 0 2px", fontWeight: "600" }}>{act.action || act.activityType}</p>
                                            <span style={{ fontSize: "12px", color: "#888" }}>{act.details || "Activity performed"}</span>
                                        </div>
                                        <span style={{ fontSize: "12px", color: "#888" }}>
                                            {act.createdAt ? new Date(act.createdAt).toLocaleString() : ""}
                                        </span>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}
