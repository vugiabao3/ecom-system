type Props = {
    quantity: number;
    setQuantity: any;
    stock: number;
};

export default function ProductOptions({
    quantity,
    setQuantity,
    stock
}: Props) {

    return (

        <div className="product-options">

            <h3>Options</h3>

            <div>

                <label>Color:</label>

                <select>
                    <option>Black</option>
                    <option>White</option>
                </select>

            </div>

            <div>

                <label>Size:</label>

                <select>
                    <option>128GB</option>
                    <option>256GB</option>
                </select>

            </div>

            <div>

                <label>Quantity:</label>

                <input
                    type="number"
                    min={1}
                    max={stock}
                    value={quantity}
                    onChange={(e) =>
                        setQuantity(
                            Number(e.target.value)
                        )
                    }
                />

            </div>

            <p>
                Remaining stock:
                {stock}
            </p>

        </div>
    );
}