# AI DEVELOPMENT RULES – ECOMSYSTEM

**Project:** EcomSystem – Mini Shopee Marketplace  
**Architecture:** ASP.NET Core Microservices + Clean Architecture  
**Backend:** C# / ASP.NET Core / Entity Framework Core / SQL Server  
**Frontend:** React / TypeScript / Vite  
**Infrastructure:** Docker / RabbitMQ / Redis  
**Document Type:** Global AI Coding Rules

---

# 1. PROJECT OVERVIEW

EcomSystem is a mini Shopee-style e-commerce marketplace built using Microservices Architecture.

The system supports the following main roles:

- Customer: Browse products, manage cart, place orders, make payments, track shipments and write reviews.
- Seller: Manage shop information, products, inventory and orders.
- Shipper: Manage assigned deliveries and update shipping status.
- Admin: Manage users, products, orders, promotions and the overall platform.

The system contains the following microservices:

1. AuthService – Authentication and authorization.
2. UserService – User accounts and profile management.
3. ProductService – Products, categories and product information.
4. OrderService – Order creation and order lifecycle.
5. PaymentService – Payments and transactions.
6. InventoryService – Stock management and inventory reservations.
7. CartService – Shopping cart management.
8. ShippingService – Shipping fees, shipments and delivery tracking.
9. NotificationService – Email and in-app notifications.
10. PromotionService – Coupons and promotional campaigns.
11. ReviewService – Product ratings, comments and review images.

IMPORTANT: The list above describes the target system. Always inspect the actual repository before assuming that every service or feature is already implemented.

---

# 2. GENERAL DEVELOPMENT PRINCIPLES

AI MUST follow these principles:

1. Always inspect the existing source code before implementing anything.
2. Prioritize existing architecture and coding conventions.
3. Reuse existing classes, interfaces, DTOs, services and components whenever appropriate.
4. Do not create duplicate APIs, entities, handlers or business logic.
5. Do not implement unnecessary features just to increase the number of endpoints.
6. Implement real business logic, not just basic CRUD.
7. Do not use mock data when a real Backend API is available.
8. Do not claim that a feature is complete without verifying its implementation.
9. Do not silently ignore errors or failed operations.
10. Prioritize maintainability, security, consistency and actual business requirements.

The goal is a functional and maintainable marketplace, not merely a collection of APIs.

---

# 3. MANDATORY CODE INSPECTION

Before modifying any code, inspect the relevant existing files.

For Backend, inspect:

- Controllers / API endpoints.
- Request and Response DTOs.
- Application Services.
- Commands, Queries and Handlers.
- Domain Entities.
- Repository interfaces and implementations.
- Unit of Work, if used.
- DbContext and Entity configurations.
- Database migrations.
- Dependency Injection registrations.
- Authentication and Authorization.
- RabbitMQ publishers and consumers.
- Redis integration.
- Exception handling and validation.

For Frontend, inspect:

- Pages.
- Components.
- Routes.
- API clients.
- Axios configuration and interceptors.
- Authentication state.
- Role-based access control.
- Forms and validation.
- Loading, error and empty states.
- Existing UI design conventions.

Do not assume that a class or feature is missing until the relevant source code has been inspected.

---

# 4. BACKEND ARCHITECTURE RULES

Respect the existing Clean Architecture structure.

The expected logical responsibilities are:

## API Layer

- Controllers.
- HTTP request/response handling.
- Authentication and authorization attributes.
- Model binding.
- HTTP status codes.

Controllers should remain thin and should not contain complex business logic.

## Application Layer

- Commands and Queries.
- Handlers.
- DTOs.
- Application interfaces.
- Business use cases.
- Validation.

Business workflows should be implemented in the appropriate Application or Domain layer.

## Domain Layer

- Entities.
- Enums.
- Value Objects, if needed.
- Domain rules and invariants.

Do not place database-specific logic inside Domain.

## Infrastructure Layer

- EF Core DbContext.
- Repository implementations.
- SQL Server access.
- External service integrations.
- RabbitMQ.
- Redis.
- Email providers and file storage.

IMPORTANT:

The actual repository structure takes priority over this logical reference. Do not restructure an existing service just to match an assumed folder layout.

