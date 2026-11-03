# COMPLETE AUTHSERVICE — BACKEND + FRONTEND IMPLEMENTATION

## 1. MAIN OBJECTIVE

Thoroughly inspect, complete, improve, test, and verify the entire **AuthService Backend and Frontend** of the EcomSystem project.

The goal is to deliver a secure, production-quality authentication system with:

- Complete registration and login workflows.
- Secure JWT authentication.
- Refresh token rotation and session management.
- Logout and logout-all functionality.
- Password change and password recovery.
- Email verification.
- Google OAuth authentication.
- Admin role and account activation management.
- Secure authentication integration across all EcomSystem microservices.
- Complete, polished, responsive authentication interfaces.
- A distinctive EcomSystem visual identity inspired by Shopee's e-commerce experience.
- Successful Docker build and actual runtime verification.

**This is an enhancement and completion task, NOT a request to rewrite the entire AuthService.**

Preserve all existing correct code, business logic, APIs, authentication flows, JWT contracts, database data, and working integrations.

You may modify existing files when necessary, but every modification must be technically justified.

Do not remove working features merely to simplify implementation.

---

# 2. MANDATORY DOCUMENTATION REVIEW

Before writing any code, read these documents completely:

1. `docs/AI_RULE.md`
2. `docs/AuthService.md`

Do not skim them.

Understand:

- Existing authentication architecture.
- Current JWT configuration.
- Access token and refresh token lifecycle.
- Authentication schemes.
- Role and policy configuration.
- Database schema.
- Existing Commands, Queries, Handlers, DTOs, and repositories.
- Google OAuth configuration.
- Frontend authentication flow.
- Docker configuration.
- Existing microservice authentication integration.

If documentation and implementation differ:

1. Inspect the actual implementation.
2. Identify the inconsistency.
3. Determine the correct architecture.
4. Preserve working behavior.
5. Make the smallest reasonable change.
6. Update documentation when necessary.

Do not start coding before understanding the existing project.

---

# 3. INSPECT THE EXISTING CODE BEFORE MODIFYING IT

## Backend inspection

Inspect the complete AuthService structure:

- AuthService.Api
- AuthService.Application
- AuthService.Domain
- AuthService.Infrastructure
- Controllers
- Commands and Queries
- Handlers
- DTOs
- Validators
- Entities
- Interfaces
- Repository implementations
- DbContext
- EF Core configurations
- Dependency Injection
- JWT configuration
- Authentication middleware
- Authorization policies
- Refresh token implementation
- Password hashing
- Email services
- Google OAuth integration
- Existing migrations
- Existing tests

## Frontend inspection

Inspect:

- Login pages.
- Registration pages.
- Forgot password pages.
- Reset password pages.
- Profile/authentication state.
- Google login integration.
- Axios interceptors.
- Token storage and refresh logic.
- Protected routes.
- Role-based navigation.
- Logout functionality.
- Existing API services.
- Existing forms and validation.
- Existing routing and design system.

## Mandatory preservation rules

1. Reuse existing correct implementations.
2. Do not delete working files simply to replace them.
3. Do not duplicate existing authentication logic.
4. Do not introduce a second JWT implementation.
5. Do not change token formats without checking every dependent microservice.
6. Do not unnecessarily change JWT claims, issuer, audience, signing key configuration, or token lifetime.
7. Do not break existing API contracts.
8. Do not remove existing authentication features.
9. Do not modify unrelated microservices unless a real integration fix is necessary.
10. Reuse existing code conventions and project templates.

Before modifying authentication code, understand its impact on the entire EcomSystem.

---

# 4. AUTHSERVICE RESPONSIBILITIES AND SERVICE BOUNDARIES

AuthService is the central authentication and identity-security service.

It owns:

- User registration authentication workflow.
- Login.
- Password hashing and verification.
- Access token generation.
- JWT validation configuration.
- Refresh tokens.
- Refresh token rotation.
- Logout.
- Session revocation.
- Password change.
- Forgot password.
- Password reset.
- Email verification.
- Google OAuth authentication.
- Authentication-related account security.
- Authentication-related role and activation operations, according to the existing architecture.

### Microservice responsibility boundaries

