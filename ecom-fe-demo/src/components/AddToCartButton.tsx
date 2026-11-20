import { useState }
from "react";

import { addToCart }
from "../services/cartApi";

export default function AddToCartButton({
    productId,
    available
}: any) {

    const [count, setCount] =
        useState(1);

    const handleAddToCart =
        async () => {

        try {

            if (count > available) {

                alert(
                    "Not enough stock"
                );

                return;
            }

            const payload = {

                productId,

                quantity: count

            };

            await addToCart(payload);

            alert(
                "Added to cart"
            );

            window.location.reload();

        } catch (err) {

            console.log(err);

            alert("Add failed");
        }
    };

    return (

        <div className="cart-box">

            <h3>
                Available:
                {available}
            </h3>

            <div className="count-box">

                <span>
                    Quantity
                </span>

                <div
                    className="count-actions"
                >

                    <button
                        onClick={() =>
                            setCount(
                                count - 1
                            )
                        }

                        disabled={
                            count <= 1
                        }
                    >
                        -
                    </button>

                    <span>
                        {count}
                    </span>

                    <button
                        onClick={() =>
                            setCount(
                                count + 1
                            )
                        }

                        disabled={
                            count >= available
                        }
                    >
                        +
                    </button>

                </div>

            </div>

            <button
                className="add-cart-btn"

                onClick={
                    handleAddToCart
                }
            >
                Add To Cart
            </button>

        </div>
    );
}