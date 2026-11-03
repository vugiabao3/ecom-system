# SHIPPING SERVICE — COMPLETE BACKEND & FRONTEND IMPLEMENTATION

## 1. Main Objective

You are working on the **EcomSystem** project, a microservices-based e-commerce platform built with ASP.NET Core and React.

Your task is to thoroughly inspect, complete, improve, and implement the **entire ShippingService Backend and Frontend** according to:

- `docs/AI_RULE.md`
- `docs/ShippingService.md`
- Existing project architecture and coding conventions
- Existing Backend and Frontend implementations
- The API requirements specified in this prompt

The goal is to deliver a complete, reliable, secure, production-oriented ShippingService with a polished e-commerce user experience.

**The Frontend should have the usability and visual quality of Shopee, but maintain EcomSystem's own distinctive branding and identity.**

**The Backend must follow the existing Clean Architecture, CQRS/MediatR, Entity Framework Core, microservice boundaries, security rules, and business workflows.**

Do not merely generate code or make the project compile. You must run the system, test actual workflows, inspect the running Frontend, fix discovered problems, and verify the final result.

---

# 2. MANDATORY RULES — READ BEFORE CODING

## 2.1 Read the documentation first

Before making any changes:

1. Read the entire `docs/AI_RULE.md`.
2. Read the entire `docs/ShippingService.md`.
3. Inspect the complete existing ShippingService Backend structure.
4. Inspect the existing Frontend structure and all ShippingService-related components.
5. Inspect existing API clients, authentication, authorization, routing, layouts, shared components, and styling conventions.
6. Inspect Docker Compose and the existing service configuration.
7. Identify existing implementations, working features, incomplete features, bugs, missing APIs, and integration dependencies.

Do not start coding before understanding the existing implementation.

## 2.2 Preserve existing correct code

This is an extremely important requirement.

- Do NOT delete existing files simply because you prefer another structure.
- Do NOT rewrite entire controllers, services, handlers, repositories, components, or pages unnecessarily.
- Do NOT remove working features.
- Do NOT replace correct implementations with simplified demonstrations.
- Do NOT create duplicate classes, DTOs, interfaces, endpoints, or components when suitable implementations already exist.
- Reuse existing entities, commands, queries, handlers, repositories, interfaces, services, validators, controllers, API clients, hooks, and UI components wherever appropriate.
- If existing code is correct, keep it.
- If existing code is incomplete or incorrect, modify only the necessary parts.
- If a new file is genuinely required, create it following the existing project structure and naming conventions.
- Preserve existing API contracts unless a change is necessary to fix a real architectural, security, or business-logic problem.
- Ensure changes do not break other microservices or existing Frontend features.

**The objective is to complete and improve the existing project, not rebuild everything from scratch.**

Before modifying an existing file, understand its responsibilities and dependencies.

## 2.3 No fake implementation

Do NOT:

- Return hardcoded shipment data as if it came from the database.
- Use mock APIs to simulate successful operations.
- Display success notifications when the Backend operation failed.
- Create buttons that do nothing.
- Implement Frontend pages that only display static sample data.
- Pretend that an external shipping provider is integrated when it is not.
- Fake GPS tracking, payment confirmation, COD collection, or delivery proof.
- Ignore API errors.

All implemented features must work with real Backend APIs and persistent data.

If an external integration is not configured, provide a clean extensible integration boundary and clearly communicate its actual availability.

---

# 3. PROJECT ARCHITECTURE AND TECHNOLOGY

Follow the existing EcomSystem architecture.

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
- Docker and Docker Compose
- Swagger / OpenAPI
- Logging and exception handling

Follow the existing project structure, such as:

- ShippingService.Api
- ShippingService.Application
- ShippingService.Domain
- ShippingService.Infrastructure

Do not force a new architecture if the existing solution has a different but valid structure.

## Frontend

Expected technologies:

- React
- TypeScript
- Vite
- Existing routing solution
- Existing API client and authentication mechanism
- Existing styling system and reusable UI components

Use the project's existing conventions rather than introducing unnecessary new libraries.

---

# 4. SHIPPING SERVICE BUSINESS RESPONSIBILITIES

ShippingService is responsible for:

- Shipment creation and management.
- Shipment lifecycle and status transitions.
- Shipment tracking.
- Shipping assignments.
- Shipper delivery operations.
- Delivery attempts.
- Failed delivery handling.
- Return shipment operations.
- COD collection records and handover records.
- Shipping methods and fee calculations.
- Shipping providers and their services.
- Shipping statistics and administration.
- Secure internal communication with other microservices.

## 4.1 Strict microservice boundaries

Respect these responsibilities:

| Service | Responsibility |
|---|---|
| AuthService | Authentication, JWT, credentials and identity |
| UserService | Buyer, Seller and Shipper user profiles |
| ProductService | Product information, weight and dimensions |
| InventoryService | Stock quantity, reservation, release and restocking |
| OrderService | Orders, order status and order lifecycle |
| PaymentService | Payments, financial transactions and refunds |
| ShippingService | Shipments, tracking, assignments, delivery and COD collection records |
| NotificationService | Sending and managing notifications |

Important rules:

1. ShippingService must not directly modify OrderService database tables.
2. ShippingService must not directly modify InventoryService stock.
3. ShippingService must not independently mark a financial payment as successful.
4. ShippingService must not independently process refunds.
5. ShippingService must not own Buyer or Seller authentication credentials.
6. ShippingService should not duplicate complete Shipper user profiles if UserService already owns them.
7. Use authenticated internal APIs and/or RabbitMQ events for cross-service communication.
8. Use the existing integration patterns in the project.
9. Do not introduce unnecessary synchronous dependencies between services.

### COD ownership

ShippingService records:

- COD amount expected for collection.
- Amount collected by the Shipper.
- Collection time.
- Collection status.
- COD handover and reconciliation records.

