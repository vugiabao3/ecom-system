# TASK: COMPLETE REVIEW SERVICE — BACKEND + FRONTEND IMPLEMENTATION

You are a senior .NET Microservices Architect, ASP.NET Core Backend Engineer, React/TypeScript Frontend Engineer, Database Engineer, and UI/UX Designer.

Your task is to thoroughly inspect, complete, integrate, test, and polish the entire **ReviewService** in the EcomSystem project, including both Backend and Frontend.

The goal is to deliver a production-quality product and shop review system inspired by Shopee's review experience, while maintaining EcomSystem's own distinctive visual identity.

**IMPORTANT: Do not stop after generating code or making the application compile. You must run the application, test the actual APIs and workflows, inspect the real frontend, fix discovered problems, and verify the final result.**

---

# 1. MANDATORY DOCUMENTATION — READ BEFORE CODING

Before modifying any code:

1. Read the entire `docs/AI_RULE.md`.
2. Read the entire `docs/ReviewService.md`.
3. Inspect the existing ReviewService Backend project structure.
4. Inspect the existing Frontend project structure.
5. Inspect the existing service implementations and sample code from other services.
6. Inspect the existing API clients, routing, authentication, authorization, shared components, layouts, and UI conventions.
7. Inspect `docker-compose.yml` and the existing Docker configuration.
8. Inspect the existing database configuration, migrations, RabbitMQ configuration, Redis configuration, and service-to-service authentication.

Do not begin implementation until you understand the existing architecture and conventions.

**Existing code is the primary reference.**

Use the existing working implementations as coding templates. Follow the same conventions for:

- Clean Architecture.
- Domain entities.
- Application layer.
- Commands and Queries.
- MediatR handlers.
- DTOs and validators.
- Repository interfaces and implementations.
- Dependency injection.
- API controllers.
- Authentication and authorization.
- EF Core configurations and migrations.
- RabbitMQ publishers and consumers.
- React components and API services.
- Error handling and response formats.

Do not introduce a completely different architecture just because you prefer it.

---

# 2. CRITICAL CODE PRESERVATION RULES

These rules are mandatory.

1. Do not delete existing files simply to make implementation easier.
2. Do not rewrite entire working services or modules unnecessarily.
3. Do not remove existing features that already work correctly.
4. Do not replace correct implementations with simplified placeholder implementations.
5. Do not change unrelated services or their APIs unless integration genuinely requires it.
6. Do not rename existing classes, DTOs, properties, endpoints, or database tables without a clear technical reason.
7. Reuse existing entities, repositories, handlers, interfaces, DTOs, API clients, and React components wherever appropriate.
8. If an existing implementation is incomplete or incorrect, modify only the necessary parts.
9. Preserve existing API contracts whenever possible.
10. If a breaking change is genuinely necessary, inspect every dependent service and update all affected integrations consistently.
11. Do not duplicate existing functionality by creating unnecessary controllers, handlers, repositories, or endpoints.
12. Do not introduce mock data or fake success responses into production workflows.
13. Do not silently ignore errors to make tests pass.
14. Never claim a feature works unless you have actually tested it.

Before changing a file, understand its current responsibility and its dependencies.

**The objective is to complete and improve the existing EcomSystem, not rebuild it from scratch.**

---

# 3. REVIEW SERVICE RESPONSIBILITIES AND MICROSERVICE BOUNDARIES

ReviewService owns:

- Product reviews.
- Shop/Seller reviews.
- Star ratings.
- Review content and moderation status.
- Review images and videos metadata.
- Helpful votes.
- Seller responses.
- Review reports.
- Review eligibility verification workflow.
- Product and shop rating aggregates.
- Review analytics.
- Review-related domain events.

Respect the ownership boundaries of other services:

| Service | Responsibility |
|---|---|
| AuthService | Authentication, JWT, credentials |
| UserService | User profiles, Seller identity and shop information |
| ProductService | Product and variant information |
| OrderService | Orders, order items, delivery status, purchase ownership |
| ReviewService | Reviews, ratings, moderation, helpful votes |
| NotificationService | Sending notifications |
| PaymentService | Payments and refunds |
| InventoryService | Stock and inventory |

