# PAYMENTSERVICE — COMPLETE BACKEND & FRONTEND IMPLEMENTATION

## 1. MAIN OBJECTIVE

You are working on **EcomSystem**, a microservices-based e-commerce platform built with ASP.NET Core and React.

Your task is to thoroughly inspect, implement, improve, and complete the entire **PaymentService**, including both Backend and Frontend.

The final result must be a professional, secure, reliable, and fully integrated payment management system for a multi-vendor e-commerce marketplace.

The UI should be inspired by Shopee's professional e-commerce experience while maintaining a distinctive EcomSystem brand identity.

### Core requirements

- Complete the PaymentService Backend.
- Complete the PaymentService Frontend.
- Implement real payment workflows supported by the existing project.
- Integrate with OrderService, ShippingService, UserService, NotificationService, and other relevant services.
- Implement payment history, transaction management, refunds, COD, payment methods, Seller earnings, and Admin payment management where supported.
- Use the existing architecture and coding conventions.
- Preserve existing correct code and features.
- Do not create fake payment success responses.
- Do not expose sensitive payment information.
- Do not claim a payment is successful until it has been verified by a trusted source.
- Test the actual running application through Docker.

**Do not stop after generating code or creating API endpoints. The complete system must be built, integrated, and tested.**

---

# PART I — MANDATORY DOCUMENTATION AND PROJECT INSPECTION

## 2. READ DOCUMENTATION FIRST

Before modifying anything:

1. Read the entire `docs/AI_RULE.md`.
2. Read the entire `docs/PaymentService.md`.
3. Inspect the complete project structure.
4. Inspect the root Docker Compose configuration.
5. Inspect the existing PaymentService Backend.
6. Inspect the existing PaymentService Frontend.
7. Inspect all existing payment entities, DTOs, commands, queries, handlers, repositories, services, controllers, and configurations.
8. Inspect existing authentication and authorization mechanisms.
9. Inspect the existing payment gateway integrations, if any.
10. Inspect existing RabbitMQ event contracts and consumers.
11. Inspect related microservice documentation and implementations.

Pay special attention to:

- `OrdersController`
- `PaymentActionsController`
- `PaymentsController`
- Existing payment commands and handlers.
- Existing transaction and refund implementations.
- Existing payment status enums.
- Existing gateway configuration.
- Existing JWT and service-to-service authentication.
- Existing Docker and environment configuration.

Do not assume that an API is missing before searching the entire codebase.

If an existing API already implements the required functionality, reuse and improve it instead of creating a duplicate.

---

# PART II — CODE PRESERVATION AND ARCHITECTURE

## 3. CRITICAL CODE PRESERVATION RULES

This is an existing EcomSystem project.

You MUST preserve all correct existing code and functionality.

Rules:

- Reuse existing correct files.
- Reuse existing entities, DTOs, commands, queries, handlers, interfaces, repositories, services, and controllers.
- Do not delete working files unnecessarily.
- Do not rewrite correct implementations simply because you prefer another coding style.
- Do not replace the existing architecture.
- Do not create duplicate business logic.
- Do not create duplicate API endpoints.
- Do not introduce unnecessary abstractions.
- Do not modify unrelated microservices unless integration genuinely requires it.
- Do not remove existing features to simplify implementation.
- If a file is incomplete or incorrect, modify only the necessary parts.
- Create new files only when the existing structure genuinely requires them.
- Preserve existing API contracts where possible.
- Maintain compatibility with the existing Frontend and dependent services.

Before modifying an existing feature, understand its current behavior and dependencies.

**The goal is to complete and improve the current PaymentService, not rebuild the entire project from scratch.**

## 4. BACKEND ARCHITECTURE

Follow the existing Clean Architecture structure.

Expected responsibilities:

- **Domain:** Payment, Transaction, Refund, PaymentMethod, PaymentAttempt, relevant enums, domain rules, and domain events.
- **Application:** Commands, queries, handlers, DTOs, validators, interfaces, and business workflows.
- **Infrastructure:** EF Core, repositories, database access, gateway integrations, message consumers, and external services.
- **API:** Controllers, authentication, authorization, dependency injection, and HTTP responses.

Follow the project's existing MediatR/CQRS conventions.

Requirements:

- Keep controllers thin.
- Do not place business logic inside controllers.
- Use DTOs instead of exposing EF Core entities.
- Validate all incoming requests.
- Use asynchronous operations.
- Use appropriate transactions.
- Handle concurrency.
- Implement idempotency.
- Avoid N+1 queries.
- Use appropriate database indexes.
- Use structured logging.
- Use consistent error responses.
- Never expose sensitive financial or payment information.
- Keep all service boundaries correct.

Do not introduce a different architecture if the existing project already has a consistent implementation.

---

# PART III — COMPLETE PAYMENT API REQUIREMENTS

## 5. GROUP 1 — PAYMENT PROCESSING

