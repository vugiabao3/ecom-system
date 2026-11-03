# COMPLETE USERSERVICE — BACKEND + FRONTEND IMPLEMENTATION

## 1. MAIN OBJECTIVE

Your task is to thoroughly inspect, complete, improve, test, and verify the entire **UserService Backend and Frontend** of the EcomSystem project.

The goal is to deliver a production-quality UserService with:

- Complete and correctly implemented Backend APIs.
- Complete Buyer, Seller, and Admin user-management interfaces.
- A polished, modern e-commerce UI inspired by Shopee.
- A distinctive EcomSystem visual identity rather than copying Shopee.
- Secure authentication, authorization, ownership validation, and data handling.
- Correct integration with the existing microservices architecture.
- Successful Docker build and actual runtime verification.

**This is an enhancement and completion task, NOT a request to rewrite the entire service.**

Preserve all existing correct code, business logic, APIs, features, and working integrations.

You may modify existing files when necessary, but every modification must be justified by a real technical or functional requirement.

---

## 2. MANDATORY DOCUMENTATION REVIEW

Before writing any code, read these documents completely:

1. `docs/AI_RULE.md`
2. `docs/UserService.md`

Do not skim these documents.

Understand and follow all architectural conventions, coding standards, API contracts, security requirements, frontend conventions, Docker instructions, and service boundaries.

If the documentation conflicts with existing implementation:

- Inspect the actual implementation.
- Identify the exact inconsistency.
- Determine which approach is architecturally correct.
- Preserve working functionality.
- Make the smallest reasonable change.
- Update documentation only when necessary.

Do not start implementing features before understanding the existing project structure.

---

## 3. INSPECT THE EXISTING PROJECT BEFORE CODING

Inspect the complete UserService Backend and all related Frontend code.

### Backend inspection

Inspect:

- UserService.Api
- UserService.Application
- UserService.Domain
- UserService.Infrastructure
- Controllers
- Commands and Queries
- Handlers
- DTOs
- Validators
- Entities
- Interfaces
- Repository implementations
- DbContext and EF Core configurations
- Dependency Injection
- Authentication and authorization configuration
- Existing middleware
- Existing migrations
- File/avatar storage implementation
- Existing internal service communication
- Existing tests

### Frontend inspection

Inspect:

- Existing React components and pages.
- User profile pages.
- Account settings pages.
- Address management components.
- Device/session management pages.
- User activity history.
- Admin user management pages.
- API services and Axios configuration.
- Authentication context and JWT handling.
- Routing and navigation.
- Existing UI components and styling.
- Existing frontend validation and error handling.

### Important preservation rules

1. Reuse existing correct code whenever possible.
2. Do not delete existing working files simply to create new versions.
3. Do not replace a working architecture with a different architecture without a compelling reason.
4. Do not duplicate existing Commands, Queries, Handlers, DTOs, or API services.
5. Do not create duplicate API endpoints with identical responsibilities.
6. Do not remove existing features that are not explicitly part of this task.
7. Do not change unrelated microservices.
8. Do not break existing API contracts without a justified compatibility plan.
9. If an existing endpoint already satisfies a required API, improve or map it appropriately instead of implementing the same functionality twice.
10. Follow the existing code structure and naming conventions.

Before modifying a file, understand its dependencies and all places where it is used.

---

# 4. USER SERVICE RESPONSIBILITIES AND MICROSERVICE BOUNDARIES

UserService is responsible for user profile and account-related information.

It should own:

- User profile information.
- Display name and personal information.
- Avatar metadata and avatar storage integration.
- Delivery addresses.
- User devices and device metadata.
- User activity history.
- Account preferences.
- User status and account administration.
- User roles and role-management operations, according to the existing authorization architecture.
- User summary information for internal services.

It must NOT become responsible for authentication infrastructure.

### Service ownership rules