---

# 5. API DESIGN RULES

All APIs must follow consistent REST principles.

Use:

- GET for retrieving resources.
- POST for creating resources or executing appropriate actions.
- PUT for replacing or updating resources.
- PATCH for partial updates when appropriate.
- DELETE for deleting resources.

Requirements:

1. Use clear and consistent route naming.
2. Use Request DTOs instead of exposing Domain Entities directly.
3. Use Response DTOs instead of returning database entities.
4. Validate all incoming requests.
5. Return appropriate HTTP status codes.
6. Handle invalid IDs and missing resources.
7. Handle unauthorized and forbidden requests correctly.
8. Implement pagination, filtering and sorting where appropriate.
9. Avoid returning sensitive information.
10. Keep API contracts compatible with existing consumers unless a coordinated change is necessary.

Do not create unnecessary endpoints.

---

# 6. BUSINESS LOGIC RULES

Every feature must implement actual business rules.

Examples:

- A customer cannot order an unavailable product.
- A customer cannot access another customer's private order.
- A seller cannot modify another seller's products.
- A shipper cannot arbitrarily modify an unassigned shipment.
- A customer cannot review a product they have not purchased and received, according to the review policy.
- A coupon must satisfy its validity period and usage restrictions.
- Inventory must not become negative.
- Payment amounts must match the expected order amount.
- An order must follow valid state transitions.

Always identify the responsible service for each business rule.

Do not duplicate ownership of the same business data across multiple services.

---

# 7. MICROSERVICES COMMUNICATION

Respect the existing communication mechanisms.

## Synchronous Communication

Use HTTP APIs for operations that require immediate responses and appropriate synchronous coordination.

## Asynchronous Communication

Use RabbitMQ for event-driven workflows where asynchronous processing is appropriate.

Examples:

- OrderCreated.
- PaymentSucceeded.
- PaymentFailed.
- ShipmentCreated.
- ShipmentDelivered.
- NotificationRequested.
- InventoryLowStock.

Rules:

1. Inspect existing exchanges, queues, routing keys and event contracts.
2. Do not create duplicate event definitions.
3. Use consistent event naming and payload structures.
4. Implement appropriate error handling.
5. Consider retry and dead-letter handling.
6. Handle duplicate event delivery safely.
7. Avoid circular service dependencies.
8. Do not publish sensitive information unnecessarily.

Use Outbox/Inbox patterns when required by the reliability needs of the workflow and existing architecture. Do not introduce unnecessary infrastructure.

---

# 8. DATABASE RULES

1. Respect each service's existing database ownership.
2. Do not directly access another service's database.
3. Use migrations for schema changes.
4. Avoid destructive database changes.
5. Never delete existing production-like data without explicit authorization.
6. Use appropriate indexes and constraints.
7. Handle concurrency for critical operations such as inventory reservation and payment processing.
8. Use transactions within the appropriate service boundary.
9. Never store plaintext passwords.
10. Do not store sensitive payment credentials.

Before changing database schemas, inspect existing migrations and relationships.

---

# 9. SECURITY RULES

Security is mandatory.

- Never hardcode secrets, passwords, API keys or JWT signing keys.
- Never commit credentials or private environment files.
- Use secure password hashing.
- Validate JWT tokens correctly.
- Enforce authorization on sensitive endpoints.
- Validate ownership of resources.
- Validate external OAuth tokens on the Backend.
- Protect against mass assignment.
- Validate uploaded files.
- Apply rate limiting to sensitive endpoints.
- Avoid leaking internal exception details to clients.
- Never trust user IDs, prices, roles or permissions supplied by the Frontend without Backend validation.

Use environment variables or appropriate secret configuration.

---

# 10. FRONTEND DEVELOPMENT RULES

Use the existing React + TypeScript + Vite architecture.

1. Respect the existing project structure.
2. Reuse existing components and API clients.
3. Do not duplicate pages or components unnecessarily.
4. Use TypeScript types for API contracts.
5. Connect features to real Backend APIs.
6. Handle loading, success, error and empty states.
7. Validate forms on the Frontend and Backend.
8. Protect private routes.
9. Apply role-based UI restrictions.
10. Do not store sensitive credentials in local storage unnecessarily.
11. Keep the existing visual design consistent.
12. Make the UI responsive for desktop and mobile.

