# INVENTORY SERVICE — COMPLETE BACKEND & FRONTEND IMPLEMENTATION

You are a senior .NET Backend Architect, Microservices Engineer, and Senior React Frontend Engineer.

Your task is to thoroughly inspect, implement, complete, integrate, test, and polish the **InventoryService across both Backend and Frontend** in the existing EcomSystem project.

This is an existing multi-service e-commerce platform inspired by Shopee/Tiki. Your goal is to make InventoryService production-oriented, reliable, secure, maintainable, and fully integrated with the existing system.

**IMPORTANT: Do not start coding immediately. First understand the existing architecture, read the required documentation, inspect the existing implementation, and identify what is already working.**

---

# PART 1 — MANDATORY DOCUMENTATION AND PROJECT INSPECTION

## 1. Read the global development rules

Read the entire file:

`docs/AI_RULE.md`

Do not skip sections or read only selected portions.

This file defines the project's coding conventions, architecture, security requirements, microservice communication rules, database rules, testing requirements, and Docker workflow.

You MUST follow it throughout the implementation.

## 2. Read InventoryService documentation

Read the complete file:

`docs/InventoryService.md`

Treat this document as the main service-specific specification.

Also inspect other relevant documentation when necessary, especially documentation for:

- ProductService
- CartService
- OrderService
- ShippingService
- NotificationService
- AuthService
- UserService

Only read the additional documents relevant to the implementation, but read each selected document thoroughly.

## 3. Inspect the existing source code

Before changing anything, inspect:

- The complete InventoryService Backend structure.
- Existing Entities and database relationships.
- DTOs and validation.
- Commands, Queries, and MediatR Handlers.
- Repository interfaces and implementations.
- EF Core DbContext and configurations.
- Existing Controllers and API routes.
- Dependency Injection registrations.
- Authentication and authorization policies.
- Existing migrations.
- RabbitMQ integration and event consumers.
- Redis integration, if present.
- Frontend project structure.
- Existing API clients and service configurations.
- React pages, components, routes, hooks, and state management.
- Existing Seller and Admin layouts.
- Docker Compose configuration and service dependencies.

Inspect the existing code carefully before deciding what needs to be added or modified.

## 4. Preserve correct existing code

This requirement is extremely important.

- Do NOT delete existing files simply because you prefer a different implementation.
- Do NOT rewrite entire modules unnecessarily.
- Do NOT replace working functionality with a new implementation without a valid technical reason.
- Do NOT remove existing API contracts that other services or frontend components depend on.
- Do NOT break existing authentication, authorization, database relationships, or integrations.
- Do NOT remove working UI components or features.
- Reuse existing correct Entities, DTOs, Commands, Queries, Handlers, Repositories, Interfaces, Services, Controllers, API clients, and React components.
- If a file is already correct and complete, leave it unchanged.
- If a file needs modification, make the smallest reasonable change.
- If a new file is genuinely necessary, create it following the existing project conventions.
- Avoid duplicate classes, duplicate endpoints, duplicate business logic, and unnecessary abstractions.

**The objective is to complete and improve the existing project, not rebuild it from scratch.**

Before implementation, identify:
1. What is already implemented correctly.
2. What is partially implemented.
3. What is missing.
4. What contains bugs or security weaknesses.
5. What needs integration with other services.

---

# PART 2 — BACKEND IMPLEMENTATION

Implement the InventoryService Backend using the existing project's architecture and coding patterns.

## 1. Architecture requirements

Follow the existing Clean Architecture structure.

Respect the separation of responsibilities:

- API: Controllers, authentication, authorization, request handling.
- Application: Commands, Queries, Handlers, DTOs, validation, interfaces, business use cases.
- Domain: Entities, enums, domain rules, and business invariants.
- Infrastructure: EF Core, repositories, database configurations, external service communication, and implementations.

Use MediatR CQRS consistently with the existing project.

Follow the established request flow:

Controller → Command/Query → Handler → Interface/Repository → Infrastructure → EF Core → SQL Server.

