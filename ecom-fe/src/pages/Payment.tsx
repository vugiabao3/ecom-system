import { useState } from "react";

import { useLocation }
from "react-router-dom";

import { useNavigate }
from "react-router-dom";

import PaymentMethodBox
from "../components/PaymentMethodBox";

import QRPayment
from "../components/QRPayment";

import { createPayment }
from "../services/paymentApi";

import "../styles/payment.css";

export default function Payment() {

    const navigate =
        useNavigate();

    const location =
        useLocation();

    const order =
        location.state;

    const [paymentMethod,
        setPaymentMethod]
        = useState("QR");

    const handlePayment =
        async () => {

            try {

                await createPayment({

                    orderId:
                        order.orderId,

                    amount:
                        order.totalPrice,

                    userId:
                        order.userId,

                    paymentMethod,

                    items:
                        order.items
                });

                navigate(
                    "/payment-success"
                );

            } catch {

                navigate(
                    "/payment-failed"
                );
            }
        };

    return (

        <div
            className="payment-page"
        >

            <h1>
                Payment
            </h1>

            <PaymentMethodBox

                paymentMethod={
                    paymentMethod
                }

                setPaymentMethod={
                    setPaymentMethod
                }
            />

            {paymentMethod === "QR" && (

                <QRPayment

                    amount={
                        order.totalPrice
                    }
                />
            )}

            <button
                onClick={
                    handlePayment
                }
            >
                Pay Now
            </button>

        </div>
    );
}