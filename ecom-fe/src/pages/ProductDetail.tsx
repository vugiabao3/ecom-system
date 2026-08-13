import {
    useEffect,
    useState
} from "react";

import {
    useParams
} from "react-router-dom";

import {
    getProductById
} from "../services/productApi";

import {
    getInventoryByProductId
} from "../services/inventoryApi";

import ProductImage
from "../components/ProductImage";

import ProductInfo
from "../components/ProductInfo";

import AddToCartModal
from "../components/AddToCartModal";

import "../styles/product-detail.css";

export default function ProductDetail() {

    const { id } = useParams();

    const [product, setProduct] =
        useState<any>(null);

    const [inventory, setInventory] =
        useState<any>(null);

    const [showModal, setShowModal] =
        useState(false);

    useEffect(() => {

        if (id) {

            fetchProduct();
            fetchInventory();
        }

    }, [id]);

    // =========================
    // PRODUCT
    // =========================

    const fetchProduct = async () => {

        try {

            const res =
                await getProductById(id!);

            console.log(
                "PRODUCT DETAIL:",
                res.data
            );

            setProduct(res.data);

        } catch (err) {

            console.log(err);

        }
    };

    // =========================
    // INVENTORY
    // =========================

    const fetchInventory = async () => {

        try {

            const res =
                await getInventoryByProductId(id!);

            console.log(
                "INVENTORY:",
                res.data
            );

            setInventory(res.data);

        } catch (err) {

            console.log(err);

        }
    };

    if (!product || !inventory) {

        return <h1>Loading...</h1>;
    }

    return (

        <div className="product-detail-page">

            {/* LEFT IMAGE */}

            <div className="detail-left">

                <ProductImage
                    imageUrl={
                        product.imageUrl
                    }
                />

            </div>

            {/* RIGHT INFO */}

            <div className="detail-right">

                <ProductInfo
                    product={product}
                />

                <div className="inventory-box">

                    <h3>
                        Available:
                        {" "}
                        {inventory?.available??0}
                    </h3>

                    <h3>
                        Reserved:
                        {" "}
                        {inventory.reserved}
                    </h3>

                </div>

                <button
                    className="add-cart-btn"
                    onClick={() =>
                        setShowModal(true)
                    }
                >
                    Add To Cart
                </button>

            </div>

            {/* MODAL */}

            {
                showModal && (

                    <AddToCartModal
                        productId={product.id}
                        available={
                            inventory?.available??0
                        }
                        onClose={() =>
                            setShowModal(false)
                        }
                    />

                )
            }

        </div>
    );
}