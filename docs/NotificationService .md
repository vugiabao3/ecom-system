# NOTIFICATION SERVICE — COMPLETE BACKEND & FRONTEND IMPLEMENTATION

## 1. MAIN OBJECTIVE

You are working on the **EcomSystem** project, a microservices-based e-commerce platform.

Your task is to thoroughly inspect, complete, improve, and implement the entire **NotificationService Backend and Frontend** according to:

- `docs/AI_RULE.md`
- `docs/NotificationService.md`
- Existing EcomSystem architecture.
- Existing Backend and Frontend coding conventions.
- Existing authentication, authorization, API integration, RabbitMQ, Redis, and SignalR implementations.
- All API requirements specified in this prompt.

The objective is to deliver a complete, reliable, secure, real-time notification system for Buyer, Seller, Shipper, and Admin.

The user experience should be inspired by Shopee's notification experience, with a beautiful notification bell, unread badge, notification center, categorized notifications, real-time updates, and notification preferences.

However, the interface must maintain **EcomSystem's own visual identity and branding**. Do not copy Shopee's logo, assets, or exact design.

The Backend must follow the existing Clean Architecture, CQRS/MediatR, Entity Framework Core, RabbitMQ, SignalR, JWT authentication, and microservice boundaries.

**Do not stop after writing code. Build, run, test, inspect the actual application, fix discovered issues, and verify the complete workflow.**

---

# 2. MANDATORY RULES — READ BEFORE CODING

## 2.1 Read documentation first

Before making any modifications:

1. Read the entire `docs/AI_RULE.md`.
2. Read the entire `docs/NotificationService.md`.
3. Inspect the complete existing NotificationService Backend.
4. Inspect the existing React Frontend.
5. Inspect existing notification-related components, layouts, pages, API clients, hooks, and routing.
6. Inspect existing authentication and JWT handling.
7. Inspect existing RabbitMQ exchanges, queues, consumers, publishers, and event contracts.
8. Inspect existing SignalR hubs and connection-management patterns.
9. Inspect Redis usage if present.
10. Inspect Docker Compose and existing service configuration.

Understand the existing implementation before changing anything.

## 2.2 Preserve existing correct code

This requirement is extremely important.

- Do NOT delete existing files unnecessarily.
- Do NOT rewrite entire controllers, handlers, services, repositories, or React components when the existing implementation is already correct.
- Do NOT remove working features.
- Do NOT replace correct implementations with simplified code.
- Do NOT create duplicate entities, DTOs, interfaces, commands, queries, services, or API routes unnecessarily.
- Reuse existing code whenever possible.
- Follow existing naming conventions, project structure, and coding style.
- Modify only the parts that genuinely need improvement or completion.
- Add new files only when they are actually required.
- Preserve existing API contracts when other services already depend on them.
- Ensure changes do not break unrelated EcomSystem services or Frontend functionality.

**The objective is to complete and improve the existing project, not rebuild it from scratch.**

If an existing feature works correctly, preserve it.

If an existing implementation is incomplete, incorrect, insecure, or inconsistent with the required architecture, fix only the necessary parts.

## 2.3 No fake implementations

Do NOT:

- Display hardcoded notifications as real database records.
- Pretend RabbitMQ events were successfully processed when they were not.
- Simulate real-time notifications without a working SignalR connection.
- Display fake unread counts.
- Show success messages before API operations succeed.
- Create buttons that do nothing.
- Use mock API responses as the final implementation.
- Pretend push notifications work without an actual push provider.
- Claim an email, SMS, or mobile push was delivered when no provider is configured.
- Ignore API errors or silently swallow exceptions.

Every implemented feature must use real Backend APIs and persistent data.

If an external delivery channel is not configured, build a proper extensible integration boundary and clearly report the limitation.

---

# 3. PROJECT ARCHITECTURE AND TECHNOLOGY

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
- RabbitMQ
- SignalR
- Redis where justified
- Swagger / OpenAPI
- Docker and Docker Compose
- Structured logging and exception handling

Follow the existing project structure, such as:

- NotificationService.Api
- NotificationService.Application
- NotificationService.Domain
- NotificationService.Infrastructure

Do not force a new architecture if the existing project already has a valid structure.

## Frontend

Expected technologies:

- React
- TypeScript
- Vite
- Existing routing solution
- Existing API client
- Existing authentication mechanism
- Existing UI components and styling conventions

Do not introduce unnecessary libraries.

---

# 4. NOTIFICATION SERVICE RESPONSIBILITIES

NotificationService is responsible for:

1. Persisting notifications.
2. Managing personal notifications.
3. Managing unread and read states.
4. Managing notification preferences.
5. Managing notification categories and channels.
6. Managing registered notification devices.
7. Receiving internal notification requests.
8. Consuming business events from RabbitMQ.
9. Rendering notification templates.
10. Managing notification campaigns.
11. Managing delivery attempts.
12. Handling retries and failed messages.
13. Sending real-time notifications through SignalR.
14. Exposing notification statistics and operational monitoring.

## 4.1 Strict microservice boundaries

Respect the following ownership:

| Service | Responsibility |
|---|---|
| AuthService | Authentication, JWT, credentials and identity |
| UserService | User profile and account information |
| OrderService | Order information and order lifecycle |
| PaymentService | Payment and refund events |
| ProductService | Product information |
| PromotionService | Promotions and voucher business rules |
| ShippingService | Shipment and delivery events |
| NotificationService | Notification persistence, preferences, delivery and real-time communication |

NotificationService must not become the owner of another service's business data.

For example:

- It may receive `OrderCreated`, but it must not create or modify an order.
- It may receive `PaymentCompleted`, but it must not process payments.
- It may receive `ShipmentDelivered`, but it must not modify shipment records.
- It may receive `PromotionCreated`, but it must not create promotion rules.
- It may send a notification about a refund, but it must not process the refund.

Use events or authenticated internal APIs to obtain the necessary business information.

Avoid direct cross-service database access.

## 4.2 Notification persistence versus delivery

Understand this distinction:

**A notification is a persistent user-facing record. A delivery is an attempt to send that notification through a specific channel.**

For example:

A single notification may have:

- An in-app notification record.
- A SignalR real-time delivery attempt.
- An email delivery attempt.
- A push notification delivery attempt.

Do not confuse these records.

A successful SignalR send does not necessarily mean the user has read the notification.

A user reading an in-app notification does not necessarily mean an email was delivered.

Use appropriate domain models and status fields.

---

# 5. COMPLETE BACKEND API REQUIREMENTS

Implement the following API requirements.

First inspect existing controllers and endpoints.

Reuse existing routes when they already provide the correct behavior.

Do not create duplicate endpoints unnecessarily. If existing endpoints have different names but provide the same correct functionality, consolidate them when appropriate and preserve compatibility for existing consumers.

Every API must have:

- Correct HTTP method and route.
- Appropriate request and response DTOs.
- Input validation.
- Authorization.
- Correct ownership checks.
- Correct database operations.
- Consistent error handling.
- Pagination for list endpoints.
- Structured logging where appropriate.
- Tests for important business behavior.

## GROUP 1 — Personal Notification Management

1. `GET /api/notifications` — Get current user's notifications with pagination.
2. `GET /api/notifications/{id}` — Get notification details.
3. `GET /api/notifications/unread` — Get unread notifications.
4. `GET /api/notifications/unread-count` — Get unread count for the notification badge.
5. `GET /api/notifications/summary` — Get total, unread, and read counts.
6. `PATCH /api/notifications/{id}/read` — Mark one notification as read.
7. `POST /api/notifications/read-all` — Mark all current user's notifications as read.
8. `DELETE /api/notifications/{id}` — Delete or hide one personal notification.
9. `POST /api/notifications/delete-batch` — Delete selected personal notifications.
10. `GET /api/notifications/by-type?type=Order` — Filter by notification type.
11. `GET /api/notifications/by-category?category=Promotion` — Filter by category.
12. `GET /api/notifications/history` — Get notification history.

Important security requirements:

- Every personal query must be scoped to the authenticated UserId.
- Derive UserId from JWT claims.
- Never trust a UserId supplied by the Frontend.
- A user must never read, mark as read, or delete another user's notification.
- Do not expose internal notification metadata to ordinary users.
- Use pagination and sensible maximum page sizes.
- Ensure unread count is calculated correctly.
- Deleting a notification must not accidentally delete the original business event.

### Notification categories

Use a consistent category system, for example:

- Order.
- Payment.
- Shipping.
- Promotion.
- Voucher.
- Account.
- Security.
- System.
- Message.
- Review.

Use the actual project's existing enum or classification system if one already exists.

### Notification types

Notification types should distinguish meaningful events, for example:

- OrderCreated.
- OrderConfirmed.
- OrderCancelled.
- PaymentSuccessful.
- PaymentFailed.
- RefundProcessed.
- ShipmentCreated.
- ShipmentAssigned.
- ShipmentInTransit.
- ShipmentDelivered.
- ShipmentDeliveryFailed.
- VoucherReceived.
- PromotionStarted.
- AccountSecurityAlert.
- NewMessage.
- ReviewResponseReceived.

Do not create dozens of duplicate types when category and metadata can express the difference.

## GROUP 2 — Notification Preferences

13. `GET /api/notifications/preferences` — Get user notification settings.
14. `PUT /api/notifications/preferences` — Update all notification preferences.
15. `PATCH /api/notifications/preferences/channels` — Enable or disable delivery channels.
16. `PATCH /api/notifications/preferences/categories` — Enable or disable notification categories.
17. `PUT /api/notifications/preferences/quiet-hours` — Configure Do Not Disturb hours.
18. `GET /api/notifications/preferences/channels` — Get supported channels.
19. `POST /api/notifications/unsubscribe` — Unsubscribe from an optional notification category or subscription.

Preferences should support relevant options:

- In-app notifications.
- Email notifications, if configured.
- Push notifications, if configured.
- SMS notifications, if configured.
- Order updates.
- Payment updates.
- Shipping updates.
- Promotions.
- Voucher notifications.
- Account and security notifications.
- Quiet hours.

Important:

- Do not allow users to disable mandatory security notifications if business policy requires them.
- Distinguish transactional notifications from marketing notifications.
- Respect user consent and applicable unsubscribe requirements.
- Validate quiet-hours start and end times.
- Handle time zones consistently.
- Do not silently discard important transactional events because a marketing preference is disabled.

## GROUP 3 — Device and Push Token Management

20. `GET /api/notifications/devices` — Get registered devices.
21. `POST /api/notifications/devices` — Register a notification device.
22. `DELETE /api/notifications/devices/{deviceId}` — Unregister a device.
23. `POST /api/notifications/devices/{deviceId}/test` — Send a test notification.
24. `POST /api/notifications/push-tokens` — Register or update a push token.
25. `DELETE /api/notifications/push-tokens/{tokenId}` — Remove a push token.

Security requirements:

- Bind registered devices to the authenticated user.
- Do not expose push tokens in ordinary API responses.
- Avoid duplicate device registrations.
- Handle token rotation.
- Validate token ownership before deletion.
- Do not claim that push delivery works if no push provider is configured.
- Keep device metadata minimal and protect sensitive information.