| Responsibility | Owning service |
|---|---|
| Login and registration credentials | AuthService |
| Password hashing and password changes | AuthService |
| JWT generation and validation | AuthService |
| Refresh tokens and authentication sessions | AuthService |
| Token/session revocation | AuthService |
| User profile | UserService |
| Avatar metadata and profile image | UserService |
| Delivery addresses | UserService |
| User device metadata | UserService |
| User activity and preferences | UserService |
| Product information | ProductService |
| Order information | OrderService |
| Payment information | PaymentService |
| Shipping and delivery operations | ShippingService |
| Notifications | NotificationService |

If UserService needs an operation owned by AuthService, communicate through the existing internal API or event-driven integration.

**Never implement password management, JWT generation, or refresh-token management inside UserService merely to complete an endpoint.**

For logout-all and device removal, coordinate with AuthService when authentication-session revocation is required.

---

# 5. COMPLETE BACKEND API IMPLEMENTATION

Implement and verify the following 36 API contracts.

These are the target API contracts. Some already exist or have equivalent implementations. Reuse those implementations when correct.

Do not create duplicate endpoints merely to match the list.

## GROUP 1 — USER PROFILE MANAGEMENT

### Required APIs

| Method | Endpoint | Description |
|---|---|---|
| POST | `/users/create` | Create a user profile |
| GET | `/users/me` | Get the currently authenticated user's profile |
| GET | `/users/{id}` | Get user information by ID |
| GET | `/users/{id}/profile` | Get public/user profile details |
| GET | `/users/by-email?email=` | Find a user by email, with appropriate authorization |
| PUT | `/users/me` | Update the current user's profile |
| POST | `/users/me/avatar` | Upload or update the user's avatar |
| DELETE | `/users/me/avatar` | Remove the current user's avatar |

### Implementation requirements

- Derive the current user ID from authenticated claims.
- Never trust a UserId submitted by the frontend to identify the current user.
- Validate all profile fields.
- Validate avatar file size, MIME type, extension, and content where supported.
- Generate safe unique storage names.
- Do not store large binary avatar files directly in SQL Server unless the existing architecture explicitly requires it.
- Store the appropriate file metadata and storage reference.
- Handle upload failures and cleanup safely.
- Do not expose sensitive internal user information through public profile APIs.
- Do not expose password hashes, authentication secrets, or private security data.
- Ensure users can only update their own profiles.
- Admin access must follow the existing authorization policy.

For `/users/me`, return a consistent profile DTO containing the information actually needed by the frontend.

---

## GROUP 2 — USER DELIVERY ADDRESS MANAGEMENT

### Required APIs

| Method | Endpoint | Description |
|---|---|---|
| GET | `/users/me/addresses` | Get current user's addresses |
| POST | `/users/me/addresses` | Create a delivery address |
| GET | `/users/me/addresses/{addressId}` | Get an address by ID |
| PUT | `/users/me/addresses/{addressId}` | Update an address |
| DELETE | `/users/me/addresses/{addressId}` | Delete an address |
| PATCH | `/users/me/addresses/{addressId}/default` | Set the default address |

### Implementation requirements

- Every operation must verify address ownership.
- A user must not access or modify another user's address.
- Validate recipient name, phone number, address fields, and required values.
- Support province/city, district, ward, street, and detailed address according to the existing data model.
- Only one default address per user.
- Setting a new default address must atomically unset the previous default.
- Handle concurrent updates correctly.
- Do not delete addresses belonging to other users.
- Do not break existing OrderService or ShippingService integrations.
- Preserve address snapshots already stored in existing orders.

Frontend should support:

- Address listing.
- Add address.
- Edit address.
- Delete address with confirmation.
- Set default address.
- Clearly identify the default delivery address.

---

## GROUP 3 — DEVICE AND ACCOUNT ACTIVITY MANAGEMENT

### Required APIs

| Method | Endpoint | Description |
|---|---|---|
| GET | `/users/me/devices` | List the current user's registered devices |
| POST | `/users/me/devices` | Register a device |
| DELETE | `/users/me/devices/{deviceId}` | Remove a registered device |
| POST | `/users/me/logout-all` | Request logout from all devices |
| GET | `/users/me/activity` | Get current user's activity history |

### Implementation requirements

