# 07 - Project Roadmap

## 1. Overview

This document defines the planned development roadmap for The Chronicles.

The goal is to build the application incrementally, starting with a stable local development environment and gradually implementing the complete MVP.

Development will follow small, understandable steps.

Each feature should be implemented, tested and reviewed before moving to the next one.

The roadmap defines the development order, not a rigid deadline schedule.

## 2. Development Principles

1. Build the application incrementally.
2. Prioritize a stable foundation before implementing complex features.
3. Keep the frontend and backend independently structured.
4. Follow the approved modular monolith architecture.
5. Implement and test features in small steps.
6. Do not introduce unapproved product features.
7. Explain important technical decisions during development.
8. Keep project documentation synchronized with approved decisions.
9. Prefer understandable solutions over unnecessary complexity.
10. Do not move forward while important problems in the current stage remain unresolved.

## 3. Development Stages

### Stage 1 - Project Foundations

Goal: Prepare a working local development environment and establish the initial project structure.

Planned tasks:

- Create and configure the monorepo.
- Configure Git and repository structure.
- Create the React + Vite + TypeScript frontend.
- Create the ASP.NET Core Web API backend.
- Configure the initial backend modular structure.
- Install and configure Docker Desktop on Windows.
- Introduce WSL 2 if required.
- Configure Docker Compose.
- Run PostgreSQL 18 in Docker.
- Configure the backend database connection.
- Configure Entity Framework Core.
- Prepare the initial database migration.
- Configure local environment variables.
- Verify communication between frontend and backend.

Expected result:

- Frontend runs locally.
- Backend runs locally.
- PostgreSQL runs in Docker.
- Backend can connect to PostgreSQL.
- The project structure follows the approved architecture.

### Stage 2 — Users and Authentication

Goal: Implement user accounts and secure access to the application.

Planned tasks:

- Configure ASP.NET Core Identity.
- Configure UUIDv7 identifiers.
- Implement user registration.
- Implement login.
- Implement logout.
- Configure JWT authentication.
- Implement Access Token storage in frontend application memory.
- Implement Refresh Token storage using HttpOnly cookies.
- Configure token lifetimes (15-minute Access Token, 7-day Refresh Token).
- Implement Refresh Token rotation.
- Implement server-side Refresh Token revocation.
- Implement automatic session refresh.
- Implement logout with Refresh Token invalidation.
- Implement CSRF protection for cookie-based authentication endpoints.
- Configure ASP.NET Core Identity password policy.
- Configure login rate limiting and account lockout protection.
- Implement secure password hashing.
- Implement password recovery.
- Implement password reset.
- Implement user profile retrieval.
- Implement display name updates.
- Implement password changes.
- Implement account deletion with password confirmation.

Expected result:

- Users can register and authenticate.
- Protected API endpoints require authentication.
- Users can manage their basic profile.
- Password recovery works.
- Account deletion follows the approved ownership-transfer and data-deletion rules.

### Stage 3 — Chronicles and Membership

Goal: Implement chronicle creation, management and collaboration.

Planned tasks:

- Implement the Chronicle entity.
- Implement ChronicleMembership.
- Implement Owner and Member roles.
- Implement Resource-Based Authorization.
- Implement chronicle creation.
- Automatically assign the creator as Owner.
- Implement chronicle listing.
- Implement chronicle details.
- Implement chronicle deletion.
- Implement membership management.
- Implement leaving a chronicle.
- Implement ownership transfer when an Owner leaves.
- Select the oldest remaining member as the new Owner.
- Implement invitation link generation.
- Implement invitation link copying.
- Implement invitation link replacement.
- Revoke previous links when generating new ones.
- Implement joining through invitation links.
- Implement invitation token hashing.
- Implement dashboard UI.
- Implement empty dashboard state.

Expected result:

- Users can create and manage chronicles.
- Multiple users can collaborate in one chronicle.
- Ownership and membership rules are enforced.
- Users can invite others through links.
- Dashboard displays the user's chronicles.

### Stage 4 — Memories

Goal: Implement the complete memory management functionality.

Planned tasks:

- Implement the Memory entity.
- Implement memory creation.
- Implement memory editing.
- Implement memory deletion.
- Implement memory date validation.
- Implement optional descriptions.
- Implement optional locations.
- Implement CreatedByUserId and UpdatedByUserId.
- Implement MemoryParticipant.
- Enforce participant membership within the same chronicle.
- Implement MemoryPhoto.
- Implement local photo storage.
- Introduce a replaceable storage abstraction.
- Implement photo upload and validation.
- Enforce photo count and size limits.
- Implement photo previews.
- Implement photo removal.
- Implement photo reordering.
- Implement primary photo selection.
- Implement physical photo deletion.
- Implement MemoryReaction.
- Implement reaction creation, replacement and removal.
- Implement reaction counts and participant lists.

Expected result:

- Enforce a maximum of 20 photos per memory.
- Enforce a maximum file size of 10 MB per photo.
- Validate JPEG, PNG and WebP formats.
- Validate actual file content, not only file extensions or MIME types.
- Members can create, edit and delete memories.
- Memories can contain multiple photos or no photos.
- Participants can be assigned from chronicle members.
- Reactions work according to approved rules.
- Photo storage is separated from database storage.

### Stage 5 — Interface and Timeline

Goal: Implement the main user experience based on approved Figma designs and UX specifications.

Planned tasks:

- Implement the application layout.
- Implement persistent top navigation.
- Implement chronicle switching.
- Implement dashboard visual design.
- Implement timeline foundation.
- Implement proportional date positioning.
- Implement horizontal timeline navigation.
- Implement Overview zoom level.
- Implement Exploration zoom level.
- Implement Memories zoom level.
- Implement continuous zoom and snapping.
- Implement horizontal dragging and inertia.
- Implement time navigation slider.
- Implement return-to-present functionality.
- Implement memory nodes.
- Implement photo thumbnails.
- Implement memory cards.
- Implement collision handling.
- Implement selected memory behavior.
- Implement memory details panel.
- Implement memory editing panel.
- Implement memory creation modal.
- Implement empty timeline state.
- Implement approved animations.
- Apply the established visual identity and design system.

Expected result:

- Users can explore their chronicles through the timeline.
- Timeline interactions follow the approved UX specification.
- Memories are presented consistently across zoom levels.
- Memory creation, editing and viewing are integrated into the interface.

### Stage 6 — Real-Time Collaboration

Goal: Enable automatic synchronization between chronicle members.

Planned tasks:

- Configure SignalR.
- Implement chronicle-specific connections.
- Implement authorization for SignalR connections.
- Broadcast memory creation events.
- Broadcast memory update events.
- Broadcast memory deletion events.
- Synchronize reactions.
- Handle concurrent editing.
- Implement editing indicators.
- Preserve each user's timeline position and zoom.
- Prevent updates from unexpectedly changing another user's navigation state.

Expected result:

- Changes made by one member appear automatically for other members.
- Users can collaborate without refreshing the application.
- Concurrent editing follows the approved conflict-resolution rules.

### Stage 7 — Stabilization and Refinement

Goal: Improve reliability, performance and overall application quality.

Planned tasks:

- Expand backend unit tests.
- Expand backend integration tests.
- Expand frontend tests.
- Test authorization boundaries.
- Test ownership transfer.
- Test account deletion.
- Test chronicle and memory deletion.
- Test photo storage cleanup.
- Improve loading states.
- Improve validation messages.
- Improve error handling.
- Implement offline connection state.
- Design and implement local preservation of unsaved changes.
- Implement server reconnection and synchronization recovery.
- Verify timeline performance.
- Optimize photo loading and thumbnails.
- Review database indexes.
- Review application security.
- Verify important user flows.

Expected result:

- Core application functionality is tested.
- Important failure scenarios are handled.
- Timeline interactions remain responsive.
- Data integrity and authorization rules are reliable.

## 4. Development Workflow

Each development stage will be divided into smaller implementation tasks.

For each task:

1. Review relevant project documentation.
2. Confirm the requirements.
3. Discuss important technical decisions.
4. Implement a small, focused change.
5. Explain the implementation and important concepts.
6. Run relevant tests.
7. Review the result.
8. Update documentation if an approved decision changes.

Large features should not be implemented in one step when they can reasonably be divided into smaller tasks.

## 5. Testing Strategy

Testing is introduced incrementally throughout development.

### Backend

- xUnit for unit tests.
- Integration tests for API endpoints.
- Integration tests for database behavior.
- Tests for authorization and membership rules.
- Tests for ownership transfer and destructive operations.

### Frontend

- Vitest.
- React Testing Library.
- Tests for important user interactions.
- Tests for forms and validation.
- Tests for timeline behavior where practical.

Testing should focus on meaningful behavior and critical business rules.

## Testing Principles

Every new feature must include appropriate automated tests before it can be considered complete.

Testing requirements:

- Unit tests for business logic and validation rules.
- Integration tests for interactions between backend components, database and external storage.
- Frontend tests for important components, forms and user interactions.
- Tests must be updated whenever existing functionality changes.
- All relevant tests must pass before a task or development stage is considered complete.
- Bug fixes should include regression tests whenever practical.

Testing tools:

- Backend: xUnit.
- Backend integration: xUnit with ASP.NET Core testing infrastructure.
- Frontend: Vitest and React Testing Library.
- Database integration tests: PostgreSQL in Docker.

No fixed code coverage percentage is required for the MVP. Priority should be given to critical business logic, security, data integrity and user-facing functionality.

## 6. MVP Scope

The roadmap prioritizes the previously approved MVP functionality:

- Authentication and profile management.
- Chronicle creation and management.
- Shared membership and invitation links.
- Owner and Member permissions.
- Memory creation, editing and deletion.
- Multiple photo support.
- Participants and locations.
- Memory reactions.
- Interactive timeline with three zoom levels.
- Memory details and editing.
- Real-time synchronization.
- Empty states.
- Offline connection handling and preservation of unsaved changes.

Features explicitly deferred from MVP remain outside the roadmap unless separately approved.

## 7. Deferred Features

The following features are not part of the current MVP plan:

- Google OAuth.
- Email invitations.
- User search.
- Public profiles.
- Public chronicles.
- Comments.
- Advanced social functionality.
- Notifications.
- AI-generated images.
- Wrapped and statistics.
- Payments.
- Mobile and tablet optimization.

These features may be reconsidered after the MVP.

## 8. Hosting and Deployment

Production hosting is intentionally postponed.

Initial development focuses on a reliable local environment.

The architecture should remain compatible with future deployment to either cloud hosting or a VPS.

The hosting provider and production infrastructure will be selected separately.

Before the application is made publicly available, the production protection layer must be configured and verified. This includes HTTPS, appropriate security headers, backend rate limiting and authorization, and infrastructure-level traffic filtering and abuse protection. Cloudflare or an equivalent provider is the preferred additional protection layer. These protections are not required for the local development environment.

## 9. Roadmap Principles

The roadmap is a development guide, not permission to implement every possible technical detail without discussion.

Approved product and architecture documentation remains the source of truth.

If implementation reveals a need to change an approved decision:

1. Explain the issue.
2. Present possible solutions.
3. Discuss trade-offs.
4. Obtain approval.
5. Update the relevant documentation.

The goal is to build The Chronicles carefully, with a clear understanding of both the product and its technical implementation.
