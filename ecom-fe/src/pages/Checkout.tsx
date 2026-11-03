import { useState, useEffect, useCallback } from "react";
import { useNavigate, Link } from "react-router-dom";
import {
    createOrder,
    previewOrder,
    validateOrder,
    calculateOrder,
    type PreviewRequest,
} from "../services/orderApi";
import { getSelectedCart } from "../services/cartApi";
import { createPayment } from "../services/paymentApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";
import "../styles/checkout.css";

type Step = "items" | "preview" | "success" | "error";

export default function Checkout() {
    const navigate = useNavigate();
    const { user, isAdmin, isSeller, isShipper } = useAuth();

    if (isAdmin || isSeller || isShipper) {
        navigate("/");
        return null;
    }

    const [step, setStep] = useState<Step>("items");
    const [items, setItems] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [checkoutId, setCheckoutId] = useState<string | null>(null);

    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    const [receiverName, setReceiverName] = useState("");
    const [couponCode, setCouponCode] = useState("");
    const [paymentMethod, setPaymentMethod] = useState("QR");

    const [appliedDiscount, setAppliedDiscount] = useState(0);
    const [shippingFee, setShippingFee] = useState(0);
    const [subTotal, setSubTotal] = useState(0);
    const [totalPrice, setTotalPrice] = useState(0);

    const loadSelectedCart = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const res = await getSelectedCart();
            const data = res.data || [];
            setItems(data);
            if (data.length === 0) {
                setError("Gioi hang trong. Vui long chon san pham de dat hang.");
            } else {
                computeTotals(data);
            }
        } catch (err: any) {
            setError("Khong the lay gio hang. Vui long thu lai sau.");
            console.error("Load selected cart error", err);
        } finally {
            setLoading(false);
        }
    }, []);

    const computeTotals = (cartItems: any[]) => {
        const sub = cartItems.reduce((acc, it) => acc + (Number(it.price) * it.quantity), 0);
        const ship = sub < 200000 ? 30000 : 0;
        let disc = appliedDiscount;
        let total = sub + ship - disc;
        if (total < 0) total = 0;
        setSubTotal(sub);
        setShippingFee(ship);
        setTotalPrice(total);
    };

    useEffect(() => {
        loadSelectedCart();
    }, [loadSelectedCart]);

    const handleApplyCoupon = async () => {
        if (!items.length) return;
        setLoading(true);
        try {
            const res = await calculateOrder({ couponCode: couponCode.trim() || undefined });
            const data = res.data;
            setAppliedDiscount(data.discount);
            setShippingFee(data.shippingFee);
            setSubTotal(data.subTotal);
            setTotalPrice(data.totalPrice);
        } catch (err: any) {
            setError(err.response?.data?.message || "Khong ap duoc ma giam gia.");
        } finally {
            setLoading(false);
        }
    };

    const handleRemoveCoupon = () => {
        setCouponCode("");
        setAppliedDiscount(0);
        computeTotals(items);
    };

    const handlePlaceOrder = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        if (!items.length) {
            setError("Gioi hang trong.");
            return;
        }
        if (!receiverName.trim() || !phone.trim() || !address.trim()) {
            setError("Vui lap day du thong tin giao hang.");
            return;
        }

        setSaving(true);
        try {
            // Step 1: Validate order conditions.
            const validateRes = await validateOrder({ couponCode: couponCode.trim() || undefined });
            const validation = validateRes.data;
            if (!validation.isValid) {
                setError(validation.errors.join("; "));
                setSaving(false);
                return;
            }

            // Step 2: Preview (creates a persisted checkout session).
            const previewReq: PreviewRequest = {
                address,
                phone,
                receiverName,
                couponCode: couponCode.trim() || undefined,
                paymentMethod,
            };
            const previewRes = await previewOrder(previewReq);
            const previewData = previewRes.data;
            setCheckoutId(previewData.checkoutId);

            if (!previewData.isValid) {
                setError(previewData.errors.join("; "));
                setSaving(false);
                return;
            }

            // Step 3: Create the order (idempotent via session).
            const idempotencyKey = `checkout-${user?.id}-${Date.now()}`;
            const checkoutRes = await createOrder({
                address,
                phone,
                receiverName,
                couponCode: couponCode.trim() || undefined,
                paymentMethod,
                checkoutId: previewData.checkoutId,
                idempotencyKey,
            });

            const orderData = checkoutRes.data;

            // For COD, record payment as cash-on-delivery.
            if (paymentMethod === "COD") {
                try {
                    await createPayment({ orderId: orderData.orderId, paymentMethod: "COD" });
                } catch {
                    // Payment recording is best-effort; order is already created.
                }
            }

            setSaving(false);
            setStep("success");
        } catch (err: any) {
            setSaving(false);
            setError(err.response?.data?.message || "Dat hang that bai. Vui long thu lai.");
            console.error("Checkout error", err);
        }
    };

    const formatMoney = (v: number) => v.toLocaleString("vi-VN") + " đ";

    const itemsByShop = (items: any[]) => {
        const map = new Map<string, any[]>();
        for (const it of items) {
            const key = String(it.sellerId || it.productId);
            if (!map.has(key)) map.set(key, []);
            map.get(key)!.push(it);
        }
        return Array.from(map.entries());
    };

    if (step === "success") {
        return (
            <div>
                <Navbar />
                <div style={{ maxWidth: "1120px", margin: "0 auto", padding: "40px 16px" }}>
                    <div style={{ textAlign: "center", padding: "60px 20px", background: "white", borderRadius: "12px", boxShadow: "0 2px 12px rgba(0,0,0,0.06)" }}>
                        <div style={{ fontSize: "48px", marginBottom: "16px" }}>&#10004;</div>
                        <h2 style={{ fontSize: "24px", color: "#2b8a3e", marginBottom: "12px", fontWeight: 700 }}>
                            Dat hang thanh cong!
                        </h2>
                        <p style={{ color: "#666", marginBottom: "24px" }}>
                            Don hang cua ban da duoc xac nhan. Chung toi se xu ly va giao hang trong som nhat.
                        </p>
                        {checkoutId && (
                            <p style={{ color: "#999", fontSize: "14px", marginBottom: "24px" }}>
                                Ma giao dich: {checkoutId}
                            </p>
                        )}
                        <Link
                            to="/orders"
                            style={{ padding: "12px 28px", background: "#ee4d2d", color: "white", borderRadius: "8px", textDecoration: "none", fontWeight: "bold", display: "inline-block" }}
                        >
                            Xem don hang
                        </Link>
                        <Link
                            to="/"
                            style={{ marginLeft: "12px", padding: "12px 28px", background: "#f0f0f0", color: "#333", borderRadius: "8px", textDecoration: "none", fontWeight: "600", display: "inline-block" }}
                        >
                            Tiep tuc mua sam
                        </Link>
                    </div>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div>
                <Navbar />
                <div style={{ maxWidth: "1120px", margin: "0 auto", padding: "40px 16px" }}>
                    <div style={{ textAlign: "center", padding: "60px 20px", background: "white", borderRadius: "12px", boxShadow: "0 2px 12px rgba(0,0,0,0.06)" }}>
                        <div style={{ fontSize: "48px", marginBottom: "16px" }}>&#10007;</div>
                        <h2 style={{ fontSize: "24px", color: "#e03131", marginBottom: "12px", fontWeight: 700 }}>
                            Co loi xay ra
                        </h2>
                        <p style={{ color: "#666", marginBottom: "24px" }}>{error}</p>
                        <button
                            onClick={loadSelectedCart}
                            disabled={loading}
                            style={{ padding: "12px 28px", background: "#ee4d2d", color: "white", borderRadius: "8px", border: "none", cursor: "pointer", fontWeight: "bold" }}
                        >
                            Thu lai
                        </button>
                        <Link to="/" style={{ marginLeft: "12px", color: "#ee4d2d", textDecoration: "none", fontWeight: 600 }}>
                            Ve trang chu
                        </Link>
                    </div>
                </div>
            </div>
        );
    }

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0", color: "#666" }}>
                    <h2>Dang tai gio hang...</h2>
                </div>
            </div>
        );
    }

    if (items.length === 0) {
        return (
            <div>
                <Navbar />
                <div style={{ maxWidth: "1120px", margin: "0 auto", padding: "40px 16px" }}>
                    <div style={{ textAlign: "center", padding: "80px 20px", background: "white", borderRadius: "12px", boxShadow: "0 2px 12px rgba(0,0,0,0.06)" }}>
                        <h2 style={{ color: "#666", marginBottom: "12px" }}>Gioi hang trong</h2>
                        <p style={{ color: "#999", marginBottom: "24px" }}>Ban chua chon san pham nao de dat hang.</p>
                        <Link
                            to="/"
                            style={{ padding: "12px 28px", background: "#ee4d2d", color: "white", borderRadius: "8px", textDecoration: "none", fontWeight: "bold" }}
                        >
                            Bat dau mua sam
                        </Link>
                    </div>
                </div>
            </div>
        );
    }

    const shops = itemsByShop(items);
    const totalItems = items.reduce((acc, it) => acc + it.quantity, 0);

    return (
        <div>
            <Navbar />
            <div className="checkout-page">
                <div className="checkout-header">
                    <h1 className="checkout-title">Xac nhan don hang</h1>
                    <p className="checkout-subtitle">Kiem tra lai thong tin don hang xong hay dat hang</p>
                </div>

                <div style={{ display: "grid", gridTemplateColumns: "1fr 360px", gap: "24px", alignItems: "start", maxWidth: "1120px", margin: "0 auto", padding: "0 16px 40px" }}>
                    {/* Left: Items + Shipping + Payment */}
                    <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                        {/* Order items by shop */}
                        <div className="checkout-card">
                            <h2 className="section-title">
                                {totalItems} san pham
                                <span style={{ float: "right", fontSize: "14px", color: "#999", fontWeight: 500 }}>{shops.length} cua hang</span>
                            </h2>
                            {shops.map(([sellerId, shopItems]) => (
                                <div key={sellerId} style={{ marginBottom: "16px", paddingBottom: "16px", borderBottom: sellerId !== shops[shops.length - 1][0] ? "1px solid #f0f0f0" : "none" }}>
                                    <div style={{ display: "flex", alignItems: "center", gap: "10px", marginBottom: "10px" }}>
                                        <div style={{ width: "32px", height: "32px", background: "#f5f5f5", borderRadius: "50%", display: "flex", alignItems: "center", justifyContent: "center", fontSize: "12px", color: "#666" }}>
                                            {shopItems[0]?.sellerName || "Cua hang"}
                                        </div>
                                        <span style={{ fontSize: "13px", color: "#666", fontWeight: 600 }}>
                                            {shopItems[0]?.productName?.split(" ")[0] || "Cua hang"} ...
                                        </span>
                                    </div>
                                    {shopItems.map((it) => (
                                        <div key={it.productId} style={{ display: "flex", alignItems: "center", gap: "12px", padding: "10px 0", borderBottom: "1px solid #f8f8f8" }}>
                                            <img src={it.imageUrl || "https://dummyimage.com/60x60/eee/999&text=Item"} alt={it.productName} style={{ width: "60px", height: "60px", objectFit: "cover", borderRadius: "8px", border: "1px solid #eee" }} />
                                            <div style={{ flex: 1, minWidth: 0 }}>
                                                <div style={{ fontSize: "14px", color: "#333", fontWeight: 600, whiteSpace: "nowrap", overflow: "hidden", textOverflow: "ellipsis" }}>{it.productName}</div>
                                                {it.variantName && (
                                                    <div style={{ fontSize: "12px", color: "#999", marginTop: "2px" }}>{it.variantName}</div>
                                                )}
                                            </div>
                                            <div style={{ fontSize: "14px", color: "#333", fontWeight: 600 }}>{formatMoney(Number(it.price) * it.quantity)}</div>
                                            <div style={{ fontSize: "12px", color: "#999" }}>x{it.quantity}</div>
                                        </div>
                                    ))}
                                </div>
                            ))}
                        </div>

                        {/* Shipping information */}
                        <div className="checkout-card">
                            <h2 className="section-title">Thong tin giao hang</h2>
                            <div style={{ display: "grid", gridTemplateColumns: "1fr", gap: "12px" }}>
                                <div>
                                    <label style={{ display: "block", fontSize: "13px", fontWeight: "600", color: "#555", marginBottom: "6px" }}>Ten nguoi nhan *</label>
                                    <input
                                        value={receiverName}
                                        onChange={(e) => setReceiverName(e.target.value)}
                                        placeholder="Ho va ten"
                                        style={{ width: "100%", padding: "10px", borderRadius: "8px", border: "1px solid #ddd", fontSize: "14px" }}
                                        required
                                    />
                                </div>
                                <div>
                                    <label style={{ display: "block", fontSize: "13px", fontWeight: "600", color: "#555", marginBottom: "6px" }}>So dien thoai *</label>
                                    <input
                                        value={phone}
                                        onChange={(e) => setPhone(e.target.value)}
                                        placeholder="0912345678"
                                        style={{ width: "100%", padding: "10px", borderRadius: "8px", border: "1px solid #ddd", fontSize: "14px" }}
                                        required
                                    />
                                </div>
                                <div>
                                    <label style={{ display: "block", fontSize: "13px", fontWeight: "600", color: "#555", marginBottom: "6px" }}>Dia chi giao hang *</label>
                                    <input
                                        value={address}
                                        onChange={(e) => setAddress(e.target.value)}
                                        placeholder="Dia chi chi tiet, phuong, quan, thanh pho"
                                        style={{ width: "100%", padding: "10px", borderRadius: "8px", border: "1px solid #ddd", fontSize: "14px" }}
                                        required
                                    />
                                </div>
                            </div>
                        </div>

                        {/* Payment method */}
                        <div className="checkout-card">
                            <h2 className="section-title">Phuong thuc thanh toan</h2>
                            <div style={{ display: "flex", gap: "10px" }}>
                                <button
                                    type="button"
                                    onClick={() => setPaymentMethod("QR")}
                                    style={{
                                        flex: 1,
                                        padding: "14px",
                                        borderRadius: "10px",
                                        border: paymentMethod === "QR" ? "2px solid #ee4d2d" : "1px solid #ddd",
                                        background: paymentMethod === "QR" ? "#fff5f2" : "white",
                                        cursor: "pointer",
                                        fontWeight: 700,
                                        color: paymentMethod === "QR" ? "#ee4d2d" : "#333",
                                        fontSize: "14px",
                                    }}
                                >
                                    <div style={{ fontSize: "20px", marginRight: "6px" }}>&#128271;</div>
                                    <div style={{ display: "flex", flexDirection: "column" }}>
                                        <div style={{ fontWeight: 700 }}>Thanh toan QR</div>
                                        <div style={{ fontSize: "12px", color: "#999" }}>Mo dom, VietQR</div>
                                    </div>
                                </button>
                                <button
                                    type="button"
                                    onClick={() => setPaymentMethod("COD")}
                                    style={{
                                        flex: 1,
                                        padding: "14px",
                                        borderRadius: "10px",
                                        border: paymentMethod === "COD" ? "2px solid #ee4d2d" : "1px solid #ddd",
                                        background: paymentMethod === "COD" ? "#fff5f2" : "white",
                                        cursor: "pointer",
                                        fontWeight: 700,
                                        color: paymentMethod === "COD" ? "#ee4d2d" : "#333",
                                        fontSize: "14px",
                                    }}
                                >
                                    <div style={{ fontSize: "20px", marginRight: "6px" }}>&#128177;</div>
                                    <div style={{ display: "flex", flexDirection: "column" }}>
                                        <div style={{ fontWeight: 700 }}>Thanh toan khi nhan hang</div>
                                        <div style={{ fontSize: "12px", color: "#999" }}>Tien mat khi giao hang</div>
                                    </div>
                                </button>
                            </div>
                        </div>

                        {/* Coupon */}
                        <div className="checkout-card">
                            <div style={{ display: "flex", gap: "8px", marginBottom: "12px" }}>
                                <input
                                    value={couponCode}
                                    onChange={(e) => setCouponCode(e.target.value)}
                                    placeholder="Ma giam gia (neu co)"
                                    style={{ flex: 1, padding: "10px", borderRadius: "8px", border: "1px solid #ddd", fontSize: "14px" }}
                                />
                                <button
                                    onClick={handleApplyCoupon}
                                    disabled={loading}
                                    style={{ padding: "10px 16px", borderRadius: "8px", background: "#ee4d2d", color: "white", border: "none", cursor: loading ? "not-allowed" : "pointer", fontWeight: 600, fontSize: "14px" }}
                                >
                                    {loading ? "Dang ap..." : "Ap dung"}
                                </button>
                            </div>
                            {couponCode.trim() && (
                                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", padding: "8px 12px", background: "#fff9db", borderRadius: "8px", fontSize: "13px", color: "#d9480f", border: "1px dashed #ffa94d" }}>
                                    <span>{couponCode.toUpperCase()}</span>
                                    <button onClick={handleRemoveCoupon} style={{ background: "none", border: "none", color: "#d9480f", cursor: "pointer", fontWeight: 600, padding: 0 }}>
                                        Bo
                                    </button>
                                </div>
                            )}
                        </div>
                    </div>

                    {/* Right: Order summary */}
                    <div className="checkout-card" style={{ position: "sticky", top: "20px" }}>
                        <h2 className="section-title">Tong tien don hang</h2>
                        <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "10px", color: "#666", fontSize: "14px" }}>
                            <span>Tam tinh</span>
                            <span>{formatMoney(subTotal)}</span>
                        </div>
                        <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "10px", color: "#666", fontSize: "14px" }}>
                            <span>Phi giao hang</span>
                            <span>{shippingFee > 0 ? formatMoney(shippingFee) : "Mien phi"}</span>
                        </div>
                        {appliedDiscount > 0 && (
                            <div style={{ display: "flex", justifyContent: "space-between", marginBottom: "14px", color: "#2b8a3e", fontSize: "14px", fontWeight: 600 }}>
                                <span>Giam gia</span>
                                <span>-{formatMoney(appliedDiscount)}</span>
                            </div>
                        )}
                        <hr style={{ border: "none", borderTop: "1px solid #eee", margin: "14px 0" }} />
                        <div style={{ display: "flex", justifyContent: "space-between", fontSize: "20px", fontWeight: 800, color: "#222", marginBottom: "16px" }}>
                            <span>Tong thanh toan</span>
                            <span style={{ color: "#ee4d2d" }}>{formatMoney(totalPrice)}</span>
                        </div>
                        <div style={{ fontSize: "13px", color: "#999", marginBottom: "18px", lineHeight: "1.6" }}>
                            Tong tien duoc tinh toan boi he thong, khong duoc su dung gia tai trang thuong mai dien tu.
                        </div>
                        <button
                            onClick={handlePlaceOrder}
                            disabled={saving}
                            className="place-order-btn"
                            style={{
                                width: "100%",
                                padding: "16px",
                                background: saving ? "#bbb" : "#ee4d2d",
                                color: "white",
                                border: "none",
                                borderRadius: "10px",
                                fontSize: "17px",
                                fontWeight: 700,
                                cursor: saving ? "not-allowed" : "pointer",
                            }}
                        >
                            {saving ? "Dang dat hang..." : `Dat hang & thanh toan (${formatMoney(totalPrice)})`}
                        </button>
                        <div style={{ marginTop: "12px", fontSize: "13px", color: "#999", textAlign: "center" }}>
                            Don hang cua ban se duoc xu ly sau khi xac nhan thanh toan.
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
