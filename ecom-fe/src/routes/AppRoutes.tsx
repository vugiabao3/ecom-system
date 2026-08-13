import {
    BrowserRouter,
    Routes,
    Route
} from "react-router-dom";
import ProductList from "../pages/ProductList";
import Checkout from "../pages/Checkout";
import ProductDetail
from "../pages/ProductDetail";
import Login from "../pages/Login";
import Register from "../pages/Register";
import ForgotPassword from "../pages/ForgotPassword";
import ResetPassword from "../pages/ResetPassword";
import ChangePassword from "../pages/ChangePassword";
import Home from "../pages/Home";

import ProtectedRoute
from "../components/ProtectedRoute";
import Cart
from "../pages/Cart";
export default function AppRoutes() {

    return (
        <BrowserRouter>

            <Routes>

                <Route
                    path="/login"
                    element={<Login />}
                />

                <Route
                    path="/register"
                    element={<Register />}
                />

                <Route
                    path="/forgot-password"
                    element={<ForgotPassword />}
                />

                <Route
                    path="/reset-password"
                    element={<ResetPassword />}
                />

                <Route
                    path="/change-password"
                    element={<ChangePassword />}
                />

                <Route
                    path="/"
                    element={
                        <ProtectedRoute>
                            <Home />
                        </ProtectedRoute>
                    }
                />
                <Route
    path="/checkout"
    element={<Checkout />}
/>

                <Route
    path="/products"
    element={<ProductList />}
/>



<Route
    path="/products/:id"
    element={<ProductDetail />}
/>
<Route
    path="/cart"
    element={<Cart />}
/>
            </Routes>

        </BrowserRouter>
    );
}