# PROMPT: COMPLETE PROMOTIONSERVICE BACKEND & FRONTEND IMPLEMENTATION — ECOMSYSTEM

## 1. PROJECT OBJECTIVE

You are working on the existing **EcomSystem** microservices-based e-commerce project.

Your task is to thoroughly inspect, complete, improve, and implement the entire **PromotionService Backend and Frontend** according to:

1. `docs/AI_RULE.md`
2. `docs/PromotionService.md`
3. Existing EcomSystem architecture.
4. Existing Backend and Frontend coding conventions.
5. Existing authentication, authorization, database, RabbitMQ, Redis, and Docker configurations.
6. The 100 API requirements specified in this prompt.

The goal is to build a complete, production-oriented promotion and discount system similar in functionality and user experience to Shopee.

The Frontend must be visually attractive, professional, modern, and consistent with EcomSystem's own branding.

The Backend must implement correct promotion business logic, accurate discount calculations, secure authorization, concurrency control, voucher reservation, and reliable integration with other microservices.

**Do not stop after writing code. You must build, run, test, inspect the actual application, fix discovered issues, and verify the complete workflows.**

---

# 2. MANDATORY RULES — READ BEFORE CODING

## 2.1 Read documentation and inspect the existing project

Before making any changes:

1. Read the entire `docs/AI_RULE.md`.
2. Read the entire `docs/PromotionService.md`.
3. Inspect the entire existing PromotionService Backend.
4. Inspect the existing React Frontend.
5. Inspect current promotion-related pages and components.
6. Inspect existing ProductService APIs and data contracts.
7. Inspect CartService discount and voucher integration.
8. Inspect OrderService checkout and order creation workflows.
9. Inspect ShippingService fee calculation contracts.
10. Inspect InventoryService integration if Flash Sale requires stock validation.
11. Inspect AuthService JWT and role handling.
12. Inspect Seller identity and ownership handling.
13. Inspect existing RabbitMQ event contracts and consumers.
14. Inspect existing Redis usage.
15. Inspect existing Docker Compose configuration.

Understand the existing implementation before coding.

## 2.2 Preserve existing correct code

This is a critical requirement.

- Do NOT delete existing files unnecessarily.
- Do NOT rewrite entire controllers, handlers, repositories, services, or React components when they are already correct.
- Do NOT remove working functionality.
- Do NOT replace correct implementations with simplified alternatives.
- Do NOT create duplicate entities, DTOs, interfaces, commands, queries, repositories, or API routes unnecessarily.
- Reuse existing correct code.
- Follow the existing project structure and coding conventions.
- Modify only the parts that genuinely require improvement or completion.
- Add new files only when necessary.
- Preserve existing API contracts used by other microservices.
- Ensure unrelated EcomSystem services continue working.

**Your task is to complete and improve the existing implementation, not rebuild the project from scratch.**

If an existing feature works correctly, preserve it.

If an implementation is incomplete, insecure, or incorrect, fix only the necessary parts.

## 2.3 No fake features

Do NOT:

- Use hardcoded promotions as real database records.
- Display fake voucher availability.
- Pretend a voucher was successfully collected when the API failed.
- Calculate final discounts exclusively on the Frontend.
- Trust prices submitted by the Frontend.
- Simulate successful voucher redemption without persistence.
- Pretend Flash Sale stock is available without checking InventoryService.
- Display fake campaign statistics.
- Create non-functional buttons.
- Show success messages before Backend operations succeed.
- Use mock data as the final implementation.

All implemented features must use real APIs and persistent data.

---

# 3. TECHNOLOGY AND ARCHITECTURE

Follow the existing EcomSystem technology stack.

## Backend

Expected technologies:

- C#
- ASP.NET Core Web API
- .NET 8
- Clean Architecture
- CQRS
- MediatR
- Entity Framework Core
- SQL Server
- JWT Authentication
- Role-based authorization
- Dependency Injection
- RabbitMQ where justified
- Redis where justified
- Swagger / OpenAPI
- Structured logging
- Docker

Follow the existing project structure, such as:

- PromotionService.Api
- PromotionService.Application
- PromotionService.Domain
- PromotionService.Infrastructure

Do not introduce a different architecture if the existing structure is already correct.

## Frontend

Expected technologies:

- React
- TypeScript
- Vite
- Existing routing system.
- Existing API clients.
- Existing authentication mechanism.
- Existing UI components and styling conventions.

Avoid unnecessary dependencies.

---

# 4. PROMOTIONSERVICE RESPONSIBILITIES AND MICROSERVICE BOUNDARIES

PromotionService owns:

- Promotion campaigns.
- Discount rules.
- Voucher definitions.
- Voucher collection.
- Voucher eligibility.
- Promotion validation.
- Discount calculation.
- Flash Sale configuration.
- Product discount rules.
- Shipping discount rules.
- Promotion usage.
- Voucher redemption.
- Promotion reservations.
- Promotion statistics and reports.

Respect the following service ownership:

| Microservice | Responsibility |
|---|---|
| ProductService | Product information, categories, brands, product list prices |
| InventoryService | Stock quantities, stock reservations, warehouse inventory |
| CartService | Shopping cart and temporary cart calculations |
| OrderService | Final checkout validation, order creation and immutable order pricing snapshot |
| ShippingService | Base shipping fee and shipping service information |
| PromotionService | Promotion rules, vouchers, discount eligibility and redemption |
| PaymentService | Payment transactions and refunds |
| UserService | User profiles and Seller identity |
| AuthService | Authentication and JWT |
| NotificationService | Promotion notifications and voucher notifications |

Important:

1. PromotionService must not own product inventory.
2. PromotionService must not directly modify product prices in ProductService.
3. PromotionService must not create orders.
4. PromotionService must not process payments.
5. PromotionService must not calculate base shipping fees independently of ShippingService.
6. PromotionService may calculate the discount applied to an eligible shipping fee.
7. OrderService remains responsible for the final order pricing snapshot.
8. Do not access another microservice's SQL Server database directly.
9. Use existing internal APIs or RabbitMQ events for cross-service communication.
10. Avoid unnecessary synchronous calls that make checkout unreliable.

