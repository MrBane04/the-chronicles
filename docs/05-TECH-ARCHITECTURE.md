# 05 - Technical Architecture

## 1. Overview

The Chronicles is built as a full-stack web application using a modular monolith architecture.

The application consists of:

- React frontend
- ASP.NET Core backend
- PostgreSQL database
- Separate file storage for photos
- Real-time communication using SignalR

The initial development environment is local. Hosting and production infrastructure will be decided later.

## 2. Technology Stack

| Layer                    | Technology                     |
| ------------------------ | ------------------------------ |
| Frontend                 | React                          |
| Frontend tooling         | Vite                           |
| Frontend language        | TypeScript                     |
| Backend                  | ASP.NET Core Web API           |
| Backend language         | C#                             |
| Architecture             | Modular Monolith               |
| Database                 | PostgreSQL                     |
| ORM                      | Entity Framework Core          |
| Authentication           | ASP.NET Core Identity + JWT    |
| API communication        | REST API                       |
| Real-time communication  | SignalR                        |
| Backend testing          | xUnit                          |
| Frontend testing         | Vitest + React Testing Library |
| Local containers         | Docker Compose                 |
| Initial operating system | Windows                        |

## 3. Repository Structure

The application uses a monorepo approach.

Frontend and backend are separate applications maintained in one Git repository.

Proposed structure:

```text
the-chronicles/
├── frontend/
│   ├── src/
│   ├── public/
│   └── package.json
│
├── backend/
│   └── TheChronicles/
│       ├── Modules/
│       │   ├── Auth/
│       │   ├── Chronicles/
│       │   ├── Membership/
│       │   ├── Invitations/
│       │   ├── Memories/
│       │   └── Realtime/
│       ├── Infrastructure/
│       └── Program.cs
│
├── docker/
│
├── docs/
│   └── decisions/
│
├── AGENTS.md
└── README.md
```

This is a proposed logical structure. Exact project and file organization can be refined during implementation without changing the approved architecture.

## 4. Backend Architecture

### 4.1 Modular Monolith

The backend will be implemented as one application with clearly separated business modules.

The modules will initially live inside one backend project. Separate deployable services are not required.

### 4.2 Business Modules

**Auth**

- User registration
- Login
- Logout
- Password recovery
- Profile management
- Authentication-related functionality

**Chronicles**

- Creating chronicles
- Retrieving chronicles
- Updating chronicle information
- Deleting chronicles
- Chronicle ownership rules

**Membership**

- Managing chronicle members
- Joining and leaving chronicles
- Membership-related permissions

**Invitations**

- Creating invitation links
- Joining through invitation links
- Revoking and replacing invitation links

**Memories**

- Creating memories
- Editing memories
- Deleting memories
- Managing memory metadata
- Managing photo references and ordering

**Realtime**

- SignalR communication
- Notifying members about memory changes
- Chronicle-specific real-time updates
- Supporting concurrent editing workflows

### 4.3 Infrastructure

Infrastructure contains technical implementations shared by the application.

Examples:

- Database configuration
- Entity Framework Core configuration
- File storage implementation
- External service integrations

Business modules should not become unnecessarily dependent on infrastructure implementation details.

## 5. Frontend Architecture

The frontend will use React, Vite and TypeScript.

The frontend and backend remain independent applications.

The frontend will communicate with the backend through a centralized API communication layer.

Components should not implement their own unrelated HTTP communication logic.

The frontend is responsible for:

- Rendering the user interface
- Dashboard and chronicle navigation
- Timeline interactions
- Memory creation and editing forms
- Displaying authentication states
- Handling user interactions
- Receiving real-time updates

The backend remains responsible for business logic, data validation, permissions and persistence.

## 6. Communication

### 6.1 REST API

REST API will be used for standard application operations.

The API will use the `/api` route prefix.

Examples of operations:

- Authentication
- Chronicle management
- Membership management
- Invitation management
- Memory management

### Invitation security

Invitation links must use cryptographically secure random tokens.

Security requirements:

- Generate tokens using a cryptographically secure random number generator.
- Store a cryptographic hash of the invitation token for verification.
- Store the original token only in encrypted form to allow authorized
  members to retrieve the active invitation link.
- Never store the original token in plain text.
- Encryption keys must be stored securely outside the database.
- Verify the submitted token on the backend whenever someone attempts to join.
- Require authentication before accepting an invitation.
- Replacing an invitation token immediately invalidates the previous token.
- Invalid or revoked tokens must never grant access to a chronicle.
- Invitation tokens must not be exposed in application logs.