Do not place complicated business logic directly inside Controllers.

Use dependency injection and the existing service registration conventions.

## 2. Inventory business ownership

InventoryService is the single source of truth for stock quantities and stock reservations.

Respect these boundaries:

| Service | Responsibility |
|---|---|
| InventoryService | OnHand, Reserved, Available, Committed, stock movements, reservations |
| ProductService | Product information, variants, SKU, images, product status |
| CartService | Shopping cart and temporary cart calculations |
| OrderService | Order creation, order lifecycle, and checkout orchestration |
| ShippingService | Shipment, delivery, and carrier operations |
| PromotionService | Discounts, promotions, and vouchers |
| NotificationService | User and Seller notifications |
| AuthService | Authentication, JWT, and credentials |
| UserService | User and Seller profiles |

InventoryService must not become the owner of product information, orders, payment processing, or notifications.

Do not directly access another service's database.

Use existing internal APIs or asynchronous events according to the project's established architecture.

## 3. Required Inventory APIs

Implement the following API contracts where they are genuinely required by the existing architecture and business workflows.

Preserve existing working routes where changing them would break integrations. If a route must change, update all dependent consumers and frontend API clients consistently.

### GROUP 1 — Inventory Management

- GET `/api/inventory`
- GET `/api/inventory/{productId}`
- GET `/api/inventory/variants/{variantId}`
- GET `/api/inventory/products/{productId}/summary`
- GET `/api/inventory/products/{productId}/variants`
- GET `/api/inventory/{productId}/availability`
- GET `/api/inventory/variants/{variantId}/availability`
- GET `/api/inventory/low-stock`
- GET `/api/inventory/out-of-stock`
- GET `/api/inventory/search`

Support appropriate pagination, sorting, filtering, and search.

Do not return the entire inventory database without pagination.

### GROUP 2 — Stock Operations

- POST `/api/inventory`
- POST `/api/inventory/add`
- POST `/api/inventory/remove`
- PATCH `/api/inventory/{inventoryId}/adjust`
- POST `/api/inventory/batch/add`
- POST `/api/inventory/batch/adjust`
- POST `/api/inventory/batch/remove`
- GET `/api/inventory/{inventoryId}/adjustments`
- GET `/api/inventory/stock-movements`
- GET `/api/inventory/stock-movements/{movementId}`

Requirements:

- Prevent negative stock.
- Record every stock modification.
- Require a valid reason for manual adjustments.
- Record the actor, timestamp, quantity before, quantity after, and operation type.
- Validate batch operations carefully.
- Handle partial failures consistently; use transactions where appropriate.
- Prevent unauthorized users from changing inventory.

### GROUP 3 — Stock Reservation

- POST `/api/inventory/reserve`
- POST `/api/inventory/reserve/batch`
- GET `/api/inventory/reservations`
- GET `/api/inventory/reservations/{reservationId}`
- GET `/api/inventory/orders/{orderId}/reservation`
- POST `/api/inventory/reservations/{reservationId}/confirm`
- POST `/api/inventory/reservations/{reservationId}/release`
- POST `/api/inventory/orders/{orderId}/release`
- POST `/api/inventory/orders/{orderId}/commit`
- POST `/api/inventory/reservations/expire`
- GET `/api/inventory/reservations/expiring`

This is one of the most important parts of the service.

Implement concurrency-safe stock reservation.

The system must prevent two customers from purchasing the same final available unit.

Clearly distinguish:

- OnHand: Physical or logically recorded stock.
- Reserved: Stock currently held for orders.
- Available: Stock available for new purchases.
- Committed: Stock committed to confirmed fulfillment, according to the existing inventory model.

For a simple model:

`Available = OnHand - Reserved`

Do not subtract the same quantity from OnHand twice when converting a reservation into committed stock.

Implement:

- Reservation expiration.
- Idempotent reserve operations.
- Idempotent release operations.
- Idempotent commit operations.
- Duplicate request protection.
- Atomic quantity updates.
- Proper transaction handling.
- Concurrency handling using appropriate SQL Server mechanisms, such as row versioning or atomic conditional updates.
- Validation that reservation quantities are positive.
- Validation that the requested stock is actually available.

Do not allow a reservation to be committed or released through an invalid state transition.

Reservation lifecycle must be clearly defined and enforced.

### GROUP 4 — Seller Inventory Management

- GET `/api/seller/inventory`
- GET `/api/seller/inventory/summary`
- GET `/api/seller/inventory/{productId}`
- GET `/api/seller/inventory/variants/{variantId}`
- GET `/api/seller/inventory/low-stock`
- GET `/api/seller/inventory/out-of-stock`
- POST `/api/seller/inventory/add-stock`
- PATCH `/api/seller/inventory/{productId}/threshold`
- GET `/api/seller/inventory/stock-movements`
- GET `/api/seller/inventory/statistics`

Requirements:

- Seller must only access inventory belonging to their own shop.
- Obtain SellerId from authenticated identity or verify it through UserService.
- Never trust SellerId submitted by the frontend.
- Verify product ownership through trusted service data.
- Prevent sellers from modifying other sellers' stock.
- Record all Seller stock operations.
- Provide useful inventory statistics and low-stock information.

### GROUP 5 — Warehouse Management

- POST `/api/warehouses`
- GET `/api/warehouses`
- GET `/api/warehouses/{warehouseId}`
- PUT `/api/warehouses/{warehouseId}`
- DELETE `/api/warehouses/{warehouseId}`
- GET `/api/warehouses/{warehouseId}/inventory`
- POST `/api/warehouses/{warehouseId}/stock`
- POST `/api/warehouses/{warehouseId}/transfer`
- GET `/api/warehouses/{warehouseId}/movements`
- GET `/api/warehouses/{warehouseId}/statistics`

IMPORTANT:

Warehouse Management is an advanced capability.

First inspect the current database and business model.

If the current system uses one logical inventory record per product or variant, do not introduce a complex physical warehouse architecture merely to increase the API count.

Implement this group only if it fits the existing architecture and can be completed correctly.

Do not introduce unnecessary database complexity or break existing inventory records.

### GROUP 6 — Stock Alerts

- GET `/api/inventory/alerts`
- GET `/api/inventory/alerts/low-stock`
- GET `/api/inventory/alerts/out-of-stock`
- GET `/api/inventory/alerts/history`
- PATCH `/api/inventory/alerts/{alertId}/read`
- PATCH `/api/inventory/alerts/{alertId}/resolve`
- GET `/api/seller/inventory/alerts`

Integrate with NotificationService where appropriate.

Do not create a separate notification system inside InventoryService.

Avoid generating duplicate alerts repeatedly for the same stock condition.

### GROUP 7 — Stocktake

- POST `/api/inventory/stocktakes`
- GET `/api/inventory/stocktakes`
- GET `/api/inventory/stocktakes/{stocktakeId}`
- POST `/api/inventory/stocktakes/{stocktakeId}/items`
- PUT `/api/inventory/stocktakes/{stocktakeId}/items/{itemId}`
- POST `/api/inventory/stocktakes/{stocktakeId}/complete`
- GET `/api/inventory/stocktakes/{stocktakeId}/discrepancies`
- POST `/api/inventory/stocktakes/{stocktakeId}/adjust`

Implement this as an advanced module only if it fits the existing project requirements.

A completed stocktake must not silently overwrite inventory quantities.

Every adjustment must be validated and audited.

### GROUP 8 — Inventory Reports and Analytics

- GET `/api/inventory/statistics`
- GET `/api/inventory/statistics/summary`
- GET `/api/inventory/statistics/low-stock`
- GET `/api/inventory/statistics/out-of-stock`
- GET `/api/inventory/statistics/movements`
- GET `/api/inventory/statistics/reservations`
- GET `/api/seller/inventory/reports`
- GET `/api/admin/inventory/reports`
- GET `/api/admin/inventory/export`

