import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import {
    getCart,
    removeCartItem,
    clearCart
} from "../services/cartApi";

export default function Cart() {

    const navigate = useNavigate(); // ✅ Đặt trong component

    const [cart, setCart] = useState<any>(null);

    const fetchCart = async () => {
        try {
            const res = await getCart();
            setCart(res.data);
        } catch (err) {
            console.log(err);
            setCart({ items: [], totalPrice: 0 });
        }
    };

    useEffect(() => {
        fetchCart();
    }, []);

    const handleRemove = async (productId: string) => {
        await removeCartItem(productId);
        fetchCart();
    };

    const handleClear = async () => {
        await clearCart();
        fetchCart();
    };

    if (!cart) return <h2>Loading...</h2>;

    // =========================
    // GROUP PRODUCTS (Shopee style)
    // =========================
    const groupedItems = cart.items?.reduce((acc: any, item: any) => {

        const key = item.productId;

        if (!acc[key]) {
            acc[key] = {
                ...item
            };
        } else {
            acc[key].quantity += item.quantity;
        }

        return acc;

    }, {});

    const items = Object.values(groupedItems || {});

    return (
        <div style={{ padding: "20px", maxWidth: "900px", margin: "0 auto" }}>

            <h2>🛒 My Cart</h2>

            {items.length === 0 && (
                <p>Cart empty</p>
            )}

            {/* CART LIST */}
            <div>
                {items.map((item: any) => (
                    <div
                        key={item.productId}
                        style={{
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "space-between",
                            border: "1px solid #ddd",
                            padding: "12px",
                            marginBottom: "10px",
                            borderRadius: "8px"
                        }}
                    >

                        {/* LEFT: IMAGE + INFO */}
                        <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>

                            <img
                                src={item.imageUrl || "https://via.placeholder.com/60"}
                                style={{
                                    width: "60px",
                                    height: "60px",
                                    objectFit: "cover",
                                    borderRadius: "6px"
                                }}
                            />

                            <div>
                                <h4 style={{ margin: 0 }}>
                                    {item.productName}
                                </h4>

                                <p style={{ margin: "4px 0", color: "gray" }}>
                                    {item.price?.toLocaleString()} đ
                                </p>
                            </div>

                        </div>

                        {/* RIGHT: QTY + ACTION */}
                        <div style={{
                            display: "flex",
                            alignItems: "center",
                            gap: "20px"
                        }}>

                            <span>
                                Qty: <b>{item.quantity}</b>
                            </span>

                            <button
                                onClick={() =>
                                    handleRemove(item.productId)
                                }
                                style={{
                                    background: "red",
                                    color: "white",
                                    border: "none",
                                    padding: "6px 10px",
                                    borderRadius: "5px",
                                    cursor: "pointer"
                                }}
                            >
                                Delete
                            </button>

                        </div>

                    </div>
                ))}
            </div>

            {/* TOTAL */}
            <hr />

            <h3>
                Total: {cart.totalPrice?.toLocaleString()} đ
            </h3>

            <button
                onClick={handleClear}
                style={{
                    marginTop: "10px",
                    background: "black",
                    color: "white",
                    padding: "10px",
                    borderRadius: "6px"
                }}
            >
                Clear Cart
            </button>
 
            <button
    onClick={() =>
                        navigate("/checkout")
                    }
                >
                    Checkout
                </button>
        </div>
    );
}