import {
    demoStore,
    generateId,
    generateOrderNumber,
    hashPassword,
    verifyPassword,
} from "./store";
import type {
    DemoAddress,
    DemoCoupon,
    DemoOrder,
    DemoOrderItem,
    DemoPayment,
    DemoProduct,
    DemoShipment,
    DemoUser,
} from "./store";

function ok<T>(data: T) {
    return Promise.resolve({ data });
}

function fail(message: string, status = 400) {
    return Promise.reject({
        response: { status, data: { message } },
        message,
    });
}

function currentUserId(): string {
    return demoStore.loadSession()?.userId || "u1";
}

function currentUser(): DemoUser | undefined {
    const id = currentUserId();
    return demoStore.load().users.find((u) => u.id === id) || demoStore.load().users[0];
}

export const demoCategories = [
    { id: 1, name: "Điện tử" },
    { id: 2, name: "Thời trang" },
    { id: 3, name: "Gia dụng" },
    { id: 4, name: "Làm đẹp" },
    { id: 5, name: "Phụ kiện" },
];

function sortProducts(list: DemoProduct[], sortBy?: string): DemoProduct[] {
    const sorted = [...list];
    switch (sortBy) {
        case "price":
        case "price_asc":
            return sorted.sort((a, b) => (a.salePrice ?? a.price) - (b.salePrice ?? b.price));
        case "price_desc":
            return sorted.sort((a, b) => (b.salePrice ?? b.price) - (a.salePrice ?? a.price));
        case "rating":
            return sorted.sort((a, b) => b.rating - a.rating);
        case "name":
            return sorted.sort((a, b) => a.name.localeCompare(b.name));
        case "newest":
        default:
            return sorted.sort((a, b) => {
                const aTime = a.createdAt ? new Date(a.createdAt).getTime() : 0;
                const bTime = b.createdAt ? new Date(b.createdAt).getTime() : 0;
                return bTime - aTime;
            });
    }
}

function paginate<T>(items: T[], page = 1, pageSize = 10) {
    const totalCount = items.length;
    const totalPages = Math.max(1, Math.ceil(totalCount / pageSize));
    const safePage = Math.min(Math.max(1, page), totalPages);
    const start = (safePage - 1) * pageSize;
    return {
        items: items.slice(start, start + pageSize),
        page: safePage,
        pageSize,
        totalCount,
        totalPages,
        hasNextPage: safePage < totalPages,
        hasPreviousPage: safePage > 1,
    };
}

function productPrice(product: DemoProduct): number {
    return product.salePrice ?? product.price;
}

function findProduct(id: string): DemoProduct | undefined {
    return demoStore.load().products.find((p) => p.id === id);
}

function cartItemPrice(item: DemoOrderItem): number {
    return Number(item.priceSnapshot) || 0;
}

function couponDiscount(coupon: DemoCoupon | undefined, subTotal: number): number {
    if (!coupon) return 0;
    if (!coupon.isActive) return 0;
    if (new Date(coupon.endDate).getTime() < Date.now()) return 0;
    if (subTotal < coupon.minOrderValue) return 0;
    if (coupon.discountType === "percent") {
        const raw = (subTotal * coupon.discountValue) / 100;
        return Math.min(raw, coupon.maxDiscount ?? Number.MAX_SAFE_INTEGER);
    }
    return Math.min(coupon.discountValue, subTotal);
}

function shippingFeeFor(subTotal: number): number {
    return subTotal < 200000 ? 30000 : 0;
}

function selectedCartItems(): DemoOrderItem[] {
    const cart = demoStore.load().cart.items;
    return cart.filter((item) => item.selected !== false);
}

export const demoAuthApi = {
    register: (data: { email: string; password: string }) => {
        const store = demoStore.load();
        const email = data.email.trim();
        if (store.users.some((u) => u.email.toLowerCase() === email.toLowerCase())) {
            return fail("Email already registered.");
        }
        const user: DemoUser = {
            id: generateId("u"),
            email,
            fullName: email.split("@")[0],
            phone: "",
            passwordHash: hashPassword(data.password),
            role: "User",
            status: "Active",
            createdAt: new Date().toISOString(),
        };
        store.users.push(user);
        demoStore.save(store);
        return ok({ id: user.id, email: user.email });
    },
    forgotPassword: (data: { email: string }) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.email.toLowerCase() === data.email.trim().toLowerCase());
        if (user) {
            (store as unknown as { resetToken?: { email: string; token: string } }).resetToken = {
                email: user.email,
                token: "DEMO",
            };
            demoStore.save(store);
        }
        return ok({ success: true });
    },
    resetPassword: (data: { token: string; newPassword: string }) => {
        const store = demoStore.load();
        const reset = (store as unknown as { resetToken?: { email: string; token: string } }).resetToken;
        if (!reset) return fail("No reset request found. Use Forgot Password first.");
        if (data.token.trim() !== reset.token) return fail("Invalid or expired reset token. Use DEMO.");
        const user = store.users.find((u) => u.email === reset.email);
        if (!user) return fail("User not found.");
        user.passwordHash = hashPassword(data.newPassword);
        delete (store as unknown as { resetToken?: unknown }).resetToken;
        demoStore.save(store);
        return ok({ success: true });
    },
    changePassword: (data: { oldPassword: string; newPassword: string }) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === currentUserId());
        if (!user) return fail("User not found.", 404);
        if (!verifyPassword(data.oldPassword, user.passwordHash)) return fail("Current password is incorrect.");
        user.passwordHash = hashPassword(data.newPassword);
        demoStore.save(store);
        return ok({ success: true });
    },
    logout: () => ok({ success: true }),
    logoutAll: () => {
        const store = demoStore.load();
        store.devices = [];
        demoStore.save(store);
        demoStore.clearSession();
        return ok({ success: true });
    },
    verifyEmail: () => ok({ success: true }),
    resendVerification: () => ok({ success: true }),
    adminSetRole: (userId: string, role: string) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === userId);
        if (user) {
            user.role = role as DemoUser["role"];
            demoStore.save(store);
        }
        return ok({ success: true });
    },
    adminSetActive: (userId: string, status: string) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === userId);
        if (user) {
            user.status = status;
            demoStore.save(store);
        }
        return ok({ success: true });
    },
};