Important:

- ReviewService must not directly modify OrderService data.
- ReviewService must not directly modify ProductService data.
- ReviewService must not manage user credentials.
- ReviewService must not manage payment or inventory operations.
- Cross-service communication must use the existing API or messaging architecture.
- Never share another service's database context or directly write to its database.

---

# 4. COMPLETE BACKEND IMPLEMENTATION

Implement all necessary Backend functionality according to `docs/ReviewService.md` and the API contracts below.

The API list is the target functional scope, not an instruction to create redundant endpoints. Consolidate routes when existing architecture already supports equivalent functionality, but do not silently omit required capabilities.

## GROUP 1 — Product Reviews

Implement:

- POST `/api/reviews`
- GET `/api/reviews`
- GET `/api/reviews/{id}`
- PUT `/api/reviews/{id}`
- DELETE `/api/reviews/{id}`
- GET `/api/products/{productId}/reviews`
- GET `/api/products/{productId}/reviews/summary`
- GET `/api/products/{productId}/reviews/distribution`
- GET `/api/products/{productId}/reviews/recent`
- GET `/api/products/{productId}/reviews/top`
- GET `/api/products/{productId}/reviews/with-media`
- GET `/api/products/{productId}/reviews/filter`

Required functionality:

- Create, update, and delete product reviews.
- Filter and paginate reviews.
- Filter by star rating, variant, date, and media.
- Sort by newest, oldest, rating, and helpfulness.
- Display average rating and total review count.
- Display the distribution of 1–5 star ratings.
- Display reviews containing photos or videos.
- Support verified-purchase labels.

## GROUP 2 — Shop Reviews

Implement:

- POST `/api/shop-reviews`
- GET `/api/shops/{sellerId}/reviews`
- GET `/api/shops/{sellerId}/reviews/summary`
- GET `/api/shop-reviews/{id}`
- PUT `/api/shop-reviews/{id}`
- DELETE `/api/shop-reviews/{id}`

Support optional shop reviews covering:

- Packaging quality.
- Seller service.
- Order processing speed.
- Product accuracy.

Do not force buyers to submit a shop review whenever they submit a product review unless an existing documented business rule explicitly requires it.

## GROUP 3 — Review Media

Implement:

- POST `/api/reviews/{reviewId}/media`
- GET `/api/reviews/{reviewId}/media`
- DELETE `/api/reviews/{reviewId}/media/{mediaId}`
- PATCH `/api/reviews/{reviewId}/media/reorder`
- POST `/api/reviews/media/upload-url`
- POST `/api/reviews/media/{mediaId}/confirm`

Requirements:

- Support images and videos where the existing storage infrastructure allows.
- Store metadata in the ReviewService database.
- Store actual files in the existing shared storage or object storage solution.
- Do not store binary image/video content directly in SQL Server.
- Validate file types, size, ownership, and upload status.
- Prevent users from deleting another user's media.
- Limit the number of files per review.
- Do not expose an upload URL as proof that an upload has completed.
- Verify uploaded files before marking them as confirmed.

If shared storage already exists, reuse it. Do not introduce a second storage system unnecessarily.

## GROUP 4 — Helpful Votes

Implement:

- POST `/api/reviews/{reviewId}/helpful`
- DELETE `/api/reviews/{reviewId}/helpful`
- GET `/api/reviews/{reviewId}/helpful-count`
- GET `/api/reviews/{reviewId}/my-vote`
- GET `/api/reviews/helpful/top`

Business rules:

- One user can have at most one active vote per review.
- Users cannot vote for their own reviews.
- Repeated requests must not increase the count.
- Users can remove their votes.
- Use database uniqueness constraints and concurrency-safe operations.
- Helpful votes are ranking signals, not proof that a review is genuine.

## GROUP 5 — Seller Responses

