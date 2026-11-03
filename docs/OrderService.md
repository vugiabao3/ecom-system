# ORDER SERVICE — COMPLETE BACKEND & FRONTEND IMPLEMENTATION

## 1. Main Objective

You are working on **EcomSystem**, a microservices-based e-commerce platform built with ASP.NET Core and React.

Your task is to thoroughly inspect, implement, improve, and complete the entire **OrderService**, including both Backend and Frontend.

The final result must be a fully functional, production-minded order management system with a premium e-commerce user experience inspired by Shopee.

**Important design requirement:**

- Use Shopee as a reference for its professional e-commerce experience, order workflows, layout organization, and usability.
- Do NOT copy Shopee's branding, logos, or exact interface.
- Create a distinctive EcomSystem identity with a consistent color system, typography, spacing, icons, components, and professional visual hierarchy.
- The UI must look like a real commercial e-commerce platform, not a basic student CRUD application.
- Both Backend and Frontend must be properly implemented and integrated.

Do not stop after creating API endpoints or compiling the project. The entire workflow must work with real data.

---

## 2. Mandatory Documentation Reading

Before modifying any code:

1. Read the entire `docs/AI_RULE.md`.
2. Read the entire `docs/OrderService.md`.
3. Inspect the root project structure and existing `docker-compose.yml` / `compose.yaml`.
4. Inspect the existing OrderService Backend implementation.
5. Inspect the existing Frontend architecture and components.
6. Inspect the API configuration, authentication mechanism, routing, state management, and existing service integrations.
7. Inspect related services that OrderService depends on, including:
   - AuthService
   - UserService
   - CartService
   - ProductService
   - InventoryService
   - PaymentService
   - ShippingService
   - PromotionService
   - ReviewService
   - NotificationService

Read relevant service documentation before integrating with another service.

**Do not start coding before understanding the existing architecture and implementation.**

---

## 3. Critical Code Preservation Rules

This is an existing project, not a new project.

You MUST follow these rules:

- Reuse existing correct code, entities, DTOs, commands, queries, handlers, repositories, interfaces, services, controllers, components, hooks, and utilities.
- Do not delete files simply because you prefer a different implementation.
- Do not rewrite working code unnecessarily.
- Do not replace the existing architecture with a new architecture.
- Do not introduce duplicate entities, duplicate APIs, duplicate business logic, or unnecessary abstractions.
- If an existing file is correct and complete, preserve it.
- If an existing file is incomplete or incorrect, modify only the parts that genuinely need improvement.
- If a new file is required, create it in the appropriate existing directory.
- Preserve existing API contracts and integrations unless there is a clear technical reason to change them.
- Avoid modifying unrelated microservices.
- Do not remove existing features to make implementation easier.
- Do not use mock data to pretend that an API or feature works.
- Do not hardcode fake success responses.
- Do not silently ignore errors.

Before making significant changes, understand how the existing code works and why it was implemented that way.

**The priority is to complete and improve the existing system without damaging working functionality.**

---

# PART I — BACKEND IMPLEMENTATION

## 4. Backend Architecture Requirements

Follow the existing Clean Architecture structure and conventions.

Expected architectural responsibilities:

- **Domain:** Order entities, order items, business rules, enums, domain events, and relevant value objects.
- **Application:** Commands, queries, handlers, DTOs, validators, interfaces, and business workflows.
- **Infrastructure:** EF Core, repositories, database access, external service clients, message consumers, and persistence.
- **API:** Controllers, authentication, authorization, dependency injection, and HTTP responses.

Follow the project's existing MediatR/CQRS patterns.

Requirements:

- Controllers must remain thin.
- Business logic must not be placed directly inside controllers.
- Use DTOs rather than exposing EF Core entities.
- Validate all incoming requests.
- Use consistent error handling and response formats.
- Implement pagination for list endpoints.
- Use asynchronous operations.
- Avoid unnecessary database queries and N+1 problems.
- Use appropriate database indexes.
- Handle concurrency and duplicate requests.
- Use transactions where appropriate.
- Ensure correct ownership checks.
- Implement structured logging.
- Keep service boundaries clear.

Do not introduce a new architectural pattern if the existing project already has a consistent implementation.

## 5. Complete OrderService API Requirements

Implement and verify the following API capabilities.

