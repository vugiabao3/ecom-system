interface Props {
    amount: number;
}

export default function QRPayment({ amount }: Props) {
    return (
        <div
            style={{
                textAlign: "center",
                padding: "20px",
                background: "#f9f9f9",
                borderRadius: "10px",
                border: "1px dashed #ccc",
                margin: "20px 0",
            }}
        >
            <h4 style={{ color: "#333", marginBottom: "8px" }}>Scan QR to complete payment</h4>
            <img
                src={`https://api.qrserver.com/v1/create-qr-code/?size=200x200&data=PAYMENT_AMOUNT_${amount}`}
                alt="Payment QR"
                style={{ borderRadius: "8px", margin: "10px auto", display: "block" }}
            />
            <p style={{ fontSize: "16px", fontWeight: "bold", color: "#ee4d2d", marginTop: "8px" }}>
                Total Amount: {amount.toLocaleString()} đ
            </p>
            <p style={{ fontSize: "12px", color: "#888" }}>
                Open banking app and scan to transfer.
            </p>
        </div>
    );
}