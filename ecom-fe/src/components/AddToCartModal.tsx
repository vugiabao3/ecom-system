import { useState } from "react";
import { addToCart } from "../services/cartApi";

interface Props {
    productId: string;
    available: number;
    onClose: () => void;
}

export default function AddToCartModal({ productId, available, onClose }: Props) {
    const [count, setCount] = useState(1);
    const [loading, setLoading] = useState(false);

    const handleAdd = async () => {
        setLoading(true);
        try {
            await addToCart({
                productId,
                quantity: count,
            });
            alert("Added to cart successfully!");
            onClose();
        } catch {
            alert("Failed to add to cart.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div
                className="modal-content"
                onClick={(e) => e.stopPropagation()}
                style={{
                    background: "white",
                    padding: "30px",
                    borderRadius: "12px",
                    maxWidth: "400px",
                    width: "90%",
                }}
            >
                <h2 style={{ marginBottom: "16px", color: "#333" }}>Add To Cart</h2>

                <p style={{ margin: "8px 0", color: "#666" }}>
                    Available stock: <strong>{available}</strong>
                </p>

                <div style={{ margin: "20px 0" }}>
                    <label style={{ display: "block", marginBottom: "8px", fontWeight: "600" }}>
                        Quantity:
                    </label>
                    <input
                        type="number"
                        min={1}
                        max={available || 1}
                        value={count}
                        onChange={(e) =>
                            setCount(
                                Math.max(
                                    1,
                                    Math.min(Number(e.target.value) || 1, available || 1)
                                )
                            )
                        }
                        style={{
                            width: "100%",
                            padding: "10px",
                            borderRadius: "6px",
                            border: "1px solid #ddd",
                            fontSize: "16px",
                        }}
                    />
                </div>

                <div style={{ display: "flex", gap: "10px", marginTop: "24px" }}>
                    <button
                        onClick={handleAdd}
                        disabled={loading || available <= 0}
                        style={{
                            flex: 1,
                            padding: "12px",
                            background: "#ee4d2d",
                            color: "white",
                            border: "none",
                            borderRadius: "6px",
                            cursor: "pointer",
                            fontWeight: "bold",
                        }}
                    >
                        {loading ? "Adding..." : "Confirm Add"}
                    </button>

                    <button
                        onClick={onClose}
                        style={{
                            padding: "12px 20px",
                            background: "#f0f0f0",
                            color: "#333",
                            border: "none",
                            borderRadius: "6px",
                            cursor: "pointer",
                        }}
                    >
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    );
}