export const demoCategoryApi = {
    getAll: () => ok(demoCategories),
    getById: (id: number) => ok(demoCategories.find((c) => c.id === id) || null),
};

export const demoProductApi = {
    getProducts: (params?: {
        Page?: number;
        PageSize?: number;
        CategoryId?: number;
        MinPrice?: number;
        MaxPrice?: number;
        SortBy?: string;
    }) => {
        let list = demoStore.load().products.filter((p) => !p.isDeleted);
        if (params?.CategoryId) list = list.filter((p) => p.categoryId === params.CategoryId);
        if (params?.MinPrice != null) list = list.filter((p) => productPrice(p) >= params.MinPrice!);
        if (params?.MaxPrice != null) list = list.filter((p) => productPrice(p) <= params.MaxPrice!);
        list = sortProducts(list, params?.SortBy);

        const page = params?.Page ?? 1;
        const pageSize = params?.PageSize ?? 12;
        return ok(paginate(list, page, pageSize));
    },
    getProductById: (id: string) => ok(findProduct(id) || null),
    searchProducts: (params: {
        keyword?: string;
        categoryId?: number;
        minPrice?: number;
        maxPrice?: number;
        page?: number;
        pageSize?: number;
        sortBy?: string;
    }) => {
        const keyword = (params?.keyword || "").trim().toLowerCase();
        let list = demoStore.load().products.filter((p) => !p.isDeleted);
        if (keyword) {
            list = list.filter((p) =>
                p.name.toLowerCase().includes(keyword) ||
                p.description.toLowerCase().includes(keyword) ||
                (p.brandName || "").toLowerCase().includes(keyword) ||
                p.categoryName.toLowerCase().includes(keyword)
            );
        }
        if (params?.categoryId) list = list.filter((p) => p.categoryId === params.categoryId);
        if (params?.minPrice != null) list = list.filter((p) => productPrice(p) >= params.minPrice!);
        if (params?.maxPrice != null) list = list.filter((p) => productPrice(p) <= params.maxPrice!);
        list = sortProducts(list, params?.sortBy);
        return ok(paginate(list, params?.page ?? 1, params?.pageSize ?? 12));
    },
    getProductsByIds: (ids: string[]) => {
        const products = demoStore.load().products.filter((p) => ids.includes(p.id));
        return ok(products);
    },
    getProductsBySeller: (sellerId: string) =>
        ok(demoStore.load().products.filter((p) => (p.sellerId || "seller1") === sellerId)),
    getBrandsBySeller: (sellerId: string) => {
        const brands = demoStore
            .load()
            .products.filter((p) => (p.sellerId || "seller1") === sellerId && p.brandName)
            .map((p) => ({ id: p.brandName, name: p.brandName }));
        return ok(Array.from(new Map(brands.map((b) => [b.name, b])).values()));
    },
    createProduct: (data: { name: string; price: number; categoryId: number; imageUrl?: string }) => {
        const store = demoStore.load();
        const category = demoCategories.find((c) => c.id === data.categoryId);
        const product: DemoProduct = {
            id: generateId("p"),
            name: data.name,
            description: "",
            price: data.price,
            categoryId: data.categoryId,
            categoryName: category?.name || "Khác",
            imageUrl: data.imageUrl || `https://picsum.photos/seed/${generateId("img")}/500/500`,
            rating: 5,
            reviewCount: 0,
            stock: 10,
            sellerId: currentUserId(),
            sellerName: currentUser()?.fullName || "EcomSystem Official Store",
            createdAt: new Date().toISOString(),
        };
        store.products.unshift(product);
        demoStore.save(store);
        return ok(product);
    },
    createBrand: (data: { name: string }) => ok({ id: data.name, name: data.name }),
    updateProduct: (id: string, data: { name: string; price: number; categoryId: number }) => {
        const store = demoStore.load();
        const product = store.products.find((p) => p.id === id);
        if (!product) return fail("Product not found.", 404);
        product.name = data.name;
        product.price = data.price;
        product.categoryId = data.categoryId;
        product.categoryName = demoCategories.find((c) => c.id === data.categoryId)?.name || product.categoryName;
        demoStore.save(store);
        return ok(product);
    },
    deleteProduct: (id: string) => {
        const store = demoStore.load();
        const product = store.products.find((p) => p.id === id);
        if (product) product.isDeleted = true;
        demoStore.save(store);
        return ok({ success: true });
    },
    restoreProduct: (id: string) => {
        const store = demoStore.load();
        const product = store.products.find((p) => p.id === id);
        if (product) product.isDeleted = false;
        demoStore.save(store);
        return ok({ success: true });
    },
};

