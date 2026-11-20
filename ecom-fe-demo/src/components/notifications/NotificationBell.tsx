import { useEffect, useState } from "react";
import axios from "axios";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { isDemoMode } from "../../demo/store";
const api = axios.create({ baseURL: import.meta.env.VITE_NOTIFICATION_API_URL || "http://localhost:5039" });
api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});
type Notification = { id: string; title: string; message: string; type: string; isRead: boolean; createdAt: string; navigationUrl?: string };
export default function NotificationBell() {
  const [open, setOpen] = useState(false);
  const [items, setItems] = useState<Notification[]>([]);
  const [count, setCount] = useState(0);
  const [loading, setLoading] = useState(false);
  if (isDemoMode()) return null;
  const load = async () => {
    setLoading(true);
    try {
      const [list, cnt] = await Promise.all([
        api.get<Notification[]>("/api/notifications", { params: { page: 1, pageSize: 10 } }),
        api.get<{ count: number }>("/api/notifications/unread-count"),
      ]);
      setItems(list.data); setCount(cnt.data.count);
    } catch {} finally { setLoading(false); }
  };
  useEffect(() => {
    if (!localStorage.getItem("token")) return;
    load();
    const conn = new HubConnectionBuilder().withUrl(`${import.meta.env.VITE_SIGNALR_URL||"http://localhost:5039/hubs/notifications"}`, { accessTokenFactory: () => localStorage.getItem("token") || "" }).configureLogging(LogLevel.None).withAutomaticReconnect().build();
    conn.on("UnreadCountUpdated", (n: number) => setCount(n));
conn.on("ReceiveNotification", () => load());
    conn.onreconnected(() => { conn.invoke("JoinGroup").catch(()=>{}); load(); });
    conn.start().catch(()=>{});
    return () => { conn.stop(); };
  }, []);
  const markRead = async (id: string) => {
    await api.patch(`/api/notifications/${id}/read`);
    setItems(items.map(n => n.id === id ? { ...n, isRead: true } : n));
    setCount(c => Math.max(0, c - 1));
  };
  return (
    <div style={{ position: "relative" }}>
      <button className="navbar-bell" onClick={() => { setOpen(!open); if (!open) load(); }} aria-label="Notifications">??{count > 0 && <span className="navbar-bell-badge">{count > 99 ? "99+" : count}</span>}</button>
      {open && (
        <div className="notification-dropdown" role="dialog">
          <div className="notification-dropdown-header"><strong>Notifications</strong><span>{count} unread</span></div>
          {loading ? <div className="notification-skeleton">Loading.</div> : items.length === 0 ? <div className="notification-empty">No notifications</div> : (
            <ul>{items.map(n => <li key={n.id} className={n.isRead ? "" : "unread"} onClick={() => markRead(n.id)}>
              <div className="notification-title">{n.title || n.type}</div><div className="notification-message">{n.message}</div><div className="notification-time">{new Date(n.createdAt).toLocaleString()}</div>
</li>)}</ul>
          )}
          <div className="notification-dropdown-header" style={{ borderTop: "1px solid #eee", borderBottom: "none", justifyContent: "center" }}>
            <a href="/notifications" onClick={() => setOpen(false)} style={{ textDecoration: "none", color: "#ee4d2d", fontWeight: 700 }}>View All</a>
          </div>
        </div>
      )}
    </div>
  );
}
