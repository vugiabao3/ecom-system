import { useEffect, useState, useCallback } from "react";
import { Link } from "react-router-dom";
import Navbar from "../../components/Navbar";
import {
    getPromotionsBySeller,
    createPromotion,
    updatePromotion,
    deletePromotion,
    type PromotionDto,
} from "../../services/promotionApi";
import { useAuth } from "../../context/AuthContext";
import "../../styles/seller.css";

export default function SellerPromotions() {
    const { user } = useAuth();
    const [promotions, setPromotions] = useState<PromotionDto[]>([]);
    const [loading, setLoading] = useState(true);

    const [showCreateModal, setShowCreateModal] = useState(false);
    const [editingPromotion, setEditingPromotion] = useState<PromotionDto | null>(null);

    const [code, setCode] = useState("");
    const [discountPercent, setDiscountPercent] = useState<number>(10);
    const [startDate, setStartDate] = useState<string>(new Date().toISOString().slice(0, 10));
    const [endDate, setEndDate] = useState<string>(
        new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().slice(0, 10)
    );
    const [quantity, setQuantity] = useState<number>(50);
    const [isActive, setIsActive] = useState<boolean>(true);

    const loadPromotions = useCallback(async () => {
        if (!user?.id) return;
        setLoading(true);
        try {
            const res = await getPromotionsBySeller(user.id);
            setPromotions(res.data || []);
        } catch {
            setPromotions([]);
        } finally {
            setLoading(false);
        }
    }, [user?.id]);

    useEffect(() => {
        loadPromotions();
    }, [loadPromotions]);

    const handleCreate = async (e: React.FormEvent) => {
        e.preventDefault();
        try {
            await createPromotion({
                code: code.toUpperCase().trim(),
                discountPercent: Number(discountPercent),
                startDate: new Date(startDate).toISOString(),
                endDate: new Date(endDate).toISOString(),
                quantity: Number(quantity),
            });
            alert("Promotion created!");
            setShowCreateModal(false);
            resetForm();
            loadPromotions();
        } catch (err: any) {
            alert(err.response?.data?.message || "Failed to create promotion.");
        }
    };

    const handleUpdate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!editingPromotion) return;
        try {
            await updatePromotion({
                id: editingPromotion.id,
                code: code.toUpperCase().trim(),
                discountPercent: Number(discountPercent),
                startDate: new Date(startDate).toISOString(),
                endDate: new Date(endDate).toISOString(),
                quantity: Number(quantity),
                isActive,
            });
            alert("Promotion updated!");
            setEditingPromotion(null);
            resetForm();
            loadPromotions();
        } catch (err: any) {
            alert(err.response?.data?.message || "Failed to update promotion.");
        }
    };

    const handleDelete = async (id: string) => {
        if (!window.confirm("Are you sure you want to delete this promotion?")) return;
        try {
            await deletePromotion(id);
            alert("Promotion deleted!");
            loadPromotions();
        } catch {
            alert("Failed to delete promotion.");
        }
    };

    const openEdit = (p: PromotionDto) => {
        setEditingPromotion(p);
        setCode(p.code);
        setDiscountPercent(p.discountPercent);
        setStartDate(p.startDate ? p.startDate.slice(0, 10) : "");
        setEndDate(p.endDate ? p.endDate.slice(0, 10) : "");
        setQuantity(p.quantity);
        setIsActive(p.isActive);
    };

    const resetForm = () => {
        setCode("");
        setDiscountPercent(10);
        setStartDate(new Date().toISOString().slice(0, 10));
        setEndDate(new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().slice(0, 10));
        setQuantity(50);
        setIsActive(true);
    };

    return (
        <div>
            <Navbar />
            <div className="portal-container">
                <div className="portal-header">
                    <div>
                        <h1>🏷️ My Promotions</h1>
                        <p style={{ color: "#666" }}>Create and manage discount codes for your products.</p>
                    </div>
                    <button
                        className="portal-btn primary"
                        onClick={() => {
                            resetForm();
                            setShowCreateModal(true);
                        }}
                        style={{ padding: "10px 18px", fontSize: "14px" }}
                    >
                        + Create Promotion
                    </button>
                </div>

                <div className="portal-nav">
                    <Link to="/seller" className="portal-nav-item">Dashboard</Link>
                    <Link to="/seller/products" className="portal-nav-item">Products</Link>
                    <Link to="/seller/orders" className="portal-nav-item">Orders</Link>
                    <Link to="/seller/promotions" className="portal-nav-item active">Promotions</Link>
                    <Link to="/seller/revenue" className="portal-nav-item">Revenue</Link>
                </div>

                <div className="portal-card">
                    <h3>My Promotions ({promotions.length})</h3>
                    {loading ? (
                        <p style={{ padding: "20px", textAlign: "center" }}>Loading...</p>
                    ) : promotions.length === 0 ? (
                        <p style={{ padding: "20px", textAlign: "center", color: "#888" }}>No promotions created yet.</p>
                    ) : (
                        <table className="portal-table">
                            <thead>
                                <tr>
                                    <th>Code</th>
                                    <th>Discount</th>
                                    <th>Quantity</th>
                                    <th>Valid Until</th>
                                    <th>Status</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                {promotions.map((p) => (
                                    <tr key={p.id}>
                                        <td style={{ fontWeight: "bold", color: "#d9480f" }}>{p.code}</td>
                                        <td>{p.discountPercent}% OFF</td>
                                        <td>{p.quantity} left</td>
                                        <td>{p.endDate ? new Date(p.endDate).toLocaleDateString() : "N/A"}</td>
                                        <td>
                                            <span className={`admin-badge ${p.isActive ? "success" : "danger"}`}>
                                                {p.isActive ? "Active" : "Inactive"}
                                            </span>
                                        </td>
                                        <td>
                                            <button className="portal-btn info" onClick={() => openEdit(p)}>
                                                Edit
                                            </button>
                                            <button className="portal-btn danger" onClick={() => handleDelete(p.id)}>
                                                Delete
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    )}
                </div>

                {/* Create Modal */}
                {showCreateModal && (
                    <div className="modal-overlay" onClick={() => setShowCreateModal(false)}>
                        <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                            <h2 style={{ marginBottom: "16px" }}>Create Promotion</h2>
                            <form onSubmit={handleCreate}>
                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Coupon Code *</label>
                                    <input
                                        value={code}
                                        onChange={(e) => setCode(e.target.value)}
                                        placeholder="e.g. FLASH50"
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Discount Percent (%) *</label>
                                    <input
                                        type="number"
                                        min={1}
                                        max={100}
                                        value={discountPercent}
                                        onChange={(e) => setDiscountPercent(Number(e.target.value))}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "10px", marginBottom: "12px" }}>
                                    <div>
                                        <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Start Date</label>
                                        <input
                                            type="date"
                                            value={startDate}
                                            onChange={(e) => setStartDate(e.target.value)}
                                            style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                        />
                                    </div>
                                    <div>
                                        <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>End Date</label>
                                        <input
                                            type="date"
                                            value={endDate}
                                            onChange={(e) => setEndDate(e.target.value)}
                                            style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                        />
                                    </div>
                                </div>

                                <div style={{ marginBottom: "16px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Quantity (Total Uses) *</label>
                                    <input
                                        type="number"
                                        min={1}
                                        value={quantity}
                                        onChange={(e) => setQuantity(Number(e.target.value))}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ display: "flex", gap: "10px", justifyContent: "flex-end" }}>
                                    <button type="button" className="portal-btn secondary" onClick={() => setShowCreateModal(false)}>Cancel</button>
                                    <button type="submit" className="portal-btn primary">Create</button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}

                {/* Edit Modal */}
                {editingPromotion && (
                    <div className="modal-overlay" onClick={() => setEditingPromotion(null)}>
                        <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                            <h2 style={{ marginBottom: "16px" }}>Edit Promotion</h2>
                            <form onSubmit={handleUpdate}>
                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Coupon Code *</label>
                                    <input
                                        value={code}
                                        onChange={(e) => setCode(e.target.value)}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Discount Percent (%) *</label>
                                    <input
                                        type="number"
                                        min={1}
                                        max={100}
                                        value={discountPercent}
                                        onChange={(e) => setDiscountPercent(Number(e.target.value))}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: "10px", marginBottom: "12px" }}>
                                    <div>
                                        <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Start Date</label>
                                        <input
                                            type="date"
                                            value={startDate}
                                            onChange={(e) => setStartDate(e.target.value)}
                                            style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                        />
                                    </div>
                                    <div>
                                        <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>End Date</label>
                                        <input
                                            type="date"
                                            value={endDate}
                                            onChange={(e) => setEndDate(e.target.value)}
                                            style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                        />
                                    </div>
                                </div>

                                <div style={{ marginBottom: "12px" }}>
                                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", marginBottom: "4px" }}>Quantity *</label>
                                    <input
                                        type="number"
                                        min={0}
                                        value={quantity}
                                        onChange={(e) => setQuantity(Number(e.target.value))}
                                        required
                                        style={{ width: "100%", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                                    />
                                </div>

                                <div style={{ marginBottom: "16px" }}>
                                    <label style={{ display: "flex", alignItems: "center", gap: "8px", cursor: "pointer" }}>
                                        <input
                                            type="checkbox"
                                            checked={isActive}
                                            onChange={(e) => setIsActive(e.target.checked)}
                                        />
                                        <span style={{ fontSize: "14px", fontWeight: "600" }}>Active Status</span>
                                    </label>
                                </div>

                                <div style={{ display: "flex", gap: "10px", justifyContent: "flex-end" }}>
                                    <button type="button" className="portal-btn secondary" onClick={() => setEditingPromotion(null)}>Cancel</button>
                                    <button type="submit" className="portal-btn primary">Save Changes</button>
                                </div>
                            </form>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