Implement:

- POST `/api/reviews/{reviewId}/seller-response`
- GET `/api/reviews/{reviewId}/seller-response`
- PUT `/api/reviews/{reviewId}/seller-response`
- DELETE `/api/reviews/{reviewId}/seller-response`
- GET `/api/seller/reviews/without-response`

Rules:

- Sellers can only respond to reviews belonging to their own shop/products.
- Seller identity must come from authenticated claims.
- Never trust a SellerId supplied by the frontend.
- Sellers cannot edit, overwrite, or delete a buyer's original review.
- Prevent multiple conflicting responses.
- Preserve response history if the existing architecture supports auditing.

## GROUP 6 — Buyer Review Center

Implement:

- GET `/api/reviews/me`
- GET `/api/reviews/me/pending`
- GET `/api/reviews/me/published`
- GET `/api/reviews/me/rejected`
- GET `/api/reviews/me/eligible-orders`

The buyer review center must allow users to:

- View their submitted reviews.
- View published reviews.
- View pending reviews.
- View rejected or hidden reviews with appropriate explanations.
- Find delivered order items that are eligible for review.

Eligibility must be verified through OrderService.

## GROUP 7 — Review Reports

Implement:

- POST `/api/reviews/{reviewId}/reports`
- GET `/api/reports/me`
- GET `/api/reports/{id}`
- GET `/api/admin/review-reports`
- POST `/api/admin/review-reports/{id}/resolve`
- POST `/api/admin/review-reports/{id}/dismiss`

Support report reasons:

- Fake review.
- Spam or advertising.
- Abusive content.
- Personal information.
- Irrelevant content.
- Inappropriate image/video.
- Other policy violations.

Requirements:

- Prevent duplicate or abusive reporting.
- Record report reason and processing history.
- Admin must provide a resolution reason.
- A review must not automatically be deleted just because it receives many reports.
- Preserve fairness and transparency.
- Negative reviews must not be hidden merely because a Seller dislikes them.

## GROUP 8 — Seller Review Dashboard

Implement:

- GET `/api/seller/reviews`
- GET `/api/seller/reviews/summary`
- GET `/api/seller/reviews/statistics`
- GET `/api/seller/reviews/low-rating`
- GET `/api/seller/reviews/recent`
- GET `/api/seller/reviews/search`

Provide:

- Average rating.
- Total review count.
- 1-star to 5-star distribution.
- Reviews without seller responses.
- Recent reviews.
- Low-rating reviews.
- Search and filtering.

Sellers must only access their own shop's information.

## GROUP 9 — Admin Review Moderation

Implement:

- GET `/api/admin/reviews`
- GET `/api/admin/reviews/{id}`
- GET `/api/admin/reviews/pending`
- POST `/api/admin/reviews/{id}/approve`
- POST `/api/admin/reviews/{id}/reject`
- POST `/api/admin/reviews/{id}/hide`
- POST `/api/admin/reviews/{id}/restore`
- GET `/api/admin/reviews/flagged`
- GET `/api/admin/reviews/statistics`
- GET `/api/admin/reviews/search`

Review statuses:

- Pending
- Published
- Rejected
- Hidden
- Flagged

Requirements:

- Implement valid status transitions.
- Record who performed moderation actions.
- Store reasons for rejection or hiding.
- Prevent unauthorized moderation.
- Keep audit history.
- Do not manipulate rating aggregates incorrectly when a review changes status.
- Do not automatically suppress low-rating reviews.

Not every review requires manual approval. Use reasonable moderation rules consistent with the existing product design.

## GROUP 10 — Internal APIs

Implement:

- POST `/api/internal/reviews/eligibility/check`
- GET `/api/internal/orders/{orderId}/review-eligibility`
- GET `/api/internal/products/{productId}/review-summary`
- GET `/api/internal/shops/{sellerId}/review-summary`
- POST `/api/internal/reviews/aggregate/recalculate`
- POST `/api/internal/reviews/events`
- GET `/api/internal/reviews/{reviewId}/summary`
- GET `/api/internal/reviews/statistics`