Implement the following capabilities:

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/payments` | Initialize payment for an order |
| POST | `/api/payments/checkout` | Create payment from checkout session |
| POST | `/api/payments/initialize` | Initialize payment transaction |
| POST | `/api/payments/{paymentId}/process` | Process a payment |
| GET | `/api/payments/{paymentId}` | Get payment details |
| GET | `/api/payments/{paymentId}/status` | Get payment status |
| GET | `/api/payments/{paymentId}/result` | Get payment result |
| POST | `/api/payments/{paymentId}/retry` | Retry eligible failed payment |
| POST | `/api/payments/{paymentId}/cancel` | Cancel an unfinished payment |
| GET | `/api/payments/{paymentId}/receipt` | Retrieve payment receipt |

Business requirements:

- Validate that the order exists through OrderService.
- Verify order ownership.
- Verify the payable amount using trusted order data.
- Never trust the amount submitted by Frontend.
- Prevent duplicate payments for the same order where the business rules prohibit them.
- Support multiple payment attempts when appropriate.
- Maintain payment and transaction history.
- Handle concurrent payment requests safely.
- Use idempotency keys for operations that can create financial side effects.
- Do not mark an order paid before receiving a verified payment result.
- Prevent retrying payments that are already completed or otherwise ineligible.

Do not create redundant initialization and processing endpoints if the existing architecture already provides one clear payment workflow.

## 6. GROUP 2 — BUYER PAYMENT MANAGEMENT

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/payments/me` | Buyer's payment history |
| GET | `/api/payments/me/summary` | Payment statistics |
| GET | `/api/payments/me/{paymentId}` | View own payment |
| GET | `/api/payments/me/search` | Search payments |
| GET | `/api/payments/me/filter` | Filter payments |
| GET | `/api/payments/me/pending` | Pending payments |
| GET | `/api/payments/me/successful` | Successful payments |
| GET | `/api/payments/me/failed` | Failed payments |
| GET | `/api/payments/me/refunded` | Refunded payments |
| GET | `/api/payments/me/{paymentId}/history` | Payment history |

Where appropriate, consolidate status filtering into:

`GET /api/payments/me?status=...&page=...&pageSize=...`

Requirements:

- Obtain UserId from the authenticated JWT.
- Never trust UserId submitted by Frontend.
- Verify payment ownership before returning details.
- Implement pagination, search, and filtering.
- Show payment history accurately.
- Do not expose another user's payment information.

## 7. GROUP 3 — PAYMENT METHODS

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/payment-methods` | List supported payment methods |
| GET | `/api/payment-methods/available` | List currently available methods |
| GET | `/api/payment-methods/{methodId}` | Payment method details |
| GET | `/api/payments/order/{orderId}/methods` | Available methods for an order |
| GET | `/api/payment-methods/configuration` | Public payment method configuration |

Potential payment methods:

- COD.
- Bank Transfer.
- VNPay.
- MoMo.
- ZaloPay.
- Domestic ATM cards.
- Visa/Mastercard.

Important:

- Only display payment methods that are genuinely configured and integrated.
- Do not show fake gateway options.
- Do not create UI buttons that pretend to support unavailable providers.
- Separate payment method availability from actual gateway processing.
- Never expose secret gateway configuration to Frontend.

## 8. GROUP 4 — COD PAYMENT

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/payments/cod` | Initialize COD payment |
| GET | `/api/payments/cod/{paymentId}` | View COD payment |
| GET | `/api/payments/cod/pending` | Pending COD collection |
| POST | `/api/payments/cod/{paymentId}/confirm-received` | Confirm collected COD |
| POST | `/api/payments/cod/{paymentId}/report-failure` | Report collection failure |
| GET | `/api/payments/cod/{paymentId}/collection-history` | COD collection history |

Security and business rules:

- Only an authorized Shipper assigned to the relevant shipment may confirm COD collection.
- Buyers must never confirm their own COD collection.
- Verify the order, shipment, payment, and expected amount.
- Prevent duplicate COD confirmation.
- Record who confirmed collection and when.
- Verify the caller's assignment through ShippingService or a trusted internal event.
- Do not trust a Shipper ID supplied by Frontend.
- Use idempotency for collection confirmation.
- Synchronize verified COD results with OrderService.

COD collection must not be treated as a normal Buyer payment confirmation.

## 9. GROUP 5 — BANK TRANSFER

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/payments/bank-transfer` | Initialize bank transfer |
| GET | `/api/payments/bank-transfer/{paymentId}` | Transfer information |
| GET | `/api/payments/bank-transfer/{paymentId}/instructions` | Transfer instructions |
| GET | `/api/payments/bank-transfer/{paymentId}/status` | Transfer status |
| POST | `/api/payments/bank-transfer/{paymentId}/verify` | Verify transfer using trusted source |
| GET | `/api/payments/bank-transfer/pending` | Pending transfers |

Requirements:

- Generate unique payment references.
- Display accurate bank account and transfer instructions.
- Match incoming bank transactions to the correct payment.
- Verify amount and transaction reference.
- Prevent duplicate matching.
- Record reconciliation information.
- Do not accept screenshots or user-entered descriptions as proof of successful payment.
- If no trusted bank integration exists, clearly mark the workflow as pending manual/admin verification.
- Do not falsely claim automatic bank verification.

## 10. GROUP 6 — PAYMENT GATEWAY INTEGRATION

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/payments/gateway/{provider}/create` | Create gateway payment |
| GET | `/api/payments/gateway/{provider}/return` | Handle user return URL |
| POST | `/api/payments/gateway/{provider}/callback` | Handle provider callback |
| POST | `/api/payments/gateway/{provider}/webhook` | Handle provider webhook |
| GET | `/api/payments/gateway/{provider}/status/{transactionId}` | Verify gateway transaction |
| POST | `/api/payments/gateway/{provider}/refund` | Request gateway refund |
| GET | `/api/payments/gateway/providers` | List configured providers |

Critical security requirements:

### Return URL