PaymentService remains the authority for official payment status and financial transactions.

A successful COD collection must be communicated to PaymentService through a properly authenticated internal operation or verified event.

Never let the Frontend directly mark an order as financially paid.

### Return ownership

Distinguish clearly:

- Failed delivery: the delivery attempt was unsuccessful.
- Return shipment: the package is being transported back.
- Return completed: the package has returned to the appropriate seller or warehouse.
- Refund: belongs to PaymentService.
- Inventory restocking: belongs to InventoryService after the returned goods have been inspected and approved.

ShippingService must not automatically refund customers or increase inventory quantities.

---

# 5. COMPLETE BACKEND API REQUIREMENTS

Implement the following API requirements.

First inspect existing controllers and endpoints. Reuse existing routes when they already provide the correct behavior.

Do not create duplicate endpoints simply to match a list mechanically. Where two endpoints have the same business purpose, consolidate them when architecturally appropriate, and preserve compatibility when existing consumers depend on an old route.

Every implemented endpoint must have:

- Correct HTTP method and route.
- Correct request and response DTOs.
- Input validation.
- Appropriate authorization.
- Business-rule validation.
- Correct error responses.
- Logging where appropriate.
- Pagination for list endpoints.
- Consistent response conventions.
- Tests for important business behavior.

## GROUP 1 — Shipment Management

1. `POST /api/shipments` — Create a shipment from an eligible order.
2. `GET /api/shipments` — Get shipment list with pagination and filters.
3. `GET /api/shipments/{shipmentId}` — Get shipment details.
4. `GET /api/shipments/by-order/{orderId}` — Get shipment by order ID.
5. `GET /api/shipments/track/{trackingNumber}` — Track using tracking number.
6. `PATCH /api/shipments/{shipmentId}/address` — Update delivery address when permitted.
7. `PATCH /api/shipments/{shipmentId}/recipient` — Update recipient information when permitted.
8. `POST /api/shipments/{shipmentId}/cancel` — Cancel shipment when eligible.
9. `GET /api/shipments/{shipmentId}/history` — Get shipment history.
10. `GET /api/shipments/{shipmentId}/tracking` — Get shipment tracking timeline.

## GROUP 2 — Shipment Status Management

11. `POST /api/shipments/{shipmentId}/confirm` — Confirm shipment acceptance.
12. `POST /api/shipments/{shipmentId}/prepare` — Start preparing the package.
13. `POST /api/shipments/{shipmentId}/ready-for-pickup` — Mark package ready for pickup.
14. `POST /api/shipments/{shipmentId}/pickup` — Confirm package pickup.
15. `POST /api/shipments/{shipmentId}/in-transit` — Mark shipment in transit.
16. `POST /api/shipments/{shipmentId}/start-delivery` — Start delivery.
17. `POST /api/shipments/{shipmentId}/complete` — Confirm successful delivery.
18. `POST /api/shipments/{shipmentId}/delivery-failed` — Record unsuccessful delivery.
19. `POST /api/shipments/{shipmentId}/return` — Start return shipment.
20. `POST /api/shipments/{shipmentId}/returned` — Confirm returned package.
21. `PATCH /api/shipments/{shipmentId}/status` — Change status through authorized state transitions only.

Never allow arbitrary status updates without validating the current state, caller role, shipment ownership, and allowed transition.

## GROUP 3 — Buyer Shipping

22. `GET /api/shipping/me` — Get current Buyer's shipments.
23. `GET /api/shipping/me/summary` — Get shipping overview.
24. `GET /api/shipping/me/{shipmentId}` — Get shipment details.
25. `GET /api/shipping/me/{shipmentId}/tracking` — Get tracking information.
26. `GET /api/shipping/me/{shipmentId}/history` — Get shipping history.
27. `POST /api/shipping/me/{shipmentId}/delivery-instructions` — Submit delivery instructions.
28. `POST /api/shipping/me/{shipmentId}/delivery-reschedule` — Request delivery rescheduling.
29. `POST /api/shipping/me/{shipmentId}/report-issue` — Report a delivery problem.

Buyer access must always be restricted to shipments associated with the Buyer's own orders.

Never trust a UserId supplied by the Frontend when the current identity can be obtained from JWT claims.

## GROUP 4 — Seller Shipping

30. `GET /api/seller/shipments` — Get Seller's shipments.
31. `GET /api/seller/shipments/summary` — Get Seller shipping overview.
32. `GET /api/seller/shipments/{shipmentId}` — Get shipment details.
33. `GET /api/seller/shipments/pending` — Get shipments awaiting preparation.
34. `GET /api/seller/shipments/ready-for-pickup` — Get packages awaiting carrier pickup.
35. `POST /api/seller/shipments/{shipmentId}/prepare` — Confirm preparation.
36. `POST /api/seller/shipments/{shipmentId}/ready-for-pickup` — Confirm package ready.
37. `POST /api/seller/shipments/{shipmentId}/handover` — Confirm package handover.
38. `GET /api/seller/shipments/{shipmentId}/history` — Get shipment history.
39. `GET /api/seller/shipping/statistics` — Get Seller shipping statistics.
40. `GET /api/seller/shipping/failed` — Get failed deliveries.
41. `GET /api/seller/shipping/returns` — Get return shipments.

A Seller must only access shipments belonging to their own shop.

Derive SellerId from authenticated identity or a trusted UserService integration. Do not trust arbitrary SellerId values from the Frontend.

## GROUP 5 — Shipper Management

42. `GET /api/shippers` — Get Shipper list.
43. `GET /api/shippers/{shipperId}` — Get Shipper details.
44. `POST /api/shippers` — Create operational Shipper information if required.
45. `PUT /api/shippers/{shipperId}` — Update operational information.
46. `PATCH /api/shippers/{shipperId}/status` — Update operational status.
47. `GET /api/shippers/{shipperId}/shipments` — Get assigned shipments.
48. `GET /api/shippers/{shipperId}/statistics` — Get delivery performance.
49. `GET /api/shippers/{shipperId}/availability` — Check availability.
50. `PATCH /api/shippers/me/availability` — Update current Shipper availability.
51. `GET /api/shippers/me/profile` — Get current Shipper profile.

