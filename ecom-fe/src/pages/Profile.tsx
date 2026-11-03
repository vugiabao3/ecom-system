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

    const [fullName, setFullName] = useState("");
    const [phone, setPhone] = useState("");
    const [loadingProfile, setLoadingProfile] = useState(false);
    const [profileMessage, setProfileMessage] = useState<string | null>(null);

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

    const [activities, setActivities] = useState<any[]>([]);
    const [devices, setDevices] = useState<any[]>([]);

    const loadUserData = useCallback(async () => {
        if (!user?.id) return;
        setLoadingProfile(true);
        try {
            const res = await getUserById(user.id);
            if (res.data) {
                setFullName(res.data.fullName || "");
                setPhone(res.data.phone || "");
            }
        } catch {
        } finally {
            setLoadingProfile(false);
        }
    }, [user?.id]);

    const loadAddresses = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getUserAddresses(user.id);
            setAddresses(Array.isArray(res.data?.addresses) ? res.data.addresses : Array.isArray(res.data) ? res.data : []);
        } catch {
            setAddresses([]);
        }
    }, [user?.id]);

    const loadActivity = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getUserActivity(user.id);
            setActivities(Array.isArray(res.data) ? res.data : []);
        } catch {
            setActivities([]);
        }
    }, [user?.id]);

    const loadDevices = useCallback(async () => {
        if (!user?.id) return;
        try {
            const res = await getUserDevices(user.id);
            setDevices(Array.isArray(res.data) ? res.data : []);
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
            await updateUser(user.id, { fullName, phone });
            setProfileMessage("Profile updated successfully!");
            loadUserData();
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
        <div style={{ background: "#f5f5f5", minHeight: "100vh" }}>
            <Navbar />
            <div className="profile-page-premium">
                <div className="profile-hero">
                    <div className="profile-hero-content">
                        <div className="profile-avatar-large">
                            {(fullName || user?.email || "U").charAt(0).toUpperCase()}
                        </div>
                        <div className="profile-hero-info">
                            <h1 className="profile-hero-name">{fullName || "User Profile"}</h1>
                            <p className="profile-hero-email">{user?.email}</p>
                            <div className="profile-hero-badges">
                                <span className="profile-badge">Role: {user?.role || "User"}</span>
                                <span className="profile-badge">Active Account</span>
                            </div>
                        </div>
                    </div>
                    <Link to="/change-password" className="profile-action-btn">
                        🔒 Change Password
                    </Link>
                </div>

                <div className="profile-layout">
                    <div className="profile-sidebar">
                        <button
                            className={`profile-sidebar-btn ${activeTab === "profile" ? "active" : ""}`}
                            onClick={() => setActiveTab("profile")}
                        >
                            👤 My Profile
                        </button>
                        <button
                            className={`profile-sidebar-btn ${activeTab === "addresses" ? "active" : ""}`}
                            onClick={() => setActiveTab("addresses")}
                        >
                            📍 Addresses
                        </button>
                        <button
                            className={`profile-sidebar-btn ${activeTab === "devices" ? "active" : ""}`}
                            onClick={() => setActiveTab("devices")}
                        >
                            📱 Devices & Security
                        </button>
                        <button
                            className={`profile-sidebar-btn ${activeTab === "activity" ? "active" : ""}`}
                            onClick={() => setActiveTab("activity")}
                        >
                            📊 Account Activity
                        </button>
                    </div>

                    <div className="profile-main">
                        {activeTab === "profile" && (
                            <div className="profile-card-premium">
                                <div className="profile-card-header">
                                    <h2 className="profile-card-title">Personal Information</h2>
                                    <p className="profile-card-subtitle">Manage your personal details</p>
                                </div>

                                {profileMessage && (
                                    <div className={`profile-alert ${profileMessage.includes('success') ? 'success' : 'error'}`}>
                                        {profileMessage}
                                    </div>
                                )}

                                <form onSubmit={handleUpdateProfile} className="profile-form-premium">
                                    <div className="profile-form-row">
                                        <div className="profile-form-group-premium">
                                            <label className="profile-label">Email Address</label>
                                            <input type="email" className="profile-input-premium" value={user?.email || ""} disabled />
                                        </div>
                                        <div className="profile-form-group-premium">
                                            <label className="profile-label">Full Name</label>
                                            <input
                                                type="text"
                                                className="profile-input-premium"
                                                value={fullName}
                                                onChange={(e) => setFullName(e.target.value)}
                                                placeholder="Enter your full name"
                                                required
                                            />
                                        </div>
                                    </div>

                                    <div className="profile-form-group-premium">
                                        <label className="profile-label">Phone Number</label>
                                        <input
                                            type="tel"
                                            className="profile-input-premium"
                                            value={phone}
                                            onChange={(e) => setPhone(e.target.value)}
                                            placeholder="Enter your phone number"
                                        />
                                    </div>

                                    <button type="submit" className="profile-submit-btn" disabled={loadingProfile}>
                                        {loadingProfile ? "Saving..." : "Save Changes"}
                                    </button>
                                </form>

                                {isShipper && (
                                    <>
                                        <div className="profile-divider"></div>
                                        <div className="profile-card-header">
                                            <h2 className="profile-card-title">🚚 Delivery Location</h2>
                                            <p className="profile-card-subtitle">Update your current delivery location</p>
                                        </div>
                                        {locationMessage && (
                                            <div className={`profile-alert ${locationMessage.includes('success') ? 'success' : 'error'}`}>
                                                {locationMessage}
                                            </div>
                                        )}
                                        <form onSubmit={handleUpdateLocation} className="profile-form-premium">
                                            <div className="profile-form-group-premium">
                                                <label className="profile-label">Current Address</label>
                                                <input
                                                    type="text"
                                                    className="profile-input-premium"
                                                    value={currentAddress}
                                                    onChange={(e) => setCurrentAddress(e.target.value)}
                                                    placeholder="Enter your current delivery address"
                                                />
                                            </div>
                                            <div className="profile-form-group-premium">
                                                <label className="profile-label">Current Location (City/Area)</label>
                                                <input
                                                    type="text"
                                                    className="profile-input-premium"
                                                    value={currentLocation}
                                                    onChange={(e) => setCurrentLocation(e.target.value)}
                                                    placeholder="e.g. District 1, Ho Chi Minh City"
                                                />
                                            </div>
                                            <button type="submit" className="profile-submit-btn" disabled={savingLocation}>
                                                {savingLocation ? "Saving..." : "Update Location"}
                                            </button>
                                        </form>
                                    </>
                                )}
                            </div>
                        )}

                        {activeTab === "addresses" && (
                            <div className="profile-card-premium">
                                <div className="profile-card-header">
                                    <h2 className="profile-card-title">Shipping Addresses</h2>
                                    <p className="profile-card-subtitle">Manage your delivery addresses</p>
                                </div>

                                {!Array.isArray(addresses) || addresses.length === 0 ? (
                                    <div className="empty-state-premium">No saved addresses yet</div>
                                ) : (
                                    <div className="addresses-grid">
                                        {addresses.map((a, idx) => (
                                            <div key={a.id || idx} className="address-card">
                                                <div className="address-card-header">
                                                    <strong>{a.fullName}</strong>
                                                    <span className="address-phone">{a.phone}</span>
                                                </div>
                                                <p className="address-details">
                                                    {a.addressLine}, {a.city}, {a.country} {a.postalCode}
                                                </p>
                                            </div>
                                        ))}
                                    </div>
                                )}

                                <div className="profile-divider"></div>
                                <div className="profile-card-header">
                                    <h3 className="profile-card-title">Add New Address</h3>
                                </div>
                                <form onSubmit={handleAddAddress} className="profile-form-premium">
                                    <div className="profile-form-row">
                                        <div className="profile-form-group-premium">
                                            <label className="profile-label">Recipient Name</label>
                                            <input
                                                className="profile-input-premium"
                                                value={newAddress.fullName}
                                                onChange={(e) => setNewAddress({ ...newAddress, fullName: e.target.value })}
                                                required
                                            />
                                        </div>
                                        <div className="profile-form-group-premium">
                                            <label className="profile-label">Phone Number</label>
                                            <input
                                                className="profile-input-premium"
                                                value={newAddress.phone}
                                                onChange={(e) => setNewAddress({ ...newAddress, phone: e.target.value })}
                                                required
                                            />
                                        </div>
                                    </div>
                                    <div className="profile-form-group-premium">
                                        <label className="profile-label">Street Address</label>
                                        <input
                                            className="profile-input-premium"
                                            value={newAddress.addressLine}
                                            onChange={(e) => setNewAddress({ ...newAddress, addressLine: e.target.value })}
                                            required
                                        />
                                    </div>
                                    <div className="profile-form-row">
                                        <div className="profile-form-group-premium">
                                            <label className="profile-label">City</label>
                                            <input
                                                className="profile-input-premium"
                                                value={newAddress.city}
                                                onChange={(e) => setNewAddress({ ...newAddress, city: e.target.value })}
                                                required
                                            />
                                        </div>
                                        <div className="profile-form-group-premium">
                                            <label className="profile-label">Country</label>
                                            <input
                                                className="profile-input-premium"
                                                value={newAddress.country}
                                                onChange={(e) => setNewAddress({ ...newAddress, country: e.target.value })}
                                                required
                                            />
                                        </div>
                                        <div className="profile-form-group-premium">
                                            <label className="profile-label">Postal Code</label>
                                            <input
                                                className="profile-input-premium"
                                                value={newAddress.postalCode}
                                                onChange={(e) => setNewAddress({ ...newAddress, postalCode: e.target.value })}
                                                required
                                            />
                                        </div>
                                    </div>
                                    <button type="submit" className="profile-submit-btn" disabled={addingAddress}>
                                        {addingAddress ? "Adding..." : "Add Address"}
                                    </button>
                                </form>
                            </div>
                        )}

                        {activeTab === "devices" && (
                            <div className="profile-card-premium">
                                <div className="profile-card-header" style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                    <div>
                                        <h2 className="profile-card-title">Devices & Security</h2>
                                        <p className="profile-card-subtitle">Manage your active sessions</p>
                                    </div>
                                    <button onClick={handleLogoutAll} className="danger-btn">
                                        Logout All Devices
                                    </button>
                                </div>
                                {!Array.isArray(devices) || devices.length === 0 ? (
                                    <div className="empty-state-premium">No registered devices</div>
                                ) : (
                                    <div className="activity-list">
                                        {devices.map((d, idx) => (
                                            <div key={d.id || idx} className="activity-item-premium">
                                                <div>
                                                    <div className="activity-action">{d.deviceInfo || d.deviceName || "Web Browser"}</div>
                                                    <div className="activity-details">IP: {d.ipAddress || "Unknown"}</div>
                                                </div>
                                                <div className="activity-time">
                                                    {d.loginAt ? new Date(d.loginAt).toLocaleString() : "Active"}
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                )}
                            </div>
                        )}

                        {activeTab === "activity" && (
                            <div className="profile-card-premium">
                                <div className="profile-card-header">
                                    <h2 className="profile-card-title">Account Activity</h2>
                                    <p className="profile-card-subtitle">Recent account activities</p>
                                </div>
                                {!Array.isArray(activities) || activities.length === 0 ? (
                                    <div className="empty-state-premium">No recent activity</div>
                                ) : (
                                    <div className="activity-list">
                                        {activities.map((act, idx) => (
                                            <div key={act.id || idx} className="activity-item-premium">
                                                <div>
                                                    <div className="activity-action">{act.action || act.activityType || "Activity"}</div>
                                                    <div className="activity-details">{act.description || act.details || ""}</div>
                                                </div>
                                                <div className="activity-time">
                                                    {act.createdAt ? new Date(act.createdAt).toLocaleString() : ""}
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                )}
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}