---

# 5. COMPLETE BACKEND API REQUIREMENTS

Implement all required API capabilities below.

Inspect existing controllers and endpoints first.

Reuse existing correct routes.

Do not create duplicate APIs merely to match the endpoint count. Consolidate equivalent functionality where appropriate while preserving existing consumers and API contracts.

Every endpoint must have:

- Correct HTTP method.
- Correct route.
- Appropriate request and response DTOs.
- Validation.
- Authorization.
- Correct business logic.
- Ownership checks.
- Consistent error handling.
- Pagination for collection endpoints.
- Structured logging where appropriate.
- Tests for critical business rules.

## GROUP 1 — Promotion Management (12 APIs)

1. `POST /api/promotions` — Create a promotion.
2. `GET /api/promotions` — Get promotions with pagination and filtering.
3. `GET /api/promotions/{id}` — Get promotion details.
4. `PUT /api/promotions/{id}` — Update a promotion.
5. `DELETE /api/promotions/{id}` — Delete or deactivate a promotion according to policy.
6. `PATCH /api/promotions/{id}/activate` — Activate a promotion.
7. `PATCH /api/promotions/{id}/deactivate` — Deactivate a promotion.
8. `PATCH /api/promotions/{id}/status` — Change promotion status according to business rules.
9. `GET /api/promotions/active` — Get active promotions.
10. `GET /api/promotions/upcoming` — Get upcoming promotions.
11. `GET /api/promotions/expired` — Get expired promotions.
12. `GET /api/promotions/search?keyword=` — Search promotions.

Support promotion configurations such as:

- Percentage discount.
- Fixed amount discount.
- Minimum order value.
- Maximum discount amount.
- Category-based promotions.
- Seller-specific promotions.
- Product-specific promotions.
- Start and end dates.
- Usage limits.
- Per-user usage limits.
- Applicable customer groups where supported.

Validate promotion dates, discount values, minimum order amounts, and eligibility rules.

Prevent invalid discount configurations.

## GROUP 2 — Voucher Management (12 APIs)

13. `POST /api/vouchers` — Create a voucher.
14. `GET /api/vouchers` — List vouchers.
15. `GET /api/vouchers/{id}` — Get voucher details.
16. `PUT /api/vouchers/{id}` — Update a voucher.
17. `DELETE /api/vouchers/{id}` — Delete or deactivate a voucher.
18. `GET /api/vouchers/code/{code}` — Find voucher by code.
19. `GET /api/vouchers/active` — Get active vouchers.
20. `PATCH /api/vouchers/{id}/activate` — Activate voucher.
21. `PATCH /api/vouchers/{id}/deactivate` — Deactivate voucher.
22. `GET /api/vouchers/{id}/statistics` — Voucher usage statistics.
23. `POST /api/vouchers/{id}/duplicate` — Duplicate voucher configuration.
24. `GET /api/vouchers/search?keyword=` — Search vouchers.

Voucher configuration should support:

- Unique voucher code.
- Percentage or fixed discount.
- Minimum order value.
- Maximum discount amount.
- Total usage limit.
- Per-user usage limit.
- Start and expiration time.
- Applicable products.
- Applicable categories.
- Applicable Seller.
- Applicable customer audience.
- Collection limit.
- Voucher status.
- Optional shipping discount.

Requirements:

- Normalize voucher codes consistently.
- Enforce code uniqueness.
- Validate expiration.
- Validate usage limits.
- Prevent unauthorized modifications.
- Avoid hard deletion of vouchers with historical redemption records.

## GROUP 3 — Buyer Voucher Management (9 APIs)

25. `GET /api/vouchers/available` — Get vouchers available for collection.
26. `GET /api/vouchers/me` — Get the authenticated user's voucher wallet.
27. `GET /api/vouchers/me/available` — Get collected and currently usable vouchers.
28. `GET /api/vouchers/me/used` — Get used vouchers.
29. `GET /api/vouchers/me/expired` — Get expired vouchers.
30. `POST /api/vouchers/{id}/collect` — Collect a voucher.
31. `POST /api/vouchers/collect/code` — Collect a voucher using its code.
32. `DELETE /api/vouchers/me/{voucherId}` — Remove a voucher from the personal wallet if policy permits.
33. `GET /api/vouchers/me/statistics` — Get personal voucher statistics.

Important distinction:

- A voucher being available on the platform does not mean the user has collected it.
- A collected voucher does not automatically qualify for every cart.
- A voucher preview does not mean it has been redeemed.
- A voucher reservation is temporary and must have an expiration policy.
- A voucher becomes officially used only after the corresponding order workflow confirms redemption.

Enforce per-user collection limits.

Prevent duplicate collection where the business rules do not allow it.

Derive the current Buyer UserId from JWT.

## GROUP 4 — Promotion Validation and Calculation (9 APIs)

34. `POST /api/promotions/validate` — Validate promotion eligibility.
35. `POST /api/vouchers/validate` — Validate voucher eligibility.
36. `POST /api/promotions/calculate` — Calculate promotion discount.
37. `POST /api/vouchers/calculate` — Calculate voucher discount.
38. `POST /api/promotions/preview` — Preview promotion application.
39. `POST /api/vouchers/preview` — Preview voucher application.
40. `POST /api/promotions/validate-batch` — Validate multiple promotions.
41. `POST /api/vouchers/validate-batch` — Validate multiple vouchers.
42. `POST /api/promotions/best-deal` — Find the best eligible promotion combination according to business rules.

This is one of the most important parts of PromotionService.

Implement centralized discount calculation logic.

### Validation rules

Validate:

- Promotion or voucher exists.
- Active status.
- Start and expiration time.
- Minimum order amount.
- Maximum discount.
- Eligible products.
- Eligible categories.
- Eligible Seller.
- User eligibility.
- Collection status where required.
- Total usage limit.
- Per-user usage limit.
- Existing redemption records.
- Promotion stacking rules.
- Applicable shipping conditions.
- Currency and rounding rules.

### Discount calculation

Support:

- Percentage discounts.
- Fixed amount discounts.
- Maximum discount caps.
- Minimum purchase thresholds.
- Product-level discounts.
- Order-level discounts.
- Seller-level discounts.
- Shipping discounts.
- Flash Sale discounts.

Important:

- Never trust a Frontend-submitted price as authoritative.
- Retrieve or validate product pricing using trusted ProductService contracts.
- Use trusted shipping fee data from ShippingService.
- Clearly distinguish estimated discounts from confirmed order discounts.
- Use decimal arithmetic, not floating-point arithmetic, for monetary calculations.
- Define consistent rounding rules.
- Prevent discounts from exceeding the eligible amount.
- Avoid applying the same promotion twice.
- Return clear validation reasons when a voucher is invalid.

### Promotion stacking

Define explicit stacking rules.

For example:

- Product discount.
- Shop voucher.
- Platform voucher.
- Shipping voucher.

Do not automatically combine all discounts.

Check compatibility and business policy.

The best-deal algorithm must calculate valid combinations without allowing incompatible promotions to stack.

## GROUP 5 — Flash Sale (9 APIs)

43. `POST /api/flash-sales` — Create a Flash Sale.
44. `GET /api/flash-sales` — List Flash Sales.
45. `GET /api/flash-sales/{id}` — Get Flash Sale details.
46. `PUT /api/flash-sales/{id}` — Update Flash Sale.
47. `DELETE /api/flash-sales/{id}` — Delete or deactivate Flash Sale.
48. `GET /api/flash-sales/active` — Get active Flash Sales.
49. `GET /api/flash-sales/upcoming` — Get upcoming Flash Sales.
50. `POST /api/flash-sales/{id}/products` — Add a product to Flash Sale.
51. `DELETE /api/flash-sales/{id}/products/{productId}` — Remove a product from Flash Sale.

Flash Sale requirements:

- Start and end time.
- Participating products.
- Discount configuration.
- Maximum promotional quantity where supported.
- Per-user purchase limits where supported.
- Seller ownership.
- Campaign status.
- Product eligibility.
- Actual stock validation through InventoryService.

Critical boundary:

**PromotionService owns Flash Sale rules and promotional limits, but InventoryService owns actual stock.**

Do not create a second stock database inside PromotionService.

Prevent overselling by coordinating promotional purchase limits with InventoryService and the existing OrderService checkout flow.

Avoid trusting the Frontend countdown timer as the source of truth.

The Backend must determine whether a Flash Sale is active.

## GROUP 6 — Product Discounts (7 APIs)

52. `POST /api/product-discounts` — Create a product discount.
53. `GET /api/product-discounts` — List product discounts.
54. `GET /api/product-discounts/{id}` — Get discount details.
55. `PUT /api/product-discounts/{id}` — Update discount.
56. `DELETE /api/product-discounts/{id}` — Delete discount.
57. `GET /api/product-discounts/product/{productId}` — Get discounts for a product.
58. `GET /api/product-discounts/active` — Get active product discounts.

Requirements:

- Associate discounts with existing ProductService product identifiers.
- Validate Seller ownership.
- Validate discount period.
- Prevent invalid discount amounts.
- Handle conflicting active discounts.
- Define precedence between product discounts and other promotions.
- Do not directly overwrite ProductService's base product price.
- Return calculated promotional pricing information through a clear API contract.

The Frontend may display original and promotional prices, but the Backend remains authoritative.

## GROUP 7 — Shipping Promotions (5 APIs)

59. `POST /api/shipping-promotions` — Create a shipping promotion.
60. `GET /api/shipping-promotions` — List shipping promotions.
61. `GET /api/shipping-promotions/available` — Get available shipping promotions.
62. `POST /api/shipping-promotions/validate` — Validate shipping promotion.
63. `POST /api/shipping-promotions/calculate` — Calculate shipping discount.

Requirements:

- Free shipping.
- Fixed shipping discount.
- Percentage shipping discount.
- Maximum shipping discount.
- Minimum order amount.
- Eligible destinations or shipping methods where supported.
- Validity period.
- User eligibility.
- Usage limits.

Important:

ShippingService calculates the original shipping fee.

PromotionService calculates the eligible discount against that fee.

Do not invent or independently calculate the base shipping fee.

Do not allow the final shipping cost to become negative.

## GROUP 8 — Seller Promotion Management (6 APIs)

64. `GET /api/seller/promotions` — Get current Seller's promotions.
65. `GET /api/seller/promotions/{id}` — Get Seller promotion details.
66. `GET /api/seller/vouchers` — Get Seller's vouchers.
67. `GET /api/seller/vouchers/statistics` — Get Seller voucher statistics.
68. `GET /api/seller/flash-sales` — Get Seller Flash Sales.
69. `GET /api/seller/promotions/statistics` — Get Seller promotion statistics.

Requirements:

- SellerId must be derived from authenticated JWT claims or trusted existing identity resolution.
- Never trust SellerId from a query parameter or request body for ownership.
- A Seller must only manage their own promotions.
- A Seller must never view another Seller's private promotion statistics.
- Validate ownership before updating, deleting, activating, or duplicating a promotion.
- Follow the existing Seller role and account-status rules.

If Admin approval is required for certain promotions, implement the approval workflow consistently.

## GROUP 9 — Promotion Usage and Redemption (7 APIs)