Exact endpoint definitions will be documented during API design.

### 6.2 SignalR

SignalR is used for real-time communication between chronicle members.

The hub endpoint:

`/hubs/chronicles`

The primary responsibility of the hub is distributing notifications about successfully saved changes to connected members of the relevant chronicle.

#### Connection and subscription scope

- A browser tab subscribes only to the chronicle currently open in that tab.
- When a user opens another chronicle, the connection subscription is updated accordingly.
- Different browser tabs may independently subscribe to different chronicles.
- Users must be authorized as current members before joining a chronicle's real-time communication group.
- Real-time updates must be scoped to the relevant chronicle.

#### Synchronization behavior

- Changes should appear to other connected members practically immediately after being successfully saved by the backend.
- PostgreSQL remains the source of truth.
- Database persistence must succeed before a change notification is distributed through SignalR.
- SignalR notifications do not replace normal backend data retrieval or persistence.

Synchronize the following changes:

- Memory creation.
- Memory updates.
- Memory deletion.
- Reaction creation.
- Reaction changes.
- Reaction removal.
- Memory participant changes.
- Photo additions and removals.
- Photo ordering changes.

Changes to participants and photos are synchronized as part of the corresponding memory update.

#### Reconnection and missed updates

- The application must automatically attempt to reconnect after a connection is lost.
- After reconnection, the client must request synchronization with the backend.
- The backend must provide authoritative current data so that changes missed during disconnection are recovered.
- Network connectivity alone does not mean the application server is reachable.
- The timeline must preserve the user's previous navigation position and zoom level during reconnection and synchronization.

The exact recovery protocol and the choice between incremental synchronization and retrieving an authoritative snapshot remain open implementation decisions.

#### Concurrent editing

- Multiple members may edit the same memory simultaneously.
- Editing is not locked when another member is editing.
- Changes to different fields must be merged.
- When multiple users change the same field, the last successfully saved change wins.
- Updates must affect only fields actually changed by the user.
- Older full-memory versions must not overwrite newer changes to unrelated fields.
- The backend must use an appropriate concurrency control mechanism.

#### Active editing indicator

- Display the display name of each member currently editing a memory.
- Example: "Anna is editing this memory."
- If multiple members are editing the same memory, show all active editors.
- The indicator is informational and must not block editing.
- The indicator disappears when the user finishes editing or is no longer considered active.

The exact mechanism for detecting and expiring active editing sessions remains an implementation decision.

Detailed event contracts, concurrency implementation and connection management remain to be designed.

## 7. Database

PostgreSQL will be the primary relational database.

Entity Framework Core will be used as the ORM.

The database will store application data such as:

- User information
- Chronicles
- Chronicle memberships
- Invitations
- Memories
- Photo metadata and references

The detailed database structure will be documented in `06-DATA-MODEL.md`.

## 8. Authentication and Authorization

### 8.1 Authentication

ASP.NET Core Identity will handle user management and password-related functionality.

JWT will be used for API authentication.

Authentication responsibilities include:

- Registration
- Login
- Logout
- Password hashing
- Password recovery
- Password reset
- User management

#### Password Policy

ASP.NET Core Identity will handle password hashing and password validation.

Password requirements:

- Minimum length: 12 characters.
- Maximum length: 128 characters.
- Uppercase letters are not required.
- Lowercase letters are not required.
- Numbers are not required.
- Special characters are not required.
- Password confirmation is required during registration.

Additional security requirements:

- Login attempts must be rate-limited to protect against brute-force attacks.
- Account lockout protection must be configured.
- Passwords must never be stored in plain text.
- Passwords must not require periodic changes.
- Password changes require the user's current password.
- Password reset must use a secure, single-use, time-limited reset token.

#### Access Token

- Access tokens are JWT tokens.
- Access tokens are stored only in frontend application memory.
- Access tokens must never be stored in localStorage or sessionStorage.
- Access tokens are sent to the backend using the Authorization header.
- Access token lifetime: 15 minutes.

#### Refresh Token and Session Management

- Refresh tokens are stored in the database only as cryptographic hashes.
- Refresh tokens are stored in HttpOnly cookies.
- Refresh token lifetime is 7 days.
- Each device or browser has an independent session.
- A user can maintain multiple active sessions simultaneously.
- Each session has a stable `SessionId`, shared across refresh token rotations.
- Every successful refresh operation rotates the refresh token:
  - the previous token is revoked;
  - a new refresh token is generated;
  - the new token hash is persisted;
  - the new token is returned through the HttpOnly cookie.
