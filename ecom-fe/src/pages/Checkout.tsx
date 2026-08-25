import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { createOrder } from "../services/orderApi";
import { createPayment } from "../services/paymentApi";
import { getCart } from "../services/cartApi";
import { getUserAddresses, type UserAddressDto } from "../services/userApi";
import { getAllPromotions, type PromotionDto } from "../services/promotionApi";
import { useAuth } from "../context/AuthContext";
import Navbar from "../components/Navbar";
import CheckoutForm from "../components/CheckoutForm";
import CheckoutSummary from "../components/CheckoutSummary";
import "../styles/checkout.css";

export default function Checkout() {
    const navigate = useNavigate();
    const { user, isAdmin, isSeller, isShipper } = useAuth();

    if (isAdmin || isSeller || isShipper) {
        navigate("/");
        return null;
    }

    const [address, setAddress] = useState("");
    const [phone, setPhone] = useState("");
    const [receiverName, setReceiverName] = useState("");
    const [couponCode, setCouponCode] = useState("");
    const [paymentMethod, setPaymentMethod] = useState("QR");

    const [cart, setCart] = useState<any>(null);
    const [savedAddresses, setSavedAddresses] = useState<UserAddressDto[]>([]);
    const [promotions, setPromotions] = useState<PromotionDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        // Fetch cart summary
        getCart()
            .then((res) => setCart(res.data))
            .catch(() => setCart(null));

        // Fetch user addresses if logged in
        if (user?.id) {
            getUserAddresses(user.id)
                .then((res) => {
                    const addrs = res.data?.addresses || [];
                    setSavedAddresses(addrs);
                    if (addrs.length > 0) {
                        setReceiverName(addrs[0].fullName || "");
                        setPhone(addrs[0].phone || "");
                        setAddress(`${addrs[0].addressLine}, ${addrs[0].city}, ${addrs[0].country}`);
                    }
                })
                .catch(() => setSavedAddresses([]));
        }

        // Fetch active promotions
        getAllPromotions()
            .then((res) => {
                const active = (res.data || []).filter((p) => p.isActive && p.quantity > 0);
                setPromotions(active);
            })
            .catch(() => setPromotions([]));
    }, [user?.id]);

    const handleOrder = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!receiverName.trim() || !phone.trim() || !address.trim()) {
            alert("Please fill in all required shipping details.");
            return;
        }

        setLoading(true);
        setError(null);
        try {
            const res = await createOrder({
                address,
                phone,
                receiverName,
                couponCode: couponCode.trim() || undefined,
                paymentMethod,
            });

            if (paymentMethod === "COD") {
                await createPayment({
                    orderId: res.data.orderId,
                    paymentMethod: "COD",
                });
                navigate("/orders");
            } else {
                await createPayment({
                    orderId: res.data.orderId,
                    paymentMethod: "QR",
                });
                navigate("/orders");
            }
        } catch (err: any) {
            setError("Mua sản phẩm không thành công");
        } finally {
            setLoading(false);
        }
    };

    const subTotal = cart?.totalPrice || 0;
    const shippingFee = subTotal < 200000 ? 30000 : 0;

    return (
        <div>
            <Navbar />
            <div className="checkout-page">
                <h1 className="checkout-title">Checkout</h1>

                {error && (
                    <div style={{ padding: "16px", background: "#ffe3e3", color: "#e03131", borderRadius: "8px", marginBottom: "20px" }}>
                        {error}
                    </div>
                )}

                <div style={{ display: "grid", gridTemplateColumns: "1.6fr 1fr", gap: "24px", alignItems: "start" }}>
                    <div>
                        <div className="checkout-card">
                            <CheckoutForm
                                address={address}
                                phone={phone}
                                receiverName={receiverName}
                                couponCode={couponCode}
                                savedAddresses={savedAddresses}
                                setAddress={setAddress}
                                setPhone={setPhone}
                                setReceiverName={setReceiverName}
                                setCouponCode={setCouponCode}
                            />
                        </div>

                        <div className="checkout-card" style={{ marginTop: "16px" }}>
                            <h3 style={{ fontSize: "16px", marginBottom: "12px", color: "#333" }}>
                                💳 Payment Method
                            </h3>
                            <div style={{ display: "flex", gap: "12px" }}>
                                <button
                                    type="button"
                                    onClick={() => setPaymentMethod("QR")}
                                    style={{
                                        flex: 1,
                                        padding: "12px",
                                        borderRadius: "8px",
                                        border: paymentMethod === "QR" ? "2px solid #ee4d2d" : "1px solid #ddd",
                                        background: paymentMethod === "QR" ? "#fff5f2" : "white",
                                        cursor: "pointer",
                                        fontWeight: paymentMethod === "QR" ? "700" : "500",
                                        color: paymentMethod === "QR" ? "#ee4d2d" : "#333",
                                    }}
                                >
                                    📱 QR Payment
                                </button>
                                <button
                                    type="button"
                                    onClick={() => setPaymentMethod("COD")}
                                    style={{
                                        flex: 1,
                                        padding: "12px",
                                        borderRadius: "8px",
                                        border: paymentMethod === "COD" ? "2px solid #ee4d2d" : "1px solid #ddd",
                                        background: paymentMethod === "COD" ? "#fff5f2" : "white",
                                        cursor: "pointer",
                                        fontWeight: paymentMethod === "COD" ? "700" : "500",
                                        color: paymentMethod === "COD" ? "#ee4d2d" : "#333",
                                    }}
                                >
                                    💵 Cash On Delivery
                                </button>
                            </div>
                        </div>

                        {/* Available Promotions */}
                        {promotions.length > 0 && (
                            <div className="checkout-card">
                                <h3 style={{ fontSize: "16px", marginBottom: "12px", color: "#333" }}>
                                    🏷️ Available Promotions
                                </h3>
                                <div style={{ display: "flex", gap: "10px", flexWrap: "wrap" }}>
                                    {promotions.map((p) => (
                                        <div
                                            key={p.id}
                                            onClick={() => setCouponCode(p.code)}
                                            style={{
                                                padding: "8px 12px",
                                                border: couponCode === p.code ? "2px solid #ee4d2d" : "1px dashed #ffa94d",
                                                background: couponCode === p.code ? "#fff5f2" : "#fff9db",
                                                borderRadius: "6px",
                                                cursor: "pointer",
                                            }}
                                        >
                                            <div style={{ fontWeight: "bold", color: "#d9480f" }}>{p.code}</div>
                                            <div style={{ fontSize: "12px", color: "#666" }}>
                                                {p.discountPercent}% OFF (Qty: {p.quantity})
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        )}
                    </div>

                    <div>
                        <div className="checkout-card">
                            <CheckoutSummary
                                subTotal={subTotal}
                                shippingFee={shippingFee}
                                totalPrice={subTotal + shippingFee}
                            />

                            <button
                                className="place-order-btn"
                                onClick={handleOrder}
                                disabled={loading}
                            >
                                {loading ? "Processing Order..." : "Proceed to Payment"}
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}