70. `POST /api/promotions/reservations` — Reserve promotion usage during checkout.
71. `POST /api/promotions/reservations/{id}/confirm` — Confirm redemption after a valid order is created.
72. `POST /api/promotions/reservations/{id}/release` — Release reservation after failed or cancelled checkout.
73. `GET /api/promotions/usage/me` — Get current user's promotion usage history.
74. `GET /api/promotions/usage/{promotionId}` — Get promotion usage history.
75. `GET /api/vouchers/usage/{voucherId}` — Get voucher usage history.
76. `GET /api/promotions/usage/me/{promotionId}` — Check current user's usage of a promotion.

This group is critical for financial correctness.

### Reservation lifecycle

Implement a proper reservation state machine:

- Reserved.
- Confirmed.
- Released.
- Expired.

Expected workflow:

1. Buyer applies a voucher or promotion during checkout.
2. CartService or OrderService requests validation.
3. OrderService requests a reservation.
4. PromotionService atomically reserves the applicable usage capacity.
5. Reservation receives a unique identifier and expiration time.
6. OrderService creates the valid order.
7. OrderService confirms the reservation.
8. PromotionService records the official redemption.
9. If checkout fails or is cancelled, the reservation is released.
10. Expired reservations are automatically released according to a reliable expiration mechanism.

### Concurrency and consistency requirements

- Prevent usage limits from being exceeded.
- Prevent two concurrent requests from reserving the last available voucher usage.
- Use transactions and appropriate database concurrency controls.
- Use unique idempotency identifiers.
- Prevent duplicate reservation confirmation.
- Prevent duplicate redemption records.
- Prevent invalid release after confirmation.
- Ensure expiration and confirmation are handled safely when they happen concurrently.
- Use appropriate indexes and concurrency tokens.
- Use background processing for reservation expiration if needed.

Do not implement a simple check-then-insert flow that can oversubscribe a voucher under concurrent checkout requests.

## GROUP 10 — Internal Microservice APIs (8 APIs)

77. `POST /api/internal/promotions/apply` — Apply a promotion for OrderService.
78. `POST /api/internal/promotions/validate` — Validate promotion for CartService or OrderService.
79. `POST /api/internal/promotions/calculate` — Calculate discount for CartService or OrderService.
80. `GET /api/internal/promotions/{id}/summary` — Get promotion summary.
81. `GET /api/internal/vouchers/{code}/summary` — Get voucher summary.
82. `POST /api/internal/promotions/reservations` — Create promotion reservation.
83. `POST /api/internal/promotions/reservations/{id}/confirm` — Confirm reservation.
84. `POST /api/internal/promotions/reservations/{id}/release` — Release reservation.

Requirements:

- Use existing service-to-service authentication.
- Do not expose internal APIs to ordinary Buyer or Seller sessions.
- Validate the calling service where appropriate.
- Validate payloads and identifiers.
- Use idempotency.
- Return stable response contracts.
- Keep internal endpoints compatible with CartService and OrderService.
- Do not accept an arbitrary final discount amount from another service without validating the applicable promotion rules.

## GROUP 11 — Admin Promotion Management (7 APIs)

85. `GET /api/admin/promotions` — Manage all promotions.
86. `GET /api/admin/promotions/pending` — List promotions awaiting approval.
87. `POST /api/admin/promotions/{id}/approve` — Approve promotion.
88. `POST /api/admin/promotions/{id}/reject` — Reject promotion.
89. `POST /api/admin/promotions/{id}/suspend` — Suspend promotion.
90. `GET /api/admin/vouchers` — Manage all vouchers.
91. `GET /api/admin/promotions/statistics` — Platform-wide promotion statistics.

Requirements:

- Admin-only authorization.
- Search and filtering.
- Pagination.
- Promotion approval history.
- Rejection reason.
- Suspension reason.
- Audit logs.
- Platform-wide statistics.
- Proper validation before approval.

If existing business rules allow certain Seller promotions without approval, preserve those rules.

Do not force every Seller promotion into an approval workflow if the existing architecture does not require it.

Platform-wide or financially sensitive campaigns should remain subject to the appropriate Admin controls.

## GROUP 12 — Reports and Analytics (5 APIs)

92. `GET /api/reports/promotions/overview` — Promotion overview.
93. `GET /api/reports/promotions/usage` — Promotion usage report.
94. `GET /api/reports/vouchers/performance` — Voucher performance.
95. `GET /api/reports/flash-sales/performance` — Flash Sale performance.
96. `GET /api/reports/promotions/revenue-impact` — Estimated promotion revenue impact.

Use actual persisted data.

Potential metrics:

- Total campaigns.
- Active campaigns.
- Total vouchers issued.
- Vouchers collected.
- Voucher redemption count.
- Redemption rate.
- Total discount amount.
- Promotion usage by category.
- Flash Sale performance.
- Seller promotion performance.
- Revenue before and after discounts where the data is available.

Important:

- Clearly distinguish actual revenue from estimated revenue impact.
- Do not fabricate conversion rates.
- Use reliable order and redemption data.
- Avoid directly querying another service's database.
- Use existing event data, internal APIs, or suitable denormalized reporting data.
- Respect data ownership.

## GROUP 13 — Internal Monitoring (4 APIs)

97. `GET /api/internal/health` — PromotionService health.
98. `GET /api/internal/statistics` — Internal service statistics.
99. `GET /api/internal/events/failed` — Monitor failed events.
100. `POST /api/internal/events/{eventId}/retry` — Retry failed event.

These endpoints must be protected according to the existing internal authorization and Admin policies.

Do not expose sensitive infrastructure information publicly.

---

# 6. FRONTEND — COMPLETE SHOPEE-INSPIRED PROMOTION EXPERIENCE

This is a major part of the task.

Do not build only a basic CRUD management page.

Implement a complete promotion experience integrated into the existing EcomSystem React Frontend.

The UI should feel like a modern, professional e-commerce platform inspired by Shopee, while maintaining EcomSystem's own branding.

## 6.1 General UI requirements

Design a polished interface with:

- Attractive promotion banners.
- Modern voucher cards.
- Flash Sale countdown.
- Product discount labels.
- Clear original and promotional prices.
- Seller promotion dashboard.
- Admin campaign management.
- Consistent typography.
- Professional spacing.
- Responsive layouts.
- Appropriate icons.
- Clear visual hierarchy.
- Subtle animations.
- Loading states.
- Empty states.
- Error states.
- Confirmation dialogs.

Avoid generic CRUD tables as the entire user experience.

Use tables where appropriate for Admin and Seller management, but use more attractive e-commerce layouts for Buyer-facing pages.

Do not copy Shopee's exact branding, logo, or assets.

## 6.2 Buyer promotion homepage

Create or improve the promotion discovery experience.

Include:

- Promotion banners.
- Active promotions.
- Upcoming promotions.
- Flash Sale section.
- Shop vouchers.
- Platform vouchers.
- Free shipping vouchers.
- Product discounts.
- Promotion categories.
- Search and filters.
- Voucher collection actions.

Use real API data.

Do not hardcode campaigns.

## 6.3 Buyer Voucher Wallet

Create a complete voucher wallet page.

Expected tabs:

- All.
- Available.
- Used.
- Expired.

Each voucher card should display:

- Voucher code.
- Discount type.
- Discount amount or percentage.
- Maximum discount.
- Minimum order value.
- Applicable conditions.
- Expiration date.
- Collection status.
- Usage status.
- Applicable Seller or product scope.
- Clear action button.

Required interactions:

- Collect voucher.
- Collect by code.
- View voucher details.
- Copy voucher code.
- Navigate to applicable products or checkout.
- Remove voucher if permitted.
- Filter and search vouchers.

Display clear messages explaining why a voucher cannot be used.

## 6.4 Checkout voucher application

Integrate promotion and voucher functionality into the existing checkout workflow.

Display:

- Available vouchers.
- Collected vouchers.
- Shop voucher.
- Platform voucher.
- Shipping voucher.
- Selected voucher.
- Discount preview.
- Minimum order requirement.
- Invalid voucher reason.
- Total estimated discount.

Requirements:

- Call Backend validation APIs.
- Call Backend calculation APIs.
- Never calculate the authoritative discount only on the Frontend.
- Do not trust client-submitted product prices.
- Display the Backend's returned calculation breakdown.
- Recalculate when the cart changes.
- Handle voucher expiration and invalidation.
- Do not mark a voucher as used during preview.
- Reserve and confirm redemption through the appropriate checkout workflow.

Preserve existing CartService and OrderService checkout behavior.

Do not break existing cart or order functionality.

## 6.5 Flash Sale page

Create a visually attractive Flash Sale experience.

Include:

- Active Flash Sale banner.
- Countdown timer.
- Upcoming Flash Sale schedule.
- Product cards.
- Promotional price.
- Original price.
- Discount percentage.
- Product images.
- Stock information only when available from InventoryService.
- Seller information.
- Navigation to product detail.

Countdown requirements:

- Use Backend start and end timestamps.
- Calculate remaining time correctly.
- Handle timezone consistently.
- Do not trust the Frontend timer for campaign eligibility.
- Automatically refresh campaign status where appropriate.

## 6.6 Product detail and product listing integration

Integrate product discount information into existing product cards and product detail pages.

Display:

- Original price.
- Promotional price.
- Discount percentage.
- Flash Sale badge.
- Applicable promotion labels.
- Voucher eligibility where useful.

Do not modify unrelated ProductService logic.

Use existing ProductService data contracts and PromotionService pricing information.

Avoid duplicate or inconsistent discount calculations across Frontend components.

## 6.7 Seller Center — Promotion Management

Create a dedicated Seller promotion management experience.

Expected sections:

1. Seller Promotion Dashboard.
2. My Promotions.
3. Create Promotion.
4. Edit Promotion.
5. My Vouchers.
6. Create Voucher.
7. Voucher Statistics.
8. My Flash Sales.
9. Create or Manage Flash Sale participation.
10. Promotion Performance.

Seller should be able to:

- Create eligible shop promotions.
- Create shop vouchers.
- Update their own promotions.
- Activate or deactivate permitted promotions.
- View voucher usage.
- View promotion statistics.
- Manage Flash Sale participation where supported.

Security:

- Never allow the Frontend to choose an arbitrary SellerId for ownership.
- Backend must resolve Seller identity from JWT.
- Hide unavailable operations based on actual permissions.
- Display pending approval or rejected status when applicable.

## 6.8 Admin Promotion Center

Create a professional Admin management area.

Expected pages:

1. Promotion Overview Dashboard.
2. All Promotions.
3. Pending Promotions.
4. Promotion Details.
5. Approve Promotion.
6. Reject Promotion.
7. Suspend Promotion.
8. All Vouchers.
9. Voucher Details.
10. Flash Sale Management.
11. Product Discount Management.
12. Shipping Promotion Management.
13. Promotion Reports.
14. Voucher Performance.
15. Flash Sale Performance.
16. Revenue Impact.
17. Internal Monitoring.

Admin dashboard must use actual API data.

Display:

- Active promotions.
- Upcoming promotions.
- Expired promotions.
- Pending approvals.
- Total vouchers.
- Total collected vouchers.
- Total redemptions.
- Total discount amount.
- Flash Sale statistics.
- Failed event count where available.

Include search, filters, pagination, sorting, and detail views.

## 6.9 Promotion forms

Create proper validation for:

- Promotion name.
- Description.
- Promotion type.
- Discount value.
- Minimum order amount.
- Maximum discount.
- Start date.
- End date.
- Eligible products.
- Eligible categories.
- Seller scope.
- Usage limits.
- Per-user limits.
- Voucher code.
- Flash Sale settings.
- Shipping discount rules.

Use clear validation messages.

Prevent invalid date ranges and negative monetary values.

Do not allow invalid discounts to be submitted.

## 6.10 Navigation and integration

Integrate promotion pages into the existing application navigation.

