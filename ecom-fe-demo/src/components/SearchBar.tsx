import { useState } from "react";

interface SearchBarProps {
    onSearch: (keyword: string) => void;
}

export default function SearchBar({ onSearch }: SearchBarProps) {
    const [keyword, setKeyword] = useState("");

    const handleSearch = () => {
        onSearch(keyword);
    };

    const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") {
            handleSearch();
        }
    };

    return (
        <div
            style={{
                display: "flex",
                gap: "10px",
                marginBottom: "20px",
                width: "100%",
                maxWidth: "600px",
            }}
        >
            <input
                type="text"
                placeholder="Search products by name..."
                value={keyword}
                onChange={(e) => setKeyword(e.target.value)}
                onKeyDown={handleKeyDown}
                style={{
                    flex: 1,
                    padding: "10px 14px",
                    borderRadius: "8px",
                    border: "1px solid #ddd",
                    fontSize: "15px",
                }}
            />

            <button
                onClick={handleSearch}
                style={{
                    padding: "10px 20px",
                    background: "#ee4d2d",
                    color: "white",
                    border: "none",
                    borderRadius: "8px",
                    cursor: "pointer",
                    fontWeight: "600",
                    fontSize: "15px",
                }}
            >
                Search
            </button>
        </div>
    );
}