# Notification Acceptance Tests

## Test accounts
- A: `notifA@example.com` / `Password123!`
- B: `notifB@example.com` / `Password123!`

## Results

| # | Test | Result | Evidence |
|---|---|---|---|
| 1 | Docker Compose stack starts | PASS | All containers running; NotificationService, SQL Server, RabbitMQ, web healthy. |
| 2 | NotificationService health | PASS | `GET /api/internal/health` -> 200. |
| 3 | Unauthenticated notification API | PASS | `GET /api/notifications` -> 401. |
| 4 | Unauthenticated SignalR negotiate | PASS | `POST /hubs/notifications/negotiate` -> 401. |
| 5 | CORS for frontend origin | PASS | Preflight -> 204, `Access-Control-Allow-Origin: http://localhost:5173`. |
| 6 | Migrations applied | PASS | 8 tables present including `__EFMigrationsHistory`. |
| 7 | Create notification in SQL | PASS | Internal API persisted row; DB query confirmed. |
| 8 | List own notifications | PASS | `GET /api/notifications` -> 200 with A's rows. |
| 9 | Unread count | PASS | `GET /api/notifications/unread-count` -> real count, not hardcoded. |
| 10 | Mark read | PASS | `PATCH /api/notifications/{id}/read` -> 200; subsequent GET shows `isRead: true`. |
| 11 | Read state persists after refresh | PASS | GET after PATCH still returns `isRead: true`. |
| 12 | Delete own notification | PASS | DELETE -> 200; DB row removed. |
| 13 | User B cannot list A's notifications | PASS | B list -> `[]`. |
| 14 | User B cannot read A's notification | PASS | B detail -> 404. |
| 15 | User B cannot mark A's notification read | PASS | B mark-read -> 404. |
| 16 | User B cannot delete A's notification | PASS | B delete -> 404. |
| 17 | SignalR authenticated connection | PASS | .NET SignalR client connected with JWT. |
| 18 | SignalR real-time delivery | PASS | Client received `ReceiveNotification` after RabbitMQ publish. |
| 19 | SignalR cross-user isolation | PASS | B client timed out when A's event was published. |
| 20 | SignalR reconnect | PASS | Reconnect handler invokes `JoinGroup`. |
| 21 | RabbitMQ consumer persists notification | PASS | Consumer log received message; SQL insert succeeded. |
| 22 | RabbitMQ end-to-end from OrderService | BLOCKED | Message published directly to default exchange; full order creation E2E not run. |
| 23 | Browser bell visible | BLOCKED | No browser automation available. |
| 24 | Browser dropdown opens | BLOCKED | No browser automation available. |
| 25 | Browser unread badge | BLOCKED | No browser automation available. |
| 26 | Browser console/network errors | BLOCKED | No browser automation available. |
| 27 | External email/push/SMS delivery | BLOCKED | No provider configured. |

## Notes
- The SignalR test client was a .NET console app using the same JWT and hub endpoint as the browser.
- RabbitMQ flow was verified by publishing an `OrderCreated` message directly to the default exchange.
- PaymentService was temporarily stopped during one RabbitMQ test because its manual-ack consumer starved the shared `OrderCreated` queue. It was restarted afterward.
- Test notifications created during verification remain in `NotificationServiceDb`; no existing data was deleted.