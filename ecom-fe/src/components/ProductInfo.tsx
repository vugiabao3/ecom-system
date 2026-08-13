

export default function ProductInfo({
    product
}: any) {

    return (

        <div>

            <h1>
                {product.name}
            </h1>

            <h2>
                {
                    product.price?.toLocaleString()
                } đ
            </h2>

            <p>
                Category:
                {product.categoryName}
            </p>

            <p>
                Description:
                {
                    product.description ||
                    "No description"
                }
            </p>

        </div>
    );
}