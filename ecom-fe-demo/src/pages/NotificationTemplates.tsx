import { useEffect, useState } from "react";
import axios from "axios";
const api = axios.create({ baseURL: import.meta.env.VITE_NOTIFICATION_API_URL || "http://localhost:5039" });
export default function NotificationTemplates() {
  const [items, setItems] = useState<any[]>([]);
  useEffect(() => { api.get("/api/admin/notification-templates").then(r => setItems(r.data)).catch(()=>{}); }, []);
  return <div><h2>Notification Templates</h2><ul>{items.map(t => <li key={t.id}>{t.templateKey} - {t.category} - {t.channel} - {t.active ? "Active" : "Inactive"}</li>)}</ul></div>;
}