- Return URL is used for user navigation.
- Never treat its query parameters alone as proof of successful payment.
- Verify the transaction server-side before updating payment status.

### Callback and Webhook

- Validate the provider signature.
- Validate the transaction reference.
- Validate the amount and currency.
- Validate provider transaction status.
- Validate that the transaction belongs to the correct payment.
- Handle repeated webhook deliveries idempotently.
- Reject invalid or tampered requests.
- Store relevant callback metadata safely.
- Never log secrets, card numbers, CVV, or sensitive payment credentials.

### Gateway Configuration

- Store credentials securely using the existing project's configuration/secrets mechanism.
- Never expose secret keys to Frontend.
- Never hardcode production credentials.
- Use sandbox/test mode where supported.
- Only activate a provider when the integration is actually functional.

If the project has no credentials or provider integration, implement the proper integration structure and clearly document the missing external configuration. Do not simulate a successful gateway transaction.

## 11. GROUP 7 — REFUND MANAGEMENT

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/refunds` | Create refund request |
| GET | `/api/refunds/{refundId}` | Refund details |
| GET | `/api/refunds/{refundId}/status` | Refund status |
| GET | `/api/refunds/me` | Buyer's refund history |
| GET | `/api/payments/{paymentId}/refunds` | Refunds for a payment |
| POST | `/api/refunds/{refundId}/process` | Process eligible refund |
| POST | `/api/refunds/{refundId}/approve` | Approve refund |
| POST | `/api/refunds/{refundId}/reject` | Reject refund |
| POST | `/api/refunds/{refundId}/cancel` | Cancel eligible refund request |
| GET | `/api/refunds/{refundId}/history` | Refund processing history |

Requirements:

- Support full and partial refunds when the payment provider supports them.
- Never refund more than the amount actually collected minus previous successful refunds.
- Validate refund eligibility.
- Prevent duplicate refund processing.
- Validate order and payment ownership.
- Record refund reasons and actors.
- Track refund states accurately.
- Handle provider failures and retry safely.
- Keep a complete audit trail.
- Synchronize verified refund results with OrderService.
- Send relevant events to NotificationService.

Important distinction:

- A refund request is not a completed refund.
- An approved refund is not necessarily a successfully transferred refund.
- Only a verified provider result can mark a gateway refund as successful.

Do not duplicate OrderService's return-request workflow.

OrderService manages order return requests and business eligibility. PaymentService executes the financial refund.

## 12. GROUP 8 — PAYMENT TRANSACTIONS

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/transactions/{transactionId}` | Transaction details |
| GET | `/api/transactions/me` | Buyer's transactions |
| GET | `/api/transactions/{transactionId}/status` | Transaction status |
| GET | `/api/transactions/{transactionId}/history` | Transaction history |
| GET | `/api/payments/{paymentId}/transactions` | Payment transactions |
| POST | `/api/transactions/{transactionId}/reconcile` | Reconcile transaction |
| GET | `/api/transactions/search` | Search transactions |

Keep the distinction clear:

- Payment represents the payment obligation associated with an order.
- Transaction represents an individual payment attempt or financial operation.
- Refund represents money being returned.

One payment may contain multiple transaction attempts.

Requirements:

- Maintain accurate transaction history.
- Track provider references.
- Record transaction amounts and currency.
- Prevent duplicate transaction records.
- Support reconciliation.
- Never overwrite historical transaction records merely to show the latest status.

## 13. GROUP 9 — PAYMENT SECURITY AND IDEMPOTENCY

| Method | Endpoint | Responsibility |
|---|---|---|
| POST | `/api/payments/idempotency/check` | Check idempotency key |
| GET | `/api/payments/{paymentId}/verification` | Verification information |
| POST | `/api/payments/{paymentId}/verify` | Verify trusted payment status |
| GET | `/api/payments/{paymentId}/attempts` | Payment attempts |
| POST | `/api/payments/{paymentId}/attempts` | Create eligible payment attempt |
| GET | `/api/payments/security/events` | Admin security events |

Requirements:

- Use idempotency keys for payment creation, confirmation, refund, and COD collection where appropriate.
- Store and validate idempotency keys safely.
- Ensure concurrent duplicate requests cannot create multiple successful financial operations.
- Record security-relevant events.
- Protect security event endpoints using Admin authorization.
- Avoid exposing sensitive verification data to ordinary users.
- Never allow a public endpoint to arbitrarily mark a payment as successful.

Do not implement a meaningless public idempotency-check endpoint if the actual idempotency mechanism belongs inside the payment operation.

## 14. GROUP 10 — SELLER EARNINGS AND PAYOUTS

