type Props = {

    paymentMethod: string;

    setPaymentMethod: (
        value: string
    ) => void;
};

export default function PaymentMethodBox({

    paymentMethod,
    setPaymentMethod

}: Props) {

    return (

        <div>

            <h3>
                Payment Method
            </h3>

            <label>

                <input
                    type="radio"
                    checked={
                        paymentMethod === "QR"
                    }
                    onChange={() =>
                        setPaymentMethod(
                            "QR"
                        )
                    }
                />

                QR Payment

            </label>

            <br />

            <label>

                <input
                    type="radio"
                    checked={
                        paymentMethod === "COD"
                    }
                    onChange={() =>
                        setPaymentMethod(
                            "COD"
                        )
                    }
                />

                Cash On Delivery

            </label>

        </div>
    );
}