Important:

If UserService already owns Shipper profiles, do not duplicate full user management in ShippingService.

ShippingService should only maintain necessary operational delivery information, such as:

- ShipperId.
- Operational status.
- Availability.
- Assigned workload.
- Delivery statistics.
- Operational metadata.

Use UserService to retrieve user profile information when needed.

## GROUP 6 — Shipment Assignment

52. `POST /api/shipments/{shipmentId}/assign` — Assign a Shipper.
53. `POST /api/shipments/{shipmentId}/reassign` — Reassign a Shipper.
54. `DELETE /api/shipments/{shipmentId}/assignment` — Remove assignment.
55. `GET /api/shipments/{shipmentId}/assignment` — Get assignment information.
56. `GET /api/admin/shipping/unassigned` — Get unassigned shipments.
57. `GET /api/shippers/me/assigned-shipments` — Get current Shipper's assignments.
58. `POST /api/shippers/me/shipments/{shipmentId}/accept` — Accept an assignment.
59. `POST /api/shippers/me/shipments/{shipmentId}/reject` — Reject an assignment.
60. `GET /api/shippers/me/shipments/today` — Get today's assigned shipments.

Critical concurrency requirements:

- Two Shippers must never successfully accept the same shipment.
- Assignment and acceptance must be atomic.
- Use database transactions and optimistic concurrency or conditional updates.
- Handle concurrent requests safely.
- Prevent duplicate assignments.
- Validate Shipper availability and workload.
- Record assignment history.
- Ensure reassignment follows business rules.
- Make important operations idempotent where appropriate.

## GROUP 7 — Delivery Operations

61. `GET /api/shippers/me/deliveries` — Get current delivery tasks.
62. `GET /api/shippers/me/deliveries/{shipmentId}` — Get delivery details.
63. `POST /api/shippers/me/deliveries/{shipmentId}/pickup` — Confirm pickup.
64. `POST /api/shippers/me/deliveries/{shipmentId}/start` — Start delivery.
65. `POST /api/shippers/me/deliveries/{shipmentId}/complete` — Confirm delivery.
66. `POST /api/shippers/me/deliveries/{shipmentId}/failed` — Report failed delivery.
67. `POST /api/shippers/me/deliveries/{shipmentId}/proof` — Submit delivery proof.
68. `POST /api/shippers/me/deliveries/{shipmentId}/retry` — Request another delivery attempt.
69. `POST /api/shippers/me/deliveries/{shipmentId}/return` — Start return shipment.
70. `GET /api/shippers/me/deliveries/statistics` — Get Shipper delivery statistics.

Delivery proof may support:

- Image metadata and uploaded image references.
- Recipient signature.
- OTP verification.
- Recipient confirmation details.
- Delivery timestamp.

Do not implement every proof mechanism if the existing project does not support it.

Implement a secure, extensible design for the supported proof mechanisms. Never accept an arbitrary OTP or proof result without server-side verification.

## GROUP 8 — Delivery Tracking

71. `POST /api/shipments/{shipmentId}/tracking-events` — Record a tracking event.
72. `GET /api/shipments/{shipmentId}/tracking-events` — Get tracking events.
73. `GET /api/tracking/{trackingNumber}` — Track by tracking number.
74. `GET /api/tracking/{trackingNumber}/latest` — Get latest tracking status.
75. `GET /api/shipments/{shipmentId}/estimated-delivery` — Get estimated delivery time.
76. `POST /api/shipments/{shipmentId}/location` — Update delivery location if GPS is supported.
77. `GET /api/shipments/{shipmentId}/locations` — Get location history if GPS is supported.

Tracking must include meaningful events, for example:

- Shipment created.
- Seller preparing package.
- Package ready for pickup.
- Shipper assigned.
- Package picked up.
- In transit.
- Arrived at delivery area.
- Out for delivery.
- Delivery completed.
- Delivery failed.
- Return initiated.
- Returned to seller or warehouse.

GPS requirements:

- Do not write unlimited GPS records to SQL Server.
- Apply sensible update intervals and retention policies.
- Restrict location access.
- Do not expose a Shipper's private location history to unauthorized users.
- If GPS is not currently supported, do not simulate real GPS tracking.

## GROUP 9 — Delivery Attempts and Returns

78. `POST /api/shipments/{shipmentId}/delivery-attempts` — Record a delivery attempt.
79. `GET /api/shipments/{shipmentId}/delivery-attempts` — Get delivery attempts.
80. `POST /api/shipments/{shipmentId}/delivery-attempts/{attemptId}/retry` — Schedule another attempt.
81. `POST /api/shipments/{shipmentId}/return-request` — Create return request.
82. `GET /api/returns/{returnId}` — Get return details.
83. `GET /api/returns` — Get return requests.
84. `POST /api/returns/{returnId}/approve` — Approve return.
85. `POST /api/returns/{returnId}/reject` — Reject return.
86. `POST /api/returns/{returnId}/pickup` — Confirm return pickup.
87. `POST /api/returns/{returnId}/complete` — Complete return.
88. `GET /api/shipments/{shipmentId}/return-history` — Get return history.

Clearly distinguish:

- Failed delivery.
- Delivery retry.
- Return request.
- Return shipment.
- Return completed.
- Refund.
- Inventory restocking.

Do not implement refunds or inventory adjustments inside ShippingService.

## GROUP 10 — COD Collection

