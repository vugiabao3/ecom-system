import { BrowserRouter, Routes, Route } from "react-router-dom";
import Inventory from "../pages/Inventory";
import ProductList from "../pages/ProductList";
import ProductDetail from "../pages/ProductDetail";
import Cart from "../pages/Cart";
import Checkout from "../pages/Checkout";
import Payment from "../pages/Payment";
import PaymentSuccess from "../pages/PaymentSuccess";
import PaymentFailed from "../pages/PaymentFailed";
import QRPaymentMock from "../pages/QRPaymentMock";
import OrderDetail from "../pages/OrderDetail";
import OrderShipping from "../pages/OrderShipping";
import OrderCompleted from "../pages/OrderCompleted";
import OrderCancelled from "../pages/OrderCancelled";
import OrderReturned from "../pages/OrderReturned";
import OrderTimeline from "../pages/OrderTimeline";
import MyOrders from "../pages/MyOrders";
import OrderConfirmation from "../pages/OrderConfirmation";
import OrderConfirmationResult from "../pages/OrderConfirmationResult";
import Login from "../pages/Login";
import Register from "../pages/Register";
import ForgotPassword from "../pages/ForgotPassword";
import ResetPassword from "../pages/ResetPassword";
import ChangePassword from "../pages/ChangePassword";
import VerifyEmail from "../pages/VerifyEmail";
import ResendVerification from "../pages/ResendVerification";
import Profile from "../pages/Profile";
import Notifications from "../pages/Notifications";
import Home from "../pages/Home";

// Seller Pages
import SellerDashboard from "../pages/seller/SellerDashboard";
import SellerProducts from "../pages/seller/SellerProducts";
import SellerOrders from "../pages/seller/SellerOrders";
import SellerPromotions from "../pages/seller/SellerPromotions";
import SellerRevenue from "../pages/seller/SellerRevenue";

// Shipper Pages
import ShipperDashboard from "../pages/shipper/ShipperDashboard";
import ShipperOrders from "../pages/shipper/ShipperOrders";
import ShipperHistory from "../pages/shipper/ShipperHistory";

// Admin Pages
import AdminDashboard from "../pages/admin/AdminDashboard";
import AdminProducts from "../pages/admin/AdminProducts";
import AdminCategories from "../pages/admin/AdminCategories";
import AdminUsers from "../pages/admin/AdminUsers";
import AdminPromotions from "../pages/admin/AdminPromotions";
import AdminShipping from "../pages/admin/AdminShipping";

// Guards
import ProtectedRoute from "../components/ProtectedRoute";
import AdminRoute from "../components/AdminRoute";
import SellerRoute from "../components/SellerRoute";
import ShipperRoute from "../components/ShipperRoute";

