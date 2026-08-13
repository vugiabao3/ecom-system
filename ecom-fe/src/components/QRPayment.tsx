type Props = {

    amount: number;
};

export default function QRPayment({

    amount

}: Props) {

    return (

        <div>

            <h3>
                Scan QR To Pay
            </h3>

            <img
                src="/qr-demo.png"
                alt="qr"
                width="250"
            />

            <p>

                Amount:

                {amount.toLocaleString()}
                đ

            </p>

        </div>
    );
}