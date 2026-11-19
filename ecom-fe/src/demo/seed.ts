import { demoStore, isDemoMode } from "./store";
import type { DemoUser, DemoProduct, DemoCoupon, DemoAddress, DemoReview } from "./store";

export function seedDemoIfEmpty() {
    if (!isDemoMode()) return;
    const store = demoStore.load();
    if (store.products.length > 0) return;
    const now = new Date().toISOString();
    const users: DemoUser[] = [
        { id: "u1", email: "111@gmail.com", fullName: "Demo Customer", phone: "0900000001", password: "111", role: "User", createdAt: now },
        { id: "u2", email: "lan@gmail.com", fullName: "Nguyen Thi Lan", phone: "0900000002", password: "111", role: "User", createdAt: now },
    ];
    const products: DemoProduct[] = [
        { id: "p1", name: "Tai nghe Bluetooth Sony WH-1000XM5", description: "Tai nghe chống ồn cao cấp", price: 8990000, salePrice: 7490000, categoryId: 1, categoryName: "Điện tử", imageUrl: "https://picsum.photos/seed/p1/400/400", rating: 4.8, reviewCount: 120, stock: 15 },
        { id: "p2", name: "iPhone 15 Pro Max 256GB", description: "Điện thoại Apple cao cấp", price: 34990000, salePrice: 32990000, categoryId: 1, categoryName: "Điện tử", imageUrl: "https://picsum.photos/seed/p2/400/400", rating: 4.9, reviewCount: 300, stock: 8 },
        { id: "p3", name: "Samsung Galaxy S24 Ultra", description: "Flagship Samsung", price: 31990000, categoryId: 1, categoryName: "Điện tử", imageUrl: "https://picsum.photos/seed/p3/400/400", rating: 4.7, reviewCount: 210, stock: 12 },
        { id: "p4", name: "Laptop Dell XPS 13", description: "Laptop siêu mỏng", price: 28990000, salePrice: 26990000, categoryId: 1, categoryName: "Điện tử", imageUrl: "https://picsum.photos/seed/p4/400/400", rating: 4.6, reviewCount: 88, stock: 5 },
        { id: "p5", name: "Áo thun nam basic cotton", description: "Áo thun thoáng mát", price: 199000, salePrice: 149000, categoryId: 2, categoryName: "Thời trang", imageUrl: "https://picsum.photos/seed/p5/400/400", rating: 4.3, reviewCount: 56, stock: 100 },
        { id: "p6", name: "Quần jean nữ skinny", description: "Quần jean co giãn", price: 450000, categoryId: 2, categoryName: "Thời trang", imageUrl: "https://picsum.photos/seed/p6/400/400", rating: 4.4, reviewCount: 42, stock: 60 },
        { id: "p7", name: "Váy maxi hoa", description: "Váy đi biển", price: 599000, salePrice: 499000, categoryId: 2, categoryName: "Thời trang", imageUrl: "https://picsum.photos/seed/p7/400/400", rating: 4.5, reviewCount: 33, stock: 40 },
        { id: "p8", name: "Giày sneaker Nike Air", description: "Giày thể thao", price: 2490000, salePrice: 2190000, categoryId: 2, categoryName: "Thời trang", imageUrl: "https://picsum.photos/seed/p8/400/400", rating: 4.7, reviewCount: 95, stock: 25 },
        { id: "p9", name: "Nồi chiên không dầu Philips", description: "Nồi chiên 5.5L", price: 3290000, salePrice: 2890000, categoryId: 3, categoryName: "Gia dụng", imageUrl: "https://picsum.photos/seed/p9/400/400", rating: 4.6, reviewCount: 150, stock: 20 },
        { id: "p10", name: "Máy lọc không khí Xiaomi", description: "Lọc bụi mịn", price: 2190000, categoryId: 3, categoryName: "Gia dụng", imageUrl: "https://picsum.photos/seed/p10/400/400", rating: 4.5, reviewCount: 77, stock: 18 },
        { id: "p11", name: "Bàn ủi hơi nước", description: "Ủi nhanh", price: 890000, categoryId: 3, categoryName: "Gia dụng", imageUrl: "https://picsum.photos/seed/p11/400/400", rating: 4.2, reviewCount: 29, stock: 35 },
        { id: "p12", name: "Máy xay sinh tố", description: "Xay đá", price: 1290000, salePrice: 990000, categoryId: 3, categoryName: "Gia dụng", imageUrl: "https://picsum.photos/seed/p12/400/400", rating: 4.4, reviewCount: 61, stock: 22 },
        { id: "p13", name: "Kem chống nắng SPF50", description: "Bảo vệ da", price: 320000, categoryId: 4, categoryName: "Làm đẹp", imageUrl: "https://picsum.photos/seed/p13/400/400", rating: 4.6, reviewCount: 110, stock: 80 },
        { id: "p14", name: "Sữa rửa mặt CeraVe", description: "Làm sạch", price: 380000, salePrice: 340000, categoryId: 4, categoryName: "Làm đẹp", imageUrl: "https://picsum.photos/seed/p14/400/400", rating: 4.7, reviewCount: 98, stock: 90 },
        { id: "p15", name: "Son môi MAC", description: "Son lì", price: 650000, categoryId: 4, categoryName: "Làm đẹp", imageUrl: "https://picsum.photos/seed/p15/400/400", rating: 4.5, reviewCount: 72, stock: 50 },
        { id: "p16", name: "Nước hoa Chanel No.5", description: "Hương hoa", price: 3990000, salePrice: 3590000, categoryId: 4, categoryName: "Làm đẹp", imageUrl: "https://picsum.photos/seed/p16/400/400", rating: 4.8, reviewCount: 44, stock: 12 },
        { id: "p17", name: "Ốp lưng iPhone silicon", description: "Bảo vệ", price: 120000, categoryId: 5, categoryName: "Phụ kiện", imageUrl: "https://picsum.photos/seed/p17/400/400", rating: 4.3, reviewCount: 200, stock: 200 },
        { id: "p18", name: "Cáp sạc Type-C 1m", description: "Sạc nhanh", price: 89000, salePrice: 69000, categoryId: 5, categoryName: "Phụ kiện", imageUrl: "https://picsum.photos/seed/p18/400/400", rating: 4.4, reviewCount: 160, stock: 300 },
        { id: "p19", name: "Chuột không dây Logitech", description: "Chuột văn phòng", price: 590000, salePrice: 490000, categoryId: 5, categoryName: "Phụ kiện", imageUrl: "https://picsum.photos/seed/p19/400/400", rating: 4.6, reviewCount: 85, stock: 45 },
        { id: "p20", name: "Balo laptop chống nước", description: "Balo 15.6 inch", price: 790000, categoryId: 5, categoryName: "Phụ kiện", imageUrl: "https://picsum.photos/seed/p20/400/400", rating: 4.5, reviewCount: 67, stock: 30 },
    ];
    const coupons: DemoCoupon[] = [
        { id: "c1", code: "WELCOME10", discountType: "percent", discountValue: 10, minOrderValue: 0, maxDiscount: 100000, startDate: "2024-01-01", endDate: "2030-12-31", quantity: 100, usedCount: 0, isActive: true },
        { id: "c2", code: "SAVE50K", discountType: "fixed", discountValue: 50000, minOrderValue: 500000, startDate: "2024-01-01", endDate: "2030-12-31", quantity: 50, usedCount: 0, isActive: true },
        { id: "c3", code: "EXPIRED", discountType: "percent", discountValue: 20, minOrderValue: 0, startDate: "2020-01-01", endDate: "2020-12-31", quantity: 10, usedCount: 0, isActive: true },
    ];
    const addresses: DemoAddress[] = [
        { id: "a1", userId: "u1", fullName: "Demo Customer", phone: "0900000001", addressLine: "123 Nguyễn Huệ", city: "TP.HCM", country: "Vietnam", postalCode: "700000" },
    ];
    const reviews: DemoReview[] = [
        { id: "r1", productId: "p1", userId: "u2", userName: "Nguyen Thi Lan", rating: 5, comment: "Âm thanh tuyệt vời!", createdAt: now },
        { id: "r2", productId: "p1", userId: "u3", userName: "Tran Van Minh", rating: 4, comment: "Tốt nhưng giá cao.", createdAt: now },
    ];
    const storeData: any = { users, products, coupons, orders: [], cart: { items: [], totalPrice: 0 }, wishlist: [], addresses, reviews, seedVersion: 1 };
    demoStore.save(storeData);
}
export const demoCategories = [
    { id: 1, name: "Điện tử" }, { id: 2, name: "Thời trang" }, { id: 3, name: "Gia dụng" },
    { id: 4, name: "Làm đẹp" }, { id: 5, name: "Phụ kiện" },
];