These APIs must use service-to-service authentication.

Do not expose internal endpoints to ordinary Buyer or Seller roles.

## GROUP 11 — Rating Aggregation and Analytics

Implement:

- GET `/api/ratings/products/{productId}`
- GET `/api/ratings/shops/{sellerId}`
- GET `/api/ratings/products/top-rated`
- GET `/api/ratings/products/most-reviewed`
- GET `/api/reports/reviews/overview`
- GET `/api/reports/reviews/trends`

Requirements:

- Calculate average rating accurately.
- Count only reviews that qualify under the current publication policy.
- Update aggregates when reviews are created, edited, deleted, hidden, rejected, or restored.
- Prevent duplicate event processing.
- Use caching or denormalized summaries where justified.
- Avoid recalculating every review for every product page request.

## GROUP 12 — System and Monitoring

Implement:

- GET `/api/internal/health`
- GET `/api/internal/consumers/status`
- GET `/api/internal/events/failed`
- POST `/api/internal/events/{eventId}/retry`
- GET `/api/internal/statistics`

Protect operational endpoints appropriately.

---

# 5. CRITICAL REVIEW ELIGIBILITY AND DATA INTEGRITY

This is one of the most important parts of the entire implementation.

Never trust the frontend to provide:

- UserId.
- SellerId.
- IsVerifiedPurchase.
- Order status.
- Order ownership.
- Review eligibility.
- Whether an order item has already been reviewed.

These values must be derived from authenticated claims or verified backend data.

## Required verification workflow

1. Buyer purchases a product.
2. OrderService stores the order and its order items.
3. The order becomes Delivered.
4. Buyer opens the review center.
5. Frontend requests eligible order items.
6. ReviewService verifies eligibility with OrderService.
7. Buyer submits a review for a specific OrderItemId.
8. ReviewService validates the order ownership, delivery status, product identity, and eligibility.
9. ReviewService creates the review.
10. ReviewService publishes appropriate events for aggregation and notifications.

A review must be associated with an `OrderItemId`, not just a `ProductId`.

Use a database unique constraint or equivalent reliable concurrency protection to prevent duplicate reviews for the same order item, according to the documented review policy.

A buyer who purchases the same product in another eligible order may submit another review if the business policy allows it.

Example request:

```json
{
  "orderItemId": "GUID",
  "rating": 5,
  "title": "Excellent product",
  "content": "Exactly as described. Carefully packaged."
}
```

The backend must obtain or verify the product, variant, seller, buyer, and purchase eligibility through trusted sources.

Do not accept `IsVerifiedPurchase: true` from the frontend.

If OrderService does not currently expose the required endpoint, inspect its existing architecture and implement the smallest necessary integration. Do not directly query its database.

---

# 6. DOMAIN MODEL AND DATABASE

Inspect existing entities before creating new ones.

Reuse or extend existing entities where appropriate.

Potential entities include:

- ProductReview
- ShopReview
- ReviewMedia
- ReviewHelpfulVote
- SellerReviewResponse
- ReviewReport
- ReviewModeration
- ProductRatingSummary
- ShopRatingSummary
- ProcessedEvent / InboxMessage

Potential ProductReview properties:

- Id
- UserId
- ProductId
- VariantId
- OrderId
- OrderItemId
- Rating
- Title
- Content
- IsVerifiedPurchase
- HelpfulCount
- Status
- CreatedAt
- UpdatedAt
- PublishedAt

Requirements:

- Use appropriate primary and foreign-key relationships within the service.
- Add indexes for common filters and lookups.
- Add unique constraints where required.
- Validate rating range from 1 to 5.
- Use UTC timestamps consistently.
- Use soft deletion or status-based hiding when appropriate.
- Keep audit information for moderation.
- Implement migrations safely.
- Never delete existing databases or volumes.
- Never reset production-like data to make a migration work.

Do not blindly create all suggested entities if an existing model already covers the responsibility.