This group is important for EcomSystem's multi-Seller marketplace.

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/seller/earnings` | Seller earnings |
| GET | `/api/seller/earnings/summary` | Earnings statistics |
| GET | `/api/seller/earnings/transactions` | Seller financial history |
| GET | `/api/seller/balance` | Withdrawable balance |
| GET | `/api/seller/payouts` | Payout history |
| POST | `/api/seller/payouts` | Create payout request |
| GET | `/api/seller/payouts/{payoutId}` | Payout details |
| POST | `/api/seller/payouts/{payoutId}/cancel` | Cancel eligible payout |
| GET | `/api/seller/payout-methods` | Payout methods |
| POST | `/api/seller/payout-methods` | Add payout method |
| PUT | `/api/seller/payout-methods/{methodId}` | Update payout method |
| DELETE | `/api/seller/payout-methods/{methodId}` | Remove payout method |

Requirements:

- SellerId must come from authenticated identity or be securely verified.
- Never trust SellerId submitted by Frontend.
- Do not calculate withdrawable balance by simply adding order totals.
- Consider payment settlement, platform commission, refunds, cancellations, chargebacks, and payout history.
- Prevent negative or duplicate payouts.
- Ensure that payout requests cannot exceed the eligible balance.
- Keep a proper financial ledger/audit trail.
- Protect bank account information.
- Mask sensitive account details in responses.
- Verify ownership before updating or deleting payout methods.

Important architectural decision:

Seller wallet, ledger, and payout operations are complex financial responsibilities.

Inspect the existing architecture before implementation.

If the current PaymentService does not contain a proper financial ledger or settlement model, do not invent an unreliable balance calculation. Implement only the capabilities that can be supported safely, and document any required future Wallet/Ledger service boundary.

## 15. GROUP 11 — ADMIN PAYMENT MANAGEMENT

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/admin/payments` | Manage all payments |
| GET | `/api/admin/payments/{paymentId}` | Payment details |
| GET | `/api/admin/payments/search` | Search payments |
| GET | `/api/admin/payments/statistics` | Payment statistics |
| GET | `/api/admin/payments/pending` | Pending payments |
| GET | `/api/admin/payments/failed` | Failed payments |
| GET | `/api/admin/payments/refunded` | Refunded payments |
| GET | `/api/admin/refunds` | Refund management |
| GET | `/api/admin/transactions` | All transactions |
| GET | `/api/admin/transactions/reconciliation` | Reconciliation reports |
| GET | `/api/admin/payouts` | Payout management |
| POST | `/api/admin/payouts/{payoutId}/approve` | Approve payout |
| POST | `/api/admin/payouts/{payoutId}/reject` | Reject payout |
| GET | `/api/admin/payment-gateways` | Gateway management |
| GET | `/api/admin/payment-gateways/{provider}/health` | Gateway health |
| GET | `/api/admin/payments/export` | Export payment reports |

Requirements:

- All Admin endpoints must require proper Admin authorization.
- Record important Admin actions.
- Protect financial data.
- Support pagination, filtering, search, and sorting.
- Calculate statistics from actual data.
- Support reconciliation workflows.
- Do not expose gateway secrets.
- Do not allow Admin to mark an unverified external payment as successful merely by changing a status field.
- Any manual financial adjustment must follow a dedicated audited workflow.

## 16. GROUP 12 — INTERNAL MICROSERVICE APIs

| Method | Endpoint | Responsibility |
|---|---|---|
| GET | `/api/internal/payments/{paymentId}/summary` | Payment summary |
| GET | `/api/internal/payments/order/{orderId}` | Payment by order |
| POST | `/api/internal/payments/order/{orderId}/initialize` | Initialize payment for order |
| POST | `/api/internal/payments/{paymentId}/confirm` | Confirm verified payment |
| POST | `/api/internal/payments/{paymentId}/fail` | Record verified failure |
| POST | `/api/internal/payments/{paymentId}/refund` | Request refund |
| GET | `/api/internal/payments/order/{orderId}/status` | Payment status |
| POST | `/api/internal/payments/cod/{paymentId}/confirm` | Confirm COD collection |
| GET | `/api/internal/payments/order/{orderId}/transactions` | Order transactions |
| GET | `/api/internal/payments/statistics` | Internal statistics |

Requirements:

- Protect internal APIs using the existing service-to-service authentication mechanism.
- Do not expose them to normal Buyer or Seller sessions.
- Validate calling service identity.
- Validate order/payment relationships.
- Use idempotency for state-changing operations.
- Reject unauthorized status changes.
- Record important internal financial operations.

---

# PART IV — PAYMENT DOMAIN AND FINANCIAL INTEGRITY

## 17. PAYMENT STATE MANAGEMENT

Inspect existing enums and domain models before introducing new states.

Possible payment states:

- Pending
- Processing
- RequiresAction
- Succeeded
- Failed
- Cancelled
- PartiallyRefunded
- Refunded
- Expired

Possible transaction states:

- Created
- Pending
- Processing
- Succeeded
- Failed
- Cancelled
- Reversed

Possible refund states:

- Requested
- PendingApproval
- Approved
- Processing
- Succeeded
- Failed
- Rejected
- Cancelled

Possible payout states:

- Requested
- PendingApproval
- Approved
- Processing
- Succeeded
- Failed
- Rejected
- Cancelled

Adapt these states to existing business logic.

Do not blindly replace existing enums.

Implement valid state transitions and reject invalid transitions.

Never treat:

- Payment initialized as payment succeeded.
- Gateway redirect as payment succeeded.
- Refund approved as refund succeeded.
- COD assigned as COD collected.
- Payout requested as payout completed.

## 18. FINANCIAL DATA RULES

Implement accurate financial calculations.

Requirements:

- Use `decimal` for monetary calculations.
- Define and validate currency.
- Never use floating-point types for financial amounts.
- Validate amount and currency against the order and provider transaction.
- Prevent duplicate charges.
- Prevent duplicate refunds.
- Prevent over-refunding.
- Prevent duplicate COD collection.
- Prevent duplicate payouts.
- Use appropriate database transactions and concurrency control.
- Maintain immutable or auditable financial history.
- Record provider transaction references.
- Use appropriate rounding rules.

Do not silently modify historical financial amounts.