Use the existing layout and routing conventions.

Buyer navigation may include:

- Promotions.
- Voucher Wallet.
- Flash Sale.

Seller navigation may include:

- My Promotions.
- My Vouchers.
- Flash Sale.
- Promotion Statistics.

Admin navigation may include:

- Promotion Management.
- Voucher Management.
- Flash Sale Management.
- Promotion Reports.

Do not create duplicate navigation entries.

Do not break existing routes or pages.

---

# 7. FRONTEND API AND STATE MANAGEMENT

Requirements:

- Reuse existing API clients.
- Reuse existing JWT handling.
- Reuse existing routing.
- Use proper TypeScript interfaces.
- Avoid unnecessary `any`.
- Connect pages to real Backend endpoints.
- Use consistent loading states.
- Use consistent error handling.
- Prevent duplicate submissions.
- Update data after successful mutations.
- Handle expired vouchers.
- Handle API validation errors.
- Handle unavailable services.
- Use appropriate caching where justified.

Do not store authoritative discount values in Frontend state.

The Backend must remain the source of truth.

---

# 8. DATABASE AND DOMAIN MODEL REQUIREMENTS

Inspect existing entities before creating new ones.

Potential domain models:

- Promotion.
- PromotionRule.
- PromotionProduct.
- PromotionCategory.
- Voucher.
- UserVoucher.
- VoucherRedemption.
- PromotionReservation.
- FlashSale.
- FlashSaleProduct.
- ProductDiscount.
- ShippingPromotion.
- PromotionUsage.
- ProcessedEvent.
- AuditLog.

Do not blindly create all models.

Reuse existing entities where appropriate.

## 8.1 Data integrity

- Unique voucher code.
- Valid discount ranges.
- Valid campaign date ranges.
- Correct relationships.
- Appropriate indexes.
- Concurrency controls.
- Idempotency identifiers.
- Proper foreign-key constraints.
- Efficient pagination.
- Consistent status transitions.

## 8.2 Database migrations

- Use existing DbContext.
- Follow current EF Core conventions.
- Create migrations only when required.
- Preserve existing records.
- Never reset the database.
- Never delete Docker volumes.
- Avoid destructive schema changes.
- Review migration scripts before applying them.

---

# 9. DISCOUNT CALCULATION AND FINANCIAL CORRECTNESS

This section is mandatory.

Implement discount calculation centrally and consistently.

For every calculation, distinguish:

1. Original product price.
2. Product discount.
3. Promotion discount.
4. Voucher discount.
5. Shipping fee.
6. Shipping discount.
7. Final eligible amount.
8. Final order amount.

Define explicit calculation order.

Do not subtract the same discount twice.

Do not allow discount stacking unless the configured rules permit it.

Use decimal types for currency.

Validate all monetary amounts on the Backend.

Do not allow a negative final payable amount.

Do not trust:

- Frontend discount values.
- Frontend product prices.
- Frontend shipping fees.
- Frontend SellerId.
- Frontend UserId.
- Frontend usage counts.

Use trusted service contracts and authenticated identity.

For a real order, OrderService must store the final price snapshot and applicable discount details.

PromotionService must retain the correct redemption records.

---

# 10. RABBITMQ, REDIS AND BACKGROUND PROCESSING

Inspect existing integrations first.

Use RabbitMQ only when asynchronous event communication is justified.

Potential events:

- PromotionCreated.
- PromotionActivated.
- PromotionDeactivated.
- VoucherCreated.
- VoucherCollected.
- VoucherRedeemed.
- VoucherExpired.
- FlashSaleStarted.
- FlashSaleEnded.
- PromotionSuspended.

Potential consumers:

- NotificationService for promotion notifications.
- Other services that need promotion lifecycle updates.

Do not invent event contracts that conflict with existing service conventions.

Requirements:

- Validate incoming events.
- Use stable event identifiers.
- Implement idempotent processing.
- Handle duplicate messages.
- Use retries and dead-letter queues according to existing architecture.
- Log failures.
- Do not acknowledge a message before the operation is safely processed.

Redis may be used for:

- Frequently requested active promotions.
- Flash Sale display data.
- Read-heavy voucher information.
- Temporary calculation caching where appropriate.

Important:

- Do not cache authoritative voucher redemption counts without a reliable consistency mechanism.
- Do not use cache alone to enforce usage limits.
- Do not rely on Redis cached availability to guarantee a voucher reservation.
- Invalidate or update caches after promotion changes.
- Ensure cache failures do not corrupt business operations.

Use background workers for:

- Expired promotion status updates.
- Reservation expiration.
- Scheduled campaign activation.
- Flash Sale transitions.
- Scheduled cleanup where policy permits.

Do not run long-running campaign processing inside HTTP request handlers.

---

# 11. AUTHORIZATION AND SECURITY

Expected access:

| Role | Access |
|---|---|
| Buyer | Browse promotions, collect vouchers, view personal wallet, validate eligible vouchers |
| Seller | Manage own eligible promotions, vouchers, and Flash Sale participation |
| Admin | Platform-wide management, approval, suspension and reports |
| Internal services | Authenticated internal validation, calculation and reservation operations |

Security requirements:

1. Derive Buyer UserId from JWT.
2. Derive SellerId from JWT or trusted existing identity resolution.
3. Never trust ownership identifiers supplied by Frontend.
4. Restrict Admin endpoints.
5. Restrict Internal endpoints.
6. Validate ownership before modifications.
7. Prevent IDOR vulnerabilities.
8. Validate all input DTOs.
9. Rate-limit voucher collection and code lookup where appropriate.
10. Prevent brute-force voucher code enumeration.
11. Avoid exposing internal financial information.
12. Audit sensitive Admin actions.
13. Prevent unauthorized promotion activation.
14. Prevent Seller access to other Sellers' statistics.
15. Enforce correct redemption and reservation permissions.

---

# 12. TESTING REQUIREMENTS