export const demoCartApi = {
    getCart: () => {
        const store = demoStore.load();
        return ok({ items: store.cart.items, totalPrice: store.cart.totalPrice });
    },
    getSelectedCart: () => {
        const items = selectedCartItems().map((item) => ({
            id: item.id,
            itemId: item.id,
            productId: item.productId,
            sellerId: item.sellerId || "seller1",
            sellerName: item.sellerName || "EcomSystem Official Store",
            productName: item.productName,
            imageUrl: item.imageUrl,
            variantName: item.variantName,
            price: cartItemPrice(item),
            priceSnapshot: cartItemPrice(item),
            quantity: item.quantity,
            subtotal: cartItemPrice(item) * item.quantity,
            selected: true,
        }));
        return ok(items);
    },
    addToCart: (data: { productId: string; quantity?: number; variantName?: string; variantId?: string }) => {
        const store = demoStore.load();
        const product = store.products.find((p) => p.id === data.productId);
        if (!product) return fail("Product not found.", 404);
        const quantity = Math.max(1, Number(data.quantity) || 1);

        const variant = data.variantId
            ? product.variants?.find((v) => v.id === data.variantId)
            : data.variantName
              ? product.variants?.find((v) => v.name === data.variantName)
              : undefined;
        const price = variant?.price ?? productPrice(product);
        const available = variant ? variant.stock : product.stock;
        if (available <= 0) return fail("This product is out of stock.");

        const existing = store.cart.items.find(
            (item) => item.productId === product.id && (item.variantName || "") === (variant?.name || "")
        );

        if (existing) {
            existing.quantity = Math.min(existing.quantity + quantity, available);
            demoStore.save(store);
            return ok(existing);
        }

        const item: DemoOrderItem = {
            id: generateId("cart"),
            productId: product.id,
            productName: product.name,
            priceSnapshot: price,
            quantity: Math.min(quantity, available),
            variantName: variant?.name,
            imageUrl: product.imageUrl,
            sellerId: product.sellerId || "seller1",
            sellerName: product.sellerName || "EcomSystem Official Store",
            selected: true,
        };
        store.cart.items.push(item);
        demoStore.save(store);
        return ok(item);
    },
    updateCartItem: (itemId: string, data: { quantity?: number; selected?: boolean; variantName?: string }) => {
        const store = demoStore.load();
        const item = store.cart.items.find((i) => i.id === itemId);
        if (!item) return fail("Cart item not found.", 404);
        if (data.quantity != null) item.quantity = Math.max(1, Number(data.quantity) || 1);
        if (data.selected != null) item.selected = data.selected;
        if (data.variantName != null) item.variantName = data.variantName;
        demoStore.save(store);
        return ok(item);
    },
    updateCartItemQuantity: (itemId: string, quantity: number) => {
        const store = demoStore.load();
        const item = store.cart.items.find((i) => i.id === itemId);
        if (!item) return fail("Cart item not found.", 404);
        item.quantity = Math.max(1, Number(quantity) || 1);
        demoStore.save(store);
        return ok(item);
    },
    removeCartItem: (itemId: string) => {
        const store = demoStore.load();
        store.cart.items = store.cart.items.filter((i) => i.id !== itemId);
        demoStore.save(store);
        return ok({ success: true });
    },
    clearCart: () => {
        const store = demoStore.load();
        store.cart = { items: [], totalPrice: 0 };
        demoStore.save(store);
        return ok({ success: true });
    },
};

function orderToListItem(order: DemoOrder) {
    return {
        id: order.id,
        orderNumber: order.orderNumber,
        status: order.status,
        paymentStatus: order.paymentStatus,
        paymentMethod: order.paymentMethod,
        totalPrice: order.totalPrice,
        subTotal: order.subTotal,
        shippingFee: order.shippingFee,
        discount: order.discount,
        createdAt: order.createdAt,
        receiverName: order.receiverName,
        address: order.address,
        itemCount: order.items.length,
        firstProductImage: order.items[0]?.imageUrl,
        firstProductName: order.items[0]?.productName,
    };
}

function orderToDetail(order: DemoOrder) {
    return {
        id: order.id,
        orderNumber: order.orderNumber,
        status: order.status,
        paymentStatus: order.paymentStatus,
        paymentMethod: order.paymentMethod,
        subTotal: order.subTotal,
        shippingFee: order.shippingFee,
        discount: order.discount,
        totalPrice: order.totalPrice,
        address: order.address,
        phone: order.phone,
        receiverName: order.receiverName,
        createdAt: order.createdAt,
        updatedAt: order.updatedAt,
        items: order.items.map((item, index) => ({
            id: item.id || `${order.id}-item-${index}`,
            productId: item.productId,
            sellerId: item.sellerId,
            sellerName: item.sellerName,
            productName: item.productName,
            variantName: item.variantName,
            imageUrl: item.imageUrl,
            price: cartItemPrice(item),
            quantity: item.quantity,
            subtotal: cartItemPrice(item) * item.quantity,
        })),
        statusHistory: (order.statusHistory && order.statusHistory.length > 0
            ? order.statusHistory
            : [{ status: order.status, note: "Order placed", createdAt: order.createdAt }]
        ).map((entry, index) => ({
            id: `${order.id}-history-${index}`,
            status: entry.status,
            note: entry.note,
            createdAt: entry.createdAt,
        })),
        shippingInfo: demoStore.load().shipments.find((s) => s.orderId === order.id),
    };
}

function myOrders(): DemoOrder[] {
    const userId = currentUserId();
    return demoStore
        .load()
        .orders.filter((o) => o.userId === userId)
        .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
}

function orderSummary() {
    const orders = myOrders();
    const count = (status: string) => orders.filter((o) => o.status === status).length;
    const totalSpent = orders
        .filter((o) => o.status !== "CANCELLED" && o.status !== "RETURNED")
        .reduce((sum, o) => sum + o.totalPrice, 0);
    return {
        totalOrders: orders.length,
        pendingOrders: count("PENDING"),
        confirmedOrders: count("CONFIRMED"),
        preparingOrders: count("PREPARING"),
        readyForShipmentOrders: count("READYFORSHIPMENT"),
        shippingOrders: count("SHIPPING"),
        deliveredOrders: count("DELIVERED"),
        cancelledOrders: count("CANCELLED"),
        returnedOrders: count("RETURNED"),
        totalSpent,
        averageOrderValue: orders.length ? Math.round(totalSpent / orders.length) : 0,
    };
}

