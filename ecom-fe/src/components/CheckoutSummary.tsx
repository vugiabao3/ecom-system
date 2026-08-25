interface Props {
    subTotal: number;
    shippingFee?: number;
    discount?: number;
    totalPrice: number;
}

export default function CheckoutSummary({
    subTotal,
    shippingFee = 0,
    discount = 0,
    totalPrice,
}: Props) {
    return (
        <div>
            <h2 style={{ fontSize: "20px", color: "#333", marginBottom: "16px" }}>
                Order Summary
            </h2>

            <div className="summary-row">
                <span style={{ color: "#666" }}>Subtotal</span>
                <span style={{ fontWeight: "600" }}>{subTotal.toLocaleString()} đ</span>
            </div>

            {shippingFee > 0 && (
                <div className="summary-row">
                    <span style={{ color: "#666" }}>Shipping Fee</span>
                    <span style={{ fontWeight: "600" }}>{shippingFee.toLocaleString()} đ</span>
                </div>
            )}

            {discount > 0 && (
                <div className="summary-row" style={{ color: "#2b8a3e" }}>
                    <span>Promotion Discount</span>
                    <span>-{discount.toLocaleString()} đ</span>
                </div>
            )}

            <hr style={{ border: "none", borderTop: "1px solid #eee", margin: "16px 0" }} />

            <div className="summary-total">
                <span>Total Payment</span>
                <span>{totalPrice.toLocaleString()} đ</span>
            </div>
        </div>
    );
}