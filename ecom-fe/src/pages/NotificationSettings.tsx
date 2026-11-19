import { useEffect, useState } from "react";
import axios from "axios";
const api = axios.create({ baseURL: import.meta.env.VITE_NOTIFICATION_API_URL || "http://localhost:5039" });
export default function NotificationSettings() {
  const [prefs, setPrefs] = useState<any[]>([]);
  const [msg, setMsg] = useState("");
  useEffect(() => { api.get("/api/notifications/preferences").then(r => setPrefs(r.data)).catch(()=>{}); }, []);
  const save = async () => { await api.put("/api/notifications/preferences", prefs); setMsg("Saved"); };
  return <div><h2>Notification Settings</h2><p>{msg}</p><button onClick={save}>Save</button></div>;
}