export const demoOrderApi = {
    createOrder: (data: {
        address: string;
        phone: string;
        receiverName: string;
        couponCode?: string;
        paymentMethod: string;
        checkoutId?: string;
        idempotencyKey?: string;
    }) => {
        const store = demoStore.load();
        const session = store.checkoutSessions.find((s) => s.id === data.checkoutId);

        if (session?.orderId) {
            const existing = store.orders.find((o) => o.id === session.orderId);
            if (existing) {
                return ok({
                    orderId: existing.id,
                    checkoutId: session.id,
                    subTotal: existing.subTotal,
                    discount: existing.discount,
                    shippingFee: existing.shippingFee,
                    totalPrice: existing.totalPrice,
                    paymentMethod: existing.paymentMethod,
                    status: existing.status,
                });
            }
        }

        const items = session
            ? session.items.map((item) => ({ ...item }))
            : selectedCartItems().map((item) => ({ ...item }));

        if (items.length === 0) return fail("Your cart is empty.");

        const subTotal = items.reduce((sum, item) => sum + cartItemPrice(item) * item.quantity, 0);
        const couponCode = (session?.couponCode || data.couponCode || "").trim().toUpperCase();
        const coupon = store.coupons.find((c) => c.code === couponCode);
        const discount = couponDiscount(coupon, subTotal);
        const shippingFee = shippingFeeFor(subTotal);
        const now = new Date().toISOString();
        const order: DemoOrder = {
            id: generateId("o"),
            orderNumber: generateOrderNumber(),
            userId: currentUserId(),
            items: items.map((item, index) => ({
                ...item,
                id: item.id || `${generateId("oi")}-${index}`,
            })),
            subTotal,
            shippingFee,
            discount,
            totalPrice: Math.max(0, subTotal + shippingFee - discount),
            status: "PENDING",
            paymentMethod: data.paymentMethod || "COD",
            paymentStatus: "PENDING",
            address: session?.address || data.address,
            phone: session?.phone || data.phone,
            receiverName: session?.receiverName || data.receiverName,
            couponCode: couponCode || undefined,
            createdAt: now,
            updatedAt: now,
            statusHistory: [{ status: "PENDING", note: "Order placed", createdAt: now }],
        };

        store.orders.unshift(order);
        if (coupon) coupon.usedCount += 1;

        const orderedIds = new Set(items.map((item) => item.id));
        store.cart.items = store.cart.items.filter((item) => !orderedIds.has(item.id));

        if (session) session.orderId = order.id;

        demoStore.save(store);

        return ok({
            orderId: order.id,
            checkoutId: session?.id,
            subTotal,
            discount,
            shippingFee,
            totalPrice: order.totalPrice,
            paymentMethod: order.paymentMethod,
            status: order.status,
        });
    },
    getOrderById: (id: string) => {
        const order = demoStore.load().orders.find((o) => o.id === id);
        return ok(order ? orderToDetail(order) : null);
    },
    getOrdersByUserId: (userId: string) =>
        ok(demoStore.load().orders.filter((o) => o.userId === userId).map(orderToDetail)),
    getOrdersBySellerId: (sellerId: string) => {
        const orders = demoStore.load().orders.filter((o) =>
            o.items.some((item) => item.sellerId === sellerId)
        );
        return ok(orders.map(orderToDetail));
    },
    updateOrderStatus: (id: string, status: string) => {
        const store = demoStore.load();
        const order = store.orders.find((o) => o.id === id);
        if (!order) return fail("Order not found.", 404);
        order.status = status.toUpperCase();
        order.updatedAt = new Date().toISOString();
        order.statusHistory = [
            ...(order.statusHistory || []),
            { status: order.status, note: `Status changed to ${order.status}`, createdAt: order.updatedAt },
        ];
        demoStore.save(store);
        return ok(orderToDetail(order));
    },
    cancelOrder: (id: string) => {
        const store = demoStore.load();
        const order = store.orders.find((o) => o.id === id);
        if (!order) return fail("Order not found.", 404);
        order.status = "CANCELLED";
        order.updatedAt = new Date().toISOString();
        order.statusHistory = [
            ...(order.statusHistory || []),
            { status: "CANCELLED", note: "Order cancelled by customer", createdAt: order.updatedAt },
        ];
        demoStore.save(store);
        return ok({ success: true });
    },
    previewOrder: (data: {
        address?: string;
        phone?: string;
        receiverName?: string;
        couponCode?: string;
        paymentMethod: string;
    }) => {
        const store = demoStore.load();
        const items = selectedCartItems();
        const errors: string[] = [];
        if (items.length === 0) errors.push("Your cart is empty.");

        const subTotal = items.reduce((sum, item) => sum + cartItemPrice(item) * item.quantity, 0);
        const couponCode = (data.couponCode || "").trim().toUpperCase();
        const coupon = store.coupons.find((c) => c.code === couponCode);
        const discount = couponDiscount(coupon, subTotal);
        const shippingFee = shippingFeeFor(subTotal);
        const totalPrice = Math.max(0, subTotal + shippingFee - discount);

        const itemDtos = items.map((item) => {
            const product = findProduct(item.productId);
            const isValid = !!product && product.stock > 0 && item.quantity <= Math.max(product.stock, item.quantity);
            if (!product) errors.push(`${item.productName} is no longer available.`);
            return {
                productId: item.productId,
                sellerId: item.sellerId,
                productName: item.productName,
                variantName: item.variantName,
                imageUrl: item.imageUrl,
                price: cartItemPrice(item),
                quantity: item.quantity,
                subtotal: cartItemPrice(item) * item.quantity,
                isValid,
            };
        });

        const session = {
            id: generateId("checkout"),
            userId: currentUserId(),
            items: items.map((item) => ({ ...item })),
            subTotal,
            shippingFee,
            discount,
            totalPrice,
            couponCode: couponCode || undefined,
            address: data.address || "",
            phone: data.phone || "",
            receiverName: data.receiverName || "",
            paymentMethod: data.paymentMethod,
            createdAt: new Date().toISOString(),
        };
        store.checkoutSessions.unshift(session);
        store.checkoutSessions = store.checkoutSessions.slice(0, 20);
        demoStore.save(store);

        return ok({
            checkoutId: session.id,
            items: itemDtos,
            subTotal,
            shippingFee,
            discount,
            totalPrice,
            isValid: errors.length === 0,
            errors,
        });
    },
    validateOrder: (data: { couponCode?: string }) => {
        const store = demoStore.load();
        const items = selectedCartItems();
        const errors: string[] = [];
        const itemValidations = items.map((item) => {
            const product = findProduct(item.productId);
            const isValid = !!product && product.stock > 0;
            if (!isValid) errors.push(`${item.productName} is out of stock.`);
            return {
                productId: item.productId,
                productName: item.productName,
                quantity: item.quantity,
                isValid,
                error: isValid ? undefined : "Out of stock",
            };
        });
        if (items.length === 0) errors.push("Your cart is empty.");

        const couponCode = (data.couponCode || "").trim().toUpperCase();
        if (couponCode) {
            const coupon = store.coupons.find((c) => c.code === couponCode);
            const subTotal = items.reduce((sum, item) => sum + cartItemPrice(item) * item.quantity, 0);
            if (!coupon || !coupon.isActive) errors.push("Coupon code is invalid.");
            else if (new Date(coupon.endDate).getTime() < Date.now()) errors.push("Coupon code has expired.");
            else if (subTotal < coupon.minOrderValue) errors.push("Order does not meet the coupon minimum value.");
        }

        return ok({ isValid: errors.length === 0, errors, itemValidations });
    },
    calculateOrder: (data: { couponCode?: string }) => {
        const store = demoStore.load();
        const items = selectedCartItems();
        const subTotal = items.reduce((sum, item) => sum + cartItemPrice(item) * item.quantity, 0);
        const couponCode = (data.couponCode || "").trim().toUpperCase();
        const coupon = store.coupons.find((c) => c.code === couponCode);
        const discount = couponDiscount(coupon, subTotal);
        const shippingFee = shippingFeeFor(subTotal);
        return ok({
            subTotal,
            shippingFee,
            discount,
            totalPrice: Math.max(0, subTotal + shippingFee - discount),
        });
    },
    getCheckoutSession: (checkoutId: string) => {
        const session = demoStore.load().checkoutSessions.find((s) => s.id === checkoutId);
        return ok(session || null);
    },
    confirmCheckout: (data: { checkoutId: string }) => {
        const store = demoStore.load();
        const session = store.checkoutSessions.find((s) => s.id === data.checkoutId);
        if (!session) return fail("Checkout session not found.", 404);

        if (session.orderId) {
            const existing = store.orders.find((o) => o.id === session.orderId);
            if (existing) {
                return ok({
                    checkoutId: session.id,
                    orderId: existing.id,
                    status: existing.status,
                    message: "Checkout already confirmed.",
                });
            }
        }

        const now = new Date().toISOString();
        const order: DemoOrder = {
            id: generateId("o"),
            orderNumber: generateOrderNumber(),
            userId: session.userId,
            items: session.items.map((item, index) => ({
                ...item,
                id: item.id || `${generateId("oi")}-${index}`,
            })),
            subTotal: session.subTotal,
            shippingFee: session.shippingFee,
            discount: session.discount,
            totalPrice: session.totalPrice,
            status: "PENDING",
            paymentMethod: session.paymentMethod,
            paymentStatus: "PENDING",
            address: session.address,
            phone: session.phone,
            receiverName: session.receiverName,
            couponCode: session.couponCode,
            createdAt: now,
            updatedAt: now,
            statusHistory: [{ status: "PENDING", note: "Order placed", createdAt: now }],
        };
        store.orders.unshift(order);
        session.orderId = order.id;

        const orderedIds = new Set(session.items.map((item) => item.id));
        store.cart.items = store.cart.items.filter((item) => !orderedIds.has(item.id));
        demoStore.save(store);

        return ok({
            checkoutId: session.id,
            orderId: order.id,
            status: order.status,
            message: "Checkout confirmed. Your order has been created.",
        });
    },
    getOrderConfirmation: (orderId: string) => {
        const order = demoStore.load().orders.find((o) => o.id === orderId);
        return ok(order ? orderToDetail(order) : null);
    },
    getMyOrders: (params: { page?: number; pageSize?: number; statusFilter?: string } = {}) => {
        let orders = myOrders();
        if (params.statusFilter) {
            const filter = params.statusFilter.toUpperCase();
            orders = orders.filter((o) => o.status.toUpperCase() === filter);
        }
        const paged = paginate(orders.map(orderToListItem), params.page ?? 1, params.pageSize ?? 10);
        return ok(paged);
    },
    getOrderSummary: () => ok(orderSummary()),
    getMyOrderDetail: (orderId: string) => {
        const order = demoStore.load().orders.find((o) => o.id === orderId);
        return ok(order ? orderToDetail(order) : null);
    },
    getRecentOrders: (params: { page?: number; pageSize?: number; limit?: number } = {}) => {
        const limit = params.limit ?? params.pageSize ?? 5;
        const paged = paginate(myOrders().map(orderToListItem), 1, limit);
        return ok(paged);
    },
    searchOrders: (params: { searchTerm: string; page?: number; pageSize?: number }) => {
        const term = (params.searchTerm || "").trim().toLowerCase();
        const orders = myOrders().filter(
            (o) =>
                o.orderNumber.toLowerCase().includes(term) ||
                o.id.toLowerCase().includes(term) ||
                o.receiverName.toLowerCase().includes(term)
        );
        return ok(paginate(orders.map(orderToListItem), params.page ?? 1, params.pageSize ?? 10));
    },
    filterOrdersByStatus: (params: { statusFilter: string; page?: number; pageSize?: number }) => {
        const filter = (params.statusFilter || "").toUpperCase();
        const orders = myOrders().filter((o) => o.status.toUpperCase() === filter);
        return ok(paginate(orders.map(orderToListItem), params.page ?? 1, params.pageSize ?? 10));
    },
    getPendingOrders: (params: { page?: number; pageSize?: number } = {}) =>
        ok(paginate(myOrders().filter((o) => o.status === "PENDING").map(orderToListItem), params.page ?? 1, params.pageSize ?? 10)),
    getProcessingOrders: (params: { page?: number; pageSize?: number } = {}) =>
        ok(paginate(myOrders().filter((o) => ["CONFIRMED", "PREPARING"].includes(o.status)).map(orderToListItem), params.page ?? 1, params.pageSize ?? 10)),
    getShippingOrders: (params: { page?: number; pageSize?: number } = {}) =>
        ok(paginate(myOrders().filter((o) => o.status === "SHIPPING").map(orderToListItem), params.page ?? 1, params.pageSize ?? 10)),
    getCompletedOrders: (params: { page?: number; pageSize?: number } = {}) =>
        ok(paginate(myOrders().filter((o) => o.status === "DELIVERED").map(orderToListItem), params.page ?? 1, params.pageSize ?? 10)),
    getCancelledOrders: (params: { page?: number; pageSize?: number } = {}) =>
        ok(paginate(myOrders().filter((o) => o.status === "CANCELLED").map(orderToListItem), params.page ?? 1, params.pageSize ?? 10)),
    getReturnedOrders: (params: { page?: number; pageSize?: number } = {}) =>
        ok(paginate(myOrders().filter((o) => o.status === "RETURNED").map(orderToListItem), params.page ?? 1, params.pageSize ?? 10)),
    getOrderTimeline: (orderId: string) => {
        const order = demoStore.load().orders.find((o) => o.id === orderId);
        if (!order) return ok([]);
        const history = order.statusHistory && order.statusHistory.length > 0
            ? order.statusHistory
            : [{ status: order.status, note: "Order placed", createdAt: order.createdAt }];
        return ok(
            history.map((entry, index) => ({
                id: `${order.id}-timeline-${index}`,
                status: entry.status,
                note: entry.note,
                createdAt: entry.createdAt,
            }))
        );
    },
};

