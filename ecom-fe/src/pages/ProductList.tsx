import { useEffect, useState }
from "react";

import ProductCard
from "../components/ProductCard";

import SearchBar
from "../components/SearchBar";

import FilterBar
from "../components/FilterBar";

import "../styles/product.css";

import {
    getProducts,
    searchProducts
} from "../services/productApi";

export default function ProductList() {

    const [products, setProducts] =
        useState<any[]>([]);

    useEffect(() => {

        fetchProducts();

    }, []);

    // =========================
    // LOAD ALL PRODUCTS
    // =========================

    const fetchProducts = async () => {

        try {

            const res = await getProducts({

                Page: 1,
                PageSize: 100

            });

            console.log(
                "ALL PRODUCTS:",
                res.data
            );

            setProducts(
                res.data.items
            );

        } catch (err) {

            console.log(err);

        }
    };

    // =========================
    // SEARCH PRODUCTS
    // =========================

    const handleSearch = async (
    keyword: string
) => {

    try {

        console.log("SEARCH KEYWORD:", keyword);

        // nếu rỗng -> load lại toàn bộ
        if (!keyword.trim()) {

            fetchProducts();

            return;
        }

        const res =
            await searchProducts(keyword);

        console.log(
            "SEARCH RESPONSE:",
            res.data
        );

        setProducts(res.data.items);

    } catch (err) {

        console.log(
            "SEARCH ERROR:",
            err
        );

    }
};

    // =========================
    // FILTER PRODUCTS
    // =========================

    const handleFilter = async (
        sort: string
    ) => {

        try {

            const res =
                await getProducts({

                    Page: 1,
                    PageSize: 100,
                    ...(sort ? { SortBy: sort } : {})

                });

            setProducts(
                res.data.items
            );

        } catch (err) {

            console.log(err);

        }
    };

    return (

        <div className="product-page">

            <SearchBar
                onSearch={handleSearch}
            />

            <FilterBar
                onFilter={handleFilter}
            />

            <div className="product-grid">

                {products.length === 0 ? (

                    <h2>
                        No products found
                    </h2>

                ) : (

                    products.map((p) => (

                        <ProductCard
                            key={p.id}
                            product={p}
                        />

                    ))

                )}

            </div>

        </div>
    );
}