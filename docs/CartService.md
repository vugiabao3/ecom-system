# COMPLETE CART SERVICE — BACKEND + FRONTEND IMPLEMENTATION

You are required to fully inspect, implement, improve, and verify the entire CartService, including BOTH Backend and Frontend.

## 1. READ DOCUMENTATION FIRST

Before doing anything, read these files:

- `docs/AI_RULE.md`
- `docs/CartService.md`

These documents are mandatory references.

Then inspect the entire existing CartService Backend and Frontend implementation.

Use existing working services and code templates as references. Follow the same project structure, naming conventions, Clean Architecture patterns, MediatR implementation, API integration style, authentication, authorization, and UI conventions.

## 2. IMPORTANT IMPLEMENTATION RULES

- Implement both Backend (C# ASP.NET Core) and Frontend (React/TypeScript).
- Do not just analyze, describe, or provide an implementation plan. Actually implement the code.
- Preserve all existing working features and correct implementations.
- Do not unnecessarily delete, rewrite, or replace existing files.
- Modify existing files only when necessary and ensure every modification is reasonable.
- Reuse existing components, services, hooks, DTOs, repositories, and infrastructure where appropriate.
- Do not create duplicate APIs or duplicate business logic.
- Follow the existing Clean Architecture and MediatR patterns.
- Ensure Backend and Frontend API contracts match.
- Do not use fake data or hardcoded cart items as a substitute for real API integration.
- Do not break other microservices or their existing integrations.

## 3. BACKEND — COMPLETE ALL 63 CART SERVICE APIs

Inspect the existing implementation first. Reuse existing working APIs and implement missing or incomplete functionality.

### A. Cart Management (1–12)

1. GET `/api/cart` — Get the current user's entire cart.
2. POST `/api/cart` — Add a product to the cart.
3. PUT `/api/cart/items/{itemId}` — Update a cart item.
4. PATCH `/api/cart/items/{itemId}/quantity` — Change product quantity.
5. DELETE `/api/cart/items/{itemId}` — Remove a cart item.
6. DELETE `/api/cart/clear` — Clear the entire cart.
7. GET `/api/cart/summary` — Get cart summary.
8. GET `/api/cart/count` — Get total product quantity.
9. GET `/api/cart/items` — Get cart items.
10. GET `/api/cart/items/{itemId}` — Get cart item details.
11. POST `/api/cart/items/batch` — Add multiple products at once.
12. DELETE `/api/cart/items/batch` — Remove multiple cart items.

### B. Cart Item Selection (13–19)

13. PATCH `/api/cart/items/{itemId}/select` — Select one item.
14. PATCH `/api/cart/items/{itemId}/unselect` — Unselect one item.
15. PATCH `/api/cart/select-all` — Select all items.
16. PATCH `/api/cart/unselect-all` — Unselect all items.
17. POST `/api/cart/selection` — Update multiple item selections.
18. GET `/api/cart/selected` — Get selected items.
19. GET `/api/cart/selected/summary` — Get selected items summary.

Implement Shopee-style item selection. Buyers must be able to select individual products, all products, or products belonging to a specific shop.

### C. Shop Grouping (20–25)

20. GET `/api/cart/shops` — Get shops in the cart.
21. GET `/api/cart/shops/{sellerId}` — Get products belonging to a shop.
22. PATCH `/api/cart/shops/{sellerId}/select` — Select all items from a shop.
23. PATCH `/api/cart/shops/{sellerId}/unselect` — Unselect all items from a shop.
24. GET `/api/cart/shops/{sellerId}/summary` — Get shop cart summary.
25. DELETE `/api/cart/shops/{sellerId}/items` — Remove all items from a shop.

Group cart items by Seller/Shop, with correct shop information and selection behavior.

### D. Cart Validation and Product Availability (26–32)

26. POST `/api/cart/validate` — Validate the entire cart.
27. POST `/api/cart/validate/selected` — Validate selected items.
28. GET `/api/cart/items/{itemId}/availability` — Check product availability.
29. POST `/api/cart/refresh` — Synchronize product information.
30. POST `/api/cart/items/refresh` — Refresh multiple products.
31. GET `/api/cart/invalid-items` — Get invalid cart items.
32. DELETE `/api/cart/invalid-items` — Remove invalid cart items.

Integrate with ProductService and InventoryService to validate product status, variant validity, and availability.

### E. Cart Pricing and Discounts (33–38)

33. GET `/api/cart/pricing` — Calculate entire cart pricing.
34. GET `/api/cart/selected/pricing` — Calculate selected items pricing.
35. GET `/api/cart/shops/{sellerId}/pricing` — Calculate shop pricing.
36. POST `/api/cart/recalculate` — Recalculate cart prices.
37. GET `/api/cart/discounts` — Get applicable discounts.
38. POST `/api/cart/discounts/preview` — Preview discounts.

Pricing must use trusted Backend data. Never trust prices or discount amounts sent by the frontend.

### F. Voucher Management (39–44)

39. GET `/api/cart/vouchers/available` — Get available vouchers.
40. POST `/api/cart/vouchers/apply` — Apply a voucher.
41. DELETE `/api/cart/vouchers/{voucherId}` — Remove an applied voucher.
42. GET `/api/cart/vouchers/applied` — Get applied vouchers.
43. POST `/api/cart/vouchers/preview` — Preview voucher discounts.
44. GET `/api/cart/shops/{sellerId}/vouchers` — Get shop vouchers.

Integrate with PromotionService for voucher validation and discount calculation.

Handle voucher expiration, eligibility, minimum order value, usage limits, shop restrictions, and duplicate application correctly.

### G. Cart Synchronization (45–49)

45. POST `/api/cart/sync` — Synchronize cart.
46. GET `/api/cart/sync/status` — Get synchronization status.
47. POST `/api/cart/merge` — Merge guest and authenticated user carts.
48. GET `/api/cart/devices` — Get synchronized device information, if supported.
49. DELETE `/api/cart/devices/{deviceId}` — Remove device-associated cart data, if supported.

Preserve cart data when users log in. Avoid duplicate items when merging guest and authenticated carts.

### H. Cart History and Recovery (50–53)

50. GET `/api/cart/history` — Get cart change history.
51. GET `/api/cart/recently-removed` — Get recently removed items.
52. POST `/api/cart/items/{itemId}/restore` — Restore a removed item.
53. POST `/api/cart/reorder/{orderId}` — Add products from a previous order.

Implement these features only with proper data models and business rules. Do not fabricate historical records.

### I. Checkout Preparation (54–57)

54. POST `/api/cart/checkout/preview` — Preview checkout information.
55. POST `/api/cart/checkout/validate` — Validate checkout conditions.
56. POST `/api/cart/checkout/prepare` — Prepare checkout data for OrderService.
57. GET `/api/cart/checkout/summary` — Get expected order summary.

Integrate with OrderService and ShippingService where required.

Ensure checkout only includes selected items.

Do not create orders or process payments inside CartService.

CartService prepares and validates cart data. OrderService owns actual order creation and immutable purchase snapshots.

### J. Internal Microservice APIs (58–63)

58. GET `/api/internal/carts/{userId}/summary` — Get cart summary for OrderService and Admin.
59. GET `/api/internal/carts/{userId}/items` — Get cart items for OrderService.
60. DELETE `/api/internal/carts/{userId}` — Clear cart after successful order placement.
61. POST `/api/internal/carts/{userId}/items/remove-batch` — Remove successfully ordered items.
62. GET `/api/internal/carts/{userId}/exists` — Check whether a cart exists.
63. GET `/api/internal/carts/statistics` — Get system-wide cart statistics.

These endpoints must use the existing Internal authentication mechanism.

Never allow unauthenticated users to access another user's cart.

## 4. BACKEND QUALITY AND MICROSERVICE BOUNDARIES

Ensure:

- Correct Clean Architecture separation: Api, Application, Domain, Infrastructure.
- MediatR Commands, Queries, and Handlers follow existing patterns.
- Correct DTOs, validation, repositories, and dependency injection.
- EF Core relationships, constraints, indexes, and migrations are correct.
- Proper pagination and filtering where necessary.
- Correct HTTP status codes and error handling.
- JWT authentication and authorization are enforced.
- UserId must come from authenticated JWT claims, not trusted frontend input.
- Prevent duplicate cart items for the same user and product variant.
- Validate quantity and product/variant status.
- Handle concurrent cart updates safely.
- Never allow negative quantities.
- Handle deleted or unavailable products correctly.
- Avoid N+1 queries and unnecessary service calls.

**Microservice boundaries are critical:**

- CartService owns cart data, cart items, selection, grouping, temporary pricing calculations, and cart synchronization.
- ProductService owns product information, variants, SKU definitions, and product prices.
- InventoryService owns actual stock quantities and reservations.
- PromotionService owns vouchers, discounts, and promotion rules.
- OrderService owns checkout completion, order creation, and immutable order snapshots.
- ShippingService owns shipping methods and shipping fee calculations.
- PaymentService owns payment transactions.
- AuthService owns authentication and JWT.
- UserService owns user and Seller profiles.

Important rules:

1. Never directly access another microservice's database.
2. Never trust frontend product prices, Seller IDs, voucher discounts, or final totals.
3. Retrieve authoritative product and variant information from ProductService.
4. Use InventoryService to check availability where necessary.
5. Use PromotionService to validate vouchers and promotions.
6. Do not treat cart validation as a final stock reservation.
7. Do not create orders or payments in CartService.
8. After an order succeeds, remove only the purchased cart items, not unselected items.
9. Internal endpoints must be protected using existing service-to-service authentication.
10. Avoid tightly coupling CartService to other services.

## 5. FRONTEND — COMPLETE CART SERVICE UI

Redesign and complete ALL CartService-related frontend screens.

The interface should have the quality and usability of Shopee, but with its own distinctive visual identity. Do not simply copy Shopee.

Create a beautiful, modern, professional shopping experience.

### A. Main Shopping Cart Page

Implement:

- Cart header and navigation.
- Products grouped by shop.
- Shop name and shop information.
- Product image and details.
- Product variants.
- Individual product selection.
- Select all products.
- Select all products from a shop.
- Quantity increase/decrease controls.
- Product price and discounted price.
- Remove individual items.
- Remove all items from a shop.
- Remove selected items.
- Invalid/unavailable product warnings.
- Recently removed products and restore actions.
- Empty cart state.

### B. Pricing and Checkout Section

Implement:

- Selected item count.
- Original subtotal.
- Product discounts.
- Voucher discounts.
- Shipping fee preview where supported.
- Estimated total.
- Available vouchers.
- Apply/remove voucher functionality.
- Checkout button.
- Clear explanation when checkout is unavailable.

The displayed total must match the actual Backend calculation.

### C. Voucher and Promotion UI

Implement:

- Available voucher list.
- Shop-specific vouchers.
- Platform vouchers.
- Voucher eligibility information.
- Minimum order value.
- Expiration information.
- Apply/remove voucher actions.
- Discount preview.
- Clear messages when a voucher cannot be applied.

### D. Cart Validation and Checkout UI

Implement:

- Product availability checks.
- Product status warnings.
- Variant validation.
- Price change notifications.
- Quantity validation.
- Checkout preview.
- Checkout validation.
- Proper navigation to the existing OrderService checkout flow.

Do not create fake checkout success screens.

### E. Responsive UI/UX

Requirements:

- Shopee-inspired shopping cart layout with a unique visual identity.
- Beautiful product cards and shop grouping.
- Consistent colors, typography, spacing, borders, shadows, and icons.
- Clear selection controls.
- Smooth quantity controls.
- Professional voucher dialogs and checkout summary.
- Responsive desktop, tablet, and mobile layouts.
- Proper loading skeletons.
- Empty states and error states.
- Clear success and failure notifications.
- No broken images.
- No overflowing layouts.
- No unnecessary animations.

Reuse the existing frontend architecture and shared design components where possible.

## 6. FRONTEND–BACKEND INTEGRATION

Connect every implemented CartService feature to actual Backend APIs.

Verify:

- Correct API base URL and endpoint paths.
- Correct request/response DTO mapping.
- JWT token attachment.
- User authentication and ownership checks.
- Add, update, remove, and clear cart items.
- Product quantity changes.
- Individual and bulk selection.
- Shop grouping and shop selection.
- Cart pricing and summary.
- Voucher application and removal.
- Product availability and cart refresh.
- Guest cart synchronization and merge.
- Checkout preview and validation.
- Internal cart cleanup after successful order placement.

Do not use hardcoded data or mock successful API responses as substitutes for real functionality.

## 7. DOCKER AND TESTING — MANDATORY BEFORE COMPLETION

I have already configured Docker for the entire microservices system.

**If you modify any C# Backend or Frontend code, you MUST finish implementation and then run:**

```bash
docker compose up --build
```

Run this command from the project root.

Wait for services to build and start. Inspect actual container status and logs.

Fix:

- Backend compilation errors.
- Frontend build errors.
- Docker startup errors.
- Database connection failures.
- Migration problems.
- API configuration issues.
- CORS problems.
- Microservice communication failures.
- Runtime exceptions.

If you are only performing normal runtime testing without code changes, use:

```bash
docker compose up
```

Do not unnecessarily rebuild unrelated services.

### Required testing

1. Build CartService Backend successfully.
2. Build Frontend successfully.
3. Verify CartService starts correctly in Docker.
4. Verify database connectivity.
5. Verify all 63 APIs are implemented or identify any intentionally unsupported endpoint with a clear reason.
6. Test actual API requests and HTTP status codes.
7. Test JWT authentication and authorization.
8. Test adding, updating, and removing cart items.
9. Test individual and bulk selection.
10. Test shop grouping and shop selection.
11. Test cart pricing and summary.
12. Test voucher application and removal.
13. Test product availability validation.
14. Test guest cart synchronization and merging.
15. Test checkout preview and validation.
16. Test internal cart cleanup after successful order placement.
17. Open the actual frontend and inspect the rendered interface.
18. Test the main cart and checkout-related user workflows.
19. Check browser console errors and failed network requests.

Do not claim an API works just because its controller or method exists.

Do not claim the UI works just because the frontend build succeeds.

## 8. DOCKER SAFETY RULES

- NEVER run `docker compose down -v`.
- NEVER delete existing Docker volumes or databases.
- NEVER delete existing user data.
- Do not modify unrelated microservices unless a genuine integration issue requires it.
- Preserve all existing working functionality.
- Avoid unnecessary image pulls and rebuilds.

## 9. FINAL COMPLETION REPORT

Only report completion after implementation and verification.

Provide:

1. Backend files created or modified.
2. Frontend files created or modified.
3. Existing APIs reused.
4. Newly implemented or fixed APIs.
5. Database changes and migrations.
6. Frontend pages and features completed.
7. Backend build result.
8. Frontend build result.
9. Docker container status.
10. Actual API testing results.
11. Actual frontend testing results.
12. Integration testing results with other microservices.
13. Remaining bugs or blockers, if any.

Do not mark untested features as completed.

**FINAL REQUIREMENT:**

Do not stop after reading documentation, analyzing code, or writing an implementation plan.

Do not simply respond "Already done" without checking the actual source code and Git changes.

Actually implement the Backend and Frontend, preserve correct existing code, run the required Docker commands, test the application, fix discovered problems, and only then report completion.