- Refresh token records retain their rotation and revocation history to support reuse detection.
- Reuse of a revoked refresh token is treated as a potential token compromise.
- When reuse is detected, all active refresh token sessions belonging to the affected user are revoked.

### Access Token Behavior After Session Revocation

- Access tokens have a lifetime of 15 minutes.
- Revoking refresh token sessions does not immediately invalidate already issued access tokens.
- Existing access tokens remain valid until their expiration, for a maximum of 15 minutes from issuance.
- After expiration, the user must authenticate again.
- The MVP does not require checking session validity on every authenticated API request.
- Immediate access token revocation may be considered in a future version.

### Session Metadata

Each refresh token session stores informational metadata:

- Device type: Desktop, Mobile, Tablet or Unknown.
- Browser name.
- Operating system.
- Last activity timestamp.

IP addresses are not stored.

Device metadata is informational and must not be treated as a reliable device identity.

### Future Session Management

The data model supports future session management features:

- Viewing active sessions.
- Revoking an individual session.
- Revoking all other active sessions.

The session management interface is not required for MVP.

#### Session Refresh

When an access token expires:

1. The frontend sends a refresh request to the backend.
2. The browser includes the HttpOnly refresh token cookie.
3. The backend validates the refresh token.
4. The backend issues a new access token and refresh token.
5. The previous refresh token is invalidated.
6. The frontend stores the new access token in application memory.

If the refresh token is invalid, expired or revoked, the user must authenticate again.

#### Security Requirements

- Refresh token cookies must use the HttpOnly attribute.
- Refresh token cookies must use the Secure attribute in production.
- CSRF protection must be implemented for endpoints that authenticate using cookies.
- Authentication tokens must never be exposed in application logs.
- Token storage must not rely on localStorage or sessionStorage.

Exact cookie configuration, CSRF implementation and refresh token persistence details will be defined during implementation.

### 8.2 Authorization

The application will use Resource-Based Authorization.

The backend must verify whether a user has permission to access or modify a particular chronicle or resource.

Authorization must never rely exclusively on frontend visibility or hidden interface elements.

Chronicle ownership and membership rules must be enforced on the server.

## 9. Photo Storage

Photos will be stored separately from PostgreSQL.

The long-term storage direction is Cloud Object Storage.

During local development, photos will be stored locally.

The implementation must use a storage abstraction so that the storage
provider can be changed without rewriting memory-related business logic.

PostgreSQL will store photo metadata and storage references, not image binaries.

### Photo access and delivery

All photo uploads and downloads must go through the ASP.NET Core backend.

The backend must:

- Authenticate the requesting user.
- Verify that the user is a current member of the chronicle
  containing the requested photo.
- Reject unauthorized access.
- Never expose physical storage paths or internal storage keys
  to the frontend.
- Keep photo storage private and inaccessible directly from the internet.

Possessing a photo URL or identifier must not grant access
without successful authorization.

The backend streams authorized photo files to the client.

### Photo upload security

The backend must validate every uploaded photo before storing it.

Validation requirements:

- Verify the actual file format, not only the extension or MIME type.
- Accept only JPEG, PNG and WebP.
- Reject corrupted or invalid image files.
- Enforce a maximum file size of 10 MB per photo.
- Generate unique storage filenames instead of using original filenames.
- Never execute uploaded files as code.
- Store original filenames only as metadata when needed.

Files that fail validation must not be stored.

### File deletion

Physical file deletion must be handled separately from database transactions.

The application will use the Outbox Pattern and a Background Worker
to process file deletion tasks reliably.

When a photo or its parent resource is deleted:

1. The database transaction records the required file deletion tasks
   in the Outbox.
2. The database transaction deletes the relevant application records.
3. After the transaction commits, the Background Worker processes
   pending Outbox tasks.
4. The worker deletes the physical files from storage.
5. Successfully processed tasks are marked as completed.

Failed operations must remain available for retry.

File deletion must be idempotent: processing the same deletion task
more than once must not cause errors or unintended side effects.

The Background Worker should initially run as part of the ASP.NET Core
backend using its hosted background service mechanism.

The specific storage provider will be selected later.

## 10. Local Development Environment

Initial development will happen locally on Windows.

Docker Compose will be introduced gradually.

Initial approach:

- React runs locally.
- ASP.NET Core runs locally.
- PostgreSQL runs in Docker.
- Photo storage initially uses local storage.

The developer will learn Docker fundamentals as part of the project.

The initial Docker setup should prioritize simplicity and reproducibility.

