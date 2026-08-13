import { useState } from "react";

export default function SearchBar({ onSearch }: any) {

    const [keyword, setKeyword] =
        useState("");

    return (

        <div>

            <input
                type="text"
                placeholder="Search products..."
                value={keyword}
                onChange={(e) =>
                    setKeyword(e.target.value)
                }
            />

            <button
                onClick={() =>
                    onSearch(keyword)
                }
            >
                Search
            </button>

        </div>
    );
}