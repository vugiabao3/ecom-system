export default function FilterBar({
    onFilter
}: any) {

    return (

        <div className="filter-bar">

            <select
                onChange={(e) =>
                    onFilter(e.target.value)
                }
            >

                <option value="">
                    Sort Product
                </option>

                <option value="price_asc">
                    Price Low → High
                </option>

                <option value="price_desc">
                    Price High → Low
                </option>

                <option value="newest">
                    Newest
                </option>

            </select>

        </div>
    );
}