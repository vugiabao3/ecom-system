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
                    getProducts({ Page: 1, PageSize: 12, SortBy: "newest" }),
                    getCategories(),
                ]);
                const products = Array.isArray(productsRes.data?.items) ? productsRes.data.items : Array.isArray(productsRes.data) ? productsRes.data : [];
                setFeaturedProducts(products.slice(0, 8));
                setNewestProducts(products.slice(0, 8));
                setCategories(Array.isArray(categoriesRes.data) ? categoriesRes.data : []);
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
        <div style={{ background: "#f5f5f5", minHeight: "100vh" }}>
            <Navbar />
            <div style={{ maxWidth: "1200px", margin: "0 auto", padding: "24px 16px" }}>
                {/* Hero Banner */}
                <div className="hero-banner-premium">
                    <div className="hero-content">
                        <div className="hero-badge">🔥 HOT DEALS</div>
                        <h1 className="hero-title">Mega Sale Up To 70% Off</h1>
                        <p className="hero-subtitle">Shop the latest trends with exclusive deals. Free shipping on orders over 500K VND</p>
                        <div className="hero-actions">
                            <Link to="/products" className="hero-btn-primary">
                                Shop Now
                            </Link>
                            <Link to="/products" className="hero-btn-secondary">
                                Browse Deals
                            </Link>
                        </div>
                    </div>
                    <div className="hero-image">
                        <div className="hero-product-card">
                            <div className="hero-product-badge">SALE</div>
                            <div className="hero-product-icon">🛍️</div>
                            <div className="hero-product-text">Premium Collection</div>
                        </div>
                    </div>
                </div>

                {/* Categories Section */}
                <div className="section-premium">
                    <div className="section-header">
                        <h2 className="section-title-premium">Shop by Category</h2>
                        <Link to="/products" className="section-link">View All Categories →</Link>
                    </div>
                    {loading ? (
                        <div className="loading-grid">
                            {[1,2,3,4,5,6,7,8].map(i => (
                                <div key={i} className="category-skeleton"></div>
                            ))}
                        </div>
                    ) : !Array.isArray(categories) || categories.length === 0 ? (
                        <div className="empty-state">No categories available</div>
                    ) : (
                        <div className="categories-grid-premium">
                            {categories.map((cat) => (
                                <Link
                                    key={cat.id}
                                    to={`/products?category=${cat.id}`}
                                    className="category-card-premium"
                                >
                                    <div className="category-icon-premium">{CATEGORY_ICONS[cat.name] || "📦"}</div>
                                    <div className="category-name-premium">{cat.name}</div>
                                </Link>
                            ))}
                        </div>
                    )}
                </div>

                {/* Featured Products */}
                <div className="section-premium">
                    <div className="section-header">
                        <h2 className="section-title-premium">✨ Featured Products</h2>
                        <Link to="/products" className="section-link">View All →</Link>
                    </div>
                    {loading ? (
                        <div className="product-grid-premium">
                            {[1,2,3,4].map(i => (
                                <div key={i} className="product-skeleton"></div>
                            ))}
                        </div>
                    ) : !Array.isArray(featuredProducts) || featuredProducts.length === 0 ? (
                        <div className="empty-state">No products available</div>
                    ) : (
                        <div className="product-grid-premium">
                            {featuredProducts.map((p) => (
                                <div key={p.id} className="product-card-premium-wrapper">
                                    <ProductCard product={p} />
                                    {p.rating && p.rating >= 4.5 && (
                                        <div className="product-badge-premium">BEST SELLER</div>
                                    )}
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                {/* New Arrivals */}
                <div className="section-premium">
                    <div className="section-header">
                        <h2 className="section-title-premium">🆕 New Arrivals</h2>
                        <Link to="/products" className="section-link">View All →</Link>
                    </div>
                    {loading ? (
                        <div className="product-grid-premium">
                            {[1,2,3,4].map(i => (
                                <div key={i} className="product-skeleton"></div>
                            ))}
                        </div>
                    ) : !Array.isArray(newestProducts) || newestProducts.length === 0 ? (
                        <div className="empty-state">No products available</div>
                    ) : (
                        <div className="product-grid-premium">
                            {newestProducts.map((p) => (
                                <ProductCard key={p.id} product={p} />
                            ))}
                        </div>
                    )}
                </div>

                {/* Benefits Section */}
                <div className="benefits-section">
                    <div className="benefit-card">
                        <div className="benefit-icon">🚚</div>
                        <div className="benefit-text">
                            <div className="benefit-title">Free Shipping</div>
                            <div className="benefit-desc">On orders over 500K VND</div>
                        </div>
                    </div>
                    <div className="benefit-card">
                        <div className="benefit-icon">🔒</div>
                        <div className="benefit-text">
                            <div className="benefit-title">Secure Payment</div>
                            <div className="benefit-desc">100% secure transactions</div>
                        </div>
                    </div>
                    <div className="benefit-card">
                        <div className="benefit-icon">↩️</div>
                        <div className="benefit-text">
                            <div className="benefit-title">Easy Returns</div>
                            <div className="benefit-desc">7-day return policy</div>
                        </div>
                    </div>
                    <div className="benefit-card">
                        <div className="benefit-icon">💬</div>
                        <div className="benefit-text">
                            <div className="benefit-title">24/7 Support</div>
                            <div className="benefit-desc">Dedicated customer care</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
