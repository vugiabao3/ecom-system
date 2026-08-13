type Props = {

    address: string;
    phone: string;
    receiverName: string;
    couponCode: string;

    setAddress: any;
    setPhone: any;
    setReceiverName: any;
    setCouponCode: any;
};

export default function CheckoutForm({
    address,
    phone,
    receiverName,
    couponCode,
    setAddress,
    setPhone,
    setReceiverName,
    setCouponCode
}: Props) {

    return (

        <div>

            <h2>
                Shipping Info
            </h2>
<div className="checkout-form"></div>
            <input
                value={receiverName}
                placeholder="Receiver Name"
                onChange={(e) =>
                    setReceiverName(
                        e.target.value
                    )
                }
            />

            <input
                value={phone}
                placeholder="Phone"
                onChange={(e) =>
                    setPhone(
                        e.target.value
                    )
                }
            />

            <input
                value={address}
                placeholder="Address"
                onChange={(e) =>
                    setAddress(
                        e.target.value
                    )
                }
            />

            <input
                value={couponCode}
                placeholder="Coupon"
                onChange={(e) =>
                    setCouponCode(
                        e.target.value
                    )
                }
            />

        </div>
    );
}