The endpoints below define the required business coverage. They are not an instruction to create duplicate or redundant routes mechanically.

If the existing project already has an equivalent endpoint, reuse and improve it rather than creating another endpoint with the same responsibility.

### GROUP 1 — Order Creation and Checkout

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/orders/checkout` | Create an order from selected cart items |
| POST | `/api/orders/preview` | Preview order before checkout |
| POST | `/api/orders/validate` | Validate checkout conditions |
| POST | `/api/orders/calculate` | Calculate trusted order totals |
| GET | `/api/orders/checkout/{checkoutId}` | Retrieve checkout session when supported |
| POST | `/api/orders/checkout/confirm` | Confirm checkout when a separate confirmation step is needed |
| GET | `/api/orders/{id}/confirmation` | Retrieve order confirmation information |

Important business rules:

- Retrieve selected cart items from CartService.
- Do not trust product prices, discounts, shipping fees, or totals submitted by the Frontend.
- Obtain authoritative product and variant information from ProductService.
- Check stock and coordinate reservations with InventoryService.
- Coordinate voucher validation and discounts with PromotionService.
- Coordinate shipping information and fees with ShippingService.
- Store immutable order-item snapshots, including product name, SKU, image, variant, quantity, and purchase price.
- Prevent duplicate orders caused by repeated checkout requests.
- Support idempotency where appropriate.
- Do not create an order if the required checkout validation fails.
- Ensure that only successfully purchased cart items are removed from the cart.

The final order price must be calculated and validated by trusted Backend logic.

### GROUP 2 — Buyer Order Management

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/orders/me` | List current user's orders |
| GET | `/api/orders/me/summary` | Buyer order statistics |
| GET | `/api/orders/me/{orderId}` | View order details |
| GET | `/api/orders/me/recent` | Recent orders |
| GET | `/api/orders/me/search` | Search orders |
| GET | `/api/orders/me/filter` | Filter by status |
| GET | `/api/orders/me/pending` | Pending orders |
| GET | `/api/orders/me/processing` | Processing orders |
| GET | `/api/orders/me/shipping` | Shipping orders |
| GET | `/api/orders/me/completed` | Completed orders |
| GET | `/api/orders/me/cancelled` | Cancelled orders |
| GET | `/api/orders/me/returned` | Returned orders |
| GET | `/api/orders/me/{orderId}/timeline` | Order status timeline |
| GET | `/api/orders/me/{orderId}/invoice` | Invoice or order receipt information |
| GET | `/api/orders/me/{orderId}/tracking` | Shipment tracking |

Use a consistent query-based filtering and pagination strategy where possible.

Do not create separate endpoints for every status if a single filtering endpoint already handles the same functionality correctly.

Security requirements:

- Obtain BuyerId/UserId from the authenticated JWT.
- Never trust a UserId supplied by the Frontend.
- Verify order ownership before returning order details.
- Prevent users from viewing other users' orders.

### GROUP 3 — Order Cancellation

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/orders/{orderId}/cancel` | Request cancellation |
| GET | `/api/orders/{orderId}/cancellation-reasons` | Retrieve cancellation reasons |
| GET | `/api/orders/{orderId}/cancellation` | Retrieve cancellation information |
| POST | `/api/orders/{orderId}/cancel-request` | Submit cancellation request requiring Seller approval |
| POST | `/api/orders/{orderId}/cancel-request/withdraw` | Withdraw an unprocessed cancellation request |

Business requirements:

- Validate the current order status before allowing cancellation.
- A delivered order must not use the normal cancellation workflow.
- Prevent cancellation after an irreversible fulfillment step unless a valid exception exists.
- Handle cancellation approval and rejection.
- Release inventory reservations when cancellation is confirmed.
- Coordinate with PaymentService when a refund is required.
- Update CartService only according to the actual checkout/cancellation workflow.
- Publish relevant events for NotificationService.
- Prevent repeated or conflicting cancellation requests.

### GROUP 4 — Seller Order Management

This is a major feature and must receive particular attention.

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/seller/orders` | List Seller's orders |
| GET | `/api/seller/orders/summary` | Seller order statistics |
| GET | `/api/seller/orders/{orderId}` | View order details |
| GET | `/api/seller/orders/pending` | Pending orders |
| GET | `/api/seller/orders/processing` | Processing orders |
| GET | `/api/seller/orders/shipping` | Shipping orders |
| GET | `/api/seller/orders/completed` | Completed orders |
| GET | `/api/seller/orders/cancelled` | Cancelled orders |
| GET | `/api/seller/orders/search` | Search Seller orders |
| PATCH | `/api/seller/orders/{orderId}/confirm` | Confirm order |
| PATCH | `/api/seller/orders/{orderId}/reject` | Reject order when permitted |
| PATCH | `/api/seller/orders/{orderId}/processing` | Start order preparation |
| PATCH | `/api/seller/orders/{orderId}/ready-to-ship` | Mark order ready for shipment |
| PATCH | `/api/seller/orders/{orderId}/ship` | Hand over to shipping workflow |
| PATCH | `/api/seller/orders/{orderId}/complete` | Complete order when permitted |
| GET | `/api/seller/orders/{orderId}/shipping-info` | View shipping information |
| GET | `/api/seller/orders/{orderId}/history` | Seller order history |
| POST | `/api/seller/orders/{orderId}/notes` | Add internal order notes |