## 11. Testing Strategy

Testing will be introduced incrementally alongside implementation.

### Backend

- xUnit for unit tests.
- Integration tests for API behavior, database interactions and authorization.

### Frontend

- Vitest
- React Testing Library

Tests should focus on meaningful application behavior, business rules and critical user flows.

## 12. Hosting and Deployment

Hosting infrastructure has not been selected.

The application should be designed so that it can eventually be deployed to cloud hosting or a VPS.

No cloud provider or production infrastructure should be assumed at this stage.

The initial priority is a stable local development environment.

## 13. Open Architectural Decisions

The following decisions remain open and must be discussed when they materially affect implementation:

- Exact API contracts and endpoint structure.
- Detailed SignalR event contracts and connection management.
- Concurrent editing implementation.
- Offline persistence and conflict recovery.
- Specific object storage provider.
- Production hosting and deployment.
- Detailed logging, monitoring and error handling implementation.
- Exact backend internal layering within the approved modular monolith.
- Backup tooling, remote storage provider and detailed restoration procedure.

The following decisions have already been approved and must not be treated as open:

- Modular monolith architecture.
- React + Vite + TypeScript frontend.
- ASP.NET Core backend.
- PostgreSQL and Entity Framework Core.
- ASP.NET Core Identity and JWT authentication.
- Access Token and Refresh Token strategy.
- SignalR as the real-time communication technology.
- Local photo storage with a replaceable storage abstraction.
- Outbox Pattern and Background Worker for file deletion.
- API and infrastructure abuse protection requirements.
- Chronicle-scoped real-time synchronization.
- Server-confirmed persistence before broadcasting changes.
- Automatic reconnection and backend synchronization after connection loss.
- Independent timeline navigation state for every connected user.

## 14. Architectural Principles

1. Prefer simplicity over unnecessary complexity.
2. Keep the backend as a modular monolith.
3. Maintain clear business module boundaries.
4. Keep frontend and backend independent.
5. Enforce business rules and permissions on the backend.
6. Avoid unnecessary dependencies between modules.
7. Keep infrastructure replaceable where practical.
8. Prioritize a reliable local development experience.
9. Implement features incrementally.
10. Do not introduce unapproved product or architectural decisions.

## API Security and Abuse Protection

The application must be protected against automated abuse, spam, brute-force attacks and excessive traffic.

Security must be implemented in multiple layers: infrastructure, backend API and file upload handling.

### API Protection

- Implement rate limiting for API requests.
- Apply stricter limits to authentication endpoints.
- Protect registration and password recovery against automated abuse.
- Limit invitation link generation and invitation token verification attempts.
- Apply appropriate request size limits.
- Reject malformed or excessively large requests.
- Ensure rate limiting cannot be bypassed by manipulating client-supplied headers.

### Bot and Spam Protection

- Prepare the production infrastructure for integration with Cloudflare or an equivalent security provider.
- Use infrastructure-level traffic filtering and bot protection where appropriate.
- Protect publicly accessible endpoints against automated abuse.
- Consider additional verification mechanisms if suspicious activity is detected.

### Monitoring and Logging

- Log suspicious activity and repeated failed requests.
- Monitor unusual traffic patterns and excessive API usage.
- Never log passwords, authentication tokens, invitation tokens or other sensitive credentials.
- Ensure logs do not expose private user or chronicle data.

### Infrastructure Security

- HTTPS is required in production.
- Configure appropriate HTTP security headers.
- Keep database and file storage inaccessible directly from the public internet.
- Do not rely exclusively on infrastructure-level protection. Backend authorization and validation must always remain active.

### Production Protection Layer

Cloudflare or an equivalent provider is the preferred additional protection layer for production deployment.

The architecture must allow this integration without requiring changes to core business logic.

Cloudflare integration is not required during local development.

## Backup and Recovery

The application must support automated backups of PostgreSQL data and uploaded photo files.

### Backup schedule

- Backups are created automatically once per week.
- A maximum of 12 backup generations are retained.
- Older backups are automatically removed after a newer backup is successfully created.

### Storage

- Backups must be stored remotely, independently from the primary application server.
- Backups must be encrypted.
- Backup storage credentials must be kept separate from application credentials.

### Backup consistency

- Database backups and photo backups must represent a consistent recovery point.
- The backup process must account for files being uploaded or deleted while a backup is running.

### Recovery

- Backup restoration procedures must be documented.
- Backup integrity must be verified.
- Restoration must be tested regularly.
- A backup must not be considered reliable until successful restoration has been verified.