Provide accurate metrics based on real database records.

Do not return fabricated statistics or hardcoded sample data.

### GROUP 9 — Admin Inventory Management

- GET `/api/admin/inventory`
- GET `/api/admin/inventory/{inventoryId}`
- GET `/api/admin/inventory/search`
- POST `/api/admin/inventory/add-stock`
- PATCH `/api/admin/inventory/{inventoryId}/adjust`
- GET `/api/admin/inventory/movements`
- GET `/api/admin/inventory/reservations`
- POST `/api/admin/inventory/reservations/{reservationId}/release`
- GET `/api/admin/inventory/alerts`
- GET `/api/admin/inventory/statistics`
- GET `/api/admin/inventory/stocktakes`
- GET `/api/admin/inventory/audit-logs`

Admin operations must:

- Require Admin authorization.
- Require a valid adjustment reason.
- Record audit logs.
- Validate all affected inventory records.
- Prevent accidental stock corruption.
- Protect against unauthorized reservation release.

### GROUP 10 — Internal Microservice APIs

- GET `/api/internal/inventory/products/{productId}`
- GET `/api/internal/inventory/variants/{variantId}`
- POST `/api/internal/inventory/availability`
- POST `/api/internal/inventory/reserve`
- POST `/api/internal/inventory/reserve/batch`
- POST `/api/internal/inventory/reservations/{reservationId}/release`
- POST `/api/internal/inventory/orders/{orderId}/commit`
- POST `/api/internal/inventory/orders/{orderId}/release`
- GET `/api/internal/inventory/orders/{orderId}/reservations`
- GET `/api/internal/inventory/products/{productId}/exists`
- POST `/api/internal/inventory/restore`
- POST `/api/internal/inventory/adjustments`
- GET `/api/internal/inventory/statistics`

Internal endpoints must use the existing service-to-service authentication mechanism.

Never expose internal operations as public buyer APIs.

Validate caller permissions and payloads.

Only restore returned goods after the appropriate return and inspection workflow has confirmed that the goods can be restocked.

---

# PART 3 — CRITICAL BUSINESS WORKFLOWS

Do not implement isolated endpoints without ensuring that the entire business workflow works.

## 1. Buyer checkout workflow

The intended workflow should be consistent with the existing EcomSystem architecture:

1. Buyer selects products in CartService.
2. OrderService validates the checkout request.
3. OrderService requests InventoryService to reserve the required quantities.
4. InventoryService atomically reserves available stock.
5. OrderService creates the order.
6. PaymentService processes the payment.
7. Depending on the payment and order workflow, InventoryService commits or releases the reservation.
8. If checkout fails, the reservation is released safely.
9. Expired reservations are released automatically.

Inspect the current OrderService implementation before choosing the exact order of operations.

Avoid creating distributed transactions across microservices.

Use an appropriate Saga/compensation workflow if the existing architecture supports it.

## 2. Order cancellation workflow

When an order is cancelled:

- OrderService validates whether cancellation is allowed.
- InventoryService releases the corresponding reservation or restores stock according to the actual inventory state.
- Repeated cancellation events must not increase stock multiple times.
- The operation must be idempotent.
- PaymentService handles financial refunds separately.
- NotificationService handles notifications.

## 3. Delivery and fulfillment workflow

When an order is shipped or delivered:

- ShippingService owns shipment and delivery state.
- OrderService owns order state.
- InventoryService owns inventory commitment and stock quantities.
- Do not let ShippingService directly modify InventoryService database tables.

Use authenticated internal APIs or reliable events.

## 4. Return and restock workflow

When goods are returned:

- OrderService manages the return request and order workflow.
- ShippingService handles the return shipment.
- PaymentService handles refunds.
- InventoryService restores stock only after receiving a trusted confirmation that the returned goods are eligible for restocking.

Do not automatically add returned quantities to available stock before inspection.

---

# PART 4 — DATABASE, SECURITY, AND RELIABILITY