Security requirements:

- Obtain SellerId from the authenticated identity or securely verify it through UserService.
- Never trust SellerId submitted by the Frontend.
- A Seller must only access orders containing that Seller's products.
- In a multi-Seller order, enforce ownership at the order-item or seller-fulfillment level.
- Do not allow one Seller to modify another Seller's order items.
- Admin overrides must be separately authorized and audited.

Implement proper Seller order processing workflows.

### GROUP 5 — Order Status Management

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/orders/{orderId}/status` | Current status |
| GET | `/api/orders/{orderId}/status-history` | Status history |
| PATCH | `/api/orders/{orderId}/status` | Authorized status update |
| POST | `/api/orders/{orderId}/status/transition` | Validated state transition |

Implement an explicit order state machine.

Examples of possible states:

- PendingConfirmation
- Confirmed
- Processing
- ReadyToShip
- Shipping
- Delivered
- Completed
- CancellationRequested
- Cancelled
- ReturnRequested
- Returning
- Returned
- Refunded
- Failed

Adapt these states to the existing Domain model rather than blindly replacing existing enums.

Requirements:

- Define valid state transitions.
- Reject invalid transitions.
- Record who performed every important transition.
- Record timestamps and relevant reasons.
- Do not let Buyers arbitrarily change order status.
- Do not expose unrestricted status updates to Frontend users.
- Integrate Seller, Admin, PaymentService, ShippingService, and internal events appropriately.

### GROUP 6 — Payment Integration

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/orders/{orderId}/payment` | Payment information |
| GET | `/api/orders/{orderId}/payment-status` | Payment status |
| POST | `/api/orders/{orderId}/payment/confirm` | Record verified payment result |
| POST | `/api/orders/{orderId}/payment/failure` | Record payment failure |
| POST | `/api/orders/{orderId}/payment/refund-request` | Request refund |
| GET | `/api/orders/{orderId}/payment/history` | Payment history |

Important:

- OrderService owns the payment status associated with the order.
- PaymentService owns actual payment transactions, payment attempts, refunds, and financial processing.
- Do not duplicate PaymentService's financial logic.
- Never trust Frontend confirmation of successful payment.
- Only accept verified PaymentService callbacks, signed webhooks, or authenticated internal service calls.
- Handle repeated events idempotently.
- Validate that the payment belongs to the corresponding order.