## 19. SENSITIVE PAYMENT DATA AND SECURITY

Strictly follow payment security principles.

- Never store CVV.
- Never store raw card numbers unless the project has a compliant, explicitly designed provider-tokenization architecture.
- Prefer payment provider tokens.
- Never expose gateway secret keys.
- Never log access tokens, secrets, CVV, or sensitive bank information.
- Mask bank account numbers.
- Validate webhook signatures.
- Validate callbacks.
- Prevent replay attacks where supported.
- Use secure configuration.
- Avoid returning sensitive internal provider payloads to Frontend.
- Use HTTPS in production configurations.
- Apply rate limiting to sensitive payment endpoints where appropriate.

Do not claim PCI compliance unless the system has actually been assessed and meets the relevant requirements.

---

# PART V — MICROSERVICE INTEGRATION

## 20. SERVICE OWNERSHIP

Respect these boundaries:

| Service | Responsibility |
|---|---|
| AuthService | Authentication, credentials, JWT |
| UserService | Buyer/Seller profiles and identities |
| OrderService | Orders, order items, order status |
| CartService | Shopping cart |
| ProductService | Products and variants |
| InventoryService | Stock and reservations |
| PromotionService | Vouchers and discounts |
| PaymentService | Payments, transactions, refunds, payment attempts |
| ShippingService | Shipment, delivery, COD collection operations |
| ReviewService | Reviews and ratings |
| NotificationService | Notifications |

Important:

- OrderService owns order state.
- PaymentService owns financial payment state.
- ShippingService owns shipping operations.
- NotificationService owns user notifications.
- UserService owns user profile information.

Do not directly access another service's database.

Do not duplicate another service's business logic.

## 21. PAYMENT AND ORDER INTEGRATION

When an order is created:

1. OrderService creates the order.
2. PaymentService initializes the appropriate payment.
3. PaymentService validates the amount using trusted OrderService data.
4. PaymentService creates the payment and transaction records.
5. The user completes the supported payment method.
6. PaymentService verifies the result.
7. PaymentService publishes a trusted payment event.
8. OrderService updates the order's payment status.
9. NotificationService sends the appropriate notification.

For failed or cancelled payments:

- Preserve payment history.
- Handle retries according to eligibility.
- Do not create duplicate successful payments.
- Coordinate order expiration/cancellation with OrderService.
- Release inventory reservations through the appropriate workflow.

Do not make Frontend responsible for orchestrating trusted financial state changes.

## 22. RABBITMQ AND EVENT-DRIVEN INTEGRATION

Inspect existing RabbitMQ code before making changes.

Reuse existing event contracts where possible.

Potential events:

- `PaymentInitialized`
- `PaymentProcessing`
- `PaymentSucceeded`
- `PaymentFailed`
- `PaymentCancelled`
- `PaymentExpired`
- `RefundRequested`
- `RefundApproved`
- `RefundSucceeded`
- `RefundFailed`
- `CODCollected`
- `CODCollectionFailed`
- `PayoutRequested`
- `PayoutApproved`
- `PayoutSucceeded`
- `PayoutFailed`

Requirements:

- Use idempotent consumers.
- Handle duplicate events.
- Use retries where appropriate.
- Use dead-letter handling where supported.
- Use Outbox/Inbox patterns where required by the existing architecture.
- Record event processing failures.
- Do not publish success events before payment verification.
- Do not publish events with misleading or unverified statuses.

Do not add RabbitMQ to simple operations that are better handled synchronously.

---

# PART VI — FRONTEND IMPLEMENTATION

## 23. MAIN FRONTEND OBJECTIVE

Complete the entire PaymentService user experience.

The UI must be professional and inspired by Shopee while maintaining a distinct EcomSystem identity.

Reuse the existing:

- React architecture.
- Routing.
- Authentication context.
- API clients.
- State management.
- Layouts.
- Design tokens.
- UI components.
- Notification system.
- CSS conventions.

Do not create unnecessary duplicate layouts or design systems.

The UI must be responsive and connected to real Backend APIs.

## 24. BUYER PAYMENT EXPERIENCE

### A. Checkout Payment Selection

Create a professional payment selection experience.

Display only payment methods that are genuinely supported and available for the order.

Potential sections:

- COD.
- Bank Transfer.
- Integrated payment gateways.
- Other configured payment providers.

Display:

- Payment method name.
- Method icon or logo where permitted.
- Brief instructions.
- Availability.
- Applicable restrictions.
- Order payable amount.

Requirements:

- Show the actual order amount from trusted Backend data.
- Do not let users modify the authoritative payable amount.
- Clearly display payment status.
- Prevent duplicate submission.
- Show loading and processing states.
- Handle payment expiration.
- Display useful error messages.

### B. Gateway Payment Flow

When an integrated provider is available:

1. User selects the provider.
2. Frontend calls the payment initialization API.
3. Backend returns the provider's supported payment URL or required action.
4. Frontend redirects or displays the provider's supported flow.
5. User returns to EcomSystem.
6. Frontend displays a pending/verification state.
7. Frontend retrieves the verified payment result from Backend.
8. Frontend displays the final result.

**Never treat a return URL query parameter as proof of payment success.**

Do not show a success screen before Backend verification.

If a provider is not configured, do not display a working-looking payment option.

### C. Bank Transfer UI

Display:

- Bank name.
- Account holder.
- Masked or appropriately displayed account number.
- Transfer amount.
- Unique transfer reference.
- Transfer instructions.
- Payment expiration time when supported.
- Current verification status.

