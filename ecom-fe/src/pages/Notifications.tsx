import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

const api = axios.create({ baseURL: import.meta.env.VITE_NOTIFICATION_API_URL || "http://localhost:5039" });
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

type Notification = {
  id: string;
  title: string;
  message: string;
  type: string;
  isRead: boolean;
  createdAt: string;
  navigationUrl?: string;
};

export default function Notifications() {
  const [items, setItems] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(true);

  const load = async () => {
    setLoading(true);
    try {
      const res = await api.get<Notification[]>("/api/notifications");
      setItems(res.data);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const markRead = async (id: string) => {
    await api.patch(`/api/notifications/${id}/read`);
    setItems(prev => prev.map(n => n.id === id ? { ...n, isRead: true } : n));
  };

  const markAllRead = async () => {
    await api.post("/api/notifications/read-all");
    await load();
  };

  return (
    <div style={{ maxWidth: 800, margin: "24px auto", padding: "0 16px" }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 16 }}>
        <h1 style={{ margin: 0 }}>Notifications</h1>
        <div>
          <button onClick={markAllRead} style={{ marginRight: 8 }}>Mark all read</button>
          <Link to="/profile" className="navbar-btn outline">Profile</Link>
        </div>
      </div>
      {loading ? <div>Loading...</div> : items.length === 0 ? <div>No notifications</div> : (
        <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
          {items.map(n => (
            <li key={n.id} onClick={() => markRead(n.id)}
              style={{ border: "1px solid #e5e7eb", borderRadius: 12, padding: 12, marginBottom: 10, cursor: "pointer", background: n.isRead ? "#fff" : "#fff7f2" }}>
              <div style={{ display: "flex", justifyContent: "space-between", gap: 12 }}>
                <strong>{n.title || n.type}</strong>
                <span style={{ color: "#888", fontSize: 12 }}>{new Date(n.createdAt).toLocaleString()}</span>
              </div>
              <div style={{ color: "#444", marginTop: 6 }}>{n.message}</div>
              {n.navigationUrl && <Link to={n.navigationUrl} style={{ fontSize: 12 }}>View</Link>}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}