### GROUP 7 — Shipping Integration

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/orders/{orderId}/shipping` | Shipping information |
| GET | `/api/orders/{orderId}/shipping/tracking` | Shipment tracking |
| POST | `/api/orders/{orderId}/shipping/confirm` | Record confirmed shipment information |
| POST | `/api/orders/{orderId}/shipping/delivered` | Record delivery completion |
| POST | `/api/orders/{orderId}/shipping/failed` | Record delivery failure |
| POST | `/api/orders/{orderId}/shipping/returned` | Record returned shipment |
| GET | `/api/orders/{orderId}/shipping/history` | Shipping history |

Important:

- ShippingService owns shipments, tracking, delivery operations, and shipper assignments.
- OrderService must not independently perform shipping operations.
- Shipment status updates must originate from authenticated ShippingService events or internal calls.
- Buyers and Sellers must only access authorized shipping information.
- Do not allow Frontend users to mark an order as delivered.

### GROUP 8 — Return and Refund Management

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/orders/{orderId}/return-requests` | Create return/refund request |
| GET | `/api/orders/{orderId}/return-requests` | List return requests |
| GET | `/api/return-requests/{requestId}` | Return request details |
| POST | `/api/return-requests/{requestId}/evidence` | Submit evidence |
| POST | `/api/return-requests/{requestId}/withdraw` | Withdraw request |
| GET | `/api/seller/return-requests` | Seller return requests |
| POST | `/api/seller/return-requests/{requestId}/approve` | Approve return |
| POST | `/api/seller/return-requests/{requestId}/reject` | Reject with reason |
| POST | `/api/seller/return-requests/{requestId}/received` | Confirm returned goods received |
| GET | `/api/admin/return-requests` | Admin return management |
| POST | `/api/admin/return-requests/{requestId}/resolve` | Resolve disputes |
| GET | `/api/orders/{orderId}/refunds` | Refund history |
| POST | `/api/orders/{orderId}/refunds` | Initiate refund request through PaymentService |

Requirements:

- Validate return eligibility and time limits.
- Store return reasons and supporting evidence.
- Enforce ownership and Seller authorization.
- Prevent duplicate return requests.
- Support Seller approval/rejection and Admin dispute resolution.
- Keep a complete audit history.
- Do not process actual money transfers inside OrderService.
- PaymentService must execute financial refunds.
- InventoryService must only restore returned stock after the appropriate inspection and confirmation workflow.
- Store evidence metadata and use the project's existing file/media storage approach.

If a separate ReturnService is not necessary for the current architecture, implement return management as a properly separated OrderService module without duplicating responsibilities.

### GROUP 9 — Review Support

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/orders/{orderId}/review-eligibility` | Check review eligibility |
| GET | `/api/orders/{orderId}/reviewed-items` | Retrieve reviewed order items |
| GET | `/api/orders/me/review-pending` | Retrieve items awaiting review |

Requirements:

- OrderService only provides order eligibility information.
- ReviewService owns review content, ratings, moderation, and review media.
- Verify that the order belongs to the authenticated Buyer.
- Only eligible delivered/completed order items may be reviewed according to the project's review rules.
- Never create duplicate review functionality inside OrderService.

### GROUP 10 — Admin Order Management

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/admin/orders` | All orders |
| GET | `/api/admin/orders/{orderId}` | Order details |
| GET | `/api/admin/orders/search` | Search orders |
| GET | `/api/admin/orders/statistics` | Order statistics |
| GET | `/api/admin/orders/pending` | Pending orders |
| GET | `/api/admin/orders/cancelled` | Cancelled orders |
| GET | `/api/admin/orders/disputed` | Disputed orders |
| PATCH | `/api/admin/orders/{orderId}/status` | Authorized status adjustment |
| POST | `/api/admin/orders/{orderId}/cancel` | Admin cancellation |
| GET | `/api/admin/orders/{orderId}/audit-logs` | Audit history |
| GET | `/api/admin/orders/export` | Export order data |
| GET | `/api/admin/orders/revenue-summary` | Order revenue summary |

Requirements:

- Restrict all Admin APIs using the project's existing role/policy system.
- Record Admin actions in audit logs.
- Do not allow unrestricted modifications to financial or shipping states.
- Statistics must be based on actual database data.
- Apply pagination and filtering.
- Avoid loading the entire order database into memory for exports or statistics.