export const demoUserApi = {
    createUser: (data: { id?: string; email: string; password?: string; fullName: string }) => {
        const store = demoStore.load();
        const user: DemoUser = {
            id: data.id || generateId("u"),
            email: data.email,
            fullName: data.fullName,
            phone: "",
            passwordHash: hashPassword(data.password || "111"),
            role: "User",
            status: "Active",
            createdAt: new Date().toISOString(),
        };
        store.users.push(user);
        demoStore.save(store);
        return ok({ id: user.id, email: user.email });
    },
    getUserById: (id: string) => {
        const user = demoStore.load().users.find((u) => u.id === id);
        if (!user) return ok(null);
        return ok({
            id: user.id,
            email: user.email,
            fullName: user.fullName,
            phone: user.phone,
            role: user.role,
            status: user.status,
            avatar: user.avatar,
            currentAddress: user.currentAddress,
            currentLocation: user.currentLocation,
            createdAt: user.createdAt,
        });
    },
    getUserProfile: (id: string) => demoUserApi.getUserById(id),
    getUserByEmail: (email: string) => {
        const user = demoStore.load().users.find((u) => u.email.toLowerCase() === email.toLowerCase());
        return ok(user ? { id: user.id, email: user.email, fullName: user.fullName, role: user.role } : null);
    },
    updateUser: (
        id: string,
        data: {
            fullName?: string;
            phone?: string;
            avatar?: string;
            currentAddress?: string;
            currentLocation?: string;
            passwordHash?: string;
        }
    ) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === id);
        if (!user) return fail("User not found.", 404);
        if (data.fullName !== undefined) user.fullName = data.fullName;
        if (data.phone !== undefined) user.phone = data.phone;
        if (data.avatar !== undefined) user.avatar = data.avatar;
        if (data.currentAddress !== undefined) user.currentAddress = data.currentAddress;
        if (data.currentLocation !== undefined) user.currentLocation = data.currentLocation;
        if (data.passwordHash) user.passwordHash = data.passwordHash;
        demoStore.save(store);

        const session = demoStore.loadSession();
        if (session && session.userId === user.id && session.fullName !== user.fullName) {
            demoStore.saveSession({ ...session, fullName: user.fullName });
        }

        return ok({ id: user.id, email: user.email, fullName: user.fullName, phone: user.phone });
    },
    getAllUsers: (page = 1, pageSize = 10) => {
        const users = demoStore.load().users.map((u) => ({
            id: u.id,
            email: u.email,
            fullName: u.fullName,
            phone: u.phone,
            role: u.role,
            status: u.status,
            createdAt: u.createdAt,
        }));
        return ok({ items: users, totalCount: users.length, page, pageSize, totalPages: 1 });
    },
    searchUsers: (keyword: string, page = 1, pageSize = 10) => {
        const term = (keyword || "").toLowerCase();
        const users = demoStore
            .load()
            .users.filter(
                (u) => u.email.toLowerCase().includes(term) || u.fullName.toLowerCase().includes(term)
            );
        return ok({ items: users, totalCount: users.length, page, pageSize, totalPages: 1 });
    },
    blockUser: (id: string) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === id);
        if (user) user.status = "Blocked";
        demoStore.save(store);
        return ok({ success: true });
    },
    unblockUser: (id: string) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === id);
        if (user) user.status = "Active";
        demoStore.save(store);
        return ok({ success: true });
    },
    softDeleteUser: (id: string) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === id);
        if (user) user.status = "Deleted";
        demoStore.save(store);
        return ok({ success: true });
    },
    restoreUser: (id: string) => {
        const store = demoStore.load();
        const user = store.users.find((u) => u.id === id);
        if (user) user.status = "Active";
        demoStore.save(store);
        return ok({ success: true });
    },
    getUserActivity: (id: string) => ok(demoStore.load().activities.filter((a) => a.userId === id)),
    getUserAddresses: (id: string) => ok(demoStore.load().addresses.filter((a) => a.userId === id)),
    addAddress: (id: string, data: Omit<DemoAddress, "id" | "userId">) => {
        const store = demoStore.load();
        const address: DemoAddress = { id: generateId("a"), userId: id, ...data };
        store.addresses.push(address);
        demoStore.save(store);
        return ok(address);
    },
    getUserDevices: (id: string) => ok(demoStore.load().devices.filter((d) => d.userId === id)),
    logoutAllDevices: (id: string) => {
        const store = demoStore.load();
        store.devices = store.devices.filter((d) => d.userId !== id);
        demoStore.save(store);
        demoStore.clearSession();
        return ok({ success: true });
    },
};