89. `GET /api/shippers/me/cod` — Get COD collection tasks.
90. `GET /api/shippers/me/cod/summary` — Get COD summary.
91. `GET /api/shipments/{shipmentId}/cod` — Get expected COD amount.
92. `POST /api/shipments/{shipmentId}/cod/collected` — Record collected COD.
93. `POST /api/shipments/{shipmentId}/cod/failed` — Record COD collection failure.
94. `GET /api/shippers/me/cod/history` — Get COD collection history.
95. `POST /api/shippers/me/cod/settlements` — Submit COD handover report.
96. `GET /api/shippers/me/cod/settlements` — Get COD settlement history.
97. `GET /api/admin/cod/settlements` — Admin COD settlement management.

Important:

- Verify the expected COD amount from a trusted source.
- Never trust the amount submitted by the Frontend.
- Prevent duplicate collection records.
- Prevent collecting COD twice for the same shipment.
- Maintain an auditable collection history.
- Validate Shipper assignment and shipment status.
- Communicate verified collection results to PaymentService.
- PaymentService remains responsible for official payment status and financial records.

## GROUP 11 — Shipping Fees and Methods

98. `GET /api/shipping/methods` — Get shipping methods.
99. `GET /api/shipping/methods/available` — Get available methods.
100. `GET /api/shipping/fees/estimate` — Estimate shipping fees.
101. `POST /api/shipping/fees/calculate` — Calculate shipping fees.
102. `GET /api/shipments/{shipmentId}/fee` — Get shipment shipping fee.
103. `GET /api/shipping/providers` — Get shipping providers.
104. `GET /api/shipping/providers/{providerId}` — Get provider details.
105. `GET /api/shipping/providers/{providerId}/services` — Get provider services.

Fee calculation must be based on available trusted information:

- Pickup address.
- Delivery address.
- Distance or delivery zone.
- Package weight.
- Package dimensions.
- Shipping method.
- Provider pricing rules.
- Additional applicable fees.

Do not let the Frontend determine the final shipping fee.

If ProductService owns weight and dimensions, retrieve them through an appropriate internal integration.

If external providers are not integrated, implement an extensible provider abstraction and clearly identify which providers are actually configured.

## GROUP 12 — Admin Shipping Management

106. `GET /api/admin/shipments` — Get all shipments.
107. `GET /api/admin/shipments/{shipmentId}` — Get shipment details.
108. `GET /api/admin/shipments/search` — Search shipments.
109. `GET /api/admin/shipments/pending` — Get pending shipments.
110. `GET /api/admin/shipments/in-transit` — Get in-transit shipments.
111. `GET /api/admin/shipments/delivering` — Get shipments being delivered.
112. `GET /api/admin/shipments/failed` — Get failed deliveries.
113. `GET /api/admin/shipments/returns` — Get return shipments.
114. `POST /api/admin/shipments/{shipmentId}/assign` — Assign Shipper.
115. `POST /api/admin/shipments/{shipmentId}/reassign` — Reassign Shipper.
116. `GET /api/admin/shippers` — Manage Shippers.
117. `GET /api/admin/shippers/statistics` — Shipper statistics.
118. `GET /api/admin/shipping/statistics` — Overall shipping statistics.
119. `GET /api/admin/shipping/fees` — Manage shipping fees.
120. `GET /api/admin/shipping/audit-logs` — View administrative audit logs.

Admin operations must be authorized and audited.

## GROUP 13 — Internal Microservice APIs

121. `POST /api/internal/shipments` — OrderService requests shipment creation.
122. `GET /api/internal/shipments/order/{orderId}` — Get shipment by order.
123. `GET /api/internal/shipments/{shipmentId}/summary` — Get shipment summary.
124. `GET /api/internal/shipments/order/{orderId}/status` — Get shipment status.
125. `POST /api/internal/shipments/{shipmentId}/cancel` — Internal cancellation request.
126. `POST /api/internal/shipments/{shipmentId}/status` — Trusted internal status operation.
127. `GET /api/internal/shipments/order/{orderId}/tracking` — Get tracking information.
128. `POST /api/internal/shipments/{shipmentId}/cod/collected` — PaymentService integration for verified COD collection.
129. `GET /api/internal/shipments/{shipmentId}/cod` — Get COD information.
130. `POST /api/internal/shipments/{shipmentId}/return-completed` — Notify relevant services that return is complete.
131. `POST /api/internal/shipments/{shipmentId}/delivery-completed` — Notify OrderService that delivery is complete.
132. `GET /api/internal/shipping/statistics` — Internal shipping statistics.

Internal endpoints must not be publicly accessible.

Use the existing Internal authentication scheme, service-to-service authorization, or an equally secure mechanism already implemented in the project.

Do not expose internal APIs to ordinary Buyer, Seller, or Shipper sessions.

---

# 6. BUSINESS WORKFLOWS — MUST WORK END TO END

Do not implement APIs independently without connecting them into coherent workflows.

## 6.1 Shipment creation workflow

Expected flow:

1. An order reaches an eligible fulfillment state.
2. OrderService requests shipment creation.
3. ShippingService validates the request.
4. ShippingService creates the shipment.
5. Generate a unique tracking number.
6. Store recipient, pickup, delivery and package information.
7. Create the initial tracking event.
8. Return shipment information to the caller.
9. Publish appropriate integration events if the existing architecture uses RabbitMQ.

Handle duplicate creation requests safely using idempotency or a unique order constraint.

## 6.2 Seller preparation workflow

1. Seller views pending shipments.
2. Seller opens shipment details.
3. Seller confirms preparation.
4. Seller marks package ready for pickup.
5. Seller confirms handover when the package is actually handed to the carrier.

Do not allow Seller operations on another Seller's shipments.

## 6.3 Assignment workflow

1. Admin or an authorized assignment process views unassigned shipments.
2. Assign a suitable available Shipper.
3. Shipper receives assignment information.
4. Shipper accepts or rejects the assignment.
5. Update assignment state atomically.
6. Prevent multiple Shippers from accepting the same shipment.
7. Record assignment history.
8. Send relevant notifications through NotificationService or RabbitMQ.