- Derive UserId from JWT claims.
- Device operations must verify ownership.
- Never allow one user to delete another user's device.
- Avoid storing unnecessary sensitive device information.
- Use UTC timestamps.
- Support pagination for activity history.
- Sort activity records consistently.
- Prevent unauthorized access to activity data.
- Integrate with AuthService for actual session/token revocation.
- Do not pretend that deleting a device record automatically invalidates a JWT.
- If AuthService does not expose the required operation, document the integration gap instead of inventing a fake implementation.

The frontend should provide:

- Registered device list.
- Device information.
- Last active time.
- Device removal.
- Logout from all devices.
- Activity history with pagination.
- Confirmation dialogs for destructive operations.

---

## GROUP 4 — ADMIN USER MANAGEMENT

### Required APIs

| Method | Endpoint | Description |
|---|---|---|
| GET | `/admin/users` | List users |
| GET | `/admin/users/{id}` | Get user details |
| GET | `/admin/users/search` | Search and filter users |
| POST | `/admin/users/{id}/block` | Block a user |
| POST | `/admin/users/{id}/unblock` | Unblock a user |
| DELETE | `/admin/users/{id}` | Soft-delete a user |
| POST | `/admin/users/{id}/restore` | Restore a deleted user |
| PATCH | `/admin/users/{id}/role` | Change a user's role |
| DELETE | `/admin/users/{id}/roles/{role}` | Remove a role |
| GET | `/admin/users/{id}/activity` | View a user's activity |
| GET | `/admin/users/statistics` | Get user statistics |

### Implementation requirements

- All admin endpoints must require appropriate Admin authorization.
- Do not rely on frontend route protection alone.
- Validate user IDs and role names.
- Prevent unauthorized privilege escalation.
- Do not allow an ordinary user to assign themselves Admin.
- Prevent dangerous self-role removal or self-deactivation scenarios where applicable.
- Do not allow removal of the last required administrator.
- Handle role changes consistently with the existing AuthService authorization model.
- Coordinate with AuthService if role changes require JWT/session invalidation.
- Implement soft deletion where the existing data model and business requirements support it.
- Preserve related records and foreign-key integrity.
- Do not hard-delete users with existing orders or other business records.
- Record important administrative actions in the audit/activity system.
- Support pagination, sorting, searching, and filtering.
- Validate status transitions.

### User statistics

The statistics endpoint should return useful metrics based on real database data, such as:

- Total users.
- Active users.
- Blocked users.
- Deleted users.
- Users created today.
- Users created this month.
- Users grouped by role.
- Registration trends if the existing activity data supports them.

Do not fabricate statistics.

---

## GROUP 5 — ACCOUNT CONTACT INFORMATION AND PREFERENCES

### Required APIs

| Method | Endpoint | Description |
|---|---|---|
| POST | `/users/me/change-phone` | Request a phone number change |
| POST | `/users/me/change-email` | Request an email change |
| GET | `/users/me/preferences` | Get account preferences |
| PUT | `/users/me/preferences` | Update account preferences |

### Implementation requirements

- Email and phone changes must follow a secure verification process.
- Do not immediately change verified contact information without the required verification.
- AuthService should own OTP and verification-token functionality if that is how the existing architecture is designed.
- Coordinate with AuthService instead of duplicating OTP logic.
- Validate email and phone formats.
- Prevent duplicate email or phone assignments where uniqueness is required.
- Avoid exposing whether another user's private account exists through insecure public responses.
- Store preferences using a structured and maintainable model.
- Validate supported preference values.
- Do not accept arbitrary JSON that can overwrite unrelated user data.

Preferences may include, where supported by the existing application:

- Language.
- Theme.
- Marketing notification preferences.
- Order notification preferences.
- Promotion notification preferences.
- Privacy settings.
- Other account-level display preferences.

Do not duplicate NotificationService's responsibility for actual notification delivery.

---

## GROUP 6 — INTERNAL USER APIs

### Required APIs

