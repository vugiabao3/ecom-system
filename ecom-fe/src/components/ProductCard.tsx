import { Link } from "react-router-dom";

type Props = {
    product: any;

    inventory?: {
        available: number;
        reserved: number;
    };

    onClick?: () => void;
};

export default function ProductCard({
    product,
    inventory,
    onClick
}: Props) {

    const stock =
        inventory
            ? inventory.available - inventory.reserved
            : 0;

    return (

        <div
            className="product-card"
            onClick={onClick}
        >

            <img
                src={
                    product.imageUrl ||
                    "https://via.placeholder.com/200"
                }
                alt={product.name}
            />

            <h3>{product.name}</h3>

            <p>{product.categoryName}</p>

            <p>
                {
                    product.price
                        ? product.price.toLocaleString()
                        : 0
                } đ
            </p>

            <p>
                Stock: {stock}
            </p>

            <Link to={`/products/${product.id}`}>
                View Detail
            </Link>

        </div>
    );
}