export const demoInventoryApi = {
    getInventoryByProductId: (productId: string) => {
        const product = findProduct(productId);
        return ok({ productId, available: product?.stock ?? 0, reserved: 0 });
    },
    addStock: (data: { productId: string; quantity: number }) => {
        const store = demoStore.load();
        const product = store.products.find((p) => p.id === data.productId);
        if (product) product.stock += Number(data.quantity) || 0;
        demoStore.save(store);
        return ok({ productId: data.productId, available: product?.stock ?? 0, reserved: 0 });
    },
    getInventoryList: (page = 1, pageSize = 20) => {
        const items = demoStore.load().products.map((p) => ({
            productId: p.id,
            productName: p.name,
            available: p.stock,
            reserved: 0,
        }));
        return ok(paginate(items, page, pageSize));
    },
    getInventoryByVariantId: (variantId: string) => {
        const product = demoStore.load().products.find((p) => p.variants?.some((v) => v.id === variantId));
        const variant = product?.variants?.find((v) => v.id === variantId);
        return ok({ variantId, productId: product?.id, available: variant?.stock ?? 0, reserved: 0 });
    },
    getInventorySummary: (productId: string) => {
        const product = findProduct(productId);
        return ok({
            productId,
            totalAvailable: product?.stock ?? 0,
            totalReserved: 0,
            variants: product?.variants ?? [],
        });
    },
    getProductVariantInventory: (productId: string) => {
        const product = findProduct(productId);
        return ok(
            (product?.variants ?? []).map((v) => ({
                variantId: v.id,
                variantName: v.name,
                available: v.stock,
                reserved: 0,
            }))
        );
    },
};