## 6.4 Delivery workflow

Expected lifecycle:

`Created → Confirmed → Preparing → ReadyForPickup → PickedUp → InTransit → Delivering → Delivered`

Alternative paths:

- `Delivering → DeliveryFailed → Delivering`
- `DeliveryFailed → ReturnInProgress → Returned`
- Eligible pre-delivery cancellation.

The exact enum names may differ. Reuse existing enums where appropriate.

Implement a valid state machine. Reject invalid transitions with meaningful errors.

## 6.5 Failed delivery workflow

When delivery fails:

- Record the attempt.
- Record the failure reason.
- Record timestamp.
- Record the responsible Shipper.
- Update shipment status according to the state machine.
- Allow another attempt if business rules permit.
- Start return flow only when permitted.
- Notify relevant users.

Do not treat every failed attempt as an automatic completed return.

## 6.6 COD workflow

1. Shipper opens assigned COD delivery.
2. Backend retrieves expected COD amount.
3. Shipper records collection result.
4. Backend validates amount and shipment eligibility.
5. Persist an auditable collection record.
6. Send verified result to PaymentService.
7. PaymentService updates official payment records.
8. Update shipping-related COD state.

## 6.7 Return workflow

1. Create return request.
2. Validate eligibility and permissions.
3. Approve or reject the request.
4. Assign return pickup where required.
5. Track return movement.
6. Confirm returned package.
7. Notify OrderService and InventoryService.
8. Let the appropriate services determine order status and inventory restocking.

---

# 7. BACKEND IMPLEMENTATION REQUIREMENTS

## 7.1 Clean Architecture and CQRS

Follow existing project conventions.

Use:

- Controllers for HTTP handling.
- Commands for state-changing operations.
- Queries for read operations.
- Handlers for business orchestration.
- Repositories/interfaces where appropriate.
- Infrastructure implementations for persistence and external integrations.
- DTOs for API contracts.
- Validators for request validation.

Avoid putting business logic directly inside controllers.

Do not introduce unnecessary abstraction layers when the existing codebase already has a suitable implementation.

## 7.2 Domain model

Inspect existing entities before adding new ones.

Depending on the existing design, relevant domain models may include:

- Shipment.
- ShipmentItem or PackageInformation.
- ShipmentStatusHistory.
- TrackingEvent.
- ShipperOperationalProfile.
- ShipmentAssignment.
- DeliveryAttempt.
- DeliveryProof.
- ReturnShipment.
- ReturnHistory.
- CodCollection.
- CodSettlement.
- ShippingMethod.
- ShippingProvider.
- ShippingProviderService.
- ShippingFeeRule.
- DeliveryInstruction.
- DeliveryIssue.
- ProcessedEvent or InboxMessage.
- AuditLog.

Do not create all these entities blindly. Add only the entities required by the actual business requirements and existing implementation.

## 7.3 Database and migrations

- Use the existing DbContext.
- Follow existing EF Core conventions.
- Configure relationships and indexes correctly.
- Add unique constraints where required.
- Add indexes for tracking numbers, order IDs, status, Shipper ID and frequently queried fields.
- Protect concurrent updates.
- Avoid storing unnecessary duplicated data.
- Do not delete existing database records.
- Do not reset databases or Docker volumes.
- Do not use destructive migrations that remove existing data without a justified and explicitly approved migration plan.

If schema changes are required, create appropriate migrations and verify them.

## 7.4 Security and authorization

Implement authorization according to actual roles and existing JWT claims.

Expected access boundaries:

| Role | Access |
|---|---|
| Buyer | Own shipments, tracking, delivery instructions, delivery issue reporting |
| Seller | Shipments belonging to own shop, preparation and handover |
| Shipper | Own assigned deliveries, pickup, delivery, COD and proof operations |
| Admin | Administrative shipment management, assignments, statistics and audit logs |
| Internal service | Only explicitly authorized internal operations |

Important:

- Obtain current UserId from JWT.
- Verify ownership for every shipment.
- Verify Seller ownership.
- Verify Shipper assignment before delivery operations.
- Do not rely on a Frontend-supplied role, UserId, SellerId, or ShipperId.
- Protect internal APIs separately.
- Prevent IDOR vulnerabilities.
- Do not allow Buyers to confirm pickup or delivery.
- Do not allow Sellers to mark deliveries as completed.
- Admin overrides must be audited.
- Validate all state transitions on the Backend.

## 7.5 RabbitMQ and Redis

Inspect existing infrastructure and event patterns before implementing integrations.

Use RabbitMQ when asynchronous communication is genuinely appropriate, for example:

- ShipmentCreated.
- ShipmentAssigned.
- ShipmentPickedUp.
- ShipmentInTransit.
- ShipmentDeliveryStarted.
- ShipmentDelivered.
- ShipmentDeliveryFailed.
- ShipmentReturnStarted.
- ShipmentReturned.
- CodCollected.

Events should be published to relevant consumers such as OrderService, PaymentService, InventoryService, and NotificationService.

Requirements:

- Follow existing exchange, queue and routing conventions.
- Use reliable event handling.
- Implement idempotent consumers.
- Handle duplicate delivery.
- Handle retries and failures.
- Use an Outbox/Inbox pattern if consistent with the existing architecture and necessary for reliability.
- Do not introduce unnecessary messaging complexity.
- Do not use RabbitMQ as a replacement for the ShippingService database.

Use Redis only where it provides a real benefit, such as caching frequently accessed tracking or shipping-method data.

Do not introduce unnecessary Redis dependencies.

## 7.6 Error handling and observability

Implement:

- Consistent API error responses.
- Appropriate HTTP status codes.
- Validation errors.
- Not-found handling.
- Unauthorized and forbidden responses.
- Conflict handling for invalid transitions and concurrent assignment.
- Structured logging.
- Correlation IDs where supported.
- Health checks.

Never silently swallow exceptions.

---

# 8. COMPLETE FRONTEND IMPLEMENTATION

The Frontend must be fully integrated with the ShippingService Backend.

Do not create a separate disconnected demo interface.

Inspect the existing React application, routing, API clients, authentication and shared components before implementation.

## 8.1 Visual design requirements

Create a polished, modern e-commerce shipping experience inspired by Shopee's usability.

However, do not copy Shopee's branding, logos, assets, or exact design.

Use EcomSystem's own brand identity.

Design expectations:

- Premium e-commerce appearance.
- Clear information hierarchy.
- Consistent typography.
- Attractive spacing.
- Professional cards and tables.
- Carefully designed status badges.
- Modern icons.
- Subtle shadows and borders.
- Smooth but restrained interactions.
- Responsive layouts.
- Consistent color palette matching EcomSystem.
- High-quality empty states, loading states and error states.
- Consistent design across Buyer, Seller, Shipper and Admin.

Avoid:

- Plain unstyled HTML.
- Generic dashboard templates.
- Overly crowded layouts.
- Excessive gradients.
- Random colors.
- Inconsistent buttons.
- Repeated unnecessary cards.
- Fake data.
- Broken navigation.
- Buttons without working actions.

## 8.2 Buyer shipping interface

Implement a complete Buyer shipping experience.

Expected pages/components:

1. My Shipments.
2. Shipment Detail.
3. Shipment Tracking Timeline.
4. Tracking Number Lookup.
5. Estimated Delivery.
6. Delivery Instructions.
7. Delivery Reschedule Request.
8. Report Delivery Issue.
9. Shipment History.

The shipment list should support:

- Search.
- Status filtering.
- Pagination.
- Useful shipment summaries.
- Tracking number display.
- Order reference.
- Delivery estimate when available.
- Clear status indicators.

The detail page should display:

- Shipment information.
- Recipient and delivery address.
- Package information.
- Shipping method.
- Shipping fee.
- Tracking number.
- Current status.
- Tracking timeline.
- Delivery attempts.
- COD information when applicable.
- Return information when applicable.

Do not expose internal or sensitive operational information unnecessarily.

## 8.3 Seller shipping interface

Build a professional Seller shipping-management area.

Expected pages/components:

1. Seller Shipping Dashboard.
2. All Shipments.
3. Pending Shipments.
4. Ready for Pickup.
5. Shipment Details.
6. Prepare Package.
7. Mark Ready for Pickup.
8. Confirm Handover.
9. Failed Deliveries.
10. Return Shipments.
11. Shipping Statistics.

The Seller should be able to:

- View shipments belonging to their shop.
- Search and filter.
- Open shipment details.
- Prepare packages.
- Mark packages ready.
- Confirm actual handover.
- View shipment history.
- Review failed deliveries.
- View return shipments.
- View shipping statistics.

Use confirmation dialogs for important operations.

Disable actions that are not allowed for the current shipment state.

## 8.4 Shipper interface

Build a dedicated Shipper delivery workspace.

Expected pages/components:

1. Shipper Dashboard.
2. My Assigned Shipments.
3. Today's Deliveries.
4. Available Assignments if supported.
5. Delivery Details.
6. Pickup Confirmation.
7. Start Delivery.
8. Complete Delivery.
9. Failed Delivery Report.
10. Delivery Proof Submission.
11. Delivery Retry.
12. Return Handling.
13. COD Collection.
14. COD History.
15. COD Settlement.
16. Delivery Statistics.
17. Availability Settings.
18. Shipper Profile.

The interface should prioritize mobile usability because Shippers commonly operate using phones.

Include:

- Large touch-friendly action buttons.
- Clear delivery status.
- Recipient information.
- Address information.
- Tracking information.
- COD amount.
- Failure reason selection.
- Proof upload where supported.
- Clear confirmation before irreversible actions.

Do not allow the Shipper to operate on shipments assigned to another Shipper.

## 8.5 Admin shipping interface

Build a complete Admin shipping management area.

Expected pages/components:

1. Shipping Overview Dashboard.
2. All Shipments.
3. Shipment Search.
4. Pending Shipments.
5. In-Transit Shipments.
6. Delivering Shipments.
7. Failed Shipments.
8. Return Shipments.
9. Unassigned Shipments.
10. Shipper Management.
11. Shipper Details.
12. Shipper Statistics.
13. Assignment and Reassignment.
14. COD Settlement Management.
15. Shipping Statistics.
16. Shipping Fee Management.
17. Audit Logs.

The Admin dashboard should display real statistics from Backend APIs.

Useful metrics:

- Total shipments.
- Pending shipments.
- In-transit shipments.
- Delivering shipments.
- Delivered shipments.
- Failed deliveries.
- Return shipments.
- Unassigned shipments.
- Delivery success rate.
- Average delivery time.
- COD awaiting handover.

Do not invent statistics or display hardcoded numbers.

## 8.6 Shared UI components

Create or reuse reusable components where appropriate:

- ShipmentStatusBadge.
- TrackingTimeline.
- ShipmentCard.
- ShipmentTable.
- ShipmentFilters.
- TrackingNumberSearch.
- ShipmentDetailPanel.
- DeliveryAttemptList.
- ReturnStatusPanel.
- CodSummaryCard.
- ShipperCard.
- ShipperAssignmentDialog.
- DeliveryProofUploader.
- AddressInformation.
- ShippingFeeDisplay.
- Pagination.
- LoadingSkeleton.
- EmptyState.
- ErrorState.
- ConfirmationDialog.

Do not duplicate existing shared components.

## 8.7 Frontend API integration