Inspect the existing schema before adding or changing database structures.

Implement only necessary changes.

Potential entities may include:

- Inventory
- StockMovement
- InventoryAdjustment
- StockReservation
- ReservationItem
- StockAlert
- Stocktake
- StocktakeItem
- Warehouse, only if required
- ProcessedEvent or InboxMessage, if required by the event architecture

Do not blindly create every entity if the existing project already has an equivalent.

Requirements:

1. Correct primary keys and foreign key relationships.
2. Appropriate indexes for ProductId, VariantId, OrderId, ReservationId, and frequently queried fields.
3. Unique constraints where needed.
4. Correct decimal/integer quantity types according to the existing model.
5. EF Core configurations consistent with the project.
6. Safe migrations.
7. No destructive migrations without a clear technical necessity.
8. No loss of existing inventory data.
9. Correct SQL Server transaction handling.
10. Concurrency protection.
11. Structured logging.
12. Centralized exception handling.
13. DTO validation.
14. Consistent API response format.
15. Appropriate HTTP status codes.

Security requirements:

- Buyer cannot modify inventory.
- Seller can only access their own inventory.
- Admin can perform authorized management operations.
- Internal service APIs require service authentication.
- Never trust ProductId ownership, SellerId, UserId, or role information submitted by the frontend.
- Never expose internal database entities directly when DTOs are appropriate.
- Prevent mass assignment.
- Validate all quantities and state transitions.

---

# PART 5 — RABBITMQ, REDIS, AND ASYNCHRONOUS PROCESSING

Inspect existing integrations before adding new ones.

## RabbitMQ

Use RabbitMQ where asynchronous communication provides a real benefit.

Potential events include:

- StockReserved
- StockReservationReleased
- StockCommitted
- StockAdjusted
- StockLow
- StockOut
- InventoryRestored

Integrate with the existing event naming conventions and message contracts.

Requirements:

- Reliable event publishing.
- Idempotent event consumption.
- Duplicate event protection.
- Appropriate retry handling.
- Dead-letter handling if already supported by the project.
- Do not publish success events before the database transaction has successfully committed.
- Use the existing Outbox/Inbox pattern where available and justified.

Do not introduce unnecessary queues, exchanges, or consumers.

## Redis

Use Redis only when it improves actual read performance or existing architecture already requires it.

Potential use cases:

- Frequently accessed availability information.
- Inventory summary caching.
- Short-lived read models.

Requirements:

- Avoid stale stock availability being treated as authoritative.
- Never use Redis cache as the final source of truth for stock reservation.
- Invalidate or update relevant cache entries after inventory changes.
- Preserve existing Redis configuration and naming conventions.

---

# PART 6 — COMPLETE FRONTEND IMPLEMENTATION

The Frontend must provide a complete, polished, usable interface for the InventoryService features that are actually implemented.

Do not stop after implementing Backend APIs.

Inspect the existing React + TypeScript/Vite frontend and follow its current architecture.

Reuse existing:

- Layouts.
- Components.
- API clients.
- Authentication context.
- Routing.
- State management.
- UI libraries.
- Styling conventions.

Do not introduce a new frontend architecture unnecessarily.

## 1. Design direction

Create a premium e-commerce management interface inspired by Shopee's usability and visual hierarchy, but with a distinctive EcomSystem identity.

The result must NOT look like a copied Shopee interface.

Use:

- A clean, modern, professional dashboard.
- Consistent EcomSystem branding.
- Carefully designed spacing and typography.
- Clear visual hierarchy.
- A polished color system.
- Professional tables and data cards.
- Responsive layouts.
- Consistent buttons, forms, dialogs, and status indicators.
- Subtle transitions and hover states.
- Useful empty states, loading states, and error states.

The interface should feel like a real commercial e-commerce platform.

Avoid:

- Generic unfinished admin templates.
- Excessive gradients.
- Random colors.
- Oversized empty cards.
- Unnecessary animations.
- Fake data.
- Non-functional buttons.
- Placeholder pages pretending to be complete.
- Inconsistent layouts between Inventory pages.

