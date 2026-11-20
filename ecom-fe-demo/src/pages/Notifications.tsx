import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import { demoStore, isDemoMode } from "../demo/store";

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

function buildDemoNotifications(): Notification[] {
  const store = demoStore.load();
  const session = demoStore.loadSession();
  const userId = session?.userId;
  const orders = store.orders.filter((o) => !userId || o.userId === userId);

  const orderNotifications: Notification[] = orders.slice(0, 5).map((order) => ({
    id: `notif-${order.id}`,
    title: `Order ${order.orderNumber}`,
    message: `Your order is currently ${order.status.toLowerCase()}. Total ${order.totalPrice.toLocaleString("vi-VN")} đ.`,
    type: "order",
    isRead: false,
    createdAt: order.createdAt,
    navigationUrl: `/orders/me/${order.id}`,
  }));

  const activityNotifications: Notification[] = store.activities
    .filter((activity) => !userId || activity.userId === userId)
    .slice(0, 3)
    .map((activity) => ({
      id: `notif-${activity.id}`,
      title: activity.action,
      message: activity.description || activity.action,
      type: "account",
      isRead: true,
      createdAt: activity.createdAt,
    }));

  return [...orderNotifications, ...activityNotifications].sort(
    (a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
  );
}

export default function Notifications() {
  const [items, setItems] = useState<Notification[]>([]);
  const [loading, setLoading] = useState(true);

  const load = async () => {
    setLoading(true);
    try {
      if (isDemoMode()) {
        setItems(buildDemoNotifications());
        return;
      }
      const res = await api.get<Notification[]>("/api/notifications");
      setItems(res.data);
    } catch {
      setItems([]);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const markRead = async (id: string) => {
    if (!isDemoMode()) {
      try {
        await api.patch(`/api/notifications/${id}/read`);
      } catch {
        return;
      }
    }
    setItems(prev => prev.map(n => n.id === id ? { ...n, isRead: true } : n));
  };

  const markAllRead = async () => {
    if (isDemoMode()) {
      setItems(prev => prev.map(n => ({ ...n, isRead: true })));
      return;
    }
    try {
      await api.post("/api/notifications/read-all");
    } catch {
      // keep the current view if the notification service is unavailable
    }
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
