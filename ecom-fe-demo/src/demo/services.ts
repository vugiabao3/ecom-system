import { demoStore, generateId, generateOrderNumber } from "./store";
import type { DemoOrder, DemoOrderItem } from "./store";

function ax<T>(data: T) { return Promise.resolve({ data }); }

export const demoProductApi = {
    getProducts: (params?: any) => {
        let list = demoStore.load().products;
        if (params?.CategoryId) list = list.filter(p => p.categoryId === params.CategoryId);
        if (params?.MinPrice != null) list = list.filter(p => (p.salePrice ?? p.price) >= params.MinPrice);
        if (params?.MaxPrice != null) list = list.filter(p => (p.salePrice ?? p.price) <= params.MaxPrice);
        return ax(list);
    },
    getProductById: (id: string) => ax(demoStore.load().products.find(p => p.id === id) || null),
    searchProducts: (params: any) => {
        const kw = (params?.keyword || params || "").toLowerCase();
        const list = demoStore.load().products.filter(p => p.name.toLowerCase().includes(kw));
        return ax(list);
    },
};

export const demoCartApi = {
    getCart: () => {
        const s = demoStore.load();
        return ax({ items: s.cart.items, totalPrice: s.cart.totalPrice });
    },
    addToCart: (data: any) => {
        const s = demoStore.load();
        const p = s.products.find(x => x.id === data.productId);
        if (!p) return ax(null);
        const item: DemoOrderItem = { productId: p.id, productName: p.name, priceSnapshot: p.salePrice ?? p.price, quantity: data.quantity ?? 1, imageUrl: p.imageUrl, sellerId: "seller1", sellerName: "Demo Seller" };
        s.cart.items.push(item);
        s.cart.totalPrice = s.cart.items.reduce((a, b) => a + b.priceSnapshot * b.quantity, 0);
        demoStore.save(s);
        return ax(item);
    },
    updateCartItemQuantity: (itemId: string, quantity: number) => {
        const s = demoStore.load();
        const idx = s.cart.items.findIndex(i => i.productId === itemId);
        if (idx >= 0) s.cart.items[idx].quantity = quantity;
        s.cart.totalPrice = s.cart.items.reduce((a, b) => a + b.priceSnapshot * b.quantity, 0);
        demoStore.save(s);
        return ax(null);
    },
    removeCartItem: (itemId: string) => {
        const s = demoStore.load();
        s.cart.items = s.cart.items.filter(i => i.productId !== itemId);
        s.cart.totalPrice = s.cart.items.reduce((a, b) => a + b.priceSnapshot * b.quantity, 0);
        demoStore.save(s);
        return ax(null);
    },
    clearCart: () => {
        const s = demoStore.load();
        s.cart = { items: [], totalPrice: 0 };
        demoStore.save(s);
        return ax(null);
    },
};

export const demoOrderApi = {
    createOrder: (data: any) => {
        const s = demoStore.load();
        const session = demoStore.loadSession();
        const userId = session?.userId || "u1";
        const items = s.cart.items.map(i => ({ ...i }));
        const subTotal = items.reduce((a, b) => a + b.priceSnapshot * b.quantity, 0);
        const shippingFee = 30000;
        let discount = 0;
        if (data.couponCode) {
            const c = s.coupons.find(x => x.code === data.couponCode);
            if (c && c.isActive) {
                discount = c.discountType === "percent" ? Math.min(subTotal * c.discountValue / 100, c.maxDiscount ?? Infinity) : c.discountValue;
            }
        }
        const order: DemoOrder = {
            id: generateId(), orderNumber: generateOrderNumber(), userId, items,
            subTotal, shippingFee, discount, totalPrice: subTotal + shippingFee - discount,
            status: "Pending", paymentMethod: data.paymentMethod || "COD", paymentStatus: "Paid",
            address: data.address, phone: data.phone, receiverName: data.receiverName,
            couponCode: data.couponCode, createdAt: new Date().toISOString(), updatedAt: new Date().toISOString(),
        };
        s.orders.unshift(order);
        s.cart = { items: [], totalPrice: 0 };
        demoStore.save(s);
        return ax({ orderId: order.id, subTotal, discount, shippingFee, totalPrice: order.totalPrice, paymentMethod: order.paymentMethod, status: order.status });
    },
    getMyOrders: () => {
        const s = demoStore.load();
        const session = demoStore.loadSession();
        const userId = session?.userId || "u1";
        const items = s.orders.filter(o => o.userId === userId).map(o => ({
            id: o.id, orderNumber: o.orderNumber, status: o.status, paymentStatus: o.paymentStatus,
            paymentMethod: o.paymentMethod, totalPrice: o.totalPrice, subTotal: o.subTotal,
            shippingFee: o.shippingFee, discount: o.discount, createdAt: o.createdAt,
            receiverName: o.receiverName, address: o.address, itemCount: o.items.length,
            firstProductImage: o.items[0]?.imageUrl, firstProductName: o.items[0]?.productName,
        }));
        return ax({ items, page: 1, pageSize: 10, totalCount: items.length, totalPages: 1, hasNextPage: false, hasPreviousPage: false });
    },
    getMyOrderDetail: (id: string) => {
        const s = demoStore.load();
        const o = s.orders.find(x => x.id === id);
        if (!o) return ax(null);
        return ax({
            id: o.id, status: o.status, paymentStatus: o.paymentStatus, paymentMethod: o.paymentMethod,
            subTotal: o.subTotal, shippingFee: o.shippingFee, discount: o.discount, totalPrice: o.totalPrice,
            address: o.address, phone: o.phone, receiverName: o.receiverName, createdAt: o.createdAt,
            updatedAt: o.updatedAt, items: o.items.map(i => ({ id: i.productId, productId: i.productId, sellerId: i.sellerId, productName: i.productName, imageUrl: i.imageUrl, price: i.priceSnapshot, quantity: i.quantity, subtotal: i.priceSnapshot * i.quantity })),
            statusHistory: [{ id: "h1", status: o.status, createdAt: o.createdAt }],
        });
    },
    cancelOrder: (id: string) => {
        const s = demoStore.load();
        const o = s.orders.find(x => x.id === id);
        if (o) { o.status = "Cancelled"; o.updatedAt = new Date().toISOString(); demoStore.save(s); }
        return ax(null);
    },
};

export const demoUserApi = {
    getProfile: () => {
        const s = demoStore.load();
        const session = demoStore.loadSession();
        const u = s.users.find(x => x.id === (session?.userId || "u1"));
        return ax(u ? { id: u.id, email: u.email, fullName: u.fullName, phone: u.phone } : null);
    },
};

export const demoPromotionApi = {
    validateCoupon: (code: string, total: number) => {
        const s = demoStore.load();
        const c = s.coupons.find(x => x.code === code);
        if (!c || !c.isActive) return ax({ valid: false, message: "Invalid coupon" });
        const now = new Date();
        if (new Date(c.endDate) < now) return ax({ valid: false, message: "Expired" });
        if (total < c.minOrderValue) return ax({ valid: false, message: "Min order not met" });
        return ax({ valid: true, coupon: c });
    },
};