Provide copy-to-clipboard functionality for transfer information.

Do not let a user upload a screenshot and automatically mark the payment as successful.

### D. Payment Result Page

Implement professional states:

- Payment successful.
- Payment pending.
- Payment failed.
- Payment cancelled.
- Payment expired.
- Refund pending.
- Refund completed.

Display:

- Payment ID.
- Order ID.
- Transaction reference.
- Amount.
- Payment method.
- Timestamp.
- Receipt access where supported.
- Navigation back to order details.

Use actual Backend results.

### E. Buyer Payment History

Create a polished payment history page.

Features:

- Payment list.
- Search.
- Status filters.
- Date filters.
- Pagination.
- Payment detail.
- Transaction history.
- Refund history.
- Receipt access.

Possible status filters:

- All
- Pending
- Processing
- Successful
- Failed
- Cancelled
- Partially Refunded
- Refunded

Use real API data.

### F. Payment Detail

Display:

- Payment reference.
- Order reference.
- Amount.
- Currency.
- Payment method.
- Payment status.
- Transaction attempts.
- Provider information safe for display.
- Payment timeline.
- Refund information.
- Receipt.

Do not expose sensitive provider payloads.

### G. Refund UI

Implement:

- Refund request status.
- Refund amount.
- Full/partial refund information.
- Refund reason.
- Refund processing timeline.
- Refund transaction details where appropriate.

The Buyer must not be able to create arbitrary refunds without the relevant OrderService return/refund eligibility workflow.

---

# PART VII — COD, SELLER AND ADMIN FRONTEND

## 25. COD COLLECTION UI

Create a Shipper-facing COD collection interface if the existing application has a Shipper role and UI.

Display:

- Assigned shipment.
- Order reference.
- Expected COD amount.
- Payment status.
- Collection status.
- Collection history.

Actions:

- Confirm COD collected.
- Report collection failure.

Requirements:

- Only assigned Shippers can perform collection actions.
- Confirm with a dialog.
- Prevent duplicate submission.
- Display accurate confirmation results.
- Never expose COD collection confirmation to Buyers.
- Verify assignment and amount through Backend.

## 26. SELLER PAYMENT DASHBOARD

Create a professional Seller financial dashboard.

Potential sections:

- Total sales.
- Pending settlement.
- Available balance.
- Withdrawable balance.
- Completed payouts.
- Pending payouts.
- Financial transactions.
- Payout history.

Provide:

- Date filters.
- Transaction search.
- Pagination.
- Payout request form.
- Payout method management.
- Payout detail.
- Payout status timeline.

Important:

- Display only financial data supported by the Backend.
- Do not calculate fake earnings in Frontend.
- Clearly distinguish order revenue, platform fees, refunds, settlement amounts, and withdrawable balance.
- Do not expose another Seller's financial information.
- Mask sensitive bank account information.
- Do not let Sellers request payouts beyond their actual eligible balance.

If the financial ledger is not implemented, do not display fabricated balance figures.

## 27. ADMIN PAYMENT MANAGEMENT

Build a professional Admin payment dashboard.

Include:

### Payment Overview

- Total payments.
- Successful payments.
- Pending payments.
- Failed payments.
- Refunded payments.
- Total verified collected amount.
- Refund statistics.

### Payment Management

- Search payments.
- Filter by status.
- Filter by payment method.
- Filter by date.
- View payment details.
- View transaction attempts.
- View payment history.

### Refund Management

- List refund requests.
- View refund details.
- Approve eligible refund requests.
- Reject with a reason.
- View refund history.
- Track provider refund results.

### Transaction Reconciliation

- View transaction references.
- View reconciliation status.
- Identify unmatched transactions.
- View failed verification events.
- Retry reconciliation where safe.

### Gateway Management

- View configured providers.
- View provider status.
- View health information.
- Display configuration status without exposing secrets.

### Payout Management

- Review payout requests.
- Approve eligible payouts.
- Reject with a reason.
- View payout history.

All Admin actions must be protected and audited.

Do not allow unrestricted manual changes to payment or refund states.

---

# PART VIII — UI/UX QUALITY

## 28. DESIGN REQUIREMENTS

The interface must look like a real commercial e-commerce platform.

### Visual Design

- Premium EcomSystem branding.
- Shopee-inspired usability.
- Clean layouts.
- Professional color palette.
- Clear typography.
- Consistent spacing.
- High-quality cards.
- Well-designed tables.
- Appropriate status badges.
- Professional icons.
- Subtle shadows and borders.
- Consistent button styles.
- Carefully designed forms and dialogs.

### Interaction Design

- Smooth hover states.
- Responsive modals.
- Confirmation dialogs.
- Search and filters.
- Loading skeletons.
- Empty states.
- Error states.
- Toast notifications.
- Copy-to-clipboard actions.
- Disabled states when operations are unavailable.

### Responsive Design

Test:

- Desktop.
- Laptop.
- Tablet.
- Mobile.

Avoid:

- Fake data.
- Dead buttons.
- Broken links.
- Placeholder pages.
- Inconsistent colors.
- Overcrowded tables.
- Excessive animations.
- Unnecessary gradients.
- Misleading payment success messages.

Every visible action must work or be clearly unavailable.

---

# PART IX — API INTEGRATION

## 29. FRONTEND AND BACKEND INTEGRATION

Connect all supported Frontend features to real Backend APIs.

