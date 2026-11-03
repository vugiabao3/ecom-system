
const STATUS_STYLES: Record<string, { bg: string; fg: string; label: string }> = {
    PENDING: { bg: "#fff3e0", fg: "#e67e22", label: "Pending" },
    CONFIRMED: { bg: "#e8f5e9", fg: "#2e7d5e", label: "Confirmed" },
    PREPARING: { bg: "#e8f5e9", fg: "#2e7d5e", label: "Preparing" },
    READYFORSHIPMENT: { bg: "#e3f2fd", fg: "#1565c0", label: "Ready to Ship" },
    SHIPPING: { bg: "#e3f2fd", fg: "#1565c0", label: "Shipping" },
    DELIVERED: { bg: "#e8f5e9", fg: "#2e7d5e", label: "Delivered" },
    RETURNED: { bg: "#ffebee", fg: "#c62828", label: "Returned" },
    CANCELLED: { bg: "#ffebee", fg: "#c62828", label: "Cancelled" },
    DELIVERYFAILED: { bg: "#ffebee", fg: "#c62828", label: "Delivery Failed" },
};

const fallbackStyle = { bg: "#f5f5f5", fg: "#666", label: "Unknown" };

type Props = {
    status: string;
    size?: "sm" | "md";
};

export default function OrderStatusBadge({ status, size = "md" }: Props) {
    const key = (status || "").toUpperCase();
    const style = STATUS_STYLES[key] ?? fallbackStyle;
    const label = STATUS_STYLES[key]?.label ?? status;

    const padding = size === "sm" ? "2px 8px" : "4px 12px";
    const fontSize = size === "sm" ? "10px" : "11px";

    return (
        <span
            style={{
                display: "inline-flex",
                alignItems: "center",
                gap: "4px",
                padding,
                borderRadius: "12px",
                background: style.bg,
                color: style.fg,
                fontSize,
                fontWeight: 700,
                textTransform: "uppercase",
                letterSpacing: "0.4px",
                whiteSpace: "nowrap",
            }}
        >
            <span
                style={{
                    width: "6px",
                    height: "6px",
                    borderRadius: "50%",
                    background: style.fg,
                    display: "inline-block",
                }}
            />
            {label}
        </span>
    );
}
