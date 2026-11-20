export const DEMO_NAMESPACE = "ecom_demo_v1_";

const DEMO_STORAGE_KEY = `${DEMO_NAMESPACE}store`;
const DEMO_SESSION_KEY = `${DEMO_NAMESPACE}session`;
const LEGACY_STORAGE_KEYS = ["ecom_demo_v1_", "ecom_demo_v1_store"];
const LEGACY_SESSION_KEYS = ["ecom_demo_session_v1_"];

export const DEMO_UPDATE_EVENT = "ecom-demo-update";

export type DemoRole = "User" | "Admin" | "Seller" | "Shipper";

export interface DemoUser {
    id: string;
    email: string;
    fullName: string;
    phone: string;
    passwordHash: string;
    role: DemoRole;
    avatar?: string;
    currentAddress?: string;
    currentLocation?: string;
    status?: string;
    createdAt: string;
}

export interface DemoVariant {
    id: string;
    name: string;
    price: number;
    stock: number;
}

export interface DemoProduct {
    id: string;
    name: string;
    description: string;
    price: number;
    salePrice?: number;
    categoryId: number;
    categoryName: string;
    imageUrl: string;
    rating: number;
    reviewCount: number;
    stock: number;
    sellerId?: string;
    sellerName?: string;
    brandName?: string;
    variants?: DemoVariant[];
    isDeleted?: boolean;
    status?: string;
    createdAt?: string;
}

export interface DemoCoupon {
    id: string;
    code: string;
    discountType: "percent" | "fixed";
    discountValue: number;
    minOrderValue: number;
    maxDiscount?: number;
    startDate: string;
    endDate: string;
    quantity: number;
    usedCount: number;
    isActive: boolean;
}

export interface DemoOrderItem {
    id?: string;
    productId: string;
    productName: string;
    priceSnapshot: number;
    quantity: number;
    variantName?: string;
    imageUrl?: string;
    sellerId: string;
    sellerName: string;
    selected?: boolean;
}

export interface DemoStatusHistory {
    status: string;
    note?: string;
    createdAt: string;
}

export interface DemoOrder {
    id: string;
    orderNumber: string;
    userId: string;
    items: DemoOrderItem[];
    subTotal: number;
    shippingFee: number;
    discount: number;
    totalPrice: number;
    status: string;
    paymentMethod: string;
    paymentStatus: string;
    address: string;
    phone: string;
    receiverName: string;
    couponCode?: string;
    createdAt: string;
    updatedAt: string;
    trackingCode?: string;
    statusHistory?: DemoStatusHistory[];
}

export interface DemoCart {
    items: DemoOrderItem[];
    totalPrice: number;
}

export interface DemoAddress {
    id: string;
    userId: string;
    fullName: string;
    phone: string;
    addressLine: string;
    city: string;
    country: string;
    postalCode: string;
}

export interface DemoReview {
    id: string;
    productId: string;
    userId: string;
    userName: string;
    rating: number;
    comment: string;
    createdAt: string;
}

export interface DemoPayment {
    id: string;
    orderId: string;
    method: string;
    status: string;
    amount: number;
    createdAt: string;
}

export interface DemoShipment {
    id: string;
    orderId: string;
    status: string;
    shipperId?: string;
    receiverName?: string;
    phone?: string;
    address?: string;
    trackingCode?: string;
    createdAt?: string;
    updatedAt?: string;
    deliveredAt?: string;
    failureReason?: string;
    paymentMethod?: string;
    paymentStatus?: string;
}

export interface DemoCheckoutSession {
    id: string;
    userId: string;
    items: DemoOrderItem[];
    subTotal: number;
    shippingFee: number;
    discount: number;
    totalPrice: number;
    couponCode?: string;
    address: string;
    phone: string;
    receiverName: string;
    paymentMethod: string;
    createdAt: string;
    orderId?: string;
}

export interface DemoActivity {
    id: string;
    userId: string;
    action: string;
    description?: string;
    createdAt: string;
}

export interface DemoDevice {
    id: string;
    userId: string;
    deviceInfo: string;
    ipAddress: string;
    loginAt: string;
}

export interface DemoSession {
    userId: string;
    email: string;
    fullName: string;
    role: DemoRole;
}

export interface DemoStore {
    users: DemoUser[];
    products: DemoProduct[];
    coupons: DemoCoupon[];
    orders: DemoOrder[];
    cart: DemoCart;
    wishlist: string[];
    addresses: DemoAddress[];
    reviews: DemoReview[];
    payments: DemoPayment[];
    shipments: DemoShipment[];
    checkoutSessions: DemoCheckoutSession[];
    activities: DemoActivity[];
    devices: DemoDevice[];
    seedVersion: number;
}

const DEFAULT_STORE: DemoStore = {
    users: [],
    products: [],
    coupons: [],
    orders: [],
    cart: { items: [], totalPrice: 0 },
    wishlist: [],
    addresses: [],
    reviews: [],
    payments: [],
    shipments: [],
    checkoutSessions: [],
    activities: [],
    devices: [],
    seedVersion: 1,
};

function readRaw(key: string): string | null {
    try {
        return localStorage.getItem(key);
    } catch {
        return null;
    }
}

