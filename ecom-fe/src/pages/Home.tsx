import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Navbar from "../components/Navbar";
import ProductCard from "../components/ProductCard";
import { getProducts, type ProductItem } from "../services/productApi";
import { getCategories, type CategoryDto } from "../services/categoryApi";
import "../styles/home.css";

const CATEGORY_ICONS: Record<string, string> = {
    "Electronics": "📱",
    "Clothing": "👕",
    "Books": "📚",
    "Home": "🏠",
    "Sports": "⚽",
    "Beauty": "💄",
    "Toys": "🧸",
    "Food": "🍔",
};

export default function Home() {
    const [featuredProducts, setFeaturedProducts] = useState<ProductItem[]>([]);
    const [newestProducts, setNewestProducts] = useState<ProductItem[]>([]);
    const [categories, setCategories] = useState<CategoryDto[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadData = async () => {
            setLoading(true);
            try {
                const [productsRes, categoriesRes] = await Promise.all([
                    getProducts({ Page: 1, PageSize: 12, SortBy: "rating" }),
                    getCategories(),
                ]);
                setFeaturedProducts(productsRes.data?.items || []);
                setNewestProducts([...(productsRes.data?.items || [])].reverse().slice(0, 8));
                setCategories(categoriesRes.data || []);
            } catch {
                setFeaturedProducts([]);
                setNewestProducts([]);
                setCategories([]);
            } finally {
                setLoading(false);
            }
        };
        loadData();
    }, []);

    return (
        <div>
            <Navbar />
            <div style={{ maxWidth: "1200px", margin: "0 auto", padding: "20px" }}>
                {/* Hero Banner */}
                <div className="hero-banner">
                    <h1>Summer Sale is Live! 🎉</h1>
                    <p>Get up to 50% off on top brands. Free shipping on orders over 500K.</p>
                    <Link to="/products" className="hero-btn">
                        Shop Now →
                    </Link>
                </div>

                {/* Categories Grid */}
                <div className="section">
                    <div className="section-title">
                        <span>Shop by Category</span>
                        <Link to="/products">View All →</Link>
                    </div>
                    {loading ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#666" }}>Loading categories...</p>
                    ) : categories.length === 0 ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#888" }}>No categories found.</p>
                    ) : (
                        <div className="categories-grid">
                            {categories.map((cat) => (
                                <Link
                                    key={cat.id}
                                    to={`/products?category=${cat.id}`}
                                    className="category-card"
                                >
                                    <div className="cat-icon">{CATEGORY_ICONS[cat.name] || "📦"}</div>
                                    <div className="cat-name">{cat.name}</div>
                                </Link>
                            ))}
                        </div>
                    )}
                </div>

                {/* Featured Products */}
                <div className="section">
                    <div className="section-title">
                        <span>⭐ Featured Products</span>
                        <Link to="/products">View All →</Link>
                    </div>
                    {loading ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#666" }}>Loading products...</p>
                    ) : featuredProducts.length === 0 ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#888" }}>No products found.</p>
                    ) : (
                        <div className="product-grid">
                            {featuredProducts.map((p) => (
                                <div key={p.id} className="product-card-wrapper">
                                    <ProductCard product={p} />
                                    {p.rating && p.rating >= 4.5 && (
                                        <div className="discount-badge">BESTSELLER</div>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                {/* Newest Products */}
                <div className="section">
                    <div className="section-title">
                        <span>🆕 Newest Arrivals</span>
                        <Link to="/products">View All →</Link>
                    </div>
                    {loading ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#666" }}>Loading products...</p>
                    ) : newestProducts.length === 0 ? (
                        <p style={{ textAlign: "center", padding: "40px", color: "#888" }}>No products found.</p>
                    ) : (
                        <div className="product-grid">
                            {newestProducts.map((p) => (
                                <div key={p.id} className="product-card-wrapper">
                                    <ProductCard product={p} />
                                </div>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}