Requirements:

- Reuse existing API clients.
- Use existing environment variables.
- Use the correct PaymentService base URL.
- Include JWT authentication where appropriate.
- Handle token expiration using existing AuthService logic.
- Handle 400, 401, 403, 404, 409, 422, and 500 responses.
- Handle gateway errors.
- Handle network failures.
- Prevent duplicate payment submissions.
- Show accurate loading states.
- Refresh relevant data after mutations.
- Use real response DTOs.
- Handle pagination and filtering correctly.

Do not implement payment business logic in Frontend.

Do not bypass Backend authorization by hiding buttons.

---

# PART X — DATABASE AND TESTING

## 30. DATABASE REQUIREMENTS

Inspect the existing PaymentService database and migrations.

Check:

- Payment.
- Transaction.
- Refund.
- PaymentAttempt.
- PaymentMethod.
- Gateway configuration.
- COD collection records.
- Bank transfer records.
- Payout.
- Payout method.
- Reconciliation records.
- Idempotency records.
- Security/audit events.

Only introduce new entities when required.

Requirements:

- Preserve existing database data.
- Do not drop tables unnecessarily.
- Do not reset databases.
- Do not delete Docker volumes.
- Use appropriate unique constraints.
- Use appropriate indexes.
- Use decimal precision for money.
- Use safe timestamp handling.
- Protect sensitive data.
- Maintain historical financial records.

Never run destructive database commands.

## 31. BACKEND TESTING

Implement and execute tests for:

### Payment

- Payment initialization.
- Payment ownership.
- Payment amount validation.
- Duplicate payment prevention.
- Payment retry eligibility.
- Payment cancellation.
- Payment status transitions.

### Gateway

- Valid webhook signature.
- Invalid webhook signature.
- Duplicate webhook.
- Amount mismatch.
- Transaction reference mismatch.
- Provider failure.
- Return URL verification.

### COD

- Authorized Shipper collection.
- Unauthorized Buyer collection.
- Unassigned Shipper collection.
- Incorrect amount.
- Duplicate collection.
- Collection failure.

### Bank Transfer

- Valid transaction matching.
- Invalid transaction reference.
- Incorrect amount.
- Duplicate bank transaction.
- Unverified screenshot submission.

### Refund

- Full refund.
- Partial refund.
- Over-refund prevention.
- Duplicate refund prevention.
- Refund failure.
- Refund retry.
- Refund ownership.

### Seller and Admin

- Seller ownership.
- Seller payout eligibility.
- Unauthorized payout access.
- Admin authorization.
- Audit history.

### Internal APIs

- Service authentication.
- Unauthorized service requests.
- Payment/order relationship validation.
- Idempotency.
- Event processing.

Never claim tests pass unless they were actually executed.

## 32. FRONTEND TESTING

Verify:

- Checkout payment selection.
- Payment initialization.
- Gateway redirect/return.
- Payment verification.
- Payment result page.
- Buyer payment history.
- Payment detail.
- Receipt.
- Bank transfer instructions.
- COD status.
- COD Shipper collection.
- Refund history.
- Seller earnings.
- Seller payout requests.
- Admin payment management.
- Admin refund management.
- Reconciliation screens.
- Responsive layouts.
- Loading states.
- Error states.
- Authentication and authorization.

Check browser console errors and failed Network requests.

---

# PART XI — DOCKER AND REAL RUNTIME VERIFICATION

## 33. MANDATORY DOCKER WORKFLOW

The EcomSystem project already has Docker configured.

You MUST follow these instructions.

### When C# Backend or Frontend code is modified

After implementation:

1. Build all relevant Backend projects.
2. Build the Frontend.
3. Run automated tests.
4. Navigate to the project root directory containing the Docker Compose file.
5. Execute:

`docker compose up --build`

6. Wait for containers to initialize.
7. Inspect all relevant container statuses.
8. Inspect logs for startup errors.
9. Verify PaymentService starts correctly.
10. Verify dependent services remain operational.
11. Verify database connectivity.
12. Verify RabbitMQ connectivity if used.
13. Open the actual Frontend in the browser.
14. Test the implemented payment pages.
15. Test important API requests.
16. Inspect browser console errors.
17. Inspect failed Network requests.
18. Fix discovered issues.
19. Rebuild and retest after modifications.

**A successful compilation is NOT sufficient.**

You must verify the actual running application.

### When only performing ordinary tests without code changes

Use:

`docker compose up`

Then test the running system.

### Data Safety

Never execute:

`docker compose down -v`

Never:

- Delete database volumes.
- Reset SQL Server data.
- Delete existing payment records.
- Remove persistent application data.
- Run destructive cleanup commands.
- Rebuild unrelated services unnecessarily.

If Docker cannot start due to environmental problems, report the exact error and identify what could not be verified.

Never claim Docker or browser verification passed without actually performing it.

---

# PART XII — IMPLEMENTATION EXECUTION PLAN

## 34. REQUIRED WORKFLOW

Follow this order.

### PHASE 1 — INSPECTION

- Read `docs/AI_RULE.md`.
- Read `docs/PaymentService.md`.
- Inspect the entire existing PaymentService.
- Inspect Frontend.
- Inspect database and migrations.
- Inspect gateway integrations.
- Inspect RabbitMQ events.
- Inspect Docker Compose.
- Identify existing working functionality.
- Identify missing functionality.
- Identify security problems.

### PHASE 2 — IMPLEMENTATION PLAN

