import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import {
    getCategories,
    createCategory,
    updateCategory,
    deleteCategory,
    type CategoryDto
} from "../../services/categoryApi";
import Navbar from "../../components/Navbar";
import "../../styles/admin.css";

export default function AdminCategories() {
    const [categories, setCategories] = useState<CategoryDto[]>([]);
    const [loading, setLoading] = useState(true);

    const [newCategoryName, setNewCategoryName] = useState("");
    const [editingCategory, setEditingCategory] = useState<CategoryDto | null>(null);
    const [editName, setEditName] = useState("");

    const loadCategories = useCallback(async () => {
        setLoading(true);
        try {
            const res = await getCategories();
            setCategories(res.data || []);
        } catch {
            setCategories([]);
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        loadCategories();
    }, [loadCategories]);

    const handleCreate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!newCategoryName.trim()) return;
        try {
            await createCategory({ name: newCategoryName.trim() });
            setNewCategoryName("");
            alert("Category created!");
            loadCategories();
        } catch (err: any) {
            alert(err.response?.data?.message || "Failed to create category.");
        }
    };

    const handleUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!editingCategory || !editName.trim()) return;
        try {
            await updateCategory(editingCategory.id, { name: editName.trim() });
            setEditingCategory(null);
            alert("Category updated!");
            loadCategories();
        } catch (err: any) {
            alert(err.response?.data?.message || "Failed to update category.");
        }
    };

    const handleDelete = async (id: number) => {
        if (!window.confirm("Are you sure you want to delete this category?")) return;
        try {
            await deleteCategory(id);
            alert("Category deleted!");
            loadCategories();
        } catch {
            alert("Failed to delete category.");
        }
    };

    return (
        <div>
            <Navbar />
            <div className="admin-container">
                <div className="admin-header">
                    <div>
                        <h1 style={{ fontSize: "24px", color: "#222" }}>🏷️ Category Management</h1>
                        <p style={{ color: "#666" }}>Organize catalog items by creating and modifying categories.</p>
                    </div>
                </div>

                <div className="admin-nav">
                    <Link to="/admin" className="admin-nav-item">Dashboard</Link>
                    <Link to="/admin/products" className="admin-nav-item">Products & Stock</Link>
                    <Link to="/admin/categories" className="admin-nav-item active">Categories</Link>
                    <Link to="/admin/users" className="admin-nav-item">Users & Roles</Link>
                    <Link to="/admin/promotions" className="admin-nav-item">Promotions</Link>
                    <Link to="/admin/shipping" className="admin-nav-item">Shipping & Orders</Link>
                </div>

                {/* Create Form */}
                <div className="admin-card" style={{ maxWidth: "500px" }}>
                    <h3 style={{ marginBottom: "12px" }}>+ Add New Category</h3>
                    <form onSubmit={handleCreate} style={{ display: "flex", gap: "10px" }}>
                        <input
                            type="text"
                            placeholder="Category Name"
                            value={newCategoryName}
                            onChange={(e) => setNewCategoryName(e.target.value)}
                            required
                            style={{ flex: 1, padding: "8px 12px", borderRadius: "6px", border: "1px solid #ddd" }}
                        />
                        <button type="submit" className="admin-btn primary">Create</button>
                    </form>
                </div>

                {/* Categories Table */}
                <div className="admin-card">
                    <h3>All Categories ({categories.length})</h3>
                    {loading ? (
                        <p style={{ padding: "20px", textAlign: "center" }}>Loading...</p>
                    ) : categories.length === 0 ? (
                        <p style={{ padding: "20px", textAlign: "center", color: "#888" }}>No categories created yet.</p>
                    ) : (
                        <table className="admin-table">
                            <thead>
                                <tr>
                                    <th style={{ width: "80px" }}>ID</th>
                                    <th>Category Name</th>
                                    <th style={{ width: "180px" }}>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {categories.map((c) => (
                                    <tr key={c.id}>
                                        <td>{c.id}</td>
                                        <td style={{ fontWeight: "600" }}>{c.name}</td>
                                        <td>
                                            <button
                                                className="admin-btn info"
                                                onClick={() => {
                                                    setEditingCategory(c);
                                                    setEditName(c.name);
                                                }}
                                            >
                                                Edit
                                            </button>
                                            <button
                                                className="admin-btn danger"
                                                onClick={() => handleDelete(c.id)}
                                            >
                                                Delete
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    )}
                </div>

                {/* Edit Modal */}
                {editingCategory && (
                    <div className="modal-overlay" onClick={() => setEditingCategory(null)}>
                        <div className="modal-content" onClick={(e) => e.stopPropagation()} style={{ width: "400px" }}>
                            <h2 style={{ marginBottom: "16px" }}>Edit Category</h2>
                            <form onSubmit={handleUpdate}>
                                <div style={{ marginBottom: "16px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "6px" }}>Category Name</label>
                                    <input
                                        value={editName}
                                        onChange={(e) => setEditName(e.target.value)}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>
                                <div style={{ display: "flex", gap: "10px", justifyContent: "flex-end" }}>
                                    <button type="button" className="admin-btn secondary" onClick={() => setEditingCategory(null)}>Cancel</button>
                                    <button type="submit" className="admin-btn primary">Save Changes</button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
