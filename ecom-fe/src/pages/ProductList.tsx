import { useEffect, useState, useCallback } from "react";
import ProductCard from "../components/ProductCard";
import SearchBar from "../components/SearchBar";
import FilterBar from "../components/FilterBar";
import Navbar from "../components/Navbar";
import { getProducts, searchProducts, type ProductItem } from "../services/productApi";
import "../styles/product.css";

export default function ProductList() {
    const [products, setProducts] = useState<ProductItem[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    // Filters & Pagination state
    const [page, setPage] = useState<number>(1);
    const [pageSize] = useState<number>(12);
    const [totalCount, setTotalCount] = useState<number>(0);
    const [keyword, setKeyword] = useState<string>("");
    const [filters, setFilters] = useState<{
        categoryId?: number;
        minPrice?: number;
        maxPrice?: number;
        sortBy?: string;
    }>({ sortBy: "newest" });

    const fetchProducts = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            if (keyword.trim()) {
                const res = await searchProducts({
                    keyword: keyword.trim(),
                    categoryId: filters.categoryId,
                    minPrice: filters.minPrice,
                    maxPrice: filters.maxPrice,
                    page,
                    pageSize,
                    sortBy: filters.sortBy || "relevance",
                });
                setProducts(res.data?.items || []);
                setTotalCount(res.data?.totalCount || 0);
            } else {
                const res = await getProducts({
                    Page: page,
                    PageSize: pageSize,
                    CategoryId: filters.categoryId,
                    MinPrice: filters.minPrice,
                    MaxPrice: filters.maxPrice,
                    SortBy: filters.sortBy || "newest",
                });
                setProducts(res.data?.items || []);
                setTotalCount(res.data?.totalCount || 0);
            }
        } catch (err: any) {
            setError("Failed to load products. Please check the backend connection.");
            setProducts([]);
        } finally {
            setLoading(false);
        }
    }, [page, pageSize, keyword, filters]);

    useEffect(() => {
        fetchProducts();
    }, [fetchProducts]);

    const handleSearch = (newKeyword: string) => {
        setKeyword(newKeyword);
        setPage(1);
    };

    const handleFilterChange = (newFilters: typeof filters) => {
        setFilters(newFilters);
        setPage(1);
    };

    const totalPages = Math.ceil(totalCount / pageSize) || 1;

    return (
        <div>
            <Navbar />
            <div className="product-page" style={{ maxWidth: "1200px", margin: "0 auto" }}>
                <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", flexWrap: "wrap" }}>
                    <SearchBar onSearch={handleSearch} />
                </div>

                <FilterBar onFilterChange={handleFilterChange} />

                {error && (
                    <div style={{ padding: "16px", background: "#ffe3e3", color: "#e03131", borderRadius: "8px", marginBottom: "20px" }}>
                        {error}
                    </div>
                )}

                {loading ? (
                    <div style={{ textAlign: "center", padding: "60px 0", fontSize: "18px", color: "#666" }}>
                        Loading products...
                    </div>
                ) : products.length === 0 ? (
                    <div style={{ textAlign: "center", padding: "60px 0", background: "white", borderRadius: "12px" }}>
                        <h2 style={{ color: "#666", marginBottom: "8px" }}>No products found</h2>
                        <p style={{ color: "#999" }}>Try adjusting your search or filters.</p>
                    </div>
                ) : (
                    <>
                        <div className="product-grid">
                            {products.map((p) => (
                                <ProductCard key={p.id} product={p} />
                            ))}
                        </div>

                        {/* Pagination */}
                        <div
                            style={{
                                display: "flex",
                                justifyContent: "center",
                                alignItems: "center",
                                gap: "8px",
                                marginTop: "30px",
                                marginBottom: "40px",
                            }}
                        >
                            <button
                                onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
                                disabled={page === 1}
                                style={{
                                    padding: "8px 16px",
                                    borderRadius: "6px",
                                    border: "1px solid #ddd",
                                    background: page === 1 ? "#f5f5f5" : "white",
                                    cursor: page === 1 ? "not-allowed" : "pointer",
                                }}
                            >
                                Previous
                            </button>

                            <span style={{ fontSize: "14px", color: "#666", padding: "0 8px" }}>
                                Page <strong>{page}</strong> of <strong>{totalPages}</strong> ({totalCount} items)
                            </span>

                            <button
                                onClick={() => setPage((prev) => Math.min(prev + 1, totalPages))}
                                disabled={page >= totalPages}
                                style={{
                                    padding: "8px 16px",
                                    borderRadius: "6px",
                                    border: "1px solid #ddd",
                                    background: page >= totalPages ? "#f5f5f5" : "white",
                                    cursor: page >= totalPages ? "not-allowed" : "pointer",
                                }}
                            >
                                Next
                            </button>
                        </div>
                    </>
                )}
            </div>
        </div>
    );
}