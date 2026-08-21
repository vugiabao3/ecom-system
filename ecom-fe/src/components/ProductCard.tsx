import { Link } from "react-router-dom";
import type { ProductItem } from "../services/productApi";

interface Props {
    product: ProductItem;
    inventory?: {
        available: number;
        reserved: number;
    };
    onClick?: () => void;
}

export default function ProductCard({ product, inventory, onClick }: Props) {
    const stock = inventory
        ? inventory.available - inventory.reserved
        : undefined;

    return (
        <div className="product-card" onClick={onClick}>
            <Link to={`/products/${product.id}`} style={{ textDecoration: "none", color: "inherit" }}>
                <img
                    src={product.imageUrl || "https://dummyimage.com/300x200/eee/999&text=Product"}
                    alt={product.name}
                    style={{
                        width: "100%",
                        height: "180px",
                        objectFit: "cover",
                        borderRadius: "8px",
                    }}
                />

                <h3
                    style={{
                        fontSize: "16px",
                        margin: "10px 0 6px",
                        color: "#333",
                        whiteSpace: "nowrap",
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                    }}
                >
                    {product.name}
                </h3>

                {product.categoryName && (
                    <span
                        style={{
                            fontSize: "12px",
                            color: "#888",
                            background: "#f0f0f0",
                            padding: "2px 6px",
                            borderRadius: "4px",
                            display: "inline-block",
                            marginBottom: "8px",
                        }}
                    >
                        {product.categoryName}
                    </span>
                )}

                <div
                    style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        marginTop: "4px",
                    }}
                >
                    <span
                        style={{
                            fontSize: "18px",
                            fontWeight: "bold",
                            color: "#ee4d2d",
                        }}
                    >
                        {product.price ? product.price.toLocaleString() : 0} đ
                    </span>

                    {product.rating !== undefined && product.rating > 0 && (
                        <span style={{ fontSize: "13px", color: "#f59f00" }}>
                            ★ {product.rating.toFixed(1)}
                        </span>
                    )}
                </div>

                {stock !== undefined && (
                    <p style={{ fontSize: "12px", color: "#666", marginTop: "4px" }}>
                        Stock: {stock > 0 ? stock : "Out of stock"}
                    </p>
                )}
            </Link>

            <Link
                to={`/products/${product.id}`}
                style={{
                    display: "block",
                    textAlign: "center",
                    marginTop: "10px",
                    padding: "8px",
                    background: "#fdf2e9",
                    color: "#d9480f",
                    borderRadius: "6px",
                    textDecoration: "none",
                    fontWeight: "600",
                    fontSize: "14px",
                }}
            >
                View Details
            </Link>
        </div>
    );
}