export const demoPaymentApi = {
    createPayment: (data: { orderId: string; paymentMethod: string }) => {
        const store = demoStore.load();
        const order = store.orders.find((o) => o.id === data.orderId);
        const payment: DemoPayment = {
            id: generateId("pay"),
            orderId: data.orderId,
            method: data.paymentMethod,
            status: data.paymentMethod === "COD" ? "PENDING" : "PENDING",
            amount: order?.totalPrice ?? 0,
            createdAt: new Date().toISOString(),
        };
        store.payments.unshift(payment);
        if (order) order.paymentStatus = payment.status;
        demoStore.save(store);
        return ok({ paymentId: payment.id, status: payment.status });
    },
    confirmPayment: (paymentId: string) => {
        const store = demoStore.load();
        const payment = store.payments.find((p) => p.id === paymentId);
        if (!payment) return fail("Payment not found.", 404);
        payment.status = "PAID";
        const order = store.orders.find((o) => o.id === payment.orderId);
        if (order) {
            order.paymentStatus = "PAID";
            order.status = order.status === "PENDING" ? "CONFIRMED" : order.status;
            order.updatedAt = new Date().toISOString();
            order.statusHistory = [
                ...(order.statusHistory || []),
                { status: order.status, note: "Payment confirmed", createdAt: order.updatedAt },
            ];
        }
        demoStore.save(store);
        return ok({ success: true, status: payment.status });
    },
    failPayment: (paymentId: string) => {
        const store = demoStore.load();
        const payment = store.payments.find((p) => p.id === paymentId);
        if (!payment) return fail("Payment not found.", 404);
        payment.status = "FAILED";
        const order = store.orders.find((o) => o.id === payment.orderId);
        if (order) order.paymentStatus = "FAILED";
        demoStore.save(store);
        return ok({ success: true, status: payment.status });
    },
};

