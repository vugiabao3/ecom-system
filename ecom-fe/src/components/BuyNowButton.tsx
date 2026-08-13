type Props = {
    onBuy: () => void;
    disabled?: boolean;
    product?: any;
};

export default function BuyNowButton({
    onBuy,
    disabled
}: Props) {

    return (

        <button
            className="buy-now-btn"
            onClick={onBuy}
            disabled={disabled}
        >
            Buy Now
        </button>
    );
}