export default function AppRoutes() {
    return (
        <BrowserRouter>
            <Routes>
                {/* Public Storefront Routes */}
                <Route path="/" element={<Home />} />
                <Route path="/products" element={<ProductList />} />
                <Route path="/inventory" element={<Inventory />} />
                <Route path="/products/:id" element={<ProductDetail />} />

                {/* Auth Routes */}
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />
                <Route path="/forgot-password" element={<ForgotPassword />} />
                <Route path="/reset-password" element={<ResetPassword />} />
                <Route path="/verify-email" element={<VerifyEmail />} />
                <Route path="/resend-verification" element={<ResendVerification />} />


                {/* User Protected Routes */}
                <Route
                    path="/profile"
                    element={
                        <ProtectedRoute>
                            <Profile />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/notifications"
                    element={
                        <ProtectedRoute>
                            <Notifications />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/change-password"
                    element={
                        <ProtectedRoute>
                            <ChangePassword />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/cart"
                    element={
                        <ProtectedRoute>
                            <Cart />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/checkout"
                    element={
                        <ProtectedRoute>
                            <Checkout />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/payment"
                    element={
                        <ProtectedRoute>
                            <Payment />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/payment-success"
                    element={
                        <ProtectedRoute>
                            <PaymentSuccess />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/payment-failed"
                    element={
                        <ProtectedRoute>
                            <PaymentFailed />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/qr-payment"
                    element={
                        <ProtectedRoute>
                            <QRPaymentMock />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders"
                    element={
                        <ProtectedRoute>
                            <MyOrders />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders/:orderId"
                    element={
                        <ProtectedRoute>
                            <OrderDetail />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders/me/:orderId"
                    element={
                        <ProtectedRoute>
                            <OrderDetail />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders/shipping"
                    element={
                        <ProtectedRoute>
                            <OrderShipping />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders/completed"
                    element={
                        <ProtectedRoute>
                            <OrderCompleted />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders/cancelled"
                    element={
                        <ProtectedRoute>
                            <OrderCancelled />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders/returned"
                    element={
                        <ProtectedRoute>
                            <OrderReturned />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/orders/timeline/:orderId"
                    element={
                        <ProtectedRoute>
                            <OrderTimeline />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/order-confirmation"
                    element={
                        <ProtectedRoute>
                            <OrderConfirmation />
                        </ProtectedRoute>
                    }
                />
                <Route
                    path="/order-confirmation/:id"
                    element={
                        <ProtectedRoute>
                            <OrderConfirmationResult />
                        </ProtectedRoute>
                    }
                />

                {/* Seller Protected Routes */}
                <Route
                    path="/seller"
                    element={
                        <SellerRoute>
                            <SellerDashboard />
                        </SellerRoute>
                    }
                />
                <Route
                    path="/seller/products"
                    element={
                        <SellerRoute>
                            <SellerProducts />
                        </SellerRoute>
                    }
                />
                <Route
                    path="/seller/orders"
                    element={
                        <SellerRoute>
                            <SellerOrders />
                        </SellerRoute>
                    }
                />
                <Route
                    path="/seller/promotions"
                    element={
                        <SellerRoute>
                            <SellerPromotions />
                        </SellerRoute>
                    }
                />
                <Route
                    path="/seller/revenue"
                    element={
                        <SellerRoute>
                            <SellerRevenue />
                        </SellerRoute>
                    }
                />

                {/* Shipper Protected Routes */}
                <Route
                    path="/shipper"
                    element={
                        <ShipperRoute>
                            <ShipperDashboard />
                        </ShipperRoute>
                    }
                />
                <Route
                    path="/shipper/orders"
                    element={
                        <ShipperRoute>
                            <ShipperOrders />
                        </ShipperRoute>
                    }
                />
                <Route
                    path="/shipper/history"
                    element={
                        <ShipperRoute>
                            <ShipperHistory />
                        </ShipperRoute>
                    }
                />

                {/* Admin Protected Routes */}
                <Route
                    path="/admin"
                    element={
                        <AdminRoute>
                            <AdminDashboard />
                        </AdminRoute>
                    }
                />
                <Route
                    path="/admin/products"
                    element={
                        <AdminRoute>
                            <AdminProducts />
                        </AdminRoute>
                    }
                />
                <Route
                    path="/admin/categories"
                    element={
                        <AdminRoute>
                            <AdminCategories />
                        </AdminRoute>
                    }
                />
                <Route
                    path="/admin/users"
                    element={
                        <AdminRoute>
                            <AdminUsers />
                        </AdminRoute>
                    }
                />
                <Route
                    path="/admin/promotions"
                    element={
                        <AdminRoute>
                            <AdminPromotions />
                        </AdminRoute>
                    }
                />
                <Route
                    path="/admin/shipping"
                    element={
                        <AdminRoute>
                            <AdminShipping />
                        </AdminRoute>
                    }
                />

                {/* Fallback to Home */}
                <Route path="*" element={<Home />} />
            </Routes>
        </BrowserRouter>
    );
}