| Responsibility | Owning service |
|---|---|
| Login and authentication | AuthService |
| Password hashing and reset | AuthService |
| JWT and refresh tokens | AuthService |
| Authentication sessions | AuthService |
| Email verification tokens | AuthService |
| Google OAuth | AuthService |
| User profile and avatar | UserService |
| Delivery addresses | UserService |
| Product information | ProductService |
| Cart | CartService |
| Orders | OrderService |
| Payment transactions | PaymentService |
| Shipping and delivery | ShippingService |
| Inventory | InventoryService |
| Notifications | NotificationService |

### Important rules

- Do not duplicate UserService profile management.
- Do not store profile information that belongs to UserService unless the existing authentication model requires a minimal identity record.
- Coordinate with UserService during registration and account creation.
- Avoid circular dependencies between AuthService and UserService.
- Use existing internal APIs or RabbitMQ events where appropriate.
- Do not allow other services to access AuthService's database directly.
- Keep authentication logic centralized.

---

# 5. COMPLETE BACKEND API IMPLEMENTATION

Implement and verify all 14 target API contracts.

Existing APIs must be inspected and tested rather than blindly rewritten.

## GROUP 1 — REGISTRATION AND LOGIN

### 1. Register

`POST /auth/register`

Requirements:

- Validate registration data.
- Validate email and password.
- Check duplicate accounts.
- Hash passwords using a secure password-hashing mechanism.
- Never store plaintext passwords.
- Never return password hashes.
- Create the required authentication identity.
- Coordinate with UserService to create the corresponding user profile.
- Handle partial failure between identity creation and profile creation safely.
- Use a reliable integration approach, such as an event/outbox workflow, when appropriate.
- Return a safe response.
- Do not automatically mark email as verified unless the verification policy explicitly allows it.

### 2. Login

`POST /auth/login`

Requirements:

- Validate credentials.
- Verify password hashes securely.
- Reject blocked, inactive, deleted, or unverified accounts according to the existing business policy.
- Generate a valid access token.
- Generate a secure refresh token.
- Store refresh token metadata safely.
- Return the token response expected by the existing frontend.
- Avoid revealing whether an email or password is incorrect through unnecessarily detailed error messages.
- Apply appropriate rate limiting or brute-force protection where supported.

### 3. Refresh token

`POST /auth/refresh`

Requirements:

- Validate refresh tokens.
- Implement secure refresh token rotation.
- Revoke or invalidate the previous refresh token after successful rotation.
- Detect reuse of revoked tokens where supported.
- Prevent concurrent refresh requests from creating inconsistent sessions.
- Handle expired, revoked, malformed, or invalid refresh tokens.
- Never trust an arbitrary UserId supplied by the frontend.
- Ensure token revocation works across the application.

### 4. Logout

`POST /auth/logout`

Requirements:

- Revoke the current refresh token/session.
- Handle repeated logout requests safely.
- Clear the frontend authentication state.
- Do not claim that deleting frontend tokens alone invalidates a server-side refresh token.
- Preserve the existing API contract if the current endpoint uses a different request format.

### 5. Current authentication profile

`GET /auth/profile`

Requirements:

- Require authentication.
- Derive the user identity from JWT claims.
- Return safe identity information.
- Include appropriate roles and verification status where supported.
- Do not expose password hashes, refresh tokens, or secrets.
- Integrate with UserService when complete profile data is required.
- Do not duplicate UserService's profile responsibilities.

### 6. Change password

`POST /auth/change-password`

Requirements:

- Require authentication.
- Verify the current password.
- Validate the new password.
- Prevent inappropriate reuse of previous passwords if the existing security policy supports it.
- Hash the new password securely.
- Revoke other sessions or refresh tokens according to the security policy.
- Never return the password or its hash.
- Do not log password values.

---

## GROUP 2 — PASSWORD RECOVERY

### 7. Forgot password

`POST /auth/forgot-password`

Requirements:

- Accept the required account identifier, such as email.
- Generate a secure, random, time-limited reset token.
- Store only a secure token representation/hash where appropriate.
- Send a password-reset email using the existing email infrastructure.
- Avoid revealing whether an account exists.
- Prevent unlimited reset requests.
- Make tokens single-use.
- Never send passwords by email.
- Never return reset secrets in production responses.

### 8. Reset password

`POST /auth/reset-password`

Requirements:

- Validate the reset token.
- Check expiration.
- Reject already-used or revoked tokens.
- Validate the new password.
- Hash the new password securely.
- Mark the reset token as used.
- Revoke existing refresh sessions where required.
- Prevent token replay.
- Return appropriate error responses.

---

## GROUP 3 — GOOGLE OAUTH

### 9. Google OAuth

`POST /auth/oauth/google`

Requirements:

- Use the existing Google OAuth configuration.
- Verify Google ID tokens on the backend.
- Validate issuer, audience, signature, expiration, and relevant claims.
- Never trust an email or Google UserId sent by the frontend without token verification.
- Prevent account takeover through unsafe email-based account linking.
- Handle first-time Google registration.
- Handle existing-account linking according to a secure policy.
- Create or retrieve the corresponding UserService profile safely.
- Issue EcomSystem access and refresh tokens after successful authentication.
- Do not expose Google client secrets in frontend code.
- Handle cancelled or invalid OAuth flows correctly.

If Google OAuth configuration is missing or invalid, report the exact configuration requirement. Do not fake a successful Google login.

---

## GROUP 4 — ADMIN AUTHENTICATION AND ACCOUNT MANAGEMENT

### 10. Set role

`POST /api/admin/set-role`

Requirements:

- Admin-only authorization.
- Validate target user and role.
- Prevent ordinary users from changing roles.
- Prevent privilege escalation.
- Protect the last required administrator account.
- Coordinate role changes with UserService if role assignment is owned or synchronized there.
- Ensure updated permissions take effect according to the JWT/session invalidation strategy.
- Audit important administrative actions.

### 11. Set active status

`POST /api/admin/set-active`

Requirements:

- Admin-only authorization.
- Validate the target account.
- Support safe activation and deactivation.
- Prevent users from changing their own account status unless explicitly authorized.
- Revoke active sessions when deactivating an account where required.
- Ensure deactivated accounts cannot continue authenticating through refresh tokens.
- Coordinate account status with UserService.
- Record administrative changes.

Do not allow arbitrary frontend requests to change roles or account activation.

---

## GROUP 5 — EMAIL VERIFICATION

### 12. Verify email

`POST /auth/verify-email`

This endpoint must be implemented.

Requirements:

- Accept the verification token or verification data according to the existing design.
- Validate the token securely.
- Verify expiration.
- Ensure tokens are single-use.
- Mark the account's email as verified.
- Handle already-verified accounts safely.
- Handle invalid, expired, and reused tokens.
- Synchronize email verification status with UserService if required.
- Return clear but safe responses.

### 13. Resend verification email

`POST /auth/resend-verification`

This endpoint must be implemented.

Requirements:

- Identify the intended account securely.
- Generate a new verification token.
- Invalidate or safely supersede previous tokens.
- Send a new verification email.
- Apply rate limiting and resend cooldown.
- Avoid exposing whether arbitrary email addresses have registered.
- Do not send unlimited verification emails.
- Do not mark email as verified merely because a resend request succeeded.

The verification flow should work from registration through email confirmation.

---

## GROUP 6 — LOGOUT ALL DEVICES

### 14. Logout all

`POST /auth/logout-all`

This endpoint is recommended and should be implemented if the existing architecture supports it.

Requirements:

- Require authentication.
- Derive the current user ID from JWT claims.
- Revoke all refresh tokens/sessions belonging to the current user.
- Handle concurrent sessions correctly.
- Do not affect other users.
- Return a clear success response.
- Ensure revoked refresh tokens cannot be reused.
- Coordinate with UserService device metadata if required.

The frontend should clear the current authentication state and redirect to the login page.

---

# 6. AUTHENTICATION SECURITY REQUIREMENTS

Authentication security is the highest priority.

## Password security

- Use a secure password hashing mechanism such as ASP.NET Core PasswordHasher or an already established secure implementation.
- Never use plaintext password storage.
- Never use reversible encryption as password hashing.
- Never log passwords.
- Never return password hashes.
- Validate password strength according to the existing policy.

## JWT security

Inspect the existing JWT implementation before making changes.

Verify:

- Signing algorithm.
- Signing key management.
- Issuer.
- Audience.
- Expiration.
- Claims.
- Role claims.
- Subject/UserId claim.
- Authentication scheme compatibility.

Do not change JWT claim names without checking all dependent services.

All microservices must continue validating tokens correctly.

Never hardcode production signing secrets.

Use the existing configuration and Docker environment-variable approach.