Create an implementation checklist.

Identify:

- Existing APIs.
- Missing APIs.
- Duplicate APIs.
- Existing working features.
- Required Backend changes.
- Required Frontend changes.
- Database changes.
- Integration changes.
- Test cases.

Do not start by replacing existing code.

### PHASE 3 — BACKEND

Prioritize:

1. Payment initialization.
2. Payment processing.
3. Payment status management.
4. Buyer payment history.
5. Payment methods.
6. COD.
7. Bank transfer.
8. Configured gateway integration.
9. Transaction history.
10. Refund processing.
11. Idempotency and security.
12. Seller earnings/payouts where supported by a reliable financial model.
13. Admin management.
14. Internal APIs.

### PHASE 4 — FRONTEND

Implement:

1. Checkout payment selection.
2. Gateway payment workflow.
3. Payment result.
4. Buyer payment history.
5. Payment details.
6. Refund management.
7. COD collection UI for authorized Shippers.
8. Seller financial dashboard.
9. Seller payout management.
10. Admin payment dashboard.
11. Admin refund and reconciliation management.

### PHASE 5 — INTEGRATION

Test interactions with:

- OrderService.
- ShippingService.
- UserService.
- NotificationService.
- RabbitMQ.
- Existing gateway providers.

Verify that all state changes are consistent.

### PHASE 6 — TESTING AND DOCKER

- Build Backend.
- Build Frontend.
- Execute tests.
- Run `docker compose up --build` after C# or Frontend code changes.
- Inspect containers.
- Inspect logs.
- Open the actual Frontend.
- Execute important end-to-end workflows.
- Fix errors.
- Repeat verification.

---

# PART XIII — DEFINITION OF DONE

## 35. FINAL COMPLETION CHECKLIST

The task is not complete merely because files have been created.

Completion requires:

- [ ] Entire `docs/AI_RULE.md` read.
- [ ] Entire `docs/PaymentService.md` read.
- [ ] Existing architecture inspected.
- [ ] Existing correct code preserved.
- [ ] Backend implementation completed as far as supported by the project.
- [ ] Frontend implementation completed as far as supported by the project.
- [ ] Payment processing works correctly.
- [ ] Buyer payment management works.
- [ ] Payment methods reflect actual configuration.
- [ ] COD authorization is secure.
- [ ] Bank transfers are not falsely verified.
- [ ] Gateway callbacks/webhooks are verified.
- [ ] Refund calculations are correct.
- [ ] Over-refunds are prevented.
- [ ] Transaction history is accurate.
- [ ] Idempotency is implemented.
- [ ] Seller ownership is enforced.
- [ ] Payout calculations are supported by reliable financial records.
- [ ] Admin authorization is enforced.
- [ ] Internal APIs are protected.
- [ ] Service boundaries are respected.
- [ ] RabbitMQ events are handled safely where applicable.
- [ ] Database changes preserve existing data.
- [ ] Backend builds successfully.
- [ ] Frontend builds successfully.
- [ ] Automated tests are executed.
- [ ] Docker containers are checked.
- [ ] Actual UI is opened and inspected.
- [ ] Important workflows are tested.
- [ ] No critical runtime errors remain.
- [ ] Existing unrelated features still work.

If a gateway requires credentials that are unavailable, or an external provider cannot be reached, clearly document the limitation. Do not fake a successful integration.

---

# PART XIV — FINAL REPORT

## 36. REQUIRED FINAL REPORT

When implementation is finished, provide:

### A. Summary

- What was completed.
- What was improved.
- What remains incomplete.

### B. Backend

- New files.
- Modified files.
- Entities.
- Database changes.
- Commands and queries.
- Handlers.
- Repositories.
- Services.
- Controllers.
- Implemented API endpoints.

### C. Frontend

- New pages.
- Modified pages.
- Components.
- API integrations.
- Buyer payment workflows.
- Shipper COD workflows.
- Seller earnings and payout workflows.
- Admin payment workflows.

### D. Integrations

- OrderService.
- ShippingService.
- UserService.
- NotificationService.
- RabbitMQ.
- Payment gateways.

For each integration, report whether it is actually functional, partially implemented, or blocked.

### E. Testing Results

Report real results:

- Backend build.
- Frontend build.
- Automated tests.
- Docker startup.
- Container status.
- Database connectivity.
- RabbitMQ connectivity.
- API tests.
- Gateway tests.
- Browser verification.
- End-to-end tests.

### F. Remaining Issues

List actual blockers, missing provider credentials, unsupported payment methods, incomplete integrations, and tests that could not be executed.

Never hide failures.

---

# FINAL INSTRUCTIONS

Work carefully and systematically.

**Complete both PaymentService Backend and Frontend.**

**Make the UI professional, premium, and Shopee-inspired while retaining EcomSystem's unique identity.**

**Preserve all existing correct code and features.**

**Do not delete or rewrite files unnecessarily.**

**Follow Clean Architecture and strict microservice ownership boundaries.**

**Treat financial correctness and payment security as the highest priority.**

**Never fake payment success, COD collection, bank transfer verification, refunds, or payouts.**

**Use real APIs and real database records.**

**Run `docker compose up --build` after modifying C# Backend or Frontend code, then inspect and test the actual running application.**

Do not finish until the implementation has been tested as thoroughly as the available environment allows.

The objective is to deliver a stable, secure, integrated, and professionally designed PaymentService for EcomSystem — not merely to generate code.