import type { ProductItem } from "../services/productApi";

interface Props {
    product: ProductItem;
}

export default function ProductInfo({ product }: Props) {
    return (
        <div>
            <h1 style={{ fontSize: "28px", color: "#222", marginBottom: "8px" }}>
                {product.name}
            </h1>

            {product.categoryName && (
                <div style={{ marginBottom: "12px" }}>
                    <span
                        style={{
                            background: "#e7f5ff",
                            color: "#1971c2",
                            padding: "4px 10px",
                            borderRadius: "16px",
                            fontSize: "13px",
                            fontWeight: "600",
                        }}
                    >
                        Category: {product.categoryName}
                    </span>
                </div>
            )}

            {product.rating !== undefined && product.rating > 0 && (
                <div style={{ display: "flex", alignItems: "center", gap: "6px", marginBottom: "16px" }}>
                    <span style={{ color: "#f59f00", fontSize: "18px" }}>★</span>
                    <span style={{ fontWeight: "bold", fontSize: "16px" }}>
                        {product.rating.toFixed(1)} / 5.0
                    </span>
                </div>
            )}

            <div style={{ background: "#fafafa", padding: "16px", borderRadius: "8px", margin: "16px 0" }}>
                <span style={{ fontSize: "32px", fontWeight: "bold", color: "#ee4d2d" }}>
                    {product.price ? product.price.toLocaleString() : 0} đ
                </span>
            </div>

            <div style={{ marginTop: "16px" }}>
                <h3 style={{ fontSize: "16px", color: "#555", marginBottom: "6px" }}>Description</h3>
                <p style={{ color: "#666", lineHeight: "1.6" }}>
                    {product.description || "High quality product from our curated catalog."}
                </p>
            </div>
        </div>
    );
}