| Method | Endpoint | Description |
|---|---|---|
| GET | `/internal/users/{id}/exists` | Check whether a user exists |
| GET | `/internal/users/{id}/summary` | Get a user summary for other services |

### Implementation requirements

These endpoints are for trusted service-to-service communication.

- Require the existing Internal authentication scheme or equivalent service authentication.
- Do not expose internal endpoints to ordinary frontend users.
- Return only the minimum information required by other services.
- Do not return passwords, tokens, secrets, or unnecessary private profile fields.
- Handle missing users consistently.
- Use efficient database queries.
- Do not introduce circular service dependencies.

Other services may need user summaries for:

- Order processing.
- Seller information.
- Shipping recipients.
- Notification recipients.
- Administrative workflows.

Respect service ownership and avoid allowing other services to modify UserService-owned data directly.

---

# 6. BACKEND ARCHITECTURE AND CODE QUALITY

Follow the existing Clean Architecture structure.

Expected flow:

`Controller → MediatR Command/Query → Handler → Interface → Infrastructure Implementation → Database / External Service`

Requirements:

1. Controllers must remain thin.
2. Business logic belongs in Application/Domain layers according to existing conventions.
3. Use Commands for operations that modify data.
4. Use Queries for read operations.
5. Reuse existing repositories and interfaces.
6. Add FluentValidation or the existing validation mechanism.
7. Use DTOs rather than exposing EF Core entities directly.
8. Configure proper dependency injection.
9. Use asynchronous database operations.
10. Use cancellation tokens where appropriate.
11. Handle exceptions consistently.
12. Return correct HTTP status codes.
13. Add pagination to list endpoints.
14. Use UTC timestamps.
15. Add database indexes where appropriate.
16. Enforce unique constraints where required.
17. Avoid N+1 queries.
18. Avoid unnecessary database round trips.
19. Avoid loading entire tables for statistics.
20. Maintain backward compatibility where practical.

### Database and migrations

- Inspect the current database schema before making changes.
- Reuse existing entities where possible.
- Do not create duplicate tables for existing concepts.
- Add only necessary entities, fields, indexes, and constraints.
- Create EF Core migrations when required.
- Ensure migrations are safe for existing Docker database volumes.
- Never delete existing user data.
- Never reset the database simply to make migrations pass.
- Never use destructive migrations without explicit justification and a safe migration plan.

### Security

Pay particular attention to:

- IDOR vulnerabilities.
- User ownership validation.
- Admin authorization.
- Internal service authentication.
- Role escalation.
- Sensitive information exposure.
- File upload security.
- Contact information verification.
- Soft deletion.
- Audit logging.
- Session revocation coordination.

---

# 7. COMPLETE USER SERVICE FRONTEND

The Frontend must not stop at a few basic profile pages.

Implement and polish all UserService-related interfaces that belong in the existing EcomSystem frontend.

First inspect the current navigation, routing, layout, API services, and authentication state. Integrate into the existing frontend rather than creating a disconnected application.

## A. BUYER ACCOUNT CENTER

Create a polished account center inspired by Shopee.

Suggested sections:

### 1. My Profile

- Profile information.
- Display name.
- Avatar.
- Email.
- Phone number.
- Gender, date of birth, and other fields supported by the backend.
- Edit profile.
- Save changes.
- Cancel editing.
- Upload avatar.
- Remove avatar.

### 2. My Addresses

- Address list.
- Default address badge.
- Add new address.
- Edit address.
- Delete address.
- Set default address.
- Form validation.
- Confirmation dialogs.

### 3. Account Security

- Registered devices.
- Last active information.
- Remove device.
- Logout from all devices.
- Change email request.
- Change phone request.
- Verification status where available.

Do not implement password changes inside UserService if they belong to AuthService.

### 4. Account Preferences

- Language.
- Theme if supported by the existing frontend.
- Notification preferences.
- Privacy settings.
- Save settings.
- Show clear success and error feedback.

### 5. Account Activity

- Activity history.
- Activity type.
- Description.
- Timestamp.
- Pagination.
- Loading and empty states.

---

## B. SELLER USER PROFILE INTERFACES

