import {
    useState
} from "react";

import {
    addToCart
} from "../services/cartApi";

export default function AddToCartModal({

    productId,
    available,
    onClose

}: any) {

    const [count, setCount] =
        useState(1);

    const handleAdd = async () => {

        try {

            await addToCart({

                productId,
                quantity: count

            });

            alert(
                "Added to cart successfully"
            );

            onClose();

            window.location.reload();

        } catch (err) {

            console.log(err);

            alert("Add failed");

        }
    };

    return (

        <div className="modal-overlay">

            <div className="modal-box">

                <h2>Add To Cart</h2>

                <p>
                    Available:
                    {" "}
                    {available}
                </p>

                <p>
                    Reserved:
                    {" "}
                    {
                        available > 0
                            ? "Will increase after add"
                            : 0
                    }
                </p>

                <input
                    type="number"
                    min={1}
                    max={available}
                    value={count}
                    onChange={(e) =>
                        setCount(
                            Number(e.target.value)
                        )
                    }
                />

                <div className="modal-actions">

                    <button
                        onClick={handleAdd}
                    >
                        Confirm Add
                    </button>

                    <button
                        onClick={onClose}
                    >
                        Cancel
                    </button>

                </div>

            </div>

        </div>
    );
}