- Use real Backend APIs.
- Follow existing API client conventions.
- Reuse existing authentication token handling.
- Handle expired sessions correctly.
- Handle loading, success and error states.
- Use appropriate request and response types.
- Avoid `any` when meaningful TypeScript types can be defined.
- Do not put business logic or authorization decisions exclusively in Frontend.
- Refresh or invalidate relevant data after mutations.
- Prevent duplicate submissions.
- Show success messages only after successful API responses.
- Display meaningful Backend error messages.

Every visible button, form, filter, table action, navigation link and dialog must work correctly.

---

# 9. FRONTEND ROUTING AND ROLE-BASED ACCESS

Inspect existing routing before adding routes.

Integrate ShippingService pages into the existing EcomSystem navigation.

Use the project's existing route naming and layout conventions.

Expected navigation separation:

### Buyer

- My Orders / My Shipments.
- Track Shipment.
- Shipment Details.

### Seller

- Shipping Dashboard.
- Manage Shipments.
- Pending Shipments.
- Ready for Pickup.
- Failed Deliveries.
- Returns.
- Shipping Statistics.

### Shipper

- Delivery Dashboard.
- Assigned Shipments.
- Today's Deliveries.
- COD Collection.
- COD History.
- Availability.
- Profile.

### Admin

- Shipping Overview.
- All Shipments.
- Shipment Assignment.
- Shipper Management.
- COD Settlements.
- Shipping Fees.
- Shipping Reports.
- Audit Logs.

Do not show navigation items to roles that cannot access them.

However, remember that Frontend route protection is not a substitute for Backend authorization.

---

# 10. API AND FRONTEND INTEGRATION

After implementing the Backend and Frontend:

1. Connect every implemented page to the correct API.
2. Verify all API base URLs.
3. Verify Vite proxy configuration.
4. Verify CORS configuration.
5. Verify JWT authentication.
6. Verify role-based navigation.
7. Verify request and response DTO compatibility.
8. Verify pagination and filtering.
9. Verify API error handling.
10. Verify that Backend changes do not break other services.

Check the existing environment variables and Docker configuration.

Do not arbitrarily change service ports or connection strings.

---

# 11. TESTING REQUIREMENTS

Testing is mandatory.

Do not finish after writing code.

## 11.1 Backend testing

Build the complete Backend solution.

Run existing tests and add tests for critical ShippingService functionality.

At minimum, verify:

- Shipment creation.
- Duplicate shipment prevention.
- Tracking number uniqueness.
- Shipment ownership validation.
- Buyer authorization.
- Seller authorization.
- Shipper assignment authorization.
- Admin authorization.
- Internal API authorization.
- Valid shipment transitions.
- Invalid shipment transitions.
- Concurrent Shipper assignment.
- Duplicate acceptance prevention.
- Delivery attempt creation.
- Failed delivery handling.
- Return workflow.
- COD amount validation.
- Duplicate COD collection prevention.
- Pagination and filtering.
- Repository persistence.
- RabbitMQ event processing if implemented.

Use realistic test scenarios.

Do not claim that a feature works simply because its handler compiles.

## 11.2 Frontend testing

Build the Frontend.

Verify:

- All ShippingService pages load.
- Navigation works.
- Role-based menus work.
- API requests work.
- Loading states work.
- Empty states work.
- Error states work.
- Forms validate correctly.
- Filters work.
- Pagination works.
- Shipment detail navigation works.
- Tracking timeline renders correctly.
- Seller actions work.
- Shipper actions work.
- Admin assignment works.
- COD screens work.
- Responsive layout works.

Fix console errors, failed requests and broken interactions.

## 11.3 End-to-end testing

Test realistic workflows wherever the available environment and data allow:

### Scenario A — Seller preparation

1. Create or use an eligible shipment.
2. Log in as Seller.
3. Open pending shipments.
4. Prepare the package.
5. Mark it ready for pickup.
6. Confirm handover.
7. Verify persisted shipment state and tracking history.

### Scenario B — Shipper delivery

1. Log in as Admin.
2. Assign an available Shipper.
3. Log in as that Shipper.
4. Accept the assignment.
5. Confirm pickup.
6. Start delivery.
7. Complete delivery.
8. Verify the tracking timeline and shipment state.

### Scenario C — Failed delivery

1. Use an eligible shipment.
2. Start delivery.
3. Record a failed attempt.
4. Verify failure reason and history.
5. Retry delivery if allowed.
6. Verify the state transition.

### Scenario D — Buyer tracking

1. Log in as the shipment owner.
2. Open My Shipments.
3. Open shipment details.
4. View tracking timeline.
5. Verify shipment ownership restrictions.

### Scenario E — COD

1. Use a valid COD shipment.
2. Verify the expected amount.
3. Record COD collection.
4. Verify duplicate collection is rejected.
5. Verify PaymentService integration when the environment supports it.

### Scenario F — Return

1. Use a shipment eligible for return.
2. Create and process a return request.
3. Record return pickup.
4. Complete return.
5. Verify return history and internal notifications/events.

If a workflow cannot be tested because another service, external provider, or test account is unavailable, clearly identify the blocker. Do not fabricate a successful result.

---

# 12. DOCKER — MANDATORY FINAL VERIFICATION

The EcomSystem project already has Docker support for the entire system.

You MUST respect the existing Docker Compose configuration.

## Critical requirement

**Whenever you modify C# Backend code or Frontend code, after completing those changes, run:**

```bash
docker compose up --build
```

Run the command from the actual project root containing the correct `docker-compose.yml` or `compose.yaml`.

This is mandatory.

Do not stop at:

- Writing source code.
- Building only one project.
- Running `dotnet build` alone.
- Running `npm run build` alone.
- Checking code statically.

You must rebuild and run the actual Docker environment after Backend or Frontend code changes.

Then inspect:

- Docker build output.
- Container startup.
- Container status.
- Backend logs.
- Frontend logs.
- Database connectivity.
- Service-to-service communication.
- RabbitMQ connectivity if used.
- Redis connectivity if used.
- API health endpoints.
- Actual running Frontend.

Open the running Frontend and inspect the ShippingService pages.

Fix problems found during actual execution.

If further C# or Frontend changes are necessary, run:

```bash
docker compose up --build
```

again after those changes.

### For ordinary testing without code changes

Use:

```bash
docker compose up
```

### Data safety

NEVER run:

```bash
docker compose down -v
```

Do not:

- Delete Docker volumes.
- Delete existing databases.
- Remove persisted user data.
- Reset SQL Server data.
- Remove unrelated service containers or images unnecessarily.
- Change existing database credentials or ports without a justified reason.

Preserve the existing EcomSystem environment and all existing data.

If a container has already been started, inspect its state and logs before deciding what to do next.

If Docker build or startup fails, investigate and fix the actual cause rather than hiding or ignoring the error.

If resource or network limitations prevent successful execution, report the exact limitation honestly.

---

# 13. QUALITY REQUIREMENTS

The completed ShippingService must satisfy the following:

## Backend quality

- Correct Clean Architecture.
- Correct CQRS/MediatR usage.
- Correct entity relationships.
- Correct database persistence.
- Proper validation.
- Correct authorization.
- Secure ownership checks.
- Valid state transitions.
- Concurrency-safe assignments.
- Idempotent critical operations.
- Reliable integration events where needed.
- Correct microservice boundaries.
- No duplicated business logic.
- No unnecessary abstractions.
- No fake integrations.
- No destructive database changes.

## Frontend quality

- Professional EcomSystem branding.
- Shopee-inspired usability.
- Attractive Buyer experience.
- Efficient Seller management.
- Mobile-friendly Shipper experience.
- Comprehensive Admin dashboard.
- Correct API integration.
- Working interactions.
- Responsive design.
- Consistent components.
- No fake data.
- No dead buttons.
- No broken navigation.
- No console errors that affect functionality.

---

# 14. IMPLEMENTATION PRIORITY

Do not sacrifice correctness to implement advanced features quickly.

Prioritize the work in this order:

### Phase 1 — Core shipment functionality

- Shipment creation.
- Shipment details.
- Shipment status state machine.
- Tracking number.
- Tracking timeline.
- Buyer shipment access.
- Seller shipment access.
- Seller package preparation.
- Basic Shipper assignment.
- Pickup and delivery.
- Core Admin management.

### Phase 2 — Complete delivery operations

- Shipper dashboard.
- Assigned shipments.
- Delivery attempts.
- Failed delivery.
- Retry.
- Return workflow.
- Delivery proof.
- Buyer delivery instructions.
- Delivery issue reporting.

### Phase 3 — COD and operational management

- COD collection.
- COD history.
- COD settlements.
- Shipper availability.
- Shipper statistics.
- Admin COD management.
- Shipping statistics.

### Phase 4 — Shipping methods and advanced features

- Shipping fee calculation.
- Shipping methods.
- Provider abstraction.
- Estimated delivery.
- GPS tracking if supported.
- Advanced reporting.
- Audit logs.
- Additional integrations.

All required API contracts must be reviewed, but do not build fake or unsupported functionality just to claim that every item has been implemented.

If an API is inappropriate for the existing architecture, consolidate it or document the reason and implement the correct equivalent.

---

# 15. FINAL ACCEPTANCE CRITERIA

Do not declare the task completed until you have:

1. Read `docs/AI_RULE.md` completely.
2. Read `docs/ShippingService.md` completely.
3. Inspected existing Backend and Frontend code.
4. Preserved all correct existing implementations.
5. Implemented the required ShippingService Backend functionality.
6. Implemented the corresponding Frontend pages and interactions.
7. Integrated Frontend with real Backend APIs.
8. Verified Buyer, Seller, Shipper and Admin permissions.
9. Tested important shipment workflows.
10. Verified database migrations and persistence where applicable.
11. Built the Backend successfully.
12. Built the Frontend successfully.
13. Run `docker compose up --build` after Backend or Frontend code changes.
14. Inspected actual running containers and logs.
15. Opened and inspected the actual running Frontend.
16. Fixed all discovered critical errors.
17. Confirmed that existing unrelated EcomSystem services and features still work.

## Final report

When finished, provide a clear report containing:

### A. Documentation
- Which documents were read.
- Which existing implementation patterns were reused.

### B. Backend
- New files.
- Modified files.
- Existing files preserved.
- APIs implemented.
- Authorization rules.
- Business workflows completed.
- Database changes and migrations.

### C. Frontend
- New pages and components.
- Modified pages and components.
- Buyer functionality.
- Seller functionality.
- Shipper functionality.
- Admin functionality.
- UI and responsive verification.

### D. Testing
- Backend build result.
- Frontend build result.
- Tests executed and results.
- End-to-end workflows actually tested.
- Any tests that could not be performed and why.

### E. Docker
- Exact Docker command executed.
- Build result.
- Container status.
- Important logs or errors.
- Whether the actual Frontend was accessible.
- Whether Backend APIs responded successfully.

### F. Remaining issues
- Incomplete features.
- External integrations not configured.
- Known limitations.
- Any blockers requiring additional work.

**Be completely honest. Never claim an API, feature, test, Docker build, integration, or UI verification succeeded unless you actually executed and verified it.**

---

# FINAL INSTRUCTION

Work carefully through the existing EcomSystem codebase.

Do not rush. Do not unnecessarily rewrite correct code. Do not remove existing functionality.

Build a complete, reliable ShippingService Backend and a beautiful, professional Shopee-inspired EcomSystem Frontend.

**The task is not finished when the code is written. It is finished only after the code has been tested, Docker has been rebuilt successfully where required, the actual application has been inspected, and discovered problems have been fixed or clearly reported.**