### GROUP 11 — Internal Microservice APIs

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/internal/orders/{orderId}/summary` | Order summary |
| GET | `/api/internal/orders/{orderId}` | Internal order details |
| POST | `/api/internal/orders/batch` | Retrieve multiple orders |
| GET | `/api/internal/orders/{orderId}/exists` | Check existence |
| GET | `/api/internal/orders/{orderId}/items` | Order items |
| POST | `/api/internal/orders/{orderId}/payment-status` | Record verified payment status |
| POST | `/api/internal/orders/{orderId}/shipping-status` | Synchronize shipping status |
| POST | `/api/internal/orders/{orderId}/complete` | Record valid completion |
| POST | `/api/internal/orders/{orderId}/cancel` | Internal cancellation workflow |
| GET | `/api/internal/orders/statistics` | Internal statistics |

Requirements:

- Use the existing service-to-service authentication mechanism.
- Do not expose internal APIs to ordinary Buyers or Sellers.
- Validate the calling service and request.
- Apply idempotency to state-changing internal operations.
- Never allow arbitrary internal status updates without business validation.

---

## 6. Microservice Integration Rules

Respect strict ownership boundaries.

| Service | Responsibility |
|---|---|
| AuthService | Authentication, JWT, credentials, tokens |
| UserService | Buyer/Seller profiles and identities |
| CartService | Cart data and selected cart items |
| ProductService | Product, SKU, variant, product information |
| InventoryService | Stock, reservations, stock movements |
| PromotionService | Vouchers, discounts, promotions |
| OrderService | Orders, order items, order lifecycle, order snapshots |
| PaymentService | Payments, transactions, refunds |
| ShippingService | Shipments, tracking, delivery, COD collection |
| ReviewService | Product/shop reviews and ratings |
| NotificationService | User notifications |

Do not:

- Access another service's database directly.
- Duplicate another service's business logic.
- Use Frontend-supplied prices as authoritative values.
- Create separate implementations of payment, shipping, inventory, or review workflows inside OrderService.

### RabbitMQ and Event-Driven Integration

Inspect the existing RabbitMQ implementation before adding anything.

Use asynchronous events where appropriate, for example:

- `OrderCreated`
- `OrderConfirmed`
- `OrderCancelled`
- `OrderProcessing`
- `OrderReadyToShip`
- `OrderCompleted`
- `OrderDelivered`
- `OrderReturnRequested`
- `OrderReturnApproved`
- `OrderRefundRequested`
- `PaymentSucceeded`
- `PaymentFailed`
- `ShipmentCreated`
- `ShipmentDelivered`
- `ShipmentFailed`

Use the existing event naming and message contracts if they already exist.

Implement or reuse:

- Idempotent event consumers.
- Retry policies.
- Dead-letter handling where appropriate.
- Outbox/Inbox patterns if required by the existing architecture.
- Structured event logging.
- Proper failure handling.

Do not introduce RabbitMQ for operations that should remain simple synchronous database operations.

---

# PART II — FRONTEND IMPLEMENTATION

## 7. Frontend Main Objective

Complete the entire OrderService Frontend experience.

The interface must be comparable in quality to a modern commercial e-commerce platform inspired by Shopee, while maintaining a unique EcomSystem identity.

Do not merely create basic tables and forms.

The UI must be visually polished, responsive, intuitive, consistent, and fully connected to real Backend APIs.

Inspect the existing React architecture and reuse:

- Existing layout components.
- Existing navigation.
- Existing authentication context.
- Existing API clients.
- Existing hooks.
- Existing UI components.
- Existing CSS conventions.
- Existing routing.
- Existing notification system.
- Existing design tokens.

Do not create a separate design system if one already exists.

## 8. Buyer Order Experience

Implement a complete Buyer order management experience.

### A. My Orders Page

Create a professional order management page with:

- Page title and clear navigation.
- Search orders.
- Status tabs.
- Order count indicators.
- Filtering and sorting.
- Pagination.
- Loading skeletons.
- Empty states.
- Error states.
- Responsive layout.

Suggested status tabs:

1. All
2. Pending Confirmation
3. Processing
4. Ready to Ship
5. Shipping
6. Completed
7. Cancelled
8. Return/Refund

Use real API data.

### B. Order Cards

Each order card should display:

- Shop/Seller name.
- Order number.
- Order creation time.
- Product image.
- Product name.
- Variant information.
- Quantity.
- Unit price.
- Total order amount.
- Payment method.
- Payment status.
- Shipping status.
- Current order status.

Provide clear actions based on the actual order state:

- View details.
- Cancel order when eligible.
- Track shipment.
- Contact Seller if the existing messaging feature supports it.
- Buy again if the required APIs support it.
- Request return/refund when eligible.
- Review products after delivery when eligible.

Do not show actions that the current order state does not permit.

### C. Order Detail Page

Build a polished order detail page with:

- Order summary.
- Order status.
- Status timeline.
- Product list.
- Seller information.
- Shipping address.
- Shipping information.
- Payment details.
- Price breakdown.
- Discounts.
- Shipping fee.
- Total payment.
- Cancellation details.
- Return/refund details when applicable.
- Invoice/receipt information.

Use a clear layout inspired by professional e-commerce order detail pages.

### D. Order Tracking

Create an intuitive shipment tracking section.

Display:

- Shipment status.
- Tracking number.
- Shipping provider.
- Delivery progress.
- Timeline of shipping events.
- Estimated delivery information if available.
- Delivery failure information if applicable.

Obtain actual tracking data from ShippingService through the appropriate integration.

Do not fabricate tracking events.

### E. Cancellation and Return/Refund UI

Implement:

- Cancellation reason selection.
- Cancellation confirmation modal.
- Cancellation request status.
- Withdrawal action where eligible.
- Return/refund request form.
- Return reason selection.
- Evidence upload.
- Request status tracking.
- Seller response information.
- Admin resolution information when applicable.

Use validation and clear user feedback.

Never display a successful result until the Backend confirms the operation.

---

## 9. Seller Order Management Dashboard

This is a high-priority Frontend feature.

Build a professional Seller order management experience comparable to a real marketplace Seller Center.

### A. Seller Order Dashboard

Display useful statistics:

- Total orders.
- Pending confirmation.
- Processing.
- Ready to ship.
- Shipping.
- Completed.
- Cancelled.
- Return requests.

Use real statistics from Backend APIs.

### B. Seller Order List

Provide:

- Search by order number.
- Search by Buyer/product when permitted.
- Status filtering.
- Date filtering.
- Sorting.
- Pagination.
- Order summary.
- Product information.
- Buyer information according to privacy rules.
- Payment status.
- Shipping status.

### C. Seller Order Detail

Display:

- Order number.
- Buyer information.
- Ordered products.
- Product variants.
- Quantities.
- Order totals.
- Payment details.
- Shipping address and shipping information.
- Order timeline.
- Internal Seller notes.
- Cancellation/return information.

### D. Seller Order Actions

Implement the appropriate actions:

- Confirm order.
- Reject order with a valid reason.
- Start preparing order.
- Mark order ready to ship.
- Hand over to ShippingService.
- View shipping information.
- Add internal notes.
- View order history.
- Process cancellation requests.
- Process return requests.

Actions must be determined by the valid state machine.

Use confirmation dialogs for important operations.

Do not allow a Seller to modify an order that does not belong to that Seller.

---

## 10. Admin Order Management

If the existing project includes an Admin interface, integrate the OrderService management pages into it.

Implement:

- All orders.
- Order search.
- Order details.
- Order status filtering.
- Cancellation management.
- Return/refund dispute management.
- Order statistics.
- Audit history.
- Revenue/order summary.
- Export functionality where supported.

Admin UI must use real Backend APIs and enforce Admin authorization.

Do not create fake statistics.

---

## 11. UI/UX Quality Requirements

The final UI must look premium and consistent.

Follow these design principles:

### Visual Style

- Modern marketplace design.
- Clean white surfaces.
- Professional EcomSystem brand colors.
- Carefully selected accent colors.
- Clear typography.
- Consistent spacing.
- Rounded cards and buttons.
- Subtle borders and shadows.
- Professional icons.
- High-quality status badges.
- Well-designed tables and order cards.
- Clear visual hierarchy.

### Interaction

- Smooth hover states.
- Clear button states.
- Responsive modals.
- Dropdown menus.
- Search interactions.
- Filters.
- Tabs.
- Toast notifications.
- Loading skeletons.
- Empty states.
- Error states.
- Confirmation dialogs.

### Responsive Design

The pages must work correctly on:

- Desktop.
- Laptop.
- Tablet.
- Mobile.

Avoid:

- Overcrowded layouts.
- Excessively large cards.
- Inconsistent colors.
- Unnecessary gradients.
- Excessive animations.
- Placeholder content.
- Fake data.
- Broken images.
- Dead buttons.
- Links that navigate nowhere.

Every visible action must have a working implementation or be clearly unavailable when its feature is not supported.

---

# PART III — API AND FRONTEND INTEGRATION

## 12. API Integration Rules

Connect the Frontend to the actual Backend.

Requirements:

- Use the existing API client and service configuration.
- Follow existing Vite environment variables.
- Use the correct service base URLs.
- Include JWT authentication where required.
- Handle expired tokens according to the existing AuthService mechanism.
- Handle HTTP 400, 401, 403, 404, 409, 422, and 500 appropriately.
- Handle network errors.
- Prevent duplicate submissions.
- Show loading states.
- Refresh relevant data after successful mutations.
- Keep pagination, filtering, and sorting synchronized with the URL when appropriate.
- Avoid unnecessary repeated requests.
- Use real API responses.

Do not bypass Backend authorization by hiding buttons in Frontend.

Frontend permissions improve usability, but Backend authorization is the actual security boundary.

---

# PART IV — DATABASE AND TESTING

## 13. Database Requirements

Inspect the existing OrderService database and migrations.

Check:

- Order entity.
- OrderItem entity.
- Order status.
- Payment status.
- Shipping information.
- Seller ownership.
- Buyer ownership.
- Cancellation records.
- Return/refund request records.
- Status history.
- Audit records.
- Order snapshots.
- Relevant indexes and constraints.

Only create or modify entities and migrations when genuinely required.

Requirements:

- Preserve existing data.
- Do not drop existing tables or databases.
- Do not delete Docker volumes.
- Do not reset the database.
- Do not run destructive migrations.
- Use proper foreign-key and unique constraints where appropriate.
- Ensure monetary values use appropriate decimal precision.
- Store timestamps consistently.
- Use soft deletion only where it makes business sense.

Never run:

`docker compose down -v`

Never delete existing database volumes or user data.

## 14. Backend Testing

Implement or update tests for:

- Order creation.
- Checkout validation.
- Price calculation.
- Order ownership.
- Seller ownership.
- Order status transitions.
- Cancellation eligibility.
- Duplicate checkout prevention.
- Payment event processing.
- Shipping event processing.
- Return/refund workflows.
- Review eligibility.
- Admin authorization.
- Internal service authentication.
- Idempotency.
- Error handling.

Test both successful and unsuccessful scenarios.

Do not claim tests pass unless you actually run them.

## 15. Frontend Testing

Verify:

- Buyer order list.
- Buyer order detail.
- Order status tabs.
- Search and filters.
- Pagination.
- Cancellation.
- Shipment tracking.
- Return/refund requests.
- Seller order dashboard.
- Seller order detail.
- Seller order processing actions.
- Admin order management.
- Authentication and authorization behavior.
- Responsive layouts.
- Loading and error states.

Check browser console errors and failed network requests.

---

# PART V — DOCKER AND REAL RUNTIME VERIFICATION

## 16. Mandatory Docker Workflow

The entire EcomSystem project already has Docker configured.

You MUST follow these rules.

### When Backend C# or Frontend code is modified

After finishing the implementation:

1. Build the Backend projects.
2. Build the Frontend.
3. Run relevant automated tests.
4. Go to the project root directory containing the Docker Compose file.
5. Execute:

`docker compose up --build`

6. Wait for the services to start.
7. Inspect Docker container status.
8. Inspect logs for errors.
9. Verify that OrderService starts correctly.
10. Verify that dependent services remain operational.
11. Open the actual Frontend in the browser.
12. Test the implemented OrderService pages and workflows.
13. Inspect browser console errors and Network requests.
14. Fix any problems discovered.
15. Rebuild and retest after fixing them.

Do not stop after a successful compilation.

**The actual Docker runtime and browser experience must be verified.**

### When only performing ordinary runtime tests without code changes

Use:

`docker compose up`

Then test the existing running system.

### Important Data Safety Rules

- Never run `docker compose down -v`.
- Never delete database volumes.
- Never reset SQL Server databases.
- Never remove existing user data.
- Do not unnecessarily rebuild unrelated services.
- Do not change unrelated Docker configurations unless required.
- Do not run destructive Docker cleanup commands.

If Docker cannot start because of an environmental problem, explain the exact error and what remains unverified.

Never claim the application works if it has not been tested.

---

# PART VI — IMPLEMENTATION WORKFLOW

## 17. Required Execution Order

Follow this sequence:

### Phase 1 — Inspect

- Read documentation.
- Inspect Backend.
- Inspect Frontend.
- Inspect existing APIs.
- Inspect database and migrations.
- Inspect Docker Compose.
- Identify existing working features.
- Identify missing or incorrect functionality.

### Phase 2 — Create an Implementation Plan

Prepare an internal checklist of:

- Existing working functionality.
- Missing APIs.
- Incorrect APIs.
- Backend changes.
- Frontend changes.
- Required integrations.
- Database changes.
- Test cases.

Avoid unnecessary architectural changes.

### Phase 3 — Backend

Implement and complete OrderService Backend functionality.

Prioritize:

1. Checkout and order creation.
2. Buyer order management.
3. Order detail and timeline.
4. Seller order management.
5. Order state machine.
6. Cancellation.
7. Payment integration.
8. Shipping integration.
9. Return/refund.
10. Review eligibility.
11. Admin management.
12. Internal APIs.

### Phase 4 — Frontend

Implement and integrate:

1. Buyer My Orders.
2. Buyer Order Details.
3. Order Tracking.
4. Cancellation and Return/Refund.
5. Seller Order Dashboard.
6. Seller Order Management.
7. Admin Order Management.

### Phase 5 — Integration

Verify that Backend and Frontend communicate correctly.

Check all important workflows end-to-end.

### Phase 6 — Testing and Docker

- Build Backend.
- Build Frontend.
- Run tests.
- Run `docker compose up --build` if C# or Frontend code changed.
- Verify actual containers.
- Open and test the actual UI.
- Fix discovered problems.
- Repeat until the implementation is stable.

---

# PART VII — DEFINITION OF DONE

## 18. Final Completion Criteria

You are NOT finished just because code files have been created.

The task is complete only when:

- [ ] `docs/AI_RULE.md` has been fully read.
- [ ] `docs/OrderService.md` has been fully read.
- [ ] Existing architecture has been inspected.
- [ ] Correct existing code has been preserved.
- [ ] Required Backend functionality has been implemented.
- [ ] Required Frontend functionality has been implemented.
- [ ] APIs follow the correct business responsibilities.
- [ ] Buyer ownership is properly enforced.
- [ ] Seller ownership is properly enforced.
- [ ] Admin authorization is properly enforced.
- [ ] Internal APIs are protected.
- [ ] Payment integration is secure.
- [ ] Shipping integration is correct.
- [ ] Inventory and promotion integrations are correct.
- [ ] Return/refund workflows respect service boundaries.
- [ ] No duplicate ReviewService implementation exists.
- [ ] Database changes are safe.
- [ ] Backend builds successfully.
- [ ] Frontend builds successfully.
- [ ] Automated tests have been executed.
- [ ] Docker runtime has been checked.
- [ ] Actual Frontend pages have been opened and inspected.
- [ ] Important workflows have been tested.
- [ ] No critical browser console or runtime errors remain.
- [ ] Existing unrelated features still work.

If a requirement cannot be completed because a dependent service or external provider is unavailable, clearly document the blocker instead of implementing fake behavior.

---

## 19. Final Report

When finished, provide a clear report containing:

### A. Implementation Summary
- What was completed.
- What was improved.
- What remains incomplete.

### B. Backend Changes
- New files.
- Modified files.
- Entities and database changes.
- Commands, queries, handlers, repositories, services, and controllers.
- Implemented API endpoints.

### C. Frontend Changes
- New pages.
- Modified pages.
- Components.
- API integrations.
- Buyer workflows.
- Seller workflows.
- Admin workflows.

### D. Microservice Integration
- CartService.
- ProductService.
- InventoryService.
- PromotionService.
- PaymentService.
- ShippingService.
- ReviewService.
- NotificationService.
- RabbitMQ events, if applicable.

### E. Testing Results
Report actual results:

- Backend build.
- Frontend build.
- Automated tests.
- Docker startup.
- Container health.
- API tests.
- Browser verification.
- End-to-end workflows.

### F. Remaining Issues
List any real blockers, unsupported features, incomplete integrations, or tests that could not be executed.

Do not hide failures.

---

# FINAL INSTRUCTION

Work carefully and systematically.

**Complete both Backend and Frontend.**

**Make the interface visually polished, professional, and inspired by Shopee while maintaining EcomSystem's own identity.**

**Preserve all existing correct code and functionality.**

**Do not delete or rewrite files unnecessarily.**

**Follow Clean Architecture and correct microservice boundaries.**

**Use real APIs and real database data, not mock implementations.**

**Run `docker compose up --build` after modifying C# Backend or Frontend code, then verify the actual running application.**

Do not finish until the implementation has been tested as thoroughly as the available environment allows.

The goal is not merely to generate code. The goal is to deliver a complete, stable, integrated, and professionally designed OrderService for EcomSystem.