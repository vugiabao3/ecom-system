# NotificationService Audit Report

## Scope
Runtime verification of the notification bell, NotificationService APIs, SQL persistence, SignalR delivery, RabbitMQ consumption, and two-user isolation.

## Environment
- Docker Compose stack: all services running.
- NotificationService: `http://localhost:5039`
- Frontend: `http://localhost:5173`
- SQL Server: `sqlserver:1433`, database `NotificationServiceDb`
- RabbitMQ: `http://localhost:15672`

## Fixes applied during verification
- Added missing JWT bearer authentication to `NotificationService.Api/Program.cs`.
- Moved `NotificationsHub` to Infrastructure and added per-user SignalR routing.
- Started RabbitMQ consumers after application build; consumers were registered but never started.
- Added missing `Title` values in consumers (SQL insert failed with error 515 before this fix).
- Added ownership checks to mark-read and delete endpoints.
- Replaced hardcoded unread count and summary with real database queries.
- Added SignalR reconnection group rejoin.
- Fixed literal `\n` text and duplicate bell rendering in `Navbar.tsx`.
- Added `/notifications` notification center page and View All link in the bell.

## Verification results

### Docker / process
- PASS: `docker compose ps` shows NotificationService, SQL Server, RabbitMQ, and web running.
- PASS: NotificationService health returns HTTP 200.
- PASS: unauthenticated notification API returns HTTP 401.
- PASS: unauthenticated SignalR negotiate returns HTTP 401.
- PASS: CORS preflight for `http://localhost:5173` returns HTTP 204 with `Access-Control-Allow-Origin`.

### SQL migrations and persistence
- PASS: `NotificationServiceDb` contains `Notifications`, `NotificationPreferences`, `NotificationDevices`, `NotificationTemplates`, `NotificationCampaigns`, `EventInboxes`, `QuietHours`, `__EFMigrationsHistory`.
- PASS: migrations were applied with an idempotent SQL script executed inside the SQL Server container. Host EF migration login failed with `Login failed for user 'sa'`, so the script path was used.
- PASS: notifications are persisted and readable through the API.
- PASS: mark-as-read updates `IsRead` and persists after a subsequent GET.
- Current data: 29 notifications, 27 unread after test reads.

### Notification APIs
- PASS: `GET /api/notifications` returns the authenticated user's notifications only.
- PASS: `GET /api/notifications/unread-count` returns the real count.
- PASS: `GET /api/notifications/{id}` returns the owner's notification.
- PASS: `PATCH /api/notifications/{id}/read` returns HTTP 200 for the owner.
- PASS: `DELETE /api/notifications/{id}` returns HTTP 200 for the owner and removes the row.
- PASS: another user receives HTTP 404 for detail, mark-read, and delete.
- PASS: `/api/internal/health` is anonymous and returns HTTP 200.

### SignalR
- PASS: JWT-authenticated .NET SignalR client connects to `/hubs/notifications`.
- PASS: authenticated user id is extracted and used for per-user routing.
- PASS: a notification published to RabbitMQ reaches the connected browser-equivalent client without refresh.
- PASS: user B connected with its own token does not receive user A's notification.
- PASS: automatic reconnect handler calls `JoinGroup`.
- BLOCKED: real browser Network/WebSocket inspection was not possible in this environment.
- BLOCKED: browser console error check was not possible.

### RabbitMQ
- PASS: queues exist with active consumers.
- PASS: `OrderCreated` message is consumed by NotificationService, persisted to SQL, and pushed through SignalR.
- PARTIAL: the event was published directly to the default exchange, not triggered end-to-end by OrderService. The OrderService publisher exists but a full order-creation E2E test was not executed.
- FAIL/BLOCKED: the `OrderCreated` queue is shared by multiple services. With PaymentService running, its manual-ack consumer can receive messages without acking and starve the NotificationService consumer. The SignalR flow was verified after temporarily stopping PaymentService.
- FAIL: several consumers create notifications with `UserId = null` because their event DTOs do not contain a recipient user id. These notifications cannot be delivered to a specific user.
- NOT TESTED: PaymentSucceeded, PaymentFailed, ShippingCreated, DeliveryFailed, DeliverySucceeded, and ReturnOrder end-to-end event flows.

### Two-user isolation
- PASS: user A sees only its notifications.
- PASS: user B receives an empty list.
- PASS: user B cannot read, mark-read, or delete user A's notification.
- PASS: user B does not receive user A's SignalR notification.

### Frontend
- PASS: `npm run build` succeeds.
- PASS: NotificationBell is imported and rendered in the authenticated Navbar.
- PASS: bell always renders regardless of unread count.
- PASS: View All navigates to `/notifications`.
- BLOCKED: actual browser rendering, dropdown click, console errors, and live badge behavior were not verified.
- BLOCKED: no real login was performed in a browser; API behavior was verified with tokens obtained from AuthService.

## Remaining issues
1. Shared RabbitMQ queues across services cause consumer starvation. NotificationService should use dedicated queues or an exchange with per-service bindings.
2. Most event DTOs lack `UserId`; those consumers persist unusable null-recipient notifications.
3. Browser E2E verification is still missing.
4. External email/push/SMS providers are not configured; no external delivery is claimed.
5. Host EF migrations cannot connect to SQL Server with `sa`; migrations must be applied via SQL script or a working host connection.
6. Other NotificationService endpoints still contain scaffold `Ok(...)` responses and need full handler/service implementation.