## GROUP 4 — Internal Notification APIs

26. `POST /api/internal/notifications` — Create a notification for one user.
27. `POST /api/internal/notifications/batch` — Create notifications for multiple users.
28. `POST /api/internal/notifications/broadcast` — Create notifications for a defined audience.
29. `POST /api/internal/events` — Receive a business event.
30. `POST /api/internal/events/batch` — Receive multiple business events.
31. `GET /api/internal/notifications/{id}` — Get notification details for internal services.
32. `GET /api/internal/notifications/users/{userId}/summary` — Get a user's notification summary.
33. `GET /api/internal/notifications/statistics` — Get internal notification statistics.

Important:

- Internal APIs must use service-to-service authentication.
- Do not expose them to normal Buyer, Seller, or Shipper sessions.
- Validate event source and event type.
- Validate recipient IDs and audience definitions.
- Protect batch endpoints against oversized payloads.
- Implement idempotency.
- Prevent duplicate notifications from repeated events.
- Do not implement unrestricted public broadcast.
- Every broadcast must have an explicitly defined and authorized audience.
- Use existing internal authentication conventions.

## GROUP 5 — Notification Template Management

34. `POST /api/admin/notification-templates` — Create a notification template.
35. `GET /api/admin/notification-templates` — List templates.
36. `GET /api/admin/notification-templates/{id}` — Get template details.
37. `PUT /api/admin/notification-templates/{id}` — Update a template.
38. `DELETE /api/admin/notification-templates/{id}` — Delete or deactivate a template according to policy.
39. `POST /api/admin/notification-templates/{id}/preview` — Preview rendered content with variables.
40. `PATCH /api/admin/notification-templates/{id}/status` — Enable or disable a template.

Templates should support:

- Template name.
- Event type.
- Category.
- Title template.
- Body template.
- Supported channels.
- Language or locale if required.
- Active status.
- CreatedAt and UpdatedAt.

Important:

- Validate template variables.
- Prevent unsafe template evaluation.
- Do not execute arbitrary code from templates.
- Handle missing variables safely.
- Do not allow unauthorized users to create or modify templates.
- Preserve historical delivery records if a template is changed or deactivated.

## GROUP 6 — Admin Notification Management

41. `GET /api/admin/notifications` — View all notification records.
42. `GET /api/admin/notifications/{id}` — View notification details.
43. `GET /api/admin/notifications/search` — Search notifications.
44. `GET /api/admin/notifications/statistics` — Notification statistics.
45. `GET /api/admin/notifications/failed` — View failed notifications.
46. `POST /api/admin/notifications/{id}/retry` — Retry eligible failed notification delivery.
47. `POST /api/admin/notifications/broadcast` — Send a system announcement.
48. `POST /api/admin/notifications/{id}/cancel` — Cancel a pending notification where supported.
49. `DELETE /api/admin/notifications/{id}` — Hide or delete a notification according to retention policy.
50. `GET /api/admin/notifications/delivery-statistics` — Delivery success and failure statistics.

Admin features must include:

- Search.
- Filters.
- Pagination.
- Notification status.
- Category.
- Event type.
- Recipient.
- Creation time.
- Delivery status.
- Failure reason.
- Retry history.

Do not expose private notification content unnecessarily to unauthorized Admin users.

Administrative access must be authorized and auditable.

## GROUP 7 — Notification Campaigns

51. `POST /api/admin/notification-campaigns` — Create a campaign.
52. `GET /api/admin/notification-campaigns` — List campaigns.
53. `GET /api/admin/notification-campaigns/{id}` — Campaign details.
54. `PUT /api/admin/notification-campaigns/{id}` — Update campaign.
55. `POST /api/admin/notification-campaigns/{id}/schedule` — Schedule campaign.
56. `POST /api/admin/notification-campaigns/{id}/send` — Start campaign.
57. `POST /api/admin/notification-campaigns/{id}/cancel` — Cancel campaign.

Campaigns may support:

- Campaign name.
- Description.
- Template.
- Audience.
- Target categories or roles.
- Selected channels.
- Scheduled time.
- Campaign status.
- Delivery progress.
- Success/failure statistics.

Requirements:

- Validate audience selection.
- Avoid sending duplicate campaign notifications.
- Respect marketing consent and notification preferences.
- Support cancellation before delivery where possible.
- Do not send an unrestricted broadcast to all users by default.
- Use background processing for large campaigns.
- Do not block HTTP requests while sending large batches.

## GROUP 8 — Notification Delivery Management

58. `GET /api/admin/notification-deliveries` — List delivery attempts.
59. `GET /api/admin/notification-deliveries/{id}` — Get delivery details.
60. `GET /api/admin/notification-deliveries/failed` — List failed delivery attempts.
61. `POST /api/admin/notification-deliveries/{id}/retry` — Retry one eligible delivery.
62. `GET /api/admin/notification-deliveries/{id}/history` — Get delivery attempt history.
63. `GET /api/admin/notification-deliveries/statistics` — Delivery statistics.
64. `POST /api/admin/notification-deliveries/retry-failed` — Retry eligible failed deliveries.

Distinguish:

- Pending.
- Processing.
- Sent.
- Delivered, only when the channel can confirm delivery.
- Failed.
- Cancelled.
- RetryScheduled.

Do not incorrectly mark a message as delivered merely because it was added to a queue.

Implement retry limits, retry policies, and appropriate failure handling.

Prevent duplicate delivery where possible through idempotency and delivery identifiers.