function normalizeStore(parsed: Partial<DemoStore> | null): DemoStore {
    const base: DemoStore = {
        ...DEFAULT_STORE,
        ...(parsed || {}),
    };

    base.users = (base.users || []).map((user) => {
        const legacy = user as DemoUser & { password?: string };
        if (!legacy.passwordHash && legacy.password) {
            legacy.passwordHash = hashPassword(legacy.password);
        }
        delete legacy.password;
        if (!legacy.status) legacy.status = "Active";
        return legacy;
    });

    base.products = (base.products || []).map((product) => ({
        ...product,
        sellerId: product.sellerId || "seller1",
        sellerName: product.sellerName || "EcomSystem Official",
        createdAt: product.createdAt || new Date().toISOString(),
    }));

    base.cart = base.cart || { items: [], totalPrice: 0 };
    base.cart.items = (base.cart.items || []).map((item) => ({
        ...item,
        id: item.id || generateId("cart"),
        selected: item.selected !== false,
    }));
    base.cart.totalPrice = recalcCartTotal(base.cart.items);
    base.wishlist = base.wishlist || [];
    base.addresses = base.addresses || [];
    base.reviews = base.reviews || [];
    base.payments = base.payments || [];
    base.shipments = base.shipments || [];
    base.checkoutSessions = base.checkoutSessions || [];
    base.activities = base.activities || [];
    base.devices = base.devices || [];

    return base;
}

function loadStore(): DemoStore {
    try {
        let raw = readRaw(DEMO_STORAGE_KEY);
        if (!raw) {
            for (const legacyKey of LEGACY_STORAGE_KEYS) {
                const legacyRaw = readRaw(legacyKey);
                if (legacyRaw) {
                    raw = legacyRaw;
                    break;
                }
            }
        }
        if (!raw) return normalizeStore(null);
        return normalizeStore(JSON.parse(raw) as Partial<DemoStore>);
    } catch {
        return normalizeStore(null);
    }
}

function saveStore(store: DemoStore) {
    try {
        store.cart.totalPrice = recalcCartTotal(store.cart.items);
        localStorage.setItem(DEMO_STORAGE_KEY, JSON.stringify(store));
    } catch {
        // Storage can be unavailable in private mode; the demo keeps working in-memory per read.
    }
    notifyDemoUpdate("store");
}

function saveSession(session: DemoSession | null) {
    try {
        if (session) localStorage.setItem(DEMO_SESSION_KEY, JSON.stringify(session));
        else localStorage.removeItem(DEMO_SESSION_KEY);
    } catch {
        // ignore storage failures
    }
    notifyDemoUpdate("session");
}

function loadSession(): DemoSession | null {
    try {
        let raw = readRaw(DEMO_SESSION_KEY);
        if (!raw) {
            for (const legacyKey of LEGACY_SESSION_KEYS) {
                const legacyRaw = readRaw(legacyKey);
                if (legacyRaw) {
                    raw = legacyRaw;
                    break;
                }
            }
        }
        if (!raw) return null;
        const parsed = JSON.parse(raw) as DemoSession;
        if (!parsed || !parsed.userId) return null;
        return parsed;
    } catch {
        return null;
    }
}

export function notifyDemoUpdate(scope: "store" | "session" | "cart" = "store") {
    try {
        window.dispatchEvent(new CustomEvent(DEMO_UPDATE_EVENT, { detail: { scope } }));
    } catch {
        // Non-browser environments ignore updates.
    }
}

export const demoStore = {
    load: loadStore,
    save: saveStore,
    loadSession,
    saveSession,
    clearSession: () => saveSession(null),
    getStorageKey: () => DEMO_STORAGE_KEY,
    getSessionKey: () => DEMO_SESSION_KEY,
    getNamespace: () => DEMO_NAMESPACE,
};

export function isDemoMode(): boolean {
    if (import.meta.env.VITE_DEMO_MODE === "false") return false;
    return true;
}

export function getEnv(): Record<string, string> {
    return import.meta.env as Record<string, string>;
}

export function formatVND(v: number): string {
    return new Intl.NumberFormat("vi-VN").format(v) + " đ";
}

export function generateId(prefix = "demo"): string {
    return `${prefix}_` + Math.random().toString(36).slice(2, 10) + Date.now().toString(36).slice(-4);
}

export function generateOrderNumber(): string {
    const d = new Date();
    const stamp = d.getFullYear().toString().slice(-2) +
        String(d.getMonth() + 1).padStart(2, "0") +
        String(d.getDate()).padStart(2, "0") +
        String(d.getHours()).padStart(2, "0") +
        String(d.getMinutes()).padStart(2, "0");
    return "ORD" + stamp + Math.floor(Math.random() * 9000 + 1000);
}

export function recalcCartTotal(items: DemoOrderItem[]): number {
    return items.reduce((acc, item) => acc + effectivePrice(item) * item.quantity, 0);
}

export function effectivePrice(item: DemoOrderItem): number {
    return Number(item.priceSnapshot) || 0;
}

export function hashPassword(password: string): string {
    const input = `ecom_demo_v1::${password}`;
    let h1 = 0x811c9dc5;
    let h2 = 0x01000193;
    for (let i = 0; i < input.length; i++) {
        const code = input.charCodeAt(i);
        h1 ^= code;
        h1 = Math.imul(h1, 0x01000193) >>> 0;
        h2 = Math.imul(h2 ^ code, 0x85ebca6b) >>> 0;
    }
    return `fnv1a$${h1.toString(16).padStart(8, "0")}${h2.toString(16).padStart(8, "0")}`;
}

export function verifyPassword(password: string, passwordHash?: string): boolean {
    if (!passwordHash) return false;
    return hashPassword(password) === passwordHash;
}
