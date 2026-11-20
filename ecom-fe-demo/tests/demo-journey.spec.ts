import { test, expect } from "@playwright/test";

test.describe("EcomSystem Demo - Complete Customer Journey", () => {
    test("should load homepage without login", async ({ page }) => {
        await page.goto("/");
        await expect(page).toHaveURL("/");
        await expect(page.locator(".navbar-brand")).toContainText("EcomSystem");
        await expect(page.locator(".hero-title")).toContainText("Mega Sale");
    });

    test("should navigate to product listing", async ({ page }) => {
        await page.goto("/");
        await page.click(".navbar-link");
        await expect(page).toHaveURL("/products");
    });

    test("should open product detail", async ({ page }) => {
        await page.goto("/products");
        await page.click(".product-card");
        await expect(page).toHaveURL(/\/products\//);
    });

    test("should add product to cart", async ({ page }) => {
        await page.goto("/products");
        await page.click(".product-card");
        await page.click(".add-cart-btn");
        await expect(page.locator(".add-cart-btn")).not.toBeDisabled();
    });

    test("should login with demo credentials", async ({ page }) => {
        await page.goto("/login");
        await page.fill('input[type="email"]', "111@gmail.com");
        await page.fill('input[type="password"]', "111");
        await page.click('button[type="submit"]');
        await expect(page).toHaveURL("/");
    });

    test("should navigate to cart after login", async ({ page }) => {
        await page.goto("/login");
        await page.click('button:has-text("Đăng nhập Demo ngay")');
        await expect(page).toHaveURL("/");
        await page.goto("/cart");
        await expect(page).toHaveURL("/cart");
    });

    test("should go to checkout", async ({ page }) => {
        await page.goto("/login");
        await page.click('button:has-text("Đăng nhập Demo ngay")');
        await page.goto("/products");
        await page.click(".product-card");
        await page.click(".add-cart-btn");
        await page.goto("/cart");
        await page.click('button:has-text("Proceed to Checkout")');
        await expect(page).toHaveURL("/checkout");
    });

    test("should complete checkout", async ({ page }) => {
        await page.goto("/login");
        await page.click('button:has-text("Đăng nhập Demo ngay")');
        await page.goto("/products");
        await page.click(".product-card");
        await page.click(".add-cart-btn");
        await page.goto("/cart");
        await page.click('button:has-text("Proceed to Checkout")');
        await page.fill('input[placeholder="Ho va ten"]', "Demo Customer");
        await page.fill('input[placeholder="0912345678"]', "0900000001");
        await page.fill('input[placeholder="Dia chi chi tiet, phuong, quan, thanh pho"]', "123 Nguyen Hue, TP.HCM");
        await page.click('button:has-text("Dat hang & thanh toan")');
        await expect(page.locator("h2")).toContainText("Dat hang thanh cong!");
    });

    test("should view orders", async ({ page }) => {
        await page.goto("/login");
        await page.click('button:has-text("Đăng nhập Demo ngay")');
        await page.goto("/orders");
        await expect(page).toHaveURL("/orders");
    });

    test("should view profile", async ({ page }) => {
        await page.goto("/login");
        await page.click('button:has-text("Đăng nhập Demo ngay")');
        await page.goto("/profile");
        await expect(page).toHaveURL("/profile");
    });

    test("should logout and redirect to login", async ({ page }) => {
        await page.goto("/login");
        await page.click('button:has-text("Đăng nhập Demo ngay")');
        await page.click('button:has-text("Logout")');
        await page.goto("/profile");
        await expect(page).toHaveURL("/login");
    });
});