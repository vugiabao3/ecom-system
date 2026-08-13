import {
    useState
} from "react";

import {
    createOrder
} from "../services/orderApi";
import {
    useNavigate
} from "react-router-dom";
import CheckoutForm
from "../components/CheckoutForm";

import CheckoutSummary
from "../components/CheckoutSummary";

import "../styles/checkout.css";

export default function Checkout() {
    const navigate =
        useNavigate();
    const [address,
        setAddress] = useState("");

    const [phone,
        setPhone] = useState("");

    const [receiverName,
        setReceiverName] =
        useState("");

    const [couponCode,
        setCouponCode] =
        useState("");

    const [summary,
        setSummary] =
        useState<any>(null);

    const handleOrder =
        async () => {

            try {

               const res =
    await createOrder({

        address,
        phone,
        receiverName,
        couponCode
    });

console.log(
    "ORDER RESULT:",
    res.data
);

navigate(
    "/payment",
    {
        state: res.data
    }
);

            } catch (err) {

                console.log(err);

                alert(
                    "Create Order Failed"
                );
            }
        };

    return (

        <div className="checkout-page">

            <h1 className="checkout-title">
                Checkout
            </h1>

            <div className="checkout-card">

                <CheckoutForm

                    address={address}
                    phone={phone}
                    receiverName={receiverName}
                    couponCode={couponCode}

                    setAddress={setAddress}
                    setPhone={setPhone}
                    setReceiverName={
                        setReceiverName
                    }
                    setCouponCode={
                        setCouponCode
                    }
                />

            </div>

            {summary && (

                <div className="checkout-card">

                    <CheckoutSummary

                        subTotal={
                            summary.subTotal
                        }

                        discount={
                            summary.discount
                        }

                        totalPrice={
                            summary.totalPrice
                        }
                    />

                </div>

            )}

            <button
                className="place-order-btn"
                onClick={
                    handleOrder
                }
            >
                Place Order
            </button>

        </div>
    );
}