Implement Seller-related profile interfaces according to the existing EcomSystem design.

Where applicable:

- Seller profile information.
- Seller display name and avatar.
- Seller contact information that is permitted to be public.
- Seller account status.
- Seller account settings.
- Seller account activity if authorized.

Do not move ProductService, OrderService, or Seller business operations into UserService.

If the current application already has a Seller Center, integrate the profile and account settings into it without breaking existing pages.

---

## C. ADMIN USER MANAGEMENT DASHBOARD

Build a complete and polished Admin User Management interface.

Required features:

1. User overview dashboard.
2. Total user statistics.
3. Active users.
4. Blocked users.
5. Deleted users.
6. User registration statistics.
7. User listing.
8. Search by name, email, phone, or ID where supported.
9. Pagination.
10. Filtering by role.
11. Filtering by account status.
12. User detail page or detail drawer.
13. Block user.
14. Unblock user.
15. Soft-delete user.
16. Restore user.
17. Assign role.
18. Remove role.
19. View user activity.
20. Confirmation dialogs for dangerous operations.

### Admin UI requirements

- Clear distinction between Admin and normal user interfaces.
- Professional dashboard layout.
- Well-designed tables.
- Consistent status badges.
- Useful search and filter controls.
- Responsive design.
- Clear loading, error, and empty states.
- No fake statistics.
- No fake user records.
- No fake success messages.

All displayed data must come from actual APIs.

---

# 8. UI/UX DESIGN REQUIREMENTS — SHOPEE-INSPIRED, DISTINCT ECOMSYSTEM BRAND

This is a major requirement.

The interface must be visually polished and suitable for a real e-commerce platform.

Use Shopee as a design-quality reference, not as a template to copy exactly.

### Design direction

Create a modern EcomSystem identity with:

- A distinctive EcomSystem logo/brand treatment using existing project assets where available.
- A consistent primary color palette.
- Clean white surfaces.
- Carefully designed cards.
- Subtle shadows.
- Rounded corners.
- Professional typography.
- Clear visual hierarchy.
- Consistent spacing.
- Modern icons.
- Smooth but restrained interactions.
- Well-designed forms and tables.
- Responsive layouts.

### Buyer interface

The Buyer account center should feel like a premium e-commerce account experience.

Suggested layout:

- EcomSystem header.
- Main navigation consistent with the existing website.
- Account sidebar.
- Profile content panel.
- Clearly separated settings sections.
- Modern address cards.
- Avatar upload area.
- Account security cards.
- Activity timeline.

### Admin interface

The Admin interface should look like a professional management dashboard rather than a buyer-facing page.

Use:

- Dedicated Admin navigation.
- Sidebar.
- Dashboard overview cards.
- Statistics.
- User data table.
- Search and filter toolbar.
- Detail drawer or detail page.
- Clear action buttons.
- Confirmation dialogs.

### Responsive requirements

Support:

- Desktop.
- Laptop.
- Tablet.
- Mobile.

### UX requirements

Every interaction must work.

Examples:

- Clicking Edit opens a working form.
- Saving calls the real API.
- Validation errors appear beside the relevant fields.
- Uploading an avatar actually calls the backend.
- Deleting an address updates the displayed list.
- Setting a default address updates the correct address.
- Blocking a user updates the Admin list after successful API response.
- Pagination loads the correct page.
- Search uses the actual backend endpoint.

Do not create decorative buttons that do nothing.

Do not use mock data to simulate completed features.

Do not show a success message before the backend confirms success.

---

# 9. FRONTEND CODE QUALITY

Follow the existing React + TypeScript + Vite architecture.

Requirements:

- Reuse existing components.
- Reuse the existing Axios/API configuration.
- Reuse authentication context.
- Reuse existing design tokens and UI components.
- Keep API logic separate from page components.
- Use typed request and response models.
- Use proper error handling.
- Handle expired authentication.
- Prevent duplicate form submissions.
- Show loading states.
- Show empty states.
- Show meaningful API error messages.
- Validate forms.
- Avoid unnecessary duplicated components.
- Avoid oversized components where separation is justified.
- Keep existing routes working.
- Avoid introducing unnecessary dependencies.