## GROUP 9 — Internal Monitoring and Queue Operations

65. `GET /api/internal/health` — NotificationService health.
66. `GET /api/internal/queue/statistics` — Queue statistics.
67. `GET /api/internal/queue/failed` — Failed event monitoring.
68. `POST /api/internal/queue/{messageId}/retry` — Retry a failed message.
69. `GET /api/internal/consumers/status` — RabbitMQ consumer status.
70. `GET /api/internal/statistics` — Internal statistics.

These endpoints must be restricted to authorized internal users or Admin operators according to the existing architecture.

Do not expose sensitive infrastructure information publicly.

---

# 6. SIGNALR REAL-TIME NOTIFICATIONS

Implement and verify:

`/hubs/notifications`

This is a SignalR Hub, not a REST endpoint.

The purpose is to push notifications to the correct connected user in real time.

## 6.1 Required real-time behavior

When a new notification is created:

1. Persist the notification in SQL Server.
2. Publish or dispatch the notification through the appropriate processing pipeline.
3. Identify currently connected sessions for the recipient.
4. Push the notification to the correct user through SignalR.
5. Update unread count.
6. Ensure the notification remains persisted if the user is offline.
7. When the user reconnects, fetch persisted notifications through REST APIs.

Possible SignalR events:

- `ReceiveNotification`
- `UnreadCountUpdated`
- `NotificationRead`
- `NotificationDeleted`
- `AllNotificationsRead`
- `NotificationUpdated`

Reuse existing event names if the project already defines them.

## 6.2 SignalR security

- Require authenticated connections.
- Resolve UserId from authenticated claims.
- Use user-specific SignalR groups or the existing secure user-targeting mechanism.
- Never trust a client-supplied UserId when joining a user notification group.
- Prevent one user from joining another user's notification group.
- Handle connection and disconnection.
- Support reconnect behavior.
- Avoid duplicate connections causing inconsistent unread counts.
- Do not expose private notification content to unrelated users.

## 6.3 Real-time consistency

SignalR is a delivery mechanism, not the source of truth.

SQL Server remains the source of truth for persisted notifications and read states.

If a SignalR message is missed because the user is offline or disconnected, the user must still receive the notification through the REST notification list after reconnecting.

Do not rely exclusively on SignalR for notification persistence.

---

# 7. RABBITMQ EVENT-DRIVEN ARCHITECTURE

NotificationService should consume business events from other microservices.

Inspect the existing RabbitMQ configuration and conventions before implementing anything.

Potential event sources:

### OrderService

- OrderCreated.
- OrderConfirmed.
- OrderCancelled.
- OrderStatusChanged.
- OrderCompleted.

### PaymentService

- PaymentSuccessful.
- PaymentFailed.
- RefundRequested.
- RefundCompleted.
- RefundRejected.

### ShippingService

- ShipmentCreated.
- ShipmentAssigned.
- ShipmentPickedUp.
- ShipmentInTransit.
- ShipmentOutForDelivery.
- ShipmentDelivered.
- ShipmentDeliveryFailed.
- ShipmentReturned.

### PromotionService

- PromotionStarted.
- PromotionEndingSoon.
- VoucherReceived.
- VoucherExpiringSoon.

### UserService / AuthService

- AccountCreated.
- AccountUpdated.
- AccountSecurityAlert.
- PasswordChanged.
- AccountBlocked.

### Other services

- NewMessage.
- ReviewCreated.
- SellerRespondedToReview.
- Other meaningful events already defined in EcomSystem.

Do not invent event contracts that conflict with existing service implementations.

Inspect the actual event names, payloads, exchanges, queues, and routing keys.

## 7.1 Event processing requirements

- Use existing RabbitMQ exchanges and queue conventions.
- Deserialize and validate event payloads.
- Persist notifications before reporting successful processing.
- Handle duplicate messages.
- Use idempotent event processing.
- Implement retries and dead-letter handling according to the existing architecture.
- Log processing failures.
- Do not silently acknowledge messages that failed before persistence.
- Avoid generating duplicate notifications after consumer restarts.

Use an Inbox or ProcessedEvent mechanism where appropriate.

If a reliable Outbox pattern already exists in the project, reuse it.

Do not introduce unnecessary new messaging abstractions.

## 7.2 Event idempotency

A business event may be delivered more than once.

For example:

`OrderCreated` may be redelivered after a consumer failure.

NotificationService must not create multiple identical notifications for the same user and event unless explicitly intended.

Use stable event identifiers and suitable unique constraints.

Handle concurrent duplicate processing safely.

## 7.3 Broadcast handling

A broadcast must have a defined audience.

Examples:

- All active users.
- Buyers only.
- Sellers only.
- Shippers only.
- Selected users.
- Users matching a permitted campaign audience.

Do not use `UserId = null` as an unrestricted shortcut that causes every user to receive every notification.

Use an explicit audience model or properly materialized recipient records.

Do not assume that a global notification record automatically provides correct per-user unread counts.

---

# 8. BACKEND DATA MODEL AND DATABASE REQUIREMENTS

Inspect existing entities before creating new ones.

Potential domain models may include:

- Notification.
- NotificationPreference.
- NotificationCategoryPreference.
- NotificationChannelPreference.
- NotificationDevice.
- PushToken.
- NotificationTemplate.
- NotificationCampaign.
- NotificationDelivery.
- NotificationDeliveryAttempt.
- NotificationSubscription.
- NotificationAudience.
- NotificationRecipient.
- ProcessedEvent or InboxMessage.
- AuditLog.

Do not create all these models blindly.

Only add models that are genuinely required by the existing architecture and actual requirements.