## Refresh token security

- Use cryptographically secure random generation.
- Store token hashes or secure representations where appropriate.
- Implement rotation.
- Implement revocation.
- Handle expiration.
- Detect token reuse when possible.
- Ensure refresh token ownership is correct.
- Prevent cross-account token reuse.
- Handle concurrent refresh operations safely.

## Email token security

Verification and password-reset tokens must:

- Be cryptographically secure.
- Have limited validity.
- Be single-use.
- Be invalidated after successful use.
- Be protected against brute-force attempts.
- Avoid leaking account existence.

## Rate limiting and abuse prevention

Apply appropriate protection to:

- Login.
- Registration.
- Forgot password.
- Resend verification.
- Google OAuth.
- Token refresh.

Use existing infrastructure where available.

Do not introduce arbitrary aggressive limits that break normal user workflows.

## Authentication failure handling

Return consistent errors without exposing:

- Password hashes.
- Refresh token hashes.
- JWT signing keys.
- Internal secrets.
- Sensitive database information.
- Stack traces in production.

---

# 7. BACKEND ARCHITECTURE AND CODE QUALITY

Follow the existing Clean Architecture and MediatR conventions.

Expected flow:

`Controller → MediatR Command/Query → Handler → Interface → Infrastructure Implementation → Database / External Service`

Requirements:

1. Keep controllers thin.
2. Keep business logic out of controllers.
3. Use Commands for state-changing operations.
4. Use Queries for read operations.
5. Reuse existing handlers and repositories.
6. Use DTOs instead of exposing database entities.
7. Validate request data.
8. Use asynchronous database operations.
9. Use CancellationToken where appropriate.
10. Configure Dependency Injection correctly.
11. Handle exceptions consistently.
12. Return correct HTTP status codes.
13. Avoid duplicated authentication logic.
14. Use transactions where appropriate.
15. Handle concurrency and token rotation safely.
16. Add indexes and constraints where necessary.
17. Keep secrets in configuration, not source code.
18. Maintain backward compatibility where practical.

### Database and migrations

Inspect the existing database schema first.

Only add necessary:

- Entities.
- Columns.
- Indexes.
- Constraints.
- Refresh token metadata.
- Verification token metadata.
- Password-reset token metadata.
- Session records.

Do not create duplicate entities or tables if equivalent implementations already exist.

Create EF Core migrations only when needed.

Ensure migrations preserve existing accounts and sessions safely.

NEVER reset or wipe the existing database to make migrations pass.

---

# 8. COMPLETE AUTHENTICATION FRONTEND

Build a complete authentication experience integrated into the existing EcomSystem React + TypeScript + Vite frontend.

Do not create a separate disconnected frontend.

Inspect the existing design system and authentication implementation first.

## A. LOGIN PAGE

Create a polished login page with:

- EcomSystem branding.
- Email or username field according to the existing API.
- Password field.
- Show/hide password.
- Remember-me behavior only if supported safely.
- Forgot password link.
- Login button.
- Google login button.
- Registration link.
- Validation messages.
- Loading state.
- Authentication error feedback.

Requirements:

- Call the actual AuthService login API.
- Store and manage tokens using the existing secure architecture.
- Integrate with Axios interceptors.
- Handle expired access tokens.
- Handle refresh token rotation.
- Prevent duplicate login submissions.
- Redirect according to the user's role and existing routing rules.

Do not hardcode demo credentials.

## B. REGISTRATION PAGE

Create a complete registration page with:

- Full name where required.
- Email.
- Phone number if required by the existing API.
- Password.
- Confirm password.
- Show/hide password.
- Password validation feedback.
- Terms acceptance if required by the existing application.
- Register button.
- Google registration/login entry point where supported.
- Login link.

After registration:

- Show the correct email verification state.
- Explain that the user needs to verify their email if required.
- Provide a resend verification action.
- Do not pretend that an account is verified before confirmation.

## C. FORGOT PASSWORD PAGE

Provide:

- Email input.
- Submit reset request.
- Loading state.
- Safe confirmation message.
- Error handling.
- Link back to login.

The message should not reveal whether an email address is registered.

## D. RESET PASSWORD PAGE

Provide:

- New password.
- Confirm password.
- Password visibility controls.
- Password validation.
- Reset button.
- Expired/invalid token handling.
- Success state.
- Link back to login.

