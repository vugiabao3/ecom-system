import { useEffect, useState } from "react";
import { getCart } from "../services/cartApi";
import { Link } from "react-router-dom";

export default function CartIcon() {

    const [count, setCount] = useState(0);

    const fetchCart = async () => {
        const res = await getCart();
        setCount(res.data.items?.length || 0);
    };

    useEffect(() => {
        fetchCart();
    }, []);

    return (
        <Link to="/cart">
            🛒 Cart ({count})
        </Link>
    );
}