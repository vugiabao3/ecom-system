const DEMO_STORAGE_KEY = "ecom_demo_v1_";
const DEMO_SESSION_KEY = "ecom_demo_session_v1_";

export interface DemoUser {
    id: string;
    email: string;
    fullName: string;
    phone: string;
    password: string;
    role: "User";
    avatar?: string;
    createdAt: string;
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
    variants?: { id: string; name: string; price: number; stock: number }[];
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
    productId: string;
    productName: string;
    priceSnapshot: number;
    quantity: number;
    variantName?: string;
    imageUrl?: string;
    sellerId: string;
    sellerName: string;
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

export interface DemoSession {
    userId: string;
    email: string;
    fullName: string;
    role: "User";
}

interface DemoStore {
    users: DemoUser[];
    products: DemoProduct[];
    coupons: DemoCoupon[];
    orders: DemoOrder[];
    cart: DemoCart;
    wishlist: string[];
    addresses: DemoAddress[];
    reviews: DemoReview[];
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
    seedVersion: 1,
};

function loadStore(): DemoStore {
    try {
        const raw = localStorage.getItem(DEMO_STORAGE_KEY);
        if (!raw) return { ...DEFAULT_STORE };
        const parsed = JSON.parse(raw) as DemoStore;
        return { ...DEFAULT_STORE, ...parsed };
    } catch {
        return { ...DEFAULT_STORE };
    }
}

function saveStore(store: DemoStore) {
    localStorage.setItem(DEMO_STORAGE_KEY, JSON.stringify(store));
}

function saveSession(session: DemoSession | null) {
    if (session) localStorage.setItem(DEMO_SESSION_KEY, JSON.stringify(session));
    else localStorage.removeItem(DEMO_SESSION_KEY);
}

function loadSession(): DemoSession | null {
    try {
        const raw = localStorage.getItem(DEMO_SESSION_KEY);
        return raw ? JSON.parse(raw) : null;
    } catch {
        return null;
    }
}

export const demoStore = {
    load,
    save,
    loadSession,
    saveSession,
    getStorageKey: () => DEMO_STORAGE_KEY,
    getSessionKey: () => DEMO_SESSION_KEY,
};

function load(): DemoStore {
    return loadStore();
}

function save(store: DemoStore) {
    saveStore(store);
}

export function isDemoMode(): boolean {
    return import.meta.env.VITE_DEMO_MODE === "true";
}

export function getEnv(): Record<string, string> {
    return import.meta.env as Record<string, string>;
}

export function formatVND(v: number): string {
    return new Intl.NumberFormat("vi-VN").format(v) + " đ";
}

export function generateId(): string {
    return "demo_" + Math.random().toString(36).slice(2, 10) + Date.now().toString(36).slice(-4);
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