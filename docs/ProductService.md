Read `docs/AI_RULE.md`, the ProductService documentation, and inspect the existing ProductService Backend and Frontend before coding.

Implement these 5 APIs completely:

1. `POST /api/products` — Seller creates a product.
2. `GET /api/products` — Paginated product listing.
3. `GET /api/products/{id}` — Product details.
4. `PUT /api/products/{id}` — Full product update.
5. `PATCH /api/products/{id}` — Partial product update.

**Backend requirements**
- Follow the existing project architecture and code samples.
- Implement actual Controller routes and complete the flow: Controller → MediatR Command/Query → Handler → Application interface → Infrastructure → EF Core → database.
- Add or reuse DTOs, validation, authorization, dependency injection, and correct HTTP responses.
- Obtain Seller ID from authenticated JWT claims, never trust Seller ID supplied by Frontend.
- Ensure sellers can only manage their own products unless existing authorization rules explicitly allow otherwise.

**Frontend requirements**
- Implement a professional, modern e-commerce interface inspired by Shopee.
- Create or improve product listing, product detail, create-product, and edit-product screens as appropriate.
- Connect to the real APIs; do not use mock data to replace API integration.
- Include loading, empty, validation, success, and error states.
- Make the UI responsive and consistent with the existing project.

**Verification**
- Test all 5 APIs through actual HTTP requests.
- Verify the corresponding Frontend workflows.
- Build Backend and Frontend.
- If source code changes, run `docker compose up --build` from the project root.
- Never run `docker compose down -v` or delete database volumes/data.

Preserve working code. Do not create duplicate routes or duplicate implementations. Do not claim an API is complete unless it has been tested.

At the end, report changed files, actual routes, test results, Docker status, Frontend verification, and any blockers. Do not silently skip unfinished work.

