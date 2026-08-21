import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getProductById, type ProductItem } from "../services/productApi";
import { getInventoryByProductId, type InventoryDto } from "../services/inventoryApi";
import { addToCart } from "../services/cartApi";
import Navbar from "../components/Navbar";
import ProductImage from "../components/ProductImage";
import ProductInfo from "../components/ProductInfo";
import AddToCartModal from "../components/AddToCartModal";
import "../styles/product-detail.css";

export default function ProductDetail() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();

    const [product, setProduct] = useState<ProductItem | null>(null);
    const [inventory, setInventory] = useState<InventoryDto | null>(null);
    const [quantity, setQuantity] = useState<number>(1);
    const [showModal, setShowModal] = useState<boolean>(false);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [adding, setAdding] = useState<boolean>(false);

    useEffect(() => {
        if (!id) return;
        setLoading(true);
        setError(null);

        Promise.all([
            getProductById(id).then((res) => setProduct(res.data)),
            getInventoryByProductId(id)
                .then((res) => setInventory(res.data))
                .catch(() => setInventory({ productId: id, available: 0, reserved: 0 })),
        ])
            .catch(() => {
                setError("Could not load product details.");
            })
            .finally(() => {
                setLoading(false);
            });
    }, [id]);

    const availableStock = inventory ? Math.max(inventory.available - inventory.reserved, 0) : 0;

    const handleAddToCartDirect = async () => {
        if (!id) return;
        setAdding(true);
        try {
            await addToCart({
                productId: id,
                quantity: quantity,
            });
            alert(`Added ${quantity} item(s) to your cart!`);
        } catch {
            alert("Failed to add to cart. Please log in or check stock.");
        } finally {
            setAdding(false);
        }
    };

    const handleBuyNow = async () => {
        if (!id) return;
        setAdding(true);
        try {
            await addToCart({
                productId: id,
                quantity: quantity,
            });
            navigate("/checkout");
        } catch {
            alert("Failed to process Buy Now. Please log in or check stock.");
        } finally {
            setAdding(false);
        }
    };

    if (loading) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Loading product...</h2>
                </div>
            </div>
        );
    }

    if (error || !product) {
        return (
            <div>
                <Navbar />
                <div style={{ textAlign: "center", padding: "80px 0" }}>
                    <h2>Product Not Found</h2>
                    <p style={{ color: "#888", marginTop: "8px" }}>{error || "The requested product does not exist."}</p>
                    <button
                        onClick={() => navigate("/")}
                        style={{
                            marginTop: "16px",
                            padding: "10px 20px",
                            background: "#ee4d2d",
                            color: "white",
                            border: "none",
                            borderRadius: "6px",
                            cursor: "pointer",
                        }}
                    >
                        Back to Products
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div>
            <Navbar />
            <div className="product-detail-page" style={{ maxWidth: "1000px", margin: "30px auto", padding: "20px" }}>
                <div className="detail-left">
                    <ProductImage imageUrl={product.imageUrl} />
                </div>

                <div className="detail-right">
                    <ProductInfo product={product} />

                    <div className="inventory-box" style={{ background: "#f8f9fa", padding: "16px", borderRadius: "8px", margin: "16px 0" }}>
                        <p style={{ margin: "4px 0", fontSize: "14px" }}>
                            Total Available Stock: <strong>{inventory?.available ?? 0}</strong>
                        </p>
                        <p style={{ margin: "4px 0", fontSize: "14px", color: "#666" }}>
                            Reserved in carts: <strong>{inventory?.reserved ?? 0}</strong>
                        </p>
                        <p style={{ margin: "6px 0 0", fontSize: "16px", fontWeight: "bold", color: availableStock > 0 ? "#2b8a3e" : "#c92a2a" }}>
                            {availableStock > 0 ? `In Stock (${availableStock} available)` : "Out of Stock"}
                        </p>
                    </div>

                    <div style={{ margin: "20px 0", display: "flex", alignItems: "center", gap: "12px" }}>
                        <label style={{ fontWeight: "600" }}>Quantity:</label>
                        <div style={{ display: "flex", alignItems: "center" }}>
                            <button
                                onClick={() => setQuantity((q) => Math.max(1, q - 1))}
                                disabled={quantity <= 1}
                                style={{ padding: "6px 12px", border: "1px solid #ccc", background: "#f0f0f0", cursor: "pointer", borderRadius: "4px 0 0 4px" }}
                            >
                                -
                            </button>
                            <input
                                type="number"
                                min={1}
                                max={availableStock || 1}
                                value={quantity}
                                onChange={(e) => setQuantity(Math.max(1, Math.min(Number(e.target.value) || 1, availableStock || 1)))}
                                style={{ width: "60px", textAlign: "center", padding: "6px", border: "1px solid #ccc", borderLeft: "none", borderRight: "none" }}
                            />
                            <button
                                onClick={() => setQuantity((q) => Math.min(availableStock || 1, q + 1))}
                                disabled={quantity >= availableStock}
                                style={{ padding: "6px 12px", border: "1px solid #ccc", background: "#f0f0f0", cursor: "pointer", borderRadius: "0 4px 4px 0" }}
                            >
                                +
                            </button>
                        </div>
                    </div>

                    <div style={{ display: "flex", gap: "12px", marginTop: "24px" }}>
                        <button
                            className="add-cart-btn"
                            onClick={handleAddToCartDirect}
                            disabled={availableStock <= 0 || adding}
                            style={{
                                flex: 1,
                                padding: "14px",
                                background: "#fff5f2",
                                color: "#ee4d2d",
                                border: "1px solid #ee4d2d",
                                borderRadius: "8px",
                                fontSize: "16px",
                                fontWeight: "bold",
                                cursor: availableStock <= 0 ? "not-allowed" : "pointer",
                            }}
                        >
                            {adding ? "Adding..." : "🛒 Add To Cart"}
                        </button>

                        <button
                            onClick={handleBuyNow}
                            disabled={availableStock <= 0 || adding}
                            style={{
                                flex: 1,
                                padding: "14px",
                                background: "#ee4d2d",
                                color: "white",
                                border: "none",
                                borderRadius: "8px",
                                fontSize: "16px",
                                fontWeight: "bold",
                                cursor: availableStock <= 0 ? "not-allowed" : "pointer",
                            }}
                        >
                            ⚡ Buy Now
                        </button>
                    </div>
                </div>

                {showModal && (
                    <AddToCartModal
                        productId={product.id}
                        available={availableStock}
                        onClose={() => setShowModal(false)}
                    />
                )}
            </div>
        </div>
    );
}