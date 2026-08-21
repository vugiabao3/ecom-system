interface Props {
    paymentMethod: string;
    setPaymentMethod: (value: string) => void;
}

export default function PaymentMethodBox({ paymentMethod, setPaymentMethod }: Props) {
    const methods = [
        { id: "QR", label: "📱 QR Code Payment", desc: "Scan to pay instantly via banking app" },
        { id: "COD", label: "💵 Cash On Delivery", desc: "Pay with cash upon package receipt" },
        { id: "VNPAY", label: "💳 VNPay", desc: "Pay via VNPay gateway / ATM card" },
        { id: "MOMO", label: "👛 MoMo e-Wallet", desc: "Pay quickly using your MoMo account" },
    ];

    return (
        <div>
            <h3 style={{ fontSize: "16px", color: "#444", marginBottom: "12px" }}>
                Select Payment Method
            </h3>

            <div style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
                {methods.map((m) => (
                    <label
                        key={m.id}
                        onClick={() => setPaymentMethod(m.id)}
                        style={{
                            display: "flex",
                            alignItems: "center",
                            gap: "12px",
                            padding: "14px 16px",
                            borderRadius: "8px",
                            border: paymentMethod === m.id ? "2px solid #ee4d2d" : "1px solid #e0e0e0",
                            background: paymentMethod === m.id ? "#fff5f2" : "#ffffff",
                            cursor: "pointer",
                            transition: "all 0.2s ease",
                        }}
                    >
                        <input
                            type="radio"
                            name="paymentMethod"
                            checked={paymentMethod === m.id}
                            onChange={() => setPaymentMethod(m.id)}
                        />
                        <div>
                            <div style={{ fontWeight: "600", color: "#333", fontSize: "15px" }}>
                                {m.label}
                            </div>
                            <div style={{ fontSize: "12px", color: "#888" }}>{m.desc}</div>
                        </div>
                    </label>
                ))}
            </div>
        </div>
    );
}