## 2. Buyer-facing inventory integration

Inventory information should appear in relevant buyer experiences where appropriate.

Examples:

- Product detail pages should show accurate availability.
- Product cards should correctly represent out-of-stock products.
- Cart should display stock-related validation errors.
- Checkout should clearly communicate unavailable quantities.
- Prevent buyers from proceeding with invalid quantities.
- Display useful messages when stock changes during checkout.

Do not expose sensitive internal stock-management information to buyers.

Do not show misleading stock quantities.

## 3. Seller Inventory Dashboard

Create a complete Seller inventory management experience.

Include, where supported by Backend APIs:

- Inventory overview.
- Total products.
- Available stock.
- Reserved stock.
- Low-stock products.
- Out-of-stock products.
- Search and filtering.
- Inventory table.
- Product and variant details.
- Stock addition.
- Stock adjustment with reason.
- Stock movement history.
- Low-stock threshold configuration.
- Inventory statistics.
- Alerts and alert history.

Seller must only see and manage their own inventory.

Provide confirmation dialogs for sensitive operations.

## 4. Admin Inventory Dashboard

Create a complete Admin inventory management experience.

Include:

- Global inventory overview.
- Inventory search.
- Inventory details.
- Stock adjustment.
- Stock movement history.
- Reservation management.
- Reservation release with proper authorization.
- Low-stock and out-of-stock alerts.
- Audit logs.
- Inventory statistics.
- Reports and export where implemented.

Clearly distinguish Seller operations from Admin operations.

## 5. Inventory forms and interactions

Every form must be functional.

Examples:

- Add stock.
- Remove stock.
- Adjust stock.
- Batch stock operations.
- Configure low-stock threshold.
- View reservation.
- Release reservation.
- Search inventory.
- Filter by availability.
- Review stock movement history.

Validate inputs on the frontend and Backend.

Show meaningful success and failure messages based on actual API responses.

Do not show a success notification if the Backend operation failed.

Disable repeated submissions while a request is being processed.

Require confirmation before destructive or sensitive operations.

## 6. Frontend API integration

Connect the UI to the real InventoryService APIs.

Use the existing environment variables, API base URLs, and Vite proxy configuration.

Do not hardcode localhost URLs if the project already uses environment variables or a centralized API client.

Requirements:

- Correct request methods and routes.
- Correct DTO structures.
- JWT authentication.
- Appropriate error handling.
- Loading indicators.
- Empty states.
- Pagination.
- Search and filtering.
- Consistent response handling.
- No mock API success.
- No fabricated inventory records.

Inspect the actual Backend response contracts before writing frontend API integrations.

---

# PART 7 — API AND FRONTEND COMPLETENESS

The API list in this prompt is a functional specification, not permission to create duplicate or meaningless endpoints.

Before implementation:

1. Compare every requested API with existing Controllers and routes.
2. Identify existing implementations.
3. Identify missing capabilities.
4. Identify duplicate or overlapping routes.
5. Consolidate endpoints where query parameters or existing operations provide the same capability.
6. Preserve existing contracts when possible.
7. Implement all essential business capabilities.
8. Implement advanced modules only when they fit the current architecture and can be completed correctly.
9. Update every affected Backend and Frontend integration if a route or DTO genuinely needs to change.

Do not claim that all APIs are complete merely because empty Controllers or placeholder routes exist.

Every implemented endpoint must have:

- Correct authorization.
- Correct request and response DTOs.
- Input validation.
- Real business logic.
- Correct database operations.
- Appropriate error handling.
- Integration tests or other meaningful verification where feasible.

Every implemented frontend feature must actually work with the Backend.

---

# PART 8 — TESTING REQUIREMENTS

Testing is mandatory.

Do not stop after writing code or successfully compiling the solution.

## 1. Backend tests

Build the affected Backend projects.

Run existing tests.

Add or update tests for important business rules, particularly:

- Stock creation.
- Stock addition.
- Stock removal.
- Stock adjustment.
- Negative stock prevention.
- Availability calculation.
- Reservation creation.
- Duplicate reservation requests.
- Reservation expiration.
- Reservation release.
- Reservation commit.
- Concurrent reservation attempts.
- Unauthorized Seller access.
- Admin-only operations.
- Invalid state transitions.
- Duplicate event processing.
- Stock restoration after a valid return.

Test both successful and unsuccessful scenarios.

## 2. API integration testing

Test the actual HTTP APIs using the project's existing test approach.

Verify:

- Correct HTTP status codes.
- Correct response structures.
- Validation errors.
- Authentication failures.
- Authorization failures.
- Real database persistence.
- Correct stock quantities after operations.
- Correct reservation lifecycle.
- Correct behavior when requests are repeated.

Do not test only by directly calling Handler methods if the important behavior depends on HTTP authentication, routing, or middleware.

## 3. Frontend testing

Build the frontend.

Run existing tests and add relevant tests where appropriate.

Check:

- Pages render correctly.
- Routes work.
- API calls use correct URLs.
- Forms submit correctly.
- Validation works.
- Loading and error states display correctly.
- Seller/Admin access is separated.
- Search and filtering work.
- Stock operations update the UI correctly.
- No broken imports.
- No console errors caused by the implementation.
- No missing assets or broken layouts.

## 4. Real Docker verification — MANDATORY

The project already has Docker configured for the entire EcomSystem platform.

You MUST verify the actual running application.

### If you modify any C# Backend code or Frontend code:

After completing the implementation, run:

`docker compose up --build`

Run this command from the project root directory containing the main `docker-compose.yml` or `compose.yaml`.

This is mandatory after C# or Frontend code changes.

Do not skip rebuilding because the code compiles locally.

### If you only perform ordinary testing without modifying C# or Frontend code:

Run:

`docker compose up`

### Verify the actual runtime

After Docker starts:

1. Check that all required services start successfully.
2. Check InventoryService logs.
3. Check SQL Server connectivity.
4. Check migrations and database initialization.
5. Check RabbitMQ connectivity if used.
6. Check Redis connectivity if used.
7. Check inter-service communication.
8. Check frontend accessibility.
9. Test important Inventory APIs.
10. Open the actual frontend and inspect the rendered interface.
11. Verify Seller inventory pages.
12. Verify Admin inventory pages.
13. Verify real stock operations.
14. Check browser console errors.
15. Check Backend exceptions and failed requests.

Do not merely assume the interface works because Vite builds successfully.

If the Docker environment or browser inspection tools are available, use them to inspect the real running application.

If a problem is found, fix it and repeat the relevant build and verification steps.

**Do not finish while known implementation-related errors remain unresolved.**

### Data safety rules

NEVER execute:

`docker compose down -v`

NEVER delete Docker volumes.

NEVER delete existing databases or inventory data.

NEVER remove unrelated Docker images, containers, volumes, or project data merely to make the build easier.

Do not reset the database to hide migration or data problems.

Preserve existing development data.

If a migration is required, apply it safely and verify that existing data remains intact.

---

# PART 9 — IMPLEMENTATION WORKFLOW

Follow this sequence:

### Phase 1 — Inspection

- Read `docs/AI_RULE.md` completely.
- Read `docs/InventoryService.md` completely.
- Inspect relevant service documentation.
- Inspect existing Backend and Frontend code.
- Inspect Docker configuration.
- Identify existing working features and missing features.
- Prepare an implementation checklist.

### Phase 2 — Backend

- Fix genuine bugs.
- Complete missing entities and configurations.
- Implement missing business logic.
- Complete Commands, Queries, Handlers, and repositories.
- Complete Controllers and API contracts.
- Implement authorization and validation.
- Implement safe database changes.
- Integrate other services where necessary.

### Phase 3 — Frontend