## 8.1 Notification entity

A notification may contain:

- Id.
- UserId.
- Type.
- Category.
- Title.
- Content.
- ImageUrl.
- ActionUrl.
- Data JSON.
- IsRead.
- CreatedAt.
- ReadAt.
- SourceEventId.
- ExpirationAt, if required.

Use the existing entity if it already provides these responsibilities.

Important:

- Keep notification content concise.
- Validate ActionUrl.
- Do not allow unsafe external redirects.
- Treat metadata as structured data.
- Avoid storing unnecessary sensitive information.

## 8.2 Delivery entity

A delivery record may contain:

- Id.
- NotificationId.
- Channel.
- RecipientReference.
- Status.
- AttemptCount.
- LastAttemptAt.
- SentAt.
- DeliveredAt, only if confirmed.
- FailureReason.
- ProviderReference.
- CreatedAt.
- UpdatedAt.

Delivery attempts should be auditable.

## 8.3 Database requirements

- Use the existing DbContext.
- Follow EF Core conventions.
- Configure relationships and indexes.
- Add indexes for UserId, CreatedAt, IsRead, Category, Type, and relevant event identifiers.
- Use appropriate unique constraints for idempotency.
- Support pagination efficiently.
- Avoid loading every notification into memory.
- Do not delete existing database records.
- Do not reset the database.
- Do not remove Docker volumes.
- Create proper EF Core migrations if schema changes are required.
- Avoid destructive migrations that remove existing data without a justified and explicitly approved migration plan.

---

# 9. BACKEND SECURITY AND AUTHORIZATION

Expected access boundaries:

| Role | Access |
|---|---|
| Buyer | Own notifications, preferences and devices |
| Seller | Own notifications, preferences and devices |
| Shipper | Own notifications, preferences and devices |
| Admin | Authorized notification administration and monitoring |
| Internal service | Authenticated internal notification operations |

Requirements:

1. Obtain current UserId from JWT.
2. Scope all personal queries by UserId.
3. Validate ownership before marking a notification read.
4. Validate ownership before deleting a notification.
5. Validate ownership before modifying preferences.
6. Validate ownership before removing devices or push tokens.
7. Restrict Admin endpoints.
8. Restrict Internal endpoints separately.
9. Protect broadcast operations.
10. Prevent IDOR vulnerabilities.
11. Validate all request payloads.
12. Never expose push tokens or sensitive internal data unnecessarily.
13. Audit important administrative actions.
14. Rate-limit sensitive endpoints where appropriate.

A user's notification ID must never be sufficient to access another user's notification.

---

# 10. FRONTEND — COMPLETE ECOMSYSTEM NOTIFICATION EXPERIENCE

The Frontend is a major part of this task.

Build a polished, complete notification experience integrated into the existing EcomSystem React application.

Do not build an isolated demo page.

Inspect existing layouts, navigation, authentication, API clients, components, and styling before making changes.

## 10.1 Visual design requirements

Design a modern e-commerce notification interface inspired by Shopee.

Maintain EcomSystem's own brand identity.

Expected quality:

- Professional e-commerce UI.
- Consistent typography.
- Attractive spacing.
- Modern notification cards.
- Clear unread indicators.
- Elegant icons.
- Carefully designed category tabs.
- Responsive layouts.
- Subtle hover states.
- Smooth but restrained transitions.
- Clear loading and empty states.
- Consistent design with the rest of EcomSystem.

Avoid:

- Plain unstyled lists.
- Generic admin templates.
- Excessive gradients.
- Random colors.
- Crowded notification cards.
- Oversized badges.
- Fake unread counts.
- Fake notifications.
- Dead buttons.
- Broken navigation.

## 10.2 Global notification bell

Integrate a notification bell into the existing authenticated application header.

This should be visible to Buyer, Seller, and Shipper, and to Admin where appropriate.

Required behavior:

1. Display a notification bell icon.
2. Display a red unread badge.
3. Fetch unread count from the Backend.
4. Show a compact notification dropdown when clicked.
5. Display the latest notifications.
6. Highlight unread notifications.
7. Allow the user to mark a notification as read.
8. Provide navigation to the full notification center.
9. Update the badge after successful operations.
10. Receive new notifications through SignalR.
11. Show newly received notifications without requiring a page refresh.

Unread badge requirements:

- Display the actual unread count.
- Use `99+` when the count exceeds 99.
- Hide the badge when the count is zero.
- Update after marking notifications read.
- Update after deleting notifications.
- Reconcile with Backend state after reconnect.

Do not hardcode the badge number.

## 10.3 Notification dropdown

Create a polished dropdown/panel containing:

- Latest notifications.
- Notification category.
- Title.
- Short content.
- Timestamp.
- Unread indicator.
- Relevant icon.
- Optional image.
- Action link where appropriate.
- Mark all as read.
- View all notifications.

Clicking a notification should:

1. Mark it as read if necessary.
2. Update the unread count.
3. Navigate to the correct page using its trusted ActionUrl or structured navigation metadata.
4. Avoid unsafe redirects.

The dropdown must handle:

- Loading.
- Empty state.
- API errors.
- Pagination or load-more if appropriate.
- Real-time incoming notifications.

## 10.4 Full notification center

Create a complete notification center.

Expected features:

- All notifications.
- Unread notifications.
- Read notifications.
- Order notifications.
- Payment notifications.
- Shipping notifications.
- Promotion notifications.
- Account notifications.
- System notifications.
- Search where useful.
- Pagination.
- Mark one as read.
- Mark all as read.
- Delete one.
- Select multiple and delete.
- Notification details.
- Navigation to related business pages.

