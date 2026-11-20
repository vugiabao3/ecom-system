export default function ProductImage({
    imageUrl
}: any) {

    return (

        <img
            src={
                imageUrl ||
                "https://dummyimage.com/200x200/cccccc/000000&text=No+Image"
            }
            alt="product"
        />

    );
}