Never hardcode API responses or user records.

Never expose internal service credentials in frontend code.

Do not store sensitive authentication information in unsafe locations.

---

# 10. API INTEGRATION AND MICROSERVICE COMPATIBILITY

Verify the integration with existing EcomSystem services.

In particular:

- AuthService: authentication, JWT claims, roles, OTP, session revocation.
- ProductService: Seller/profile information only where needed.
- OrderService: user ownership and existing order snapshots.
- ShippingService: delivery address usage.
- NotificationService: notification preferences and related settings.

Important:

- Do not directly access another service's database.
- Do not duplicate another service's business logic.
- Use existing service-to-service APIs or RabbitMQ events where appropriate.
- Do not add RabbitMQ for operations that are simple synchronous profile reads or updates.
- Do not create unnecessary dependencies between services.
- Preserve existing API contracts and frontend authentication behavior.

---

# 11. TESTING REQUIREMENTS — DO NOT STOP AT WRITING CODE

You must test the actual implementation.

A successful compilation alone is NOT sufficient.

## Backend tests

Test all implemented API groups.

At minimum, verify:

### Profile

- Create a user profile.
- Get the current user's profile.
- Update the current user's profile.
- Get another user's public profile according to authorization rules.
- Reject unauthorized profile modifications.

### Addresses

- Add an address.
- Get addresses.
- Update an address.
- Delete an address.
- Set default address.
- Verify that only one default address exists.
- Attempt to access another user's address and verify rejection.

### Devices and activity

- Register a device.
- List devices.
- Remove an owned device.
- Reject removal of another user's device.
- Retrieve paginated activity history.
- Verify logout-all integration with AuthService.

### Admin

- List users.
- Search users.
- Filter users.
- Get user details.
- Block a user.
- Unblock a user.
- Soft-delete a user.
- Restore a user.
- Assign a role.
- Remove a role.
- Get user statistics.
- Verify normal users cannot access Admin endpoints.

### Contact information and preferences

- Submit an email change request.
- Submit a phone change request.
- Verify validation behavior.
- Get preferences.
- Update preferences.
- Verify invalid preference values are rejected.

### Internal APIs

- Check whether a user exists.
- Retrieve a user summary.
- Verify unauthenticated external requests are rejected.

## Frontend tests

Verify the actual UI and interactions:

- Profile loads correctly.
- Profile updates correctly.
- Avatar upload works.
- Address CRUD works.
- Default address works.
- Device management works.
- Account activity displays real API data.
- Preferences save correctly.
- Admin user list loads.
- Search and filters work.
- Pagination works.
- Block/unblock works.
- Soft-delete/restore works.
- Role management works.
- Unauthorized pages and actions are protected.

Test loading, empty, error, success, and validation states.

## Regression testing

This is mandatory.

Verify that existing EcomSystem features remain functional.

Do not break:

- Login.
- Registration.
- JWT authentication.
- Existing Buyer navigation.
- Existing Seller navigation.
- Existing Admin navigation.
- Existing product pages.
- Existing cart and checkout.
- Existing order pages.
- Existing shipping address integrations.
- Existing service-to-service communication.

If a test reveals an existing unrelated problem, document it clearly instead of making broad unrelated changes.

---

# 12. DOCKER BUILD AND RUNTIME VERIFICATION — MANDATORY

The project already has Docker configuration for the entire EcomSystem system.

You MUST use the existing Docker Compose configuration.

### If you modify Backend C# code or Frontend code:

After completing all code changes, run:

`docker compose up --build`

Run this command from the project root containing the main `docker-compose.yml` or `compose.yaml`.

Wait for the build and containers to finish starting.

Do not stop after seeing a successful build message.

Inspect:

- Docker build output.
- Container status.
- Backend startup logs.
- Frontend startup logs.
- Database connection.
- EF Core migration behavior.
- API availability.
- Frontend accessibility.
- Runtime exceptions.
- Browser console errors.
- Network/API errors.

