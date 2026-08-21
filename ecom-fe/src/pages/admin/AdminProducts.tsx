import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import {
    getProducts,
    createProduct,
    updateProduct,
    deleteProduct,
    restoreProduct,
    type ProductItem
} from "../../services/productApi";
import { getCategories, type CategoryDto } from "../../services/categoryApi";
import { addStock } from "../../services/inventoryApi";
import Navbar from "../../components/Navbar";
import "../../styles/admin.css";

export default function AdminProducts() {
    const [products, setProducts] = useState<ProductItem[]>([]);
    const [categories, setCategories] = useState<CategoryDto[]>([]);
    const [loading, setLoading] = useState(true);

    // Create / Edit modal state
    const [showCreateModal, setShowCreateModal] = useState(false);
    const [editingProduct, setEditingProduct] = useState<ProductItem | null>(null);

    // Form state
    const [name, setName] = useState("");
    const [price, setPrice] = useState<number>(0);
    const [categoryId, setCategoryId] = useState<number>(1);
    const [imageUrl, setImageUrl] = useState("");

    // Stock addition modal state
    const [stockModalProduct, setStockModalProduct] = useState<ProductItem | null>(null);
    const [stockQuantity, setStockQuantity] = useState<number>(10);

    const loadData = useCallback(async () => {
        setLoading(true);
        try {
            const [pRes, cRes] = await Promise.all([
                getProducts({ Page: 1, PageSize: 100 }),
                getCategories(),
            ]);
            setProducts(pRes.data?.items || []);
            setCategories(cRes.data || []);
            if (cRes.data?.length > 0) {
                setCategoryId(cRes.data[0].id);
            }
        } catch {
            setProducts([]);
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        loadData();
    }, [loadData]);

    const handleCreateProduct = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await createProduct({
                name,
                price: Number(price),
                categoryId: Number(categoryId),
                imageUrl: imageUrl || undefined,
            });
            alert("Product created successfully!");
            setShowCreateModal(false);
            resetForm();
            loadData();
        } catch (err: any) {
            alert(err.response?.data?.message || "Failed to create product.");
        }
    };

    const handleUpdateProduct = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!editingProduct) return;
        try {
            await updateProduct(editingProduct.id, {
                name,
                price: Number(price),
                categoryId: Number(categoryId),
            });
            alert("Product updated successfully!");
            setEditingProduct(null);
            resetForm();
            loadData();
        } catch (err: any) {
            alert(err.response?.data?.message || "Failed to update product.");
        }
    };

    const handleDeleteProduct = async (id: string) => {
        if (!window.confirm("Are you sure you want to delete this product?")) return;
        try {
            await deleteProduct(id);
            alert("Product deleted!");
            loadData();
        } catch {
            alert("Failed to delete product.");
        }
    };

    const handleRestoreProduct = async (id: string) => {
        try {
            await restoreProduct(id);
            alert("Product restored!");
            loadData();
        } catch {
            alert("Failed to restore product.");
        }
    };

    const handleAddStock = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!stockModalProduct) return;
        try {
            await addStock({
                productId: stockModalProduct.id,
                quantity: Number(stockQuantity),
            });
            alert(`Added ${stockQuantity} units to stock!`);
            setStockModalProduct(null);
        } catch {
            alert("Failed to add stock.");
        }
    };

    const openEdit = (p: ProductItem) => {
        setEditingProduct(p);
        setName(p.name);
        setPrice(p.price);
        setCategoryId(p.categoryId || (categories[0]?.id ?? 1));
        setImageUrl(p.imageUrl || "");
    };

    const resetForm = () => {
        setName("");
        setPrice(0);
        setImageUrl("");
        if (categories.length > 0) setCategoryId(categories[0].id);
    };

    return (
        <div>
            <Navbar />
            <div className="admin-container">
                <div className="admin-header">
                    <div>
                        <h1 style={{ fontSize: "24px", color: "#222" }}>📦 Product Management</h1>
                        <p style={{ color: "#666" }}>Add, edit, remove products and manage inventory stock.</p>
                    </div>
                    <button
                        className="admin-btn primary"
                        onClick={() => {
                            resetForm();
                            setShowCreateModal(true);
                        }}
                        style={{ padding: "10px 18px", fontSize: "14px" }}
                    >
                        + Create Product
                    </button>
                </div>

                <div className="admin-nav">
                    <Link to="/admin" className="admin-nav-item">Dashboard</Link>
                    <Link to="/admin/products" className="admin-nav-item active">Products & Stock</Link>
                    <Link to="/admin/categories" className="admin-nav-item">Categories</Link>
                    <Link to="/admin/users" className="admin-nav-item">Users & Roles</Link>
                    <Link to="/admin/promotions" className="admin-nav-item">Promotions</Link>
                    <Link to="/admin/shipping" className="admin-nav-item">Shipping & Orders</Link>
                </div>

                <div className="admin-card">
                    {loading ? (
                        <p style={{ textAlign: "center", padding: "40px" }}>Loading products...</p>
                    ) : products.length === 0 ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#888" }}>No products found.</p>
                    ) : (
                        <div style={{ overflowX: "auto" }}>
                            <table className="admin-table">
                                <thead>
                                    <tr>
                                        <th>Image</th>
                                        <th>Name</th>
                                        <th>Category</th>
                                        <th>Price</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {products.map((p) => (
                                        <tr key={p.id}>
                                            <td>
                                                <img
                                                    src={p.imageUrl || "https://dummyimage.com/50x50/eee/999&text=No+Img"}
                                                    alt={p.name}
                                                    style={{ width: "40px", height: "40px", objectFit: "cover", borderRadius: "4px" }}
                                                />
                                            </td>
                                            <td style={{ fontWeight: "600" }}>{p.name}</td>
                                            <td>{p.categoryName || "N/A"}</td>
                                            <td style={{ color: "#ee4d2d", fontWeight: "bold" }}>
                                                {p.price?.toLocaleString()} đ
                                            </td>
                                            <td>
                                                <button
                                                    className="admin-btn info"
                                                    onClick={() => openEdit(p)}
                                                >
                                                    Edit
                                                </button>

                                                <button
                                                    className="admin-btn success"
                                                    onClick={() => {
                                                        setStockModalProduct(p);
                                                        setStockQuantity(10);
                                                    }}
                                                >
                                                    + Stock
                                                </button>

                                                <button
                                                    className="admin-btn danger"
                                                    onClick={() => handleDeleteProduct(p.id)}
                                                >
                                                    Delete
                                                </button>

                                                <button
                                                    className="admin-btn secondary"
                                                    onClick={() => handleRestoreProduct(p.id)}
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

                {/* Create Modal */}
                {showCreateModal && (
                    <div className="modal-overlay" onClick={() => setShowCreateModal(false)}>
                        <div className="modal-content" onClick={(e) => e.stopPropagation()} style={{ width: "450px" }}>
                            <h2 style={{ marginBottom: "16px" }}>Create New Product</h2>
                            <form onSubmit={handleCreateProduct}>
                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Product Name *</label>
                                    <input
                                        value={name}
                                        onChange={(e) => setName(e.target.value)}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Price (VND) *</label>
                                    <input
                                        type="number"
                                        value={price}
                                        onChange={(e) => setPrice(Number(e.target.value))}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Category *</label>
                                    <select
                                        value={categoryId}
                                        onChange={(e) => setCategoryId(Number(e.target.value))}
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    >
                                        {categories.map((c) => (
                                            <option key={c.id} value={c.id}>{c.name}</option>
                                        ))}
                                    </select>
                                </div>

                                <div style={{ marginBottom: "16px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Image URL</label>
                                    <input
                                        value={imageUrl}
                                        onChange={(e) => setImageUrl(e.target.value)}
                                        placeholder="https://..."
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ display: "flex", gap: "10px", justifyContent: "flex-end" }}>
                                    <button type="button" className="admin-btn secondary" onClick={() => setShowCreateModal(false)}>Cancel</button>
                                    <button type="submit" className="admin-btn primary">Create</button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}

                {/* Edit Modal */}
                {editingProduct && (
                    <div className="modal-overlay" onClick={() => setEditingProduct(null)}>
                        <div className="modal-content" onClick={(e) => e.stopPropagation()} style={{ width: "450px" }}>
                            <h2 style={{ marginBottom: "16px" }}>Edit Product</h2>
                            <form onSubmit={handleUpdateProduct}>
                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Product Name *</label>
                                    <input
                                        value={name}
                                        onChange={(e) => setName(e.target.value)}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Price (VND) *</label>
                                    <input
                                        type="number"
                                        value={price}
                                        onChange={(e) => setPrice(Number(e.target.value))}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ marginBottom: "16px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Category *</label>
                                    <select
                                        value={categoryId}
                                        onChange={(e) => setCategoryId(Number(e.target.value))}
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    >
                                        {categories.map((c) => (
                                            <option key={c.id} value={c.id}>{c.name}</option>
                                        ))}
                                    </select>
                                </div>

                                <div style={{ display: "flex", gap: "10px", justifyContent: "flex-end" }}>
                                    <button type="button" className="admin-btn secondary" onClick={() => setEditingProduct(null)}>Cancel</button>
                                    <button type="submit" className="admin-btn primary">Save Changes</button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}

                {/* Stock Modal */}
                {stockModalProduct && (
                    <div className="modal-overlay" onClick={() => setStockModalProduct(null)}>
                        <div className="modal-content" onClick={(e) => e.stopPropagation()} style={{ width: "400px" }}>
                            <h2 style={{ marginBottom: "16px" }}>Add Stock: {stockModalProduct.name}</h2>
                            <form onSubmit={handleAddStock}>
                                <div style={{ marginBottom: "16px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Quantity to Add</label>
                                    <input
                                        type="number"
                                        min={1}
                                        value={stockQuantity}
                                        onChange={(e) => setStockQuantity(Number(e.target.value))}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ display: "flex", gap: "10px", justifyContent: "flex-end" }}>
                                    <button type="button" className="admin-btn secondary" onClick={() => setStockModalProduct(null)}>Cancel</button>
                                    <button type="submit" className="admin-btn success">Add Units</button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