Never display a successful operation when the Backend request has failed.

---

# 11. ERROR HANDLING AND LOGGING

- Use consistent API error responses.
- Handle validation errors clearly.
- Handle database exceptions appropriately.
- Log important business operations.
- Do not log passwords, access tokens, refresh tokens or sensitive personal information.
- Use structured logging where available.
- Avoid swallowing exceptions without recording or handling them.

---

# 12. TESTING REQUIREMENTS

After implementing a feature, test it.

At minimum:

## Backend

- Build the affected service.
- Test successful requests.
- Test invalid requests.
- Test missing resources.
- Test unauthorized requests.
- Test forbidden requests.
- Test important business rules.
- Test database changes.

## Frontend

- Build the Frontend.
- Verify API integration.
- Verify form validation.
- Verify loading and error states.
- Verify role-based access.
- Verify important user workflows.

## Integration

When a change affects other services:

- Check API compatibility.
- Check event contracts.
- Check RabbitMQ communication.
- Check database consistency.
- Check failure scenarios.

Never claim tests passed if they were not executed.

---

# 13. FILE AND CODE MODIFICATION RULES

Before modifying files:

1. Inspect the current implementation.
2. Identify which files actually need changes.
3. Prefer minimal and targeted modifications.
4. Preserve existing working functionality.
5. Avoid unnecessary renaming or restructuring.
6. Do not delete existing files without a clear reason.
7. Do not overwrite configuration files blindly.
8. Do not modify unrelated services.

When adding or modifying a feature, explain:

- Files created.
- Files modified.
- Reason for each modification.
- API changes.
- Database changes.
- Frontend changes.

---

# 14. ENVIRONMENT AND DOCKER RULES

The project uses Docker and Docker Compose.

When modifying infrastructure:

- Inspect the existing docker-compose configuration.
- Preserve existing service names and ports unless a change is necessary.
- Avoid duplicate service definitions.
- Preserve existing environment variable conventions.
- Check service dependencies.
- Check container networking.
- Check health checks where available.
- Avoid unnecessary image rebuilds.
- Never expose secrets in configuration or logs.

Do not modify Docker infrastructure when the task does not require it.

---

# 15. SERVICE COMPLETION CRITERIA

A service is not considered complete simply because its Controllers exist.

A service can be marked complete only when:

1. Required APIs are implemented.
2. Business logic is implemented.
3. Validation is implemented.
4. Authentication and authorization are correct.
5. Database operations work correctly.
6. Error handling is appropriate.
7. RabbitMQ/Redis integration works when required.
8. Frontend functionality is connected to real APIs.
9. Backend builds successfully.
10. Frontend builds successfully.
11. Important workflows have been tested.
12. Existing integrations have not been broken.

If something cannot be completed, explicitly explain why.

---

# 16. REQUIRED FINAL REPORT

After completing each task, provide a report containing:

## A. Backend

- APIs already implemented.
- APIs newly implemented.
- APIs modified.
- Business logic completed.
- Validation and security changes.

## B. Frontend

- Pages created.
- Pages modified.
- Components created or modified.
- API integration completed.

## C. Database and Infrastructure

- Entities modified.
- Migrations created.
- RabbitMQ changes.
- Redis changes.
- Configuration changes.

## D. Testing

- Backend build result.
- Frontend build result.
- API test results.
- Integration test results.
- Known remaining issues.

## E. Completion Status

Classify each feature as:

- DONE
- PARTIAL
- NOT IMPLEMENTED
- BLOCKED

Never mark a feature as DONE without evidence.

---

# 17. FINAL PRIORITY

When making decisions, follow this priority:

1. Correct business behavior.
2. Security.
3. Data consistency.
4. Compatibility with existing services.
5. Maintainability.
6. Performance.
7. User experience.
8. Additional features.

Do not over-engineer this project.

The objective is to build a reliable, realistic, maintainable mini Shopee Marketplace suitable for a Backend ASP.NET Core portfolio project.

**These rules apply to every AI coding task in this repository.**