Then open the actual running Frontend and inspect the completed UserService interfaces.

Verify that the pages render correctly and that real API interactions work.

If a problem is found:

1. Identify its root cause.
2. Fix it without unnecessarily changing unrelated code.
3. Rebuild using `docker compose up --build`.
4. Retest the affected functionality.
5. Repeat until the implementation is working or a genuine environmental blocker is identified.

### For ordinary tests without code changes:

Use:

`docker compose up`

Then execute the required API and runtime tests.

### Critical data protection rules

NEVER run:

`docker compose down -v`

NEVER delete Docker volumes.

NEVER delete the existing SQL Server database.

NEVER reset or wipe user data to make tests pass.

NEVER remove unrelated Docker containers or services.

Do not perform destructive database operations.

Preserve all existing database data.

### Resource awareness

The development environment has limited disk space and internet bandwidth.

Therefore:

- Reuse existing Docker images where possible.
- Avoid unnecessary dependency upgrades.
- Avoid unnecessary image pulls.
- Avoid rebuilding unrelated services if a targeted build is sufficient for diagnosis.
- However, the final verification after Backend or Frontend changes MUST use the requested full-system command: `docker compose up --build`.

If Docker cannot build or start because of a genuine environmental issue, report the exact error. Do not claim runtime verification succeeded.

---

# 13. DEFINITION OF DONE

The task is complete only when all applicable requirements below have been satisfied.

### Backend

- [ ] Read `docs/AI_RULE.md` completely.
- [ ] Read `docs/UserService.md` completely.
- [ ] Inspected the existing Backend architecture.
- [ ] Preserved correct existing code.
- [ ] Implemented or verified all 36 API contracts.
- [ ] Fixed real authorization and ownership issues.
- [ ] Implemented correct validation.
- [ ] Preserved Clean Architecture and MediatR conventions.
- [ ] Database changes and migrations are safe.
- [ ] No unnecessary duplicate endpoints or business logic.
- [ ] Backend builds successfully.
- [ ] Relevant API tests pass.

### Frontend

- [ ] Completed Buyer account center.
- [ ] Completed profile management.
- [ ] Completed avatar management.
- [ ] Completed address management.
- [ ] Completed device management.
- [ ] Completed account activity.
- [ ] Completed account preferences.
- [ ] Completed contact change workflows where supported.
- [ ] Completed Admin user management.
- [ ] Completed user statistics dashboard.
- [ ] All relevant interfaces use real APIs.
- [ ] All buttons and forms work.
- [ ] UI is polished and responsive.
- [ ] Existing navigation and pages remain functional.

### Docker and runtime

- [ ] Ran `docker compose up --build` after Backend/Frontend code changes.
- [ ] Verified containers are running.
- [ ] Verified Backend startup and database connection.
- [ ] Verified Frontend is accessible.
- [ ] Tested actual API requests.
- [ ] Inspected the actual running UI.
- [ ] Checked logs and browser console.
- [ ] Fixed discovered issues or documented genuine blockers.
- [ ] Preserved existing Docker volumes and database data.

---

# 14. FINAL REPORT

When finished, provide a clear and honest report.

Include:

1. Summary of completed Backend work.
2. Summary of completed Frontend work.
3. The 36 API contracts and their status:
   - Already existed and verified.
   - Improved/fixed.
   - Newly implemented.
   - Blocked by another service or missing infrastructure.
4. List of created files.
5. List of modified files.
6. Important security fixes.
7. Database changes and migrations.
8. Tests executed and actual results.
9. Docker build result.
10. Container status.
11. Actual Frontend pages inspected.
12. Remaining issues or integration blockers.

Do not claim that something works unless you actually tested it.

Do not mark an API as completed simply because a controller method exists. Verify its behavior, authorization, and integration.

Do not mark the Frontend as completed simply because the page compiles. Verify its real interactions.

**FINAL PRIORITY: Deliver a complete, secure, maintainable, and visually polished UserService while preserving all correct existing EcomSystem functionality. Do not unnecessarily delete, rewrite, or break working code.**