Read the reset token from the appropriate URL/query parameter according to the existing flow.

Never log reset tokens.

## E. EMAIL VERIFICATION PAGE

Create a verification experience that:

- Reads the verification token from the appropriate URL.
- Calls `POST /auth/verify-email`.
- Shows verification progress.
- Displays success when verified.
- Displays expired or invalid token states.
- Allows resending verification where appropriate.
- Provides a login or account continuation action.

Do not fake verification success.

## F. GOOGLE LOGIN

Integrate the actual configured Google OAuth flow.

Requirements:

- Use the existing OAuth configuration.
- Do not expose client secrets.
- Handle success, cancellation, and failure.
- Send the correct credential/token to AuthService.
- Handle the EcomSystem JWT response.
- Prevent duplicate account creation.
- Test the complete flow when credentials are available.

## G. AUTHENTICATED ACCOUNT SECURITY

Integrate authentication security features into the existing account center.

Where appropriate:

- Change password.
- View authentication status.
- Logout current session.
- Logout all devices.
- Show email verification status.
- Provide resend verification.
- Handle expired sessions.

Use UserService for profile and device metadata rather than duplicating those responsibilities in AuthService.

---

# 9. UI/UX DESIGN — SHOPEE-INSPIRED WITH DISTINCT ECOMSYSTEM BRANDING

The authentication interface must be polished, modern, and professional.

Use Shopee as a quality and UX reference, not as an exact copy.

### Design direction

- Distinctive EcomSystem brand identity.
- Consistent primary color palette.
- Clean white surfaces.
- Attractive login and registration cards.
- Professional typography.
- Balanced spacing.
- Modern icons.
- Subtle shadows.
- Rounded corners.
- Carefully designed form fields.
- Clear primary and secondary buttons.
- Smooth, restrained transitions.
- Responsive layout.

### Visual experience

The login and registration pages should look like a real e-commerce platform.

Consider:

- Brand panel or illustration area.
- Attractive product/e-commerce visual accents.
- Clean form hierarchy.
- Clear trust and security messaging.
- Strong visual distinction between primary and secondary actions.
- Consistent branding across login, registration, verification, and password recovery.

Do not overload the page with unnecessary decorative elements.

Do not copy Shopee's logo or exact visual assets.

Use existing EcomSystem assets where available.

### Responsive support

Support:

- Desktop.
- Laptop.
- Tablet.
- Mobile.

### Interaction quality

Every button and form must work.

Examples:

- Login calls the real API.
- Registration creates the account through the real API.
- Forgot password sends the actual reset request.
- Reset password validates and submits the token.
- Email verification calls the backend.
- Resend verification calls the backend.
- Google login uses real OAuth.
- Logout revokes the appropriate session.
- Logout-all invalidates all refresh sessions.

Do not create decorative buttons that do nothing.

Do not show success messages before receiving successful backend responses.

---

# 10. FRONTEND TOKEN AND SESSION MANAGEMENT

This section is critical.

Inspect the existing token-storage strategy before modifying it.

Do not blindly change token storage because it may break existing authentication across the entire application.

Requirements:

- Keep one consistent authentication state.
- Prevent multiple competing refresh requests.
- Implement a safe refresh flow.
- Handle refresh token rotation.
- Handle simultaneous API requests during token expiration.
- Prevent infinite refresh loops.
- Clear authentication state after refresh failure.
- Avoid retrying the same failed request indefinitely.
- Handle logout correctly.
- Handle logout-all correctly.
- Protect authenticated routes.
- Support role-based navigation.
- Preserve existing microservice API authorization behavior.

Prefer secure cookie-based refresh-token handling when the existing backend and frontend architecture supports it.

If the existing implementation uses another storage strategy, evaluate its security and compatibility before changing it.

Never expose refresh tokens in logs or URLs.

Never put access tokens or secrets in source code.

---

# 11. MICROSERVICE AUTHENTICATION COMPATIBILITY

AuthService is used by other EcomSystem microservices.

Verify compatibility with:

- UserService.
- ProductService.
- CartService.
- OrderService.
- PaymentService.
- PromotionService.
- ShippingService.
- InventoryService.
- NotificationService.
- Other existing services.

Check:

- JWT signing and validation.
- Issuer and audience.
- Claim names.
- UserId claim.
- Role claims.
- Authentication schemes.
- Internal authentication.
- Token expiration.
- Refresh behavior.
- Role changes.
- Account deactivation.
- Session revocation.

**Do not modify JWT claims or signing configuration without checking all dependent microservices.**

If a change is necessary, update all affected integration points carefully and run regression tests.

Do not make all microservices depend on direct database access to AuthService.

Use the existing authentication contract.

---

# 12. TESTING REQUIREMENTS — ACTUAL FUNCTIONAL TESTS

Compilation alone is NOT sufficient.

Test the real authentication workflows.

## Backend API tests

### Registration

- Register a new account.
- Reject invalid email.
- Reject invalid password.
- Reject duplicate registration.
- Verify password is hashed.
- Verify no password hash is returned.
- Verify corresponding UserService profile creation.
- Verify partial failure handling.

### Login

- Login with valid credentials.
- Reject invalid credentials.
- Reject inactive or blocked accounts according to policy.
- Verify access token.
- Verify refresh token.
- Verify correct JWT claims and roles.

### Refresh

- Refresh a valid token.
- Verify old token rotation/revocation.
- Reject expired tokens.
- Reject revoked tokens.
- Reject malformed tokens.
- Test concurrent refresh behavior.
- Test token reuse detection where implemented.

### Logout

- Logout current session.
- Verify revoked refresh token cannot be reused.
- Verify repeated logout behavior.

### Profile

- Get authenticated profile.
- Reject unauthenticated requests.
- Verify safe response fields.

### Change password

- Change password using valid current password.
- Reject incorrect current password.
- Reject invalid new password.
- Verify new password works.
- Verify old password no longer works.
- Verify session revocation behavior.

### Forgot/reset password

- Request password reset.
- Verify email delivery or the configured test email provider.
- Reject unknown, invalid, expired, or reused tokens safely.
- Reset password.
- Verify the new password works.
- Verify old password fails.

### Google OAuth

- Test valid Google credentials when configured.
- Reject invalid credentials.
- Reject invalid audience.
- Reject expired tokens.
- Test existing-account linking safely.
- Test first-time account creation.
- Test cancellation and failure behavior.

If Google credentials are unavailable, report that external OAuth verification is blocked by configuration. Do not fabricate a successful test.

### Email verification

- Register an account.
- Obtain the verification email through the configured test mechanism.
- Verify the email.
- Reject expired tokens.
- Reject reused tokens.
- Test resend cooldown.
- Verify account status synchronization.

### Admin

- Set role as authorized Admin.
- Reject normal user access.
- Reject invalid role.
- Test account activation/deactivation.
- Verify session invalidation when deactivated.
- Verify role changes take effect correctly.

### Logout all

- Create multiple sessions.
- Logout all.
- Verify all refresh tokens are invalidated.
- Verify other users' sessions are unaffected.

## Frontend tests

Test the actual running interface:

- Login.
- Registration.
- Password visibility.
- Forgot password.
- Reset password.
- Email verification.
- Resend verification.
- Google login.
- Authenticated route protection.
- Role-based navigation.
- Access token expiration.
- Refresh token rotation.
- Logout.
- Logout all.
- Validation and error messages.
- Responsive layout.

## Regression testing

Verify that existing EcomSystem features still work:

- Buyer login.
- Seller login.
- Admin login.
- Registration.
- Existing JWT authorization.
- Product pages.
- Cart.
- Checkout.
- Orders.
- Payment.
- Shipping.
- Admin pages.
- UserService profile integration.
- Internal service authentication.

Do not break existing features while implementing new authentication workflows.

---

# 13. DOCKER BUILD AND RUNTIME VERIFICATION — MANDATORY

The project already has Docker configuration for the entire EcomSystem system.

Use the existing Docker Compose configuration.

### After modifying Backend C# or Frontend code

After all code changes are complete, run:

`docker compose up --build`

Run this command from the project root containing the main Docker Compose file.

Wait for the build and containers to finish starting.

Do not stop after a successful compilation.

Inspect:

- Docker build output.
- Container status.
- AuthService startup logs.
- Frontend startup logs.
- Database connection.
- EF Core migration behavior.
- Email configuration.
- Google OAuth configuration.
- API availability.
- JWT validation.
- Authentication failures.
- Frontend console errors.
- Network/API errors.

Then open the actual running Frontend and inspect the completed authentication interfaces.