Testing is mandatory.

Do not finish after the code compiles.

## 12.1 Backend tests

Build the Backend solution.

Run existing tests and add tests for critical business logic.

### Promotion management

- Create valid promotion.
- Reject invalid discount.
- Reject invalid date range.
- Update promotion.
- Activate promotion.
- Deactivate promotion.
- Prevent unauthorized Seller modifications.

### Voucher management

- Create voucher.
- Enforce unique voucher code.
- Reject expired voucher.
- Reject inactive voucher.
- Enforce total usage limit.
- Enforce per-user usage limit.
- Handle duplicate collection.
- Enforce Seller ownership.

### Voucher wallet

- Collect voucher.
- Retrieve personal wallet.
- Retrieve available vouchers.
- Retrieve used vouchers.
- Retrieve expired vouchers.
- Prevent accessing another user's wallet.

### Discount calculations

- Percentage discount.
- Fixed amount discount.
- Maximum discount.
- Minimum order amount.
- Product-specific eligibility.
- Category-specific eligibility.
- Seller-specific eligibility.
- Shipping discount.
- Promotion stacking.
- Best-deal calculation.
- Rounding.
- Prevent negative final amounts.
- Prevent double discount application.

### Flash Sale

- Create Flash Sale.
- Validate start and end time.
- Add eligible product.
- Remove product.
- Validate campaign status.
- Enforce promotional limits.
- Verify InventoryService integration where required.

### Reservation and redemption

- Reserve available usage.
- Confirm reservation.
- Release reservation.
- Expire reservation.
- Prevent duplicate confirmation.
- Prevent duplicate release errors.
- Prevent oversubscription under concurrent requests.
- Verify idempotency.

### Authorization

- Buyer cannot manage another user's voucher wallet.
- Seller cannot modify another Seller's promotion.
- Buyer cannot access Admin endpoints.
- Public user cannot access Internal endpoints.
- Admin permissions are enforced.

### Reports

- Statistics use actual records.
- Pagination works.
- Filters work.
- Revenue impact is not fabricated.

## 12.2 Frontend tests

Build the Frontend.

Verify:

- Promotion homepage renders.
- Promotion banners load.
- Voucher list loads from API.
- Voucher collection works.
- Voucher wallet tabs work.
- Voucher details display correctly.
- Copy code works.
- Flash Sale page works.
- Countdown displays correctly.
- Product discount badges display correctly.
- Checkout voucher preview works.
- Invalid voucher errors display correctly.
- Seller promotion dashboard works.
- Seller voucher management works.
- Admin promotion dashboard works.
- Admin approval and suspension work.
- Reports display actual values.
- Pagination and filtering work.
- Forms validate correctly.
- All buttons work.
- No broken routes.
- No critical browser console errors.

## 12.3 End-to-end workflows

### Scenario A — Buyer collects a voucher

1. Open the promotion page.
2. Load available vouchers.
3. Collect one voucher.
4. Verify the API succeeds.
5. Verify the voucher appears in the personal wallet.
6. Reload the application.
7. Verify the voucher remains in the wallet.

### Scenario B — Buyer applies a voucher

1. Add eligible products to the cart.
2. Open checkout.
3. Load eligible vouchers.
4. Select a voucher.
5. Call Backend validation.
6. Call Backend calculation.
7. Verify the discount breakdown.
8. Verify the final amount is correct.
9. Change the cart and recalculate.
10. Verify invalid or expired vouchers are rejected.

### Scenario C — Successful redemption

1. Buyer starts checkout.
2. OrderService requests a promotion reservation.
3. PromotionService creates a reservation.
4. OrderService creates a valid order.
5. OrderService confirms the reservation.
6. PromotionService records redemption.
7. Verify voucher usage count is updated exactly once.

### Scenario D — Failed checkout

1. Buyer starts checkout.
2. PromotionService reserves a voucher.
3. Order creation fails.
4. OrderService releases the reservation.
5. Verify the voucher usage capacity is restored.
6. Verify no official redemption was recorded.

### Scenario E — Concurrent voucher usage

1. Configure a voucher with a very limited usage count.
2. Send concurrent reservation requests.
3. Verify the usage limit is never exceeded.
4. Verify only eligible requests succeed.
5. Verify no duplicate redemption records exist.

### Scenario F — Seller ownership

1. Log in as Seller A.
2. Create a promotion.
3. Verify Seller A can manage it.
4. Attempt to access Seller B's promotion.
5. Verify access is denied.
6. Verify Seller B's data remains unchanged.

### Scenario G — Flash Sale

1. Create a Flash Sale.
2. Add eligible products.
3. Activate it according to configured dates.
4. Verify the active Flash Sale API.
5. Verify the Frontend countdown.
6. Verify product discount information.
7. Verify InventoryService stock remains the authoritative stock source.

### Scenario H — Admin approval

1. Create a promotion requiring approval.
2. Verify it appears in pending promotions.
3. Approve it.
4. Verify its status changes correctly.
5. Verify it becomes active only when all activation conditions are satisfied.

If an end-to-end test requires unavailable infrastructure, accounts, or another service, report the exact blocker. Never fabricate successful results.

---

# 13. DOCKER — MANDATORY FINAL VERIFICATION

The EcomSystem project already has Docker support for the entire system.

You MUST use the existing Docker Compose configuration.

## 13.1 Mandatory rebuild after code changes

Whenever you modify C# Backend or Frontend code, after completing the implementation, execute:

```bash
docker compose up --build
```

Run it from the actual project root containing the correct `docker-compose.yml` or `compose.yaml`.

This is mandatory.

Do not finish after:

- Writing code.
- Running only `dotnet build`.
- Running only `npm run build`.
- Building only PromotionService.
- Checking code statically.

You must rebuild and run the actual EcomSystem Docker environment.

Inspect:

- Docker build output.
- Container startup.
- Container status.
- PromotionService logs.
- Frontend logs.
- SQL Server connectivity.
- RabbitMQ connectivity if used.
- Redis connectivity if used.
- ProductService integration.
- CartService integration.
- OrderService integration.
- ShippingService integration.
- API health endpoints.
- Actual running Frontend.

Open the actual running Frontend and inspect:

- Promotion homepage.
- Voucher wallet.
- Flash Sale page.
- Checkout voucher application.
- Seller promotion management.
- Admin promotion management.
- Reports and statistics.

Fix problems discovered during actual execution.

If additional C# or Frontend changes are required, run:

```bash
docker compose up --build
```

again after those changes.

## 13.2 Ordinary testing without code changes

For ordinary testing when no Backend or Frontend code has changed, use:

```bash
docker compose up
```

## 13.3 Data safety

NEVER execute:

```bash
docker compose down -v
```

Do not:

- Delete Docker volumes.
- Delete SQL Server databases.
- Reset existing database data.
- Remove persisted user information.
- Delete unrelated containers or images unnecessarily.
- Change service ports without a justified reason.
- Change database credentials unnecessarily.
- Break other microservices.

Preserve the existing EcomSystem Docker environment.

If Docker fails, investigate and fix the actual cause.

If network, disk, or resource limitations prevent a successful build, report the exact problem honestly.

---

# 14. IMPLEMENTATION PRIORITY

Implement the complete functionality carefully.

### Phase 1 — Core promotion system

- Promotion CRUD.
- Voucher CRUD.
- Voucher collection.
- Personal voucher wallet.
- Promotion validation.
- Voucher validation.
- Discount calculation.
- Personal usage history.
- Core Buyer promotion UI.

### Phase 2 — Checkout and reservation

- Promotion preview.
- Voucher preview.
- Best-deal calculation.
- Internal CartService integration.
- Internal OrderService integration.
- Reservation creation.
- Reservation confirmation.
- Reservation release.
- Reservation expiration.
- Concurrency protection.

### Phase 3 — Flash Sale and Seller Center

- Flash Sale management.
- Product discounts.
- Shipping promotions.
- Seller promotion management.
- Seller voucher management.
- Seller statistics.
- Buyer Flash Sale UI.
- Product pricing integration.

### Phase 4 — Admin and analytics

- Admin promotion management.
- Approval workflow.
- Voucher administration.
- Campaign monitoring.
- Reports.
- Revenue impact.
- Internal monitoring.
- Event retry.

Review and implement all 100 API capabilities, but do not create redundant routes or unnecessary abstractions merely to match the endpoint count.

---

# 15. FINAL ACCEPTANCE CRITERIA

Do not declare the task complete until you have:

1. Read `docs/AI_RULE.md` completely.
2. Read `docs/PromotionService.md` completely.
3. Inspected existing Backend and Frontend code.
4. Preserved existing correct implementations.
5. Implemented the required promotion business functionality.
6. Implemented the Buyer promotion experience.
7. Implemented the Seller Center promotion experience.
8. Implemented the Admin promotion management experience.
9. Integrated real APIs.
10. Integrated CartService and OrderService where required.
11. Integrated ShippingService for shipping fee discounts where required.
12. Implemented secure Seller ownership.
13. Implemented voucher collection and wallet.
14. Implemented accurate discount calculation.
15. Implemented reservation and redemption lifecycle.
16. Verified concurrency and idempotency.
17. Verified database migrations and persistence.
18. Built Backend successfully.
19. Built Frontend successfully.
20. Executed `docker compose up --build` after Backend or Frontend code changes.
21. Inspected Docker containers and logs.
22. Opened and inspected the actual running Frontend.
23. Tested the important end-to-end workflows.
24. Fixed all discovered critical issues.
25. Confirmed unrelated EcomSystem features still work.

---

# 16. FINAL IMPLEMENTATION REPORT

After implementation, provide a detailed and honest report.

## A. Documentation and architecture

- Documents read.
- Existing architecture inspected.
- Existing code patterns reused.
- Existing correct files preserved.

## B. Backend

- New files.
- Modified files.
- Existing files preserved.
- APIs implemented.
- Entities and migrations.
- Discount calculation logic.
- Voucher collection and redemption.
- Reservation and concurrency handling.
- Authorization.
- Internal microservice integration.
- RabbitMQ and Redis integration where applicable.

## C. Frontend

- Promotion homepage.
- Voucher wallet.
- Voucher collection.
- Checkout integration.
- Flash Sale.
- Product discount display.
- Seller promotion dashboard.
- Admin promotion dashboard.
- Reports and analytics.
- Responsive UI verification.

## D. Testing

- Backend build result.
- Frontend build result.
- Unit tests.
- Integration tests.
- Authorization tests.
- Discount calculation tests.
- Concurrent reservation tests.
- End-to-end test results.
- Unavailable tests and exact blockers.

## E. Docker

- Exact command executed.
- Build result.
- Container status.
- Important logs.
- Database connectivity.
- Inter-service connectivity.
- Frontend availability.
- Actual UI inspection result.

## F. Remaining issues

- Incomplete functionality.
- External service dependencies.
- Known limitations.
- Required future improvements.

Never claim a feature, API, test, migration, integration, Docker build, or UI verification succeeded unless you actually executed and verified it.

---

# FINAL INSTRUCTION

Work carefully through the existing EcomSystem codebase.

Do not rush.

Do not delete correct code.

Do not unnecessarily rewrite working features.

Do not create fake data or fake integrations.

Build a complete and secure PromotionService Backend with accurate discount calculations, reliable voucher collection, concurrency-safe reservations, correct redemption, Flash Sale management, and proper microservice integration.

Build a beautiful Shopee-inspired EcomSystem promotion experience with its own distinct brand identity.

**The task is not complete when the code is written. It is complete only after the Backend and Frontend have been built, tested, run through Docker, inspected in the actual application, and all discovered critical issues have been fixed or clearly reported.**