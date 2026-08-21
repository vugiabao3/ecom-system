import type { UserAddressDto } from "../services/userApi";

interface Props {
    address: string;
    phone: string;
    receiverName: string;
    couponCode: string;
    savedAddresses?: UserAddressDto[];
    setAddress: (val: string) => void;
    setPhone: (val: string) => void;
    setReceiverName: (val: string) => void;
    setCouponCode: (val: string) => void;
}

export default function CheckoutForm({
    address,
    phone,
    receiverName,
    couponCode,
    savedAddresses = [],
    setAddress,
    setPhone,
    setReceiverName,
    setCouponCode,
}: Props) {
    const handleSelectAddress = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const selectedId = e.target.value;
        if (!selectedId) return;
        const addr = savedAddresses.find((a) => a.id === selectedId);
        if (addr) {
            setReceiverName(addr.fullName);
            setPhone(addr.phone);
            setAddress(`${addr.addressLine}, ${addr.city}, ${addr.country}`);
        }
    };

    return (
        <div>
            <h2 style={{ fontSize: "20px", color: "#333", marginBottom: "16px" }}>
                📦 Delivery Information
            </h2>

            {savedAddresses.length > 0 && (
                <div style={{ marginBottom: "16px" }}>
                    <label style={{ display: "block", fontSize: "14px", fontWeight: "600", color: "#666", marginBottom: "6px" }}>
                        Select from Saved Addresses:
                    </label>
                    <select
                        onChange={handleSelectAddress}
                        style={{
                            width: "100%",
                            padding: "10px",
                            borderRadius: "8px",
                            border: "1px solid #ddd",
                            background: "#fdfdfd",
                        }}
                    >
                        <option value="">-- Choose a saved address --</option>
                        {savedAddresses.map((a) => (
                            <option key={a.id} value={a.id}>
                                {a.fullName} ({a.phone}) - {a.addressLine}, {a.city}
                            </option>
                        ))}
                    </select>
                </div>
            )}

            <div className="checkout-form">
                <div>
                    <label style={{ display: "block", fontSize: "13px", fontWeight: "600", color: "#555", marginBottom: "4px" }}>
                        Recipient Name *
                    </label>
                    <input
                        value={receiverName}
                        placeholder="e.g. John Doe"
                        onChange={(e) => setReceiverName(e.target.value)}
                        required
                    />
                </div>

                <div>
                    <label style={{ display: "block", fontSize: "13px", fontWeight: "600", color: "#555", marginBottom: "4px" }}>
                        Phone Number *
                    </label>
                    <input
                        value={phone}
                        placeholder="e.g. 0912345678"
                        onChange={(e) => setPhone(e.target.value)}
                        required
                    />
                </div>

                <div>
                    <label style={{ display: "block", fontSize: "13px", fontWeight: "600", color: "#555", marginBottom: "4px" }}>
                        Full Shipping Address *
                    </label>
                    <input
                        value={address}
                        placeholder="e.g. 123 Main St, District 1, Ho Chi Minh City"
                        onChange={(e) => setAddress(e.target.value)}
                        required
                    />
                </div>

                <div>
                    <label style={{ display: "block", fontSize: "13px", fontWeight: "600", color: "#555", marginBottom: "4px" }}>
                        Promo Code / Coupon (Optional)
                    </label>
                    <input
                        value={couponCode}
                        placeholder="e.g. SUMMER20"
                        onChange={(e) => setCouponCode(e.target.value)}
                    />
                </div>
            </div>
        </div>
    );
}