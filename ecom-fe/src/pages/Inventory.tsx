import { useEffect, useState } from "react";
import { getInventoryList } from "../services/inventoryApi";

export default function Inventory() {
  const [items, setItems] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    setLoading(true);
    getInventoryList()
      .then((res) => setItems(Array.isArray(res.data) ? res.data : res.data.items ?? []))
      .catch(() => setError("Failed to load inventory"))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div>
      <h1>Inventory</h1>
      {loading && <p>Loading...</p>}
      {error && <p>{error}</p>}
      {!loading && !error && items.length === 0 && <p>No inventory found.</p>}
      <ul>
        {items.map((item, index) => (
          <li key={index}>{JSON.stringify(item)}</li>
        ))}
      </ul>
    </div>
  );
}