Use Backend filters rather than downloading all records and filtering everything in the browser.

Provide clear visual states:

- Unread.
- Read.
- Recently received.
- Important.
- Expired, if supported.

## 10.5 Notification preferences page

Create a professional settings page.

Expected controls:

- In-app notification toggle.
- Email notification toggle.
- Push notification toggle when supported.
- Order updates.
- Payment updates.
- Shipping updates.
- Promotions.
- Voucher notifications.
- Account and security alerts.
- Quiet hours.
- Timezone where required.
- Unsubscribe from optional categories.

Requirements:

- Fetch actual settings from Backend.
- Save changes through real APIs.
- Provide clear save feedback.
- Handle API failures.
- Do not show unavailable channels as working features.
- Clearly distinguish mandatory security notifications from optional marketing notifications.

## 10.6 Device management page

If the existing application supports device registration, provide:

- Registered device list.
- Device information.
- Registration date.
- Device status.
- Remove device.
- Test notification.

Do not expose actual push tokens in the UI.

If push notification infrastructure is not configured, clearly identify this as unavailable instead of simulating successful delivery.

---

# 11. ADMIN FRONTEND

Create a dedicated Admin notification management area.

Use the existing Admin layout and role-based navigation.

Expected pages:

1. Notification Overview Dashboard.
2. All Notifications.
3. Notification Search.
4. Notification Details.
5. Failed Notifications.
6. Delivery Statistics.
7. Delivery Attempts.
8. Delivery Attempt Details.
9. Retry Failed Deliveries.
10. Notification Templates.
11. Create Template.
12. Edit Template.
13. Template Preview.
14. Notification Campaigns.
15. Create Campaign.
16. Edit Campaign.
17. Campaign Details.
18. Schedule Campaign.
19. Broadcast System Announcement.
20. Queue Monitoring.
21. RabbitMQ Consumer Status.
22. Failed Event Monitoring.
23. Internal Statistics.

## 11.1 Admin dashboard

Display actual Backend statistics:

- Total notifications.
- Unread notifications.
- Read notifications.
- Pending deliveries.
- Successfully sent notifications.
- Failed deliveries.
- Retry count.
- Delivery success rate.
- Active campaigns.
- Queue processing status.

Use real data.

Do not fabricate metrics.

## 11.2 Template management

Admin should be able to:

- View templates.
- Create templates.
- Edit templates.
- Enable or disable templates.
- Preview template output.
- Validate variables.
- View template type and category.

## 11.3 Campaign management

Admin should be able to:

- Create campaigns.
- Select authorized audience.
- Choose notification template.
- Configure delivery channels.
- Schedule campaigns.
- Start eligible campaigns.
- Cancel eligible campaigns.
- View campaign progress.
- View delivery results.

Use confirmation dialogs before sending large broadcasts.

Clearly display the selected audience and expected impact before confirmation.

## 11.4 Delivery monitoring

Admin should be able to:

- Inspect failed deliveries.
- View failure reasons.
- Inspect attempt history.
- Retry eligible deliveries.
- Monitor queue processing.
- View consumer status.

Do not let an Admin retry an ineligible or permanently invalid delivery without a valid recovery path.

---

# 12. REAL-TIME FRONTEND INTEGRATION

Integrate the SignalR client into the existing React application.

Requirements:

- Connect to `/hubs/notifications`.
- Authenticate using the existing JWT mechanism.
- Handle connection failures.
- Handle reconnection.
- Avoid creating duplicate connections.
- Clean up connections when the authenticated session ends.
- Subscribe to the correct user notification events.
- Update React state after receiving events.
- Update the notification bell.
- Update unread counts.
- Update notification dropdown and notification center.
- Handle duplicate SignalR events safely.
- Reconcile state with REST APIs after reconnection.

Do not create a new SignalR connection every time a component renders.

Use an appropriate shared connection provider, context, hook, or existing infrastructure.

Use the existing authentication mechanism and ensure access tokens are not exposed unnecessarily.

If the user logs out, disconnect the authenticated notification connection.

**Important:** REST APIs remain the source of truth. SignalR is for real-time delivery, not permanent storage.

---

# 13. FRONTEND API INTEGRATION

Connect all implemented screens to actual Backend endpoints.

Requirements:

- Reuse existing API clients.
- Reuse existing token handling.
- Verify API base URLs.
- Verify Vite proxy configuration.
- Verify CORS configuration.
- Verify SignalR URL and authentication.
- Use correct TypeScript interfaces.
- Avoid unnecessary `any`.
- Handle loading, empty, success and error states.
- Prevent duplicate submissions.
- Refresh or invalidate data after mutations.
- Display meaningful Backend error messages.
- Do not show success messages before a successful response.

Every visible control must work.

No decorative buttons that imply functionality but do nothing.

---

# 14. RABBITMQ, SIGNALR AND NOTIFICATION PROCESSING FLOW

Implement a coherent end-to-end notification flow.

Expected architecture:

`Business Service → RabbitMQ Event → NotificationService Consumer → Event Validation → Idempotency Check → Persist Notification → Create Delivery Records → SignalR / Other Configured Channels`

A more detailed flow:

1. OrderService, PaymentService, ShippingService, or another service publishes a business event.
2. RabbitMQ delivers the event to NotificationService.
3. NotificationService validates the event.
4. NotificationService checks event idempotency.
5. NotificationService identifies the recipient.
6. NotificationService loads applicable preferences.
7. NotificationService selects the correct template.
8. NotificationService renders notification content.
9. NotificationService persists the notification.
10. NotificationService creates applicable delivery records.
11. NotificationService sends the in-app real-time event through SignalR if the user is connected.
12. Other configured channels are processed through their appropriate providers.
13. Delivery status and failure information are persisted.
14. Retry logic handles eligible failures.