---

# 7. CLEAN ARCHITECTURE AND BACKEND QUALITY

Follow the existing Clean Architecture structure.

Expected responsibilities:

**Domain**
- Entities.
- Enums.
- Domain rules.
- Domain events where appropriate.

**Application**
- Commands.
- Queries.
- Handlers.
- DTOs.
- Validators.
- Interfaces.
- Business logic.
- Authorization-related application rules.

**Infrastructure**
- EF Core.
- Repository implementations.
- Database configurations.
- External service clients.
- RabbitMQ publishers and consumers.
- Storage integrations.
- Caching implementations.

**API**
- Controllers.
- Authentication and authorization.
- Dependency injection.
- Middleware.
- Swagger/OpenAPI.

Requirements:

- Use MediatR consistently with existing services.
- Use DTOs rather than exposing EF entities.
- Validate all incoming requests.
- Implement pagination.
- Use asynchronous database operations.
- Handle exceptions consistently.
- Avoid N+1 queries.
- Avoid unnecessary database round trips.
- Use cancellation tokens where appropriate.
- Prevent unauthorized access.
- Implement meaningful logging.
- Do not place business logic inside controllers.
- Do not create unnecessary abstractions that make the project harder to maintain.

---

# 8. RABBITMQ, EVENTS, AND SERVICE INTEGRATION

Inspect the existing RabbitMQ implementation before changing it.

Reuse existing exchanges, queues, routing conventions, message contracts, and consumers whenever possible.

ReviewService may publish or consume events such as:

- OrderDelivered
- OrderCancelled
- ReviewCreated
- ReviewUpdated
- ReviewDeleted
- ReviewPublished
- ReviewReported
- SellerRespondedToReview

Use RabbitMQ for asynchronous workflows such as:

- Rating aggregation updates.
- Review-related notifications.
- Processing order-related eligibility changes.
- Updating read models where appropriate.

Requirements:

- Implement idempotent message processing.
- Use Inbox/ProcessedEvent patterns if consistent with the existing architecture.
- Handle retries safely.
- Configure dead-letter handling where appropriate.
- Log failed event processing.
- Prevent duplicate aggregate increments.
- Avoid infinite retry loops.
- Do not introduce RabbitMQ for operations that should remain simple synchronous database operations.
- Do not assume publishing a message means the downstream service has completed its work.

NotificationService owns actual notification delivery. ReviewService should publish the relevant event or call the existing notification integration rather than duplicating notification infrastructure.

ProductService owns product data. Rating summaries may be published to ProductService so product listings can display rating information without requiring synchronous ReviewService calls for every product card.

---

# 9. FRONTEND — COMPLETE SHOPEE-INSPIRED REVIEW EXPERIENCE

This is a major requirement.

The frontend must look professional, modern, polished, and consistent with EcomSystem.

Use Shopee as the UX reference, but do not copy its branding or create a generic template.

Create a distinctive EcomSystem visual identity.

## Design direction

- Premium e-commerce UI.
- Clean white backgrounds.
- EcomSystem brand colors.
- Carefully selected orange/red accents inspired by marketplace design.
- Clear typography.
- Consistent spacing.
- Modern cards.
- Subtle shadows.
- Rounded corners.
- Professional icons.
- Responsive layouts.
- Smooth but restrained transitions.
- Clear loading, empty, success, and error states.

Do not make the interface look like a basic CRUD admin dashboard.

Do not use excessive gradients, oversized elements, random colors, or inconsistent component styles.

Reuse the existing frontend design system and shared components where possible.

## Buyer experience

Implement a complete Buyer Review experience.

### A. Product detail review section

Display:

- Average star rating.
- Total number of reviews.
- 1–5 star distribution.
- Review filters.
- Sort options.
- Verified purchase labels.
- Review content.
- Buyer images and videos.
- Helpful vote button.
- Seller responses.
- Report review action.

The review section must integrate into the existing product detail page without breaking existing product functionality.

### B. Write a review

Create a polished review submission experience:

- Select 1–5 stars.
- Enter a review title.
- Enter review content.
- Upload images/videos if supported.
- Preview uploaded media.
- Remove media before submission.
- Display validation messages.
- Prevent duplicate submissions.
- Show success or failure feedback.

Only show eligible purchased order items.

Do not allow the frontend to invent order ownership or purchase verification.

### C. My Reviews

Create a Buyer review center with:

- All reviews.
- Pending reviews.
- Published reviews.
- Rejected/hidden reviews.
- Products eligible for review.
- Review detail.
- Edit review.
- Delete review.

### D. Shop reviews

Display:

- Shop average rating.
- Rating distribution.
- Buyer feedback.
- Optional shop review submission.

### E. Helpful votes

Allow users to vote and remove votes.

Update counts based on actual API responses.

Do not fake vote success.

## Seller experience

Create a complete Seller review management area.

Include:

- Review overview.
- Average shop rating.
- Rating distribution.
- Total reviews.
- Low-rating reviews.
- Recent reviews.
- Search and filtering.
- Reviews awaiting responses.
- Seller response creation.
- Seller response editing and deletion.

Seller must only see reviews associated with their own shop.

The UI must make it easy for Sellers to respond professionally to customer feedback.

## Admin experience

Create a complete Admin review moderation area.

Include:

- Review management.
- Pending reviews.
- Flagged reviews.
- Report management.
- Review detail.
- Approve/reject/hide/restore actions.
- Moderation reason.
- Moderation history.
- Review statistics.
- Search and filters.

Admin actions must require confirmation where appropriate.

Do not hide negative reviews merely because they are negative.

## Review-related navigation

Integrate the new functionality into existing EcomSystem navigation.

Use clear navigation labels such as:

- Product Reviews.
- My Reviews.
- Shop Reviews.
- Seller Review Management.
- Review Moderation.

Follow the existing Buyer/Seller/Admin layout structure.

Do not create duplicate navigation systems if suitable layouts already exist.

---

# 10. FRONTEND ENGINEERING REQUIREMENTS

Use the existing frontend stack and conventions.

Requirements:

- React.
- TypeScript.
- Existing Vite configuration.
- Existing API client patterns.
- Existing authentication token handling.
- Existing routing conventions.
- Existing shared UI components.

Implement:

- Typed request/response models.
- Dedicated ReviewService API functions.
- Reusable review components.
- Pagination.
- Filtering.
- Sorting.
- Form validation.
- Loading skeletons.
- Empty states.
- Error states.
- Toast notifications.
- Responsive design.
- Correct route protection.
- Correct role-based rendering.

Important:

- Do not hardcode production review data.
- Do not use fake reviews to make the UI look populated.
- Do not use local state as a substitute for backend persistence.
- Do not trust frontend authorization.
- Do not expose service secrets.
- Do not duplicate API base URLs unnecessarily.
- Reuse the existing environment configuration and Vite proxy conventions.

All review operations must communicate with the actual ReviewService Backend.

---

# 11. SECURITY AND AUTHORIZATION

Implement appropriate authorization for each role.

**Buyer**
- View public published reviews.
- Create reviews only for eligible purchases.
- Edit/delete only their own reviews.
- Manage their own media.
- Vote on other users' reviews.
- Report reviews.
- View their own review history.

**Seller**
- View reviews belonging to their own shop.
- Respond to reviews belonging to their own shop.
- View their own shop's review statistics.
- Never modify buyer review content or ratings.

**Admin**
- Manage review moderation.
- View reports.
- Resolve reports.
- View platform-wide statistics.
- Access administrative review management APIs.

**Internal services**
- Access internal endpoints only through service-to-service authentication.

Always validate resource ownership on the Backend.

Hiding a button in React is not an authorization mechanism.

---

# 12. PERFORMANCE AND DATA CONSISTENCY

Pay particular attention to:

- Product review pagination.
- Rating aggregate performance.
- Seller review dashboards.
- Helpful vote concurrency.
- Duplicate review prevention.
- Media metadata queries.
- Database indexing.
- RabbitMQ duplicate events.
- Cache invalidation.

Do not fetch every review just to calculate the average rating.

Do not repeatedly call OrderService for every review displayed on a product page.

Use suitable summary data, caching, or event-driven synchronization where appropriate.

Ensure aggregate values remain correct after review updates and moderation actions.

---

# 13. TESTING — YOU MUST ACTUALLY RUN THE TESTS

Do not stop after coding.

Run all relevant checks.

## Backend tests

At minimum, test:

1. Create a valid review.
2. Reject a review for an undelivered order.
3. Reject a review belonging to another buyer.
4. Reject a duplicate review for the same order item.
5. Reject a rating outside 1–5.
6. Update an owner's review.
7. Prevent another user from updating that review.
8. Delete or hide an owner's review according to policy.
9. Prevent a Seller from responding to another Seller's review.
10. Prevent users from voting for their own reviews.
11. Prevent duplicate helpful votes.
12. Remove a helpful vote correctly.
13. Validate media ownership.
14. Prevent unauthorized media deletion.
15. Report a review.
16. Process a moderation action.
17. Update rating aggregates correctly.
18. Process duplicate RabbitMQ events safely.
19. Reject unauthorized internal API requests.
20. Verify pagination and filtering.

Also test database migrations and integration with OrderService.

## Frontend tests

Verify:

- Product review section loads real API data.
- Rating distribution displays correctly.
- Review filtering works.
- Review sorting works.
- Review submission works.
- Invalid submissions display correct errors.
- Media upload and confirmation work when supported.
- Helpful voting works.
- My Reviews loads correctly.
- Seller responses work.
- Seller dashboard loads the correct shop's data.
- Admin moderation actions work.
- Unauthorized users cannot access protected pages.
- Loading, empty, and error states display correctly.
- No broken routes.
- No browser console errors.
- No failed API requests caused by incorrect frontend integration.

## API integration tests

Test actual HTTP requests against the running service.

Verify:

- HTTP status codes.
- Response DTOs.
- Validation errors.
- Authorization.
- Database persistence.
- Pagination.
- Ownership checks.
- Cross-service integration.

Do not claim that an endpoint works just because its controller exists.

---

# 14. DOCKER — MANDATORY FINAL VERIFICATION

The entire EcomSystem already has Docker configuration.

**Whenever you modify C# Backend code or Frontend code, you MUST complete the implementation and then execute:**

```bash
docker compose up --build
```

Run this command from the EcomSystem project root, where the main `docker-compose.yml` is located.

Do not skip this step.

After building:

1. Confirm the ReviewService container starts successfully.
2. Confirm the frontend container starts successfully.
3. Confirm SQL Server connectivity.
4. Confirm database migrations are applied safely.
5. Confirm RabbitMQ connectivity if used.
6. Confirm other required service dependencies are healthy.
7. Inspect container logs.
8. Fix build errors.
9. Fix runtime errors.
10. Fix service-to-service communication failures.
11. Verify the frontend can communicate with the Backend.
12. Test the actual application through the browser.

If only running ordinary tests without code changes requiring a rebuild, use:

```bash
docker compose up
```

However, after any C# or frontend implementation changes, the final verification must include:

```bash
docker compose up --build
```

Do not merely say the Docker configuration looks correct.

Actually execute the command and inspect its result.

**IMPORTANT DATA SAFETY RULES:**

- Never run `docker compose down -v`.
- Never delete Docker volumes.
- Never delete existing SQL Server data.
- Never reset databases unnecessarily.
- Never remove unrelated containers or images to make the task appear successful.
- Do not modify unrelated service configurations unless required for integration.
- Be careful with limited disk space and avoid unnecessary large image downloads.

If Docker fails because of an environment issue, report the exact error. Do not pretend that the build or runtime verification succeeded.

---

# 15. REAL FRONTEND VISUAL VERIFICATION

After Docker starts successfully:

1. Open the actual EcomSystem frontend.
2. Navigate to the product detail page.
3. Inspect the review section.
4. Open the Buyer My Reviews page.
5. Open the Seller review dashboard.
6. Open the Admin moderation interface.
7. Test the major interactions.
8. Inspect desktop layout.
9. Inspect responsive layout.
10. Check spacing, alignment, typography, colors, cards, tables, forms, and dialogs.
11. Fix visual bugs and broken interactions.
12. Rebuild and verify again if you modify C# or frontend code.

Do not consider the task finished simply because the frontend compiles.

The actual rendered UI must look professional and complete.

---

# 16. FINAL ACCEPTANCE CRITERIA

The task is complete only when all applicable conditions are satisfied:

- [ ] Read `docs/AI_RULE.md` completely.
- [ ] Read `docs/ReviewService.md` completely.
- [ ] Inspected existing Backend and Frontend architecture.
- [ ] Preserved correct existing code and features.
- [ ] Implemented the required ReviewService Backend functionality.
- [ ] Implemented the required Buyer review experience.
- [ ] Implemented the required Seller review experience.
- [ ] Implemented the required Admin moderation experience.
- [ ] Integrated product reviews into the existing product detail page.
- [ ] Implemented review eligibility verification through OrderService.
- [ ] Implemented ownership and authorization checks.
- [ ] Implemented safe media metadata/storage integration.
- [ ] Implemented helpful votes with concurrency protection.
- [ ] Implemented rating aggregation correctly.
- [ ] Implemented RabbitMQ integration where justified.
- [ ] Implemented database migrations safely.
- [ ] Backend builds successfully.
- [ ] Frontend builds successfully.
- [ ] Relevant tests pass.
- [ ] Actual API workflows have been tested.
- [ ] Docker rebuild completed successfully after code changes.
- [ ] ReviewService container runs correctly.
- [ ] Frontend runs correctly.
- [ ] Actual UI has been inspected.
- [ ] No known critical runtime errors remain.

If a feature cannot be completed because another service or external provider is missing, implement the appropriate interface and safe integration boundary where possible, clearly document the blocker, and do not create fake success behavior.

---

# 17. FINAL REPORT

When finished, provide a clear report containing:

## A. Documentation
- Which documents were read.
- Which existing modules and sample implementations were inspected.

## B. Backend
- Features implemented.
- Controllers and endpoints added or modified.
- Commands, Queries, Handlers, DTOs, Validators, and Entities changed.
- Database migrations created.
- Security and authorization implemented.

## C. Frontend
- Pages and components implemented.
- Buyer features.
- Seller features.
- Admin features.
- Product detail integration.
- Visual improvements.

## D. Microservice Integration
- OrderService integration.
- ProductService rating synchronization.
- NotificationService integration.
- RabbitMQ events.
- Storage integration.
- Any remaining dependencies.

## E. Testing
- Build results.
- Unit test results.
- Integration test results.
- API test results.
- Frontend test results.
- Actual workflows tested.

## F. Docker
- Exact Docker commands executed.
- Build results.
- Running container status.
- Relevant runtime logs.
- Any remaining errors.

## G. UI Verification
- Pages actually opened and inspected.
- Interactions actually tested.
- Responsive behavior.
- Remaining visual issues.

## H. Files Changed
List important files created, modified, or preserved.

## I. Remaining Work
Clearly distinguish:
- Fully implemented and tested.
- Implemented but not fully verified.
- Blocked by another service or external dependency.
- Not implemented and why.

Never report a test as passed if it was not executed.

---

# FINAL INSTRUCTION

Work carefully and systematically.

Prioritize correctness, maintainability, security, data integrity, and a polished user experience.

Do not delete working code.

Do not unnecessarily rewrite existing modules.

Do not stop after creating files.

Do not stop after a successful compilation.

**Complete the Backend, complete the Frontend, run the tests, execute `docker compose up --build` after code changes, inspect the actual running application, fix discovered problems, and only then consider the task finished.**