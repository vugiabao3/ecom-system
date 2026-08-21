import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import {
    getAllUsers,
    searchUsers,
    blockUser,
    unblockUser,
    softDeleteUser,
    restoreUser
} from "../../services/userApi";
import { adminSetRole, adminSetActive } from "../../services/authApi";
import Navbar from "../../components/Navbar";
import "../../styles/admin.css";

export default function AdminUsers() {
    const [users, setUsers] = useState<any[]>([]);
    const [totalCount, setTotalCount] = useState<number>(0);
    const [page, setPage] = useState<number>(1);
    const [pageSize] = useState<number>(10);
    const [keyword, setKeyword] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(true);

    const loadUsers = useCallback(async () => {
        setLoading(true);
        try {
            if (keyword.trim()) {
                const res = await searchUsers(keyword.trim(), page, pageSize);
                setUsers(res.data?.users || res.data || []);
                setTotalCount(res.data?.totalCount || 0);
            } else {
                const res = await getAllUsers(page, pageSize);
                setUsers(res.data?.users || res.data || []);
                setTotalCount(res.data?.totalCount || 0);
            }
        } catch {
            setUsers([]);
        } finally {
            setLoading(false);
        }
    }, [page, pageSize, keyword]);

    useEffect(() => {
        loadUsers();
    }, [loadUsers]);

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        setPage(1);
        loadUsers();
    };

    const handleBlock = async (id: string) => {
        try {
            await blockUser(id);
            alert("User blocked!");
            loadUsers();
        } catch {
            alert("Failed to block user.");
        }
    };

    const handleUnblock = async (id: string) => {
        try {
            await unblockUser(id);
            alert("User unblocked!");
            loadUsers();
        } catch {
            alert("Failed to unblock user.");
        }
    };

    const handleDelete = async (id: string) => {
        if (!window.confirm("Soft delete this user account?")) return;
        try {
            await softDeleteUser(id);
            alert("User deleted!");
            loadUsers();
        } catch {
            alert("Failed to delete user.");
        }
    };

    const handleRestore = async (id: string) => {
        try {
            await restoreUser(id);
            alert("User restored!");
            loadUsers();
        } catch {
            alert("Failed to restore user.");
        }
    };

    const handleToggleRole = async (id: string, currentRole: string) => {
        const newRole = currentRole === "Admin" ? "User" : "Admin";
        if (!window.confirm(`Change role to ${newRole}?`)) return;
        try {
            await adminSetRole(id, newRole);
            alert(`Role changed to ${newRole}!`);
            loadUsers();
        } catch {
            alert("Failed to change user role.");
        }
    };

    const handleToggleActive = async (id: string, currentStatus: string) => {
        const newStatus = currentStatus === "Active" ? "Inactive" : "Active";
        try {
            await adminSetActive(id, newStatus);
            alert(`Status updated to ${newStatus}!`);
            loadUsers();
        } catch {
            alert("Failed to update status.");
        }
    };

    return (
        <div>
            <Navbar />
            <div className="admin-container">
                <div className="admin-header">
                    <div>
                        <h1 style={{ fontSize: "24px", color: "#222" }}>👥 User Account Management</h1>
                        <p style={{ color: "#666" }}>Monitor users, manage roles, block access, and activate accounts.</p>
                    </div>
                </div>

                <div className="admin-nav">
                    <Link to="/admin" className="admin-nav-item">Dashboard</Link>
                    <Link to="/admin/products" className="admin-nav-item">Products & Stock</Link>
                    <Link to="/admin/categories" className="admin-nav-item">Categories</Link>
                    <Link to="/admin/users" className="admin-nav-item active">Users & Roles</Link>
                    <Link to="/admin/promotions" className="admin-nav-item">Promotions</Link>
                    <Link to="/admin/shipping" className="admin-nav-item">Shipping & Orders</Link>
                </div>

                {/* Search Box */}
                <div className="admin-card">
                    <form onSubmit={handleSearch} style={{ display: "flex", gap: "10px", maxWidth: "500px" }}>
                        <input
                            type="text"
                            placeholder="Search by email or name..."
                            value={keyword}
                            onChange={(e) => setKeyword(e.target.value)}
                            style={{ flex: 1, padding: "8px 12px", borderRadius: "6px", border: "1px solid #ddd" }}
                        />
                        <button type="submit" className="admin-btn primary">Search</button>
                    </form>
                </div>

                {/* Users List */}
                <div className="admin-card">
                    <h3>All Users ({totalCount || users.length})</h3>
                    {loading ? (
                        <p style={{ padding: "30px", textAlign: "center" }}>Loading users...</p>
                    ) : users.length === 0 ? (
                        <p style={{ padding: "30px", textAlign: "center", color: "#888" }}>No users found.</p>
                    ) : (
                        <div style={{ overflowX: "auto" }}>
                            <table className="admin-table">
                                <thead>
                                    <tr>
                                        <th>Email</th>
                                        <th>Role</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {users.map((u: any) => (
                                        <tr key={u.id}>
                                            <td style={{ fontWeight: "600" }}>{u.email}</td>
                                            <td>
                                                <span className={`admin-badge ${u.role === "Admin" ? "danger" : "info"}`}>
                                                    {u.role || "User"}
                                                </span>
                                            </td>
                                            <td>
                                                <button
                                                    className="admin-btn primary"
                                                    onClick={() => handleToggleRole(u.id, u.role)}
                                                >
                                                    {u.role === "Admin" ? "Demote to User" : "Promote to Admin"}
                                                </button>

                                                <button
                                                    className="admin-btn warning"
                                                    onClick={() => handleBlock(u.id)}
                                                >
                                                    Block
                                                </button>

                                                <button
                                                    className="admin-btn success"
                                                    onClick={() => handleUnblock(u.id)}
                                                >
                                                    Unblock
                                                </button>

                                                <button
                                                    className="admin-btn secondary"
                                                    onClick={() => handleToggleActive(u.id, u.status || "Active")}
                                                >
                                                    Set Status
                                                </button>

                                                <button
                                                    className="admin-btn danger"
                                                    onClick={() => handleDelete(u.id)}
                                                >
                                                    Delete
                                                </button>

                                                <button
                                                    className="admin-btn secondary"
                                                    onClick={() => handleRestore(u.id)}
                                                >
                                                    Restore
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
