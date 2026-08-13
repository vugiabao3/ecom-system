type Props = {
    subTotal: number;
    discount: number;
    totalPrice: number;
};

export default function CheckoutSummary({
    subTotal,
    discount,
    totalPrice
}: Props) {

    return (

        <div>

            <h2>
                Order Summary
            </h2>

            <div className="summary-row">

                <span>
                    Subtotal
                </span>

                <span>
                    {subTotal.toLocaleString()} đ
                </span>

            </div>

            <div className="summary-row">

                <span>
                    Discount
                </span>

                <span>
                    -{discount.toLocaleString()} đ
                </span>

            </div>

            <div className="summary-total">

                <span>
                    Total
                </span>

                <span>
                    {totalPrice.toLocaleString()} đ
                </span>

            </div>

        </div>
    );
}