export const demoShippingApi = {
    getShippingStatusByOrderId: (orderId: string) => {
        const shipment = demoStore.load().shipments.find((s) => s.orderId === orderId);
        return ok(shipment || null);
    },
    getMyShipments: () => {
        const orderIds = new Set(myOrders().map((o) => o.id));
        return ok(demoStore.load().shipments.filter((s) => orderIds.has(s.orderId)));
    },
    startDelivery: (shipmentId: string) => demoShippingApi.updateStatus(shipmentId, "SHIPPING"),
    pickupShipment: (shipmentId: string) => demoShippingApi.updateStatus(shipmentId, "SHIPPING"),
    deliverShipment: (shipmentId: string) => demoShippingApi.updateStatus(shipmentId, "DELIVERED"),
    completeShipping: (shipmentId: string) => demoShippingApi.updateStatus(shipmentId, "DELIVERED"),
    assignShipment: (shipmentId: string, shipperId: string) => {
        const store = demoStore.load();
        const shipment = store.shipments.find((s) => s.id === shipmentId);
        if (shipment) shipment.shipperId = shipperId;
        demoStore.save(store);
        return ok({ success: true });
    },
    failShipment: (shipmentId: string) => demoShippingApi.updateStatus(shipmentId, "DELIVERYFAILED"),
    confirmCashReceived: (orderId: string) => {
        const store = demoStore.load();
        const order = store.orders.find((o) => o.id === orderId);
        if (order) order.paymentStatus = "PAID";
        demoStore.save(store);
        return ok({ success: true });
    },
    createShipment: (data: {
        orderId: string;
        address: string;
        phone: string;
        receiverName: string;
        shipperId?: string;
    }) => {
        const store = demoStore.load();
        const shipment: DemoShipment = {
            id: generateId("s"),
            orderId: data.orderId,
            status: "PENDING",
            shipperId: data.shipperId,
            receiverName: data.receiverName,
            phone: data.phone,
            address: data.address,
            trackingCode: `TRK-${generateId("d").toUpperCase()}`,
            createdAt: new Date().toISOString(),
            updatedAt: new Date().toISOString(),
        };
        store.shipments.unshift(shipment);
        demoStore.save(store);
        return ok(shipment);
    },
    updateStatus: (shipmentId: string, status: string) => {
        const store = demoStore.load();
        const shipment = store.shipments.find((s) => s.id === shipmentId);
        if (!shipment) return fail("Shipment not found.", 404);
        shipment.status = status;
        shipment.updatedAt = new Date().toISOString();
        if (status === "DELIVERED") shipment.deliveredAt = shipment.updatedAt;

        const order = store.orders.find((o) => o.id === shipment.orderId);
        if (order) {
            order.status = status;
            order.updatedAt = shipment.updatedAt;
            order.statusHistory = [
                ...(order.statusHistory || []),
                { status, note: `Shipment ${status.toLowerCase()}`, createdAt: shipment.updatedAt },
            ];
        }
        demoStore.save(store);
        return ok({ success: true, status });
    },
};

export const demoPromotionApi = {
    getAllPromotions: () =>
        ok(
            demoStore.load().coupons.map((c) => ({
                id: c.id,
                code: c.code,
                discountPercent: c.discountType === "percent" ? c.discountValue : 0,
                discountType: c.discountType,
                discountValue: c.discountValue,
                minOrderValue: c.minOrderValue,
                maxDiscount: c.maxDiscount,
                isActive: c.isActive,
                startDate: c.startDate,
                endDate: c.endDate,
                quantity: c.quantity,
                usedCount: c.usedCount,
            }))
        ),
    getPromotionsBySeller: () => ok([]),
    createPromotion: (data: {
        code: string;
        discountPercent: number;
        startDate: string;
        endDate: string;
        quantity: number;
    }) => {
        const store = demoStore.load();
        const coupon: DemoCoupon = {
            id: generateId("c"),
            code: data.code.toUpperCase(),
            discountType: "percent",
            discountValue: data.discountPercent,
            minOrderValue: 0,
            startDate: data.startDate,
            endDate: data.endDate,
            quantity: data.quantity,
            usedCount: 0,
            isActive: true,
        };
        store.coupons.push(coupon);
        demoStore.save(store);
        return ok(coupon);
    },
    updatePromotion: (data: {
        id: string;
        code: string;
        discountPercent: number;
        startDate: string;
        endDate: string;
        quantity: number;
        isActive: boolean;
    }) => {
        const store = demoStore.load();
        const coupon = store.coupons.find((c) => c.id === data.id);
        if (!coupon) return fail("Promotion not found.", 404);
        coupon.code = data.code.toUpperCase();
        coupon.discountType = "percent";
        coupon.discountValue = data.discountPercent;
        coupon.startDate = data.startDate;
        coupon.endDate = data.endDate;
        coupon.quantity = data.quantity;
        coupon.isActive = data.isActive;
        demoStore.save(store);
        return ok(coupon);
    },
    deletePromotion: (id: string) => {
        const store = demoStore.load();
        store.coupons = store.coupons.filter((c) => c.id !== id);
        demoStore.save(store);
        return ok({ success: true });
    },
    validateCoupon: (code: string, total: number) => {
        const store = demoStore.load();
        const coupon = store.coupons.find((c) => c.code === code.trim().toUpperCase());
        if (!coupon || !coupon.isActive) return ok({ valid: false, message: "Invalid coupon" });
        if (new Date(coupon.endDate).getTime() < Date.now()) return ok({ valid: false, message: "Coupon expired" });
        if (total < coupon.minOrderValue) return ok({ valid: false, message: "Minimum order value not met" });
        return ok({ valid: true, coupon });
    },
    getActiveCoupons: () =>
        ok(
            demoStore
                .load()
                .coupons.filter((c) => c.isActive && new Date(c.endDate).getTime() >= Date.now())
        ),
};