Important:

- A notification must remain available when the user is offline.
- A failed SignalR connection must not delete a persisted notification.
- Duplicate RabbitMQ events must not create duplicate notifications.
- User preferences must be respected.
- Mandatory transactional/security notifications must follow their applicable policy.
- Do not block RabbitMQ consumers indefinitely while calling external delivery providers.
- Use background workers for long-running delivery operations.

---

# 15. TESTING REQUIREMENTS

Testing is mandatory.

Do not finish after the code compiles.

## 15.1 Backend tests

Build the Backend solution.

Run existing tests and add tests for critical NotificationService behavior.

At minimum, verify:

### Personal notifications

- User can retrieve their own notifications.
- User cannot retrieve another user's notifications.
- User can mark their own notification as read.
- User cannot mark another user's notification as read.
- User can delete their own notification.
- User cannot delete another user's notification.
- Unread count is correct.
- Read-all affects only the current user.
- Pagination works.
- Filtering by type and category works.

### Preferences

- User can retrieve preferences.
- User can update preferences.
- User cannot modify another user's preferences.
- Mandatory notification policy is respected.
- Quiet-hours validation works.

### Devices

- Device registration works.
- Duplicate device handling works.
- Device ownership is enforced.
- Push token removal is secure.

### Internal APIs

- Internal authentication is required.
- Public users cannot call internal endpoints.
- Invalid event payloads are rejected.
- Batch size is limited.
- Duplicate events are handled safely.

### RabbitMQ

- Valid events are consumed.
- Notifications are persisted.
- Duplicate events do not create duplicate notifications.
- Consumer failures are logged.
- Retry and dead-letter behavior works where configured.

### SignalR

- Authenticated connection works.
- User receives only their own notifications.
- Unauthenticated connections are rejected.
- Reconnection works.
- Offline notifications remain persisted.
- Duplicate events do not produce inconsistent UI state.

### Admin

- Unauthorized users cannot access Admin endpoints.
- Template CRUD works.
- Template preview works.
- Campaign validation works.
- Delivery retry respects retry policies.
- Queue monitoring returns correct data.

## 15.2 Frontend tests

Build the Frontend.

Verify:

- Notification bell renders.
- Unread badge displays the actual count.
- Dropdown opens and closes.
- Notifications load from Backend.
- Mark as read works.
- Mark all as read works.
- Delete works.
- Batch delete works.
- Category filtering works.
- Pagination works.
- Notification links navigate correctly.
- Preferences save correctly.
- SignalR connection works.
- New notifications appear in real time.
- Reconnection works.
- Admin pages load.
- Template management works.
- Campaign forms validate.
- Delivery monitoring works.
- No broken navigation.
- No dead buttons.
- No critical browser console errors.

## 15.3 End-to-end scenarios

### Scenario A — Order notification

1. Trigger a valid OrderCreated event.
2. Verify RabbitMQ delivers it.
3. Verify NotificationService processes it.
4. Verify notification is persisted.
5. Verify unread count increases.
6. Verify connected Buyer receives SignalR notification.
7. Open notification center.
8. Mark notification as read.
9. Verify unread count decreases.
10. Reload the application.
11. Verify persisted read state remains correct.

### Scenario B — Shipping notification

1. Trigger a valid ShipmentDelivered event.
2. Verify the correct Buyer is identified.
3. Verify a shipping notification is created.
4. Verify the notification has the correct category and navigation metadata.
5. Verify the Buyer can open the related order or shipment page.

### Scenario C — Offline user

1. Disconnect the user's SignalR session.
2. Trigger a valid business event.
3. Verify the notification is persisted.
4. Reconnect the user.
5. Fetch notifications through REST.
6. Verify the notification is available.

### Scenario D — Duplicate event

1. Submit the same event identifier twice.
2. Verify only one intended notification is created.
3. Verify processing is idempotent.

### Scenario E — User isolation

1. Create notifications for two different users.
2. Log in as User A.
3. Attempt to retrieve or modify User B's notification.
4. Verify access is denied or the resource is safely unavailable.
5. Verify User B's data remains unchanged.

### Scenario F — Preferences

1. Disable optional promotional notifications.
2. Trigger a promotional event.
3. Verify applicable preference behavior.
4. Trigger a mandatory transactional or security event.
5. Verify required notifications follow the configured policy.

### Scenario G — Admin delivery retry

1. Trigger an eligible failed delivery.
2. Inspect it in Admin.
3. View its failure reason and attempt history.
4. Retry it.
5. Verify the result is recorded correctly.

If a test requires another service, provider, test account, or unavailable infrastructure, report the actual blocker. Never fabricate a successful test result.

---

# 16. DOCKER — MANDATORY FINAL VERIFICATION

The EcomSystem project already has Docker support for the entire system.

You MUST use the existing Docker Compose configuration.

## 16.1 Mandatory rebuild after code changes

Whenever you modify C# Backend or Frontend code, after completing the changes, execute:

```bash
docker compose up --build
```

Run this from the actual project root containing the correct `docker-compose.yml` or `compose.yaml`.

This is mandatory.

Do not finish after:

- Writing code.
- Running only `dotnet build`.
- Running only `npm run build`.
- Checking code statically.
- Building only NotificationService while ignoring the actual Docker application.

You must rebuild and run the actual EcomSystem Docker environment after Backend or Frontend changes.

Then inspect:

- Docker build output.
- Container startup.
- Container status.
- NotificationService logs.
- Frontend logs.
- SQL Server connectivity.
- RabbitMQ connectivity.
- SignalR connection behavior.
- Redis connectivity if used.
- Other required service connectivity.
- API health endpoints.
- Actual running Frontend.

Open the running Frontend and inspect the notification bell, dropdown, notification center, preferences, and Admin notification pages.

Fix problems discovered during actual execution.

If further C# or Frontend changes are required, execute:

```bash
docker compose up --build
```

again after those changes.

## 16.2 Ordinary testing without code changes

For ordinary testing when no Backend or Frontend code has changed, use:

```bash
docker compose up
```

## 16.3 Data safety

NEVER run:

```bash
docker compose down -v
```

Do not:

- Delete Docker volumes.
- Delete SQL Server databases.
- Reset existing database data.
- Remove persisted user information.
- Delete unrelated containers or images unnecessarily.
- Change existing service ports without a justified reason.
- Change database credentials without a justified reason.

Preserve the entire existing EcomSystem environment.

If a container already exists, inspect its status and logs before deciding what to do.

If Docker fails, investigate and fix the real cause.

If network, disk, or resource limitations prevent successful execution, report the exact problem honestly.

---

# 17. IMPLEMENTATION PRIORITY

Implement the complete service carefully.

Prioritize correctness over unnecessary complexity.

### Phase 1 — Core notification system

- Personal notification persistence.
- Get notification list.
- Unread count.
- Notification summary.
- Mark as read.
- Mark all as read.
- Delete notification.
- Notification categories.
- Notification preferences.
- Authenticated SignalR Hub.
- Global notification bell.
- Notification dropdown.
- Full notification center.

### Phase 2 — Event-driven notification processing

- RabbitMQ event consumers.
- Event validation.
- Event idempotency.
- Notification template rendering.
- Persist notifications from business events.
- Real-time SignalR delivery.
- Offline notification persistence.
- Retry and failure handling.

### Phase 3 — Advanced user features

- Device management.
- Push token management where supported.
- Quiet hours.
- Channel preferences.
- Notification subscriptions.
- Delivery records.
- Delivery history.

### Phase 4 — Admin management

- Admin notification dashboard.
- Notification search.
- Template management.
- Template preview.
- Campaign management.
- Scheduling.
- Broadcast management.
- Delivery monitoring.
- Retry failed deliveries.
- Queue monitoring.
- Consumer status.
- Internal statistics.

Review and implement all 70 required API contracts, but do not create redundant routes or fake integrations just to match the number mechanically.

---

# 18. FINAL ACCEPTANCE CRITERIA

Do not declare the task complete until you have:

1. Read `docs/AI_RULE.md` completely.
2. Read `docs/NotificationService.md` completely.
3. Inspected the existing Backend and Frontend.
4. Preserved all correct existing implementations.
5. Implemented the required NotificationService Backend functionality.
6. Implemented the corresponding Frontend features.
7. Integrated real REST APIs.
8. Integrated the SignalR Hub.
9. Integrated RabbitMQ event processing where supported by the existing architecture.
10. Verified user ownership and authorization.
11. Verified notification preferences.
12. Verified event idempotency.
13. Tested important end-to-end workflows.
14. Verified database migrations and persistence where applicable.
15. Built Backend successfully.
16. Built Frontend successfully.
17. Executed `docker compose up --build` after Backend or Frontend code changes.
18. Inspected actual Docker containers and logs.
19. Opened and inspected the actual running Frontend.
20. Fixed all discovered critical issues.
21. Confirmed unrelated EcomSystem services and features still work.

---

# 19. FINAL REPORT

After implementation, provide a detailed and honest report.

## A. Documentation and architecture

- Documents read.
- Existing architecture inspected.
- Existing implementation patterns reused.
- Existing correct files preserved.

## B. Backend implementation

- New files.
- Modified files.
- Existing files preserved.
- APIs implemented.
- Database entities and migrations.
- Authorization rules.
- RabbitMQ integration.
- SignalR integration.
- Idempotency and retry implementation.

## C. Frontend implementation

- Notification bell.
- Notification dropdown.
- Notification center.
- Unread badge.
- Notification preferences.
- Device management.
- Admin dashboard.
- Template management.
- Campaign management.
- Delivery monitoring.
- Responsive UI verification.

## D. Testing results

- Backend build result.
- Frontend build result.
- Tests executed.
- Test results.
- SignalR tests.
- RabbitMQ tests.
- User isolation tests.
- End-to-end workflows actually tested.
- Tests that could not be performed and their exact blockers.

## E. Docker results

- Exact command executed.
- Build result.
- Container status.
- Important logs.
- API health.
- RabbitMQ status.
- SignalR status.
- Frontend availability.
- Actual UI inspection result.

## F. Remaining issues

- Incomplete features.
- External providers not configured.
- Known limitations.
- Required future work.

**Never claim an API, feature, test, integration, Docker build, or UI verification succeeded unless you actually executed and verified it.**

---

# FINAL INSTRUCTION

Work carefully through the existing EcomSystem codebase.

Do not rush.

Do not delete correct code.

Do not unnecessarily rewrite working features.

Do not create fake integrations.

Build a complete, secure, reliable NotificationService Backend and a beautiful Shopee-inspired EcomSystem notification experience.

The notification bell, unread badge, real-time SignalR delivery, persistent notification records, RabbitMQ event processing, user preferences, and Admin management must work together as one coherent system.

**The task is not finished when the code is written. It is finished only after the application has been built, tested, run through Docker, inspected in the actual Frontend, and all discovered critical issues have been fixed or clearly reported.**