- Complete buyer-facing inventory integration.
- Complete Seller inventory dashboard.
- Complete Admin inventory dashboard.
- Implement all necessary forms, tables, filters, dialogs, and statistics.
- Connect everything to real APIs.
- Polish the UI consistently.

### Phase 4 — Integration

- Verify OrderService ↔ InventoryService.
- Verify CartService ↔ InventoryService.
- Verify ProductService ↔ InventoryService.
- Verify ShippingService ↔ InventoryService where required.
- Verify NotificationService integration where required.
- Verify RabbitMQ and Redis integration where applicable.

### Phase 5 — Testing

- Run Backend builds and tests.
- Run Frontend builds and tests.
- Run API integration tests.
- Verify security and business rules.
- Fix all implementation-related errors.

### Phase 6 — Docker and real UI verification

Because this task modifies C# and/or Frontend code, run:

`docker compose up --build`

Wait for services to initialize.

Inspect logs, test actual endpoints, open the frontend, verify the real UI, and fix problems.

Repeat until the application works correctly.

### Phase 7 — Final review

Review all changes.

Make sure:

- Existing correct functionality remains intact.
- No unrelated service was unnecessarily rewritten.
- No duplicate endpoints or business logic were introduced.
- No security vulnerabilities were introduced.
- No existing database data was lost.
- No fake UI data or fake success responses remain.
- All implemented features are connected to real APIs.

---

# PART 10 — FINAL REPORT

When everything is finished, provide a clear report in English or Vietnamese.

Include:

## 1. Implementation summary

Explain what was completed in InventoryService Backend and Frontend.

## 2. Files changed

List important files that were:

- Created.
- Modified.
- Left unchanged because they were already correct.

Briefly explain why each important modification was necessary.

## 3. API implementation status

Provide a table:

| API Group | Implemented | Already Existed | Modified | Not Implemented | Reason |
|---|---|---|---|---|---|

Do not claim unimplemented APIs are complete.

Explain any endpoints consolidated into existing routes.

## 4. Frontend implementation status

List the actual buyer, Seller, and Admin features completed.

## 5. Database and integration

Report:

- Entities and migrations changed.
- SQL Server status.
- RabbitMQ integration status.
- Redis integration status.
- Other service integrations.

## 6. Testing results

List the actual commands executed and their real results.

Include:

- Backend build.
- Backend tests.
- Frontend build.
- Frontend tests.
- API integration tests.
- Docker startup.
- Important API verification.
- Actual UI inspection.

## 7. Docker verification

Explicitly report whether:

`docker compose up --build`

was executed successfully.

List any services that failed to start or any remaining runtime issues.

## 8. Remaining work

Clearly list anything that could not be implemented or verified and explain why.

Never fabricate successful tests, builds, Docker startup, API responses, or UI verification.

---

# FINAL NON-NEGOTIABLE RULES

1. Read `docs/AI_RULE.md` completely before coding.
2. Read `docs/InventoryService.md` completely before coding.
3. Inspect the existing architecture before modifying anything.
4. Preserve correct existing Backend and Frontend code.
5. Do not delete working features.
6. Do not rewrite unrelated services.
7. Do not introduce unnecessary complexity.
8. Follow the existing Clean Architecture and MediatR patterns.
9. Implement real business logic, not placeholder APIs.
10. Enforce stock consistency and concurrency safety.
11. Enforce Buyer, Seller, Admin, and Internal authorization correctly.
12. Build a polished Shopee-inspired but uniquely branded EcomSystem interface.
13. Connect the frontend to real Backend APIs.
14. Test the actual application, not just the code.
15. After any C# or Frontend code modifications, execute `docker compose up --build` from the project root.
16. Never execute `docker compose down -v`.
17. Never delete existing database volumes or data.
18. Never claim success without actual verification.
19. Continue fixing implementation-related issues until the required functionality works correctly.
20. Do not stop at partial Backend implementation while leaving the Frontend unfinished.

**Your final objective is a complete, reliable, visually polished, fully integrated InventoryService that works as part of the existing EcomSystem platform without damaging any already-correct functionality.**