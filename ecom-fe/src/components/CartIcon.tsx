import { useEffect, useState } from "react";
import { getCart } from "../services/cartApi";
import { Link } from "react-router-dom";
import { getToken } from "../utils/token";

export default function CartIcon() {
    const [count, setCount] = useState<number>(0);

    const fetchCart = async () => {
        if (!getToken()) {
            setCount(0);
            return;
        }
        try {
            const res = await getCart();
            const items = res.data?.items || [];
            const totalQty = items.reduce((sum: number, item: any) => sum + (item.quantity || 1), 0);
            setCount(totalQty);
        } catch {
            setCount(0);
        }
    };

    useEffect(() => {
        fetchCart();
    }, []);

    return (
        <Link
            to="/cart"
            style={{
                display: "flex",
                alignItems: "center",
                gap: "6px",
                textDecoration: "none",
                color: "#333",
                fontWeight: "600",
                background: "#f8f9fa",
                padding: "8px 14px",
                borderRadius: "20px",
                border: "1px solid #e9ecef",
            }}
        >
            <span>🛒 Cart</span>
            <span
                style={{
                    background: "#ee4d2d",
                    color: "white",
                    borderRadius: "10px",
                    padding: "2px 8px",
                    fontSize: "12px",
                    fontWeight: "bold",
                }}
            >
                {count}
            </span>
        </Link>
    );
}