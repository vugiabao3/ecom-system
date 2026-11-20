import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { getCart, removeCartItem, clearCart, updateCartItemQuantity } from "../services/cartApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";

export default function Cart() {
    const navigate = useNavigate();
    const { isAdmin, isSeller, isShipper } = useAuth();

    if (isAdmin || isSeller || isShipper) {
        navigate("/");
        return null;
    }

    const [cart, setCart] = useState<any>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [actionLoading, setActionLoading] = useState<boolean>(false);

    const fetchCart = async () => {
        try {
            const res = await getCart();
            setCart(res.data);
        } catch (err) {
            console.error("Fetch cart error", err);
            setCart({ items: [], totalPrice: 0 });
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchCart();
    }, []);

    const handleRemove = async (itemId: string) => {
        setActionLoading(true);
        try {
            await removeCartItem(itemId);
            await fetchCart();
        } finally {
            setActionLoading(false);
        }
    };

    const handleClear = async () => {
        if (!window.confirm("Are you sure you want to clear your cart?")) return;
        setActionLoading(true);
        try {
            await clearCart();
            await fetchCart();
        } finally {
            setActionLoading(false);
        }
    };

    const handleQuantityChange = async (itemId: string, newQty: number) => {
        if (newQty < 1) return;
        setActionLoading(true);
        try {
            await updateCartItemQuantity(itemId, newQty);
            await fetchCart();
        } finally {
            setActionLoading(false);
        }
    };

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0", color: "#555" }}>
                    <h2>Loading cart...</h2>
                </div>
            </div>
        );
    }

    const items = cart?.items || [];
    const totalPrice = cart?.totalPrice || 0;

    return (
        <div style={{ background: "#f5f5f5", minHeight: "100vh" }}>
            <Navbar />
            <div style={{ padding: "24px 16px", maxWidth: "1120px", margin: "0 auto" }}>
                <h1 style={{ fontSize: "24px", color: "#222", marginBottom: "16px", fontWeight: 700 }}>
                    Shopping Cart ({items.length})
                </h1>

                {items.length === 0 ? (
                    <div style={{ textAlign: "center", padding: "60px 20px", background: "white", borderRadius: "12px", boxShadow: "0 2px 12px rgba(0,0,0,0.06)" }}>
                        <h2 style={{ color: "#555", marginBottom: "12px" }}>Your cart is empty</h2>
                        <p style={{ color: "#888", marginBottom: "24px" }}>Looks like you haven't added anything yet.</p>
                        <Link to="/" style={{ padding: "12px 24px", background: "#ee4d2d", color: "white", borderRadius: "8px", textDecoration: "none", fontWeight: "bold" }}>
                            Start Shopping
                        </Link>
                    </div>
                ) : (
                    <div style={{ display: "grid", gridTemplateColumns: "2fr 1fr", gap: "20px", alignItems: "start" }}>
                        <div style={{ background: "white", borderRadius: "12px", padding: "16px", boxShadow: "0 2px 12px rgba(0,0,0,0.06)" }}>
                            {items.map((item: any) => (
                                <div key={item.id} style={{ display: "flex", alignItems: "center", justifyContent: "space-between", borderBottom: "1px solid #f0f0f0", padding: "14px 0", gap: "12px" }}>
                                    <div style={{ display: "flex", alignItems: "center", gap: "14px", flex: 1 }}>
                                        <img src={item.imageUrl || "https://dummyimage.com/80x80/eee/999&text=Item"} alt={item.productName} style={{ width: "72px", height: "72px", objectFit: "cover", borderRadius: "8px", border: "1px solid #eee" }} />
                                        <div>
                                            <h4 style={{ margin: "0 0 6px", fontSize: "15px", color: "#333", fontWeight: 600 }}>{item.productName}</h4>
                                            <p style={{ margin: 0, color: "#ee4d2d", fontWeight: "bold", fontSize: "15px" }}>{item.priceSnapshot?.toLocaleString()} đ</p>
                                        </div>
                                    </div>
                                    <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                                        <div style={{ display: "flex", alignItems: "center", border: "1px solid #ddd", borderRadius: "6px", overflow: "hidden" }}>
                                            <button onClick={() => handleQuantityChange(item.id, item.quantity - 1)} disabled={actionLoading} style={{ padding: "6px 12px", background: "#f8f9fa", border: "none", cursor: "pointer" }}>-</button>
                                            <span style={{ padding: "6px 12px", fontWeight: "bold", fontSize: "14px" }}>{item.quantity}</span>
                                            <button onClick={() => handleQuantityChange(item.id, item.quantity + 1)} disabled={actionLoading} style={{ padding: "6px 12px", background: "#f8f9fa", border: "none", cursor: "pointer" }}>+</button>
                                        </div>
                                        <button onClick={() => handleRemove(item.id)} disabled={actionLoading} style={{ background: "transparent", color: "#e03131", border: "1px solid #ffc9c9", padding: "6px 12px", borderRadius: "6px", cursor: "pointer", fontSize: "13px", fontWeight: 600 }}>
                                            Delete
                                        </button>
                                    </div>
                                </div>
                            ))}
                            <div style={{ marginTop: "16px", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                                <button onClick={handleClear} disabled={actionLoading} style={{ background: "transparent", color: "#868e96", border: "1px solid #ced4da", padding: "8px 16px", borderRadius: "6px", cursor: "pointer", fontSize: "14px" }}>Clear Cart</button>
                                <Link to="/" style={{ color: "#ee4d2d", textDecoration: "none", fontWeight: 600, fontSize: "14px" }}>← Continue Shopping</Link>
                            </div>
                        </div>
                        <div style={{ background: "white", borderRadius: "12px", padding: "20px", boxShadow: "0 2px 12px rgba(0,0,0,0.06)" }}>
                            <h3 style={{ fontSize: "18px", color: "#333", marginBottom: "16px", fontWeight: 700 }}>Order Summary</h3>
                            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "12px", color: "#666" }}>
                                <span>Subtotal</span>
                                <span>{totalPrice.toLocaleString()} đ</span>
                            </div>
                            <hr style={{ border: "none", borderTop: "1px solid #eee", margin: "16px 0" }} />
                            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "20px", fontSize: "20px", fontWeight: "bold" }}>
                                <span>Total</span>
                                <span style={{ color: "#ee4d2d" }}>{totalPrice.toLocaleString()} đ</span>
                            </div>
                            <button onClick={() => navigate("/checkout")} style={{ width: "100%", padding: "14px", background: "#ee4d2d", color: "white", border: "none", borderRadius: "8px", fontSize: "16px", fontWeight: "bold", cursor: "pointer" }}>
                                Proceed to Checkout
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}