Test real login, registration, logout, and other configured workflows.

If a problem is discovered:

1. Identify its root cause.
2. Fix it without unnecessary unrelated changes.
3. Run `docker compose up --build` again.
4. Retest the affected flows.
5. Continue until successful or a genuine environmental blocker is identified.

### For ordinary tests without code changes

Use:

`docker compose up`

Then perform the required API and runtime tests.

### Critical data protection rules

NEVER run:

`docker compose down -v`

NEVER delete Docker volumes.

NEVER delete the existing SQL Server database.

NEVER reset or wipe existing accounts or authentication data to make tests pass.

NEVER remove unrelated Docker services or containers.

Do not perform destructive database operations.

### Resource awareness

The development machine has limited disk space and internet bandwidth.

Therefore:

- Reuse existing Docker images.
- Avoid unnecessary dependency upgrades.
- Avoid unnecessary image pulls.
- Avoid rebuilding unrelated services for every small diagnostic test.
- However, the final verification after Backend or Frontend changes MUST run the requested full-system command: `docker compose up --build`.

If Docker cannot build or start due to a genuine environmental issue, report the exact error. Do not claim successful runtime verification.

---

# 14. DEFINITION OF DONE

The task is complete only when all applicable requirements are satisfied.

## Backend

- [ ] Read `docs/AI_RULE.md` completely.
- [ ] Read `docs/AuthService.md` completely.
- [ ] Inspected the entire existing AuthService.
- [ ] Preserved all correct existing code.
- [ ] Verified all 14 target API contracts.
- [ ] Implemented email verification.
- [ ] Implemented resend verification.
- [ ] Implemented logout-all where supported.
- [ ] Verified JWT compatibility.
- [ ] Verified refresh token security.
- [ ] Verified password security.
- [ ] Verified Google OAuth configuration and behavior.
- [ ] Verified Admin authorization.
- [ ] Verified UserService integration.
- [ ] Safe database migrations.
- [ ] Backend builds successfully.
- [ ] Relevant API tests pass.

## Frontend

- [ ] Completed Login page.
- [ ] Completed Registration page.
- [ ] Completed Forgot Password page.
- [ ] Completed Reset Password page.
- [ ] Completed Email Verification page.
- [ ] Completed Resend Verification workflow.
- [ ] Completed Google login integration.
- [ ] Completed Logout workflow.
- [ ] Completed Logout All workflow.
- [ ] Completed authentication state management.
- [ ] Completed token refresh handling.
- [ ] Protected routes work correctly.
- [ ] Role-based navigation works correctly.
- [ ] Responsive design works.
- [ ] All relevant interfaces use real APIs.
- [ ] No fake authentication success.
- [ ] Existing application navigation remains functional.

## Docker and runtime

- [ ] Ran `docker compose up --build` after Backend/Frontend changes.
- [ ] Verified containers are running.
- [ ] Verified AuthService connects to its database.
- [ ] Verified Frontend is accessible.
- [ ] Tested actual authentication APIs.
- [ ] Inspected actual running authentication pages.
- [ ] Checked browser console and network errors.
- [ ] Verified microservice JWT compatibility.
- [ ] Preserved Docker volumes and database data.

---

# 15. FINAL REPORT

When finished, provide an honest and detailed report containing:

1. Backend implementation summary.
2. Frontend implementation summary.
3. Status of all 14 API contracts:
   - Already existed and verified.
   - Improved/fixed.
   - Newly implemented.
   - Blocked by configuration or external service.
4. Created files.
5. Modified files.
6. Security improvements.
7. JWT and refresh token changes.
8. Database changes and migrations.
9. Google OAuth test results.
10. Email delivery and verification test results.
11. Backend test results.
12. Frontend test results.
13. Docker build result.
14. Container status.
15. Actual pages and flows inspected.
16. Remaining issues and configuration requirements.

Never claim that an API works simply because its controller exists.

Never claim Google OAuth works if no real credentials were configured and tested.

Never claim email verification works if the email delivery flow was not tested.

Never claim Docker runtime verification succeeded unless you actually ran the containers and checked them.

**FINAL PRIORITY: Deliver a secure, reliable, complete, maintainable AuthService with a polished EcomSystem authentication experience, while preserving all correct existing authentication functionality and keeping every dependent microservice compatible.**