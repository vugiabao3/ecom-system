import { useEffect, useState } from "react";
import { getCategories, type CategoryDto } from "../services/categoryApi";

interface FilterBarProps {
    onFilterChange: (filters: {
        categoryId?: number;
        minPrice?: number;
        maxPrice?: number;
        sortBy?: string;
    }) => void;
}

export default function FilterBar({ onFilterChange }: FilterBarProps) {
    const [categories, setCategories] = useState<CategoryDto[]>([]);
    const [selectedCategory, setSelectedCategory] = useState<number | undefined>();
    const [minPrice, setMinPrice] = useState<string>("");
    const [maxPrice, setMaxPrice] = useState<string>("");
    const [sortBy, setSortBy] = useState<string>("newest");

    useEffect(() => {
        getCategories()
            .then((res) => setCategories(res.data || []))
            .catch(() => setCategories([]));
    }, []);

    const handleApply = () => {
        onFilterChange({
            categoryId: selectedCategory,
            minPrice: minPrice ? Number(minPrice) : undefined,
            maxPrice: maxPrice ? Number(maxPrice) : undefined,
            sortBy,
        });
    };

    const handleReset = () => {
        setSelectedCategory(undefined);
        setMinPrice("");
        setMaxPrice("");
        setSortBy("newest");
        onFilterChange({
            categoryId: undefined,
            minPrice: undefined,
            maxPrice: undefined,
            sortBy: "newest",
        });
    };

    return (
        <div
            style={{
                display: "flex",
                flexWrap: "wrap",
                gap: "12px",
                alignItems: "center",
                background: "white",
                padding: "16px",
                borderRadius: "10px",
                marginBottom: "20px",
                boxShadow: "0 2px 8px rgba(0,0,0,0.05)",
            }}
        >
            <select
                value={selectedCategory ?? ""}
                onChange={(e) => {
                    const val = e.target.value ? Number(e.target.value) : undefined;
                    setSelectedCategory(val);
                    onFilterChange({
                        categoryId: val,
                        minPrice: minPrice ? Number(minPrice) : undefined,
                        maxPrice: maxPrice ? Number(maxPrice) : undefined,
                        sortBy,
                    });
                }}
                style={{ padding: "8px 12px", borderRadius: "6px", border: "1px solid #ddd" }}
            >
                <option value="">All Categories</option>
                {categories.map((c) => (
                    <option key={c.id} value={c.id}>
                        {c.name}
                    </option>
                ))}
            </select>

            <select
                value={sortBy}
                onChange={(e) => {
                    setSortBy(e.target.value);
                    onFilterChange({
                        categoryId: selectedCategory,
                        minPrice: minPrice ? Number(minPrice) : undefined,
                        maxPrice: maxPrice ? Number(maxPrice) : undefined,
                        sortBy: e.target.value,
                    });
                }}
                style={{ padding: "8px 12px", borderRadius: "6px", border: "1px solid #ddd" }}
            >
                <option value="newest">Newest</option>
                <option value="price">Price: Low to High</option>
                <option value="price_desc">Price: High to Low</option>
            </select>

            <div style={{ display: "flex", alignItems: "center", gap: "6px" }}>
                <input
                    type="number"
                    placeholder="Min đ"
                    value={minPrice}
                    onChange={(e) => setMinPrice(e.target.value)}
                    style={{ width: "100px", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                />
                <span>-</span>
                <input
                    type="number"
                    placeholder="Max đ"
                    value={maxPrice}
                    onChange={(e) => setMaxPrice(e.target.value)}
                    style={{ width: "100px", padding: "8px", borderRadius: "6px", border: "1px solid #ddd" }}
                />
                <button
                    onClick={handleApply}
                    style={{
                        padding: "8px 14px",
                        background: "#ee4d2d",
                        color: "white",
                        border: "none",
                        borderRadius: "6px",
                        cursor: "pointer",
                        fontWeight: "600",
                    }}
                >
                    Apply Price
                </button>
            </div>

            <button
                onClick={handleReset}
                style={{
                    padding: "8px 14px",
                    background: "#f0f0f0",
                    color: "#333",
                    border: "none",
                    borderRadius: "6px",
                    cursor: "pointer",
                }}
            